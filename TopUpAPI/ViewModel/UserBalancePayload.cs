using Newtonsoft.Json;

namespace TopUpAPI.ViewModel
{
    public class UserBalancePayload
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
