using System.ComponentModel.DataAnnotations;

namespace MyAPI.Model
{
    public class ContributionModel
    {
        [Key]
        public int ContributionID { get; set; }
        [StringLength(255, MinimumLength = 1)]
        public string Description { get; set; }
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }
        public DateTime DateReceived { get; set; } = DateTime.Now;
        [Required]
        public int Amount { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
    }
}
