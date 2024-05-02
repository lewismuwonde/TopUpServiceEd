
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using TopUpAPI.Services.Beneficiaries;
using TopUpAPI.Services.Users;
using TopUpAPI.Utilities;
using TopUpAPI.ViewModel;
using TopUpDB.Entity;
using TopUpDB.Interface;

namespace TopUpAPI.Services.TopUp
{
    public class TopUpService : ITopUpService
    {
        private readonly ITopUp _topUpRepository;
        private readonly IBeneficiariesService _beneficiaryService;
        private readonly IUserService _iUserservice;
        private readonly IBalance _iBalance;     

        public TopUpService(IUserService iUserservice, IBeneficiariesService beneficiariesService, ITopUp topUpRepository, IBalance iBalance)
        {
            _iUserservice = iUserservice;
            _beneficiaryService = beneficiariesService;
            _topUpRepository = topUpRepository;
            _iBalance = iBalance;     
        }
        public List<int> GetTopUpOptions()
        {
            return Const.TopUpOptions;
        }
        public async Task<bool> TopUpBeneficiary(TopUpRequest request, int userId)
        {
            try
            {   
                // check if user exists
                var user = await _iUserservice.GetUserById(userId);
                if (user == null)
                    throw new ArgumentException("User not found");             

                // Validate top-up amount
                if (!IsValidTopUpAmount(request.Amount))                
                    throw new ArgumentException("Invalid top-up amount");
                
                // check if Balance Sufficient
                var isSufficient = await IsBalanceSufficient(userId, request.Amount);
               
                if (!isSufficient)
                    throw new ArgumentException("Insufficient balance, please recharge your account!");

                var beneficiary = await _beneficiaryService.GetBeneficiaryById(request.BeneficiaryId);

                // Check monthly limits
                await CheckMonthlyLimits(userId, user.IsVerified, beneficiary.Id);

                // Call balance API to update user balance
                var isSuccess = await CallBalanceApi(userId, request.Amount);

                if (isSuccess)
                {
                    // Process top-up if API call succeeds
                    await _topUpRepository.ProcessTopUp(userId, request.BeneficiaryId, request.Amount);
                    return true;
                }
                else
                {
                    throw new ArgumentException("API call failed!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Exception occurred: {ex.Message}");
            }
        }

        private bool IsValidTopUpAmount(decimal amount)
        {
            return Const.TopUpOptions.Contains((int)amount);
        }

        private async Task CheckMonthlyLimits(long userId, bool isUserVerified, long beneficiaryId)
        {
            try
            {
                var currentMonth = DateTime.Now.Month;
                var monthlyLimitPerBeneficiary = isUserVerified ? 500 : 1000;

                var totalMonthlyTopUps = await _topUpRepository.GetTotalTopUpsForUserAndMonth(userId, currentMonth);
                var totalMonthlyTopUpsForBeneficiary = await _topUpRepository.GetTotalTopUpsForUserAndBeneficiaryAndMonth(userId, beneficiaryId, currentMonth);

                if (totalMonthlyTopUps >= 3000)
                {
                    throw new ArgumentException("Monthly top-up limit of AED 3,000 reached");
                }

                if (totalMonthlyTopUpsForBeneficiary >= monthlyLimitPerBeneficiary)
                {
                    throw new ArgumentException($"Monthly top-up limit of AED {monthlyLimitPerBeneficiary} per beneficiary reached");
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException($"Exception occurred: {ex.Message}");
            }
        }

        private async Task<bool> IsBalanceSufficient(long userId, decimal amount)
        {
            var availableBalance = await _iBalance.GetUserAvailableBalance(userId);
            if (availableBalance < amount)
                return false;

            return true;
        }

        private async Task<bool> CallBalanceApi(int userId, decimal amount)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("accept", "*/*");

                    // Add API key to request headers
                    httpClient.DefaultRequestHeaders.Add("x-api-key", "YOUR_API_KEY_HERE");

                    var payload = new UserBalancePayload
                    {
                        UserId = userId,
                        Amount = amount
                    };

                    string jsonPayload = JsonConvert.SerializeObject(payload);

                    // Create StringContent with MediaType
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PutAsync(Const.BalanceApiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {                      
                        return true;
                    }
                       
                    else
                        throw new ArgumentException($"Failed to call API. Status code: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Exception occurred while calling API: {ex.Message}");
                }
            }
        }


       
    }
}
