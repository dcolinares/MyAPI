using System.ComponentModel.DataAnnotations;

namespace PundoPH.Model
{
    public class WithdrawModel
    {
        [Key]
        public int WithdrawID { get; set; }
        [Required]
        [StringLength(255,MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Reason { get; set; }
        public decimal Amount { get; set; }
        public int UserID { get; set; }
        public DateTime WithdrawDate { get; set; } = DateTime.Now;


    }
}
