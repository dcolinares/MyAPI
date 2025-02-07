using System.ComponentModel.DataAnnotations;

namespace MyAPI.Model
{
    public class WithdrawsWithUserModel
    {
        public int WithdrawID { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
        public int UserID { get; set; }
        public DateTime WithdrawDate { get; set; } = DateTime.Now;

        public string UserName { get; set; }
    }

    public class ContributionAndDisbursement
    { 
        public decimal TotalContribution { get; set; }
        public decimal TotalDisbursement { get; set; }
        public decimal AvailabelBalance { get; set; }
    }
}
