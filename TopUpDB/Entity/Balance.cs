using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUpDB.Entity
{
    public class Balance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]     
        public long UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal Amount { get; set; }

      
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; }

    }
}
