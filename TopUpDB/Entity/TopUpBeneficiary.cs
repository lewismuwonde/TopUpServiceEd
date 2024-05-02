using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUpDB.Entity
{
    public class TopUpBeneficiary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]      
        public long UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string NickName { get; set; }

        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsActive { get; set; }

       
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

       
        [DataType(DataType.DateTime)]
        public DateTime UpdateDate { get; set; }
  
    }

}
