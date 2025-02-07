using Microsoft.EntityFrameworkCore;
using MyAPI.Model;
using MyAPI.Application;

namespace MyAPI.Repositories
{
    public class WithdrawService
    {
        private readonly ApplicationDbContext _appDbContext;
        public WithdrawService(ApplicationDbContext appDbContext) 
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public WithdrawService() { }

        public async Task<string> Save(WithdrawModel withdrawModel)
        {
            string resutl = "";
            try
            {
                _appDbContext.Withdraws.Add(withdrawModel);
                await _appDbContext.SaveChangesAsync();
            } catch (Exception ex)
            {
                resutl = ex.Message;
            }

            return resutl;
        }

        public async Task<List<WithdrawModel>> Get()
        {
            List<WithdrawModel> withdrawModel = new List<WithdrawModel>();
            withdrawModel = _appDbContext.Withdraws.OrderByDescending(x => x.WithdrawID).ToList();
            return withdrawModel;
        }

        public async Task<List<WithdrawsWithUserModel>> GetWithdrawWithUser()
        {
            var result = await (from w in _appDbContext.Withdraws
                                join u in _appDbContext.Users
                                on w.UserID equals u.Id
                                select new WithdrawsWithUserModel {
                                    WithdrawID = w.WithdrawID,
                                    Name = w.Name,
                                    Reason = w.Reason,
                                    Amount = w.Amount,
                                    UserID = w.UserID,
                                    WithdrawDate = w.WithdrawDate,
                                    UserName = u.FirstName,
                                }).ToListAsync();
            return result;
        }

        public async Task<ContributionAndDisbursement> GetTotalContributionAndDisbursement()
        {
            decimal totalContribution = _appDbContext.Contributions.Sum(c => c.Amount);
            decimal totalDisbursement = _appDbContext.Withdraws.Sum(w => w.Amount);
            var result = new ContributionAndDisbursement() {
                TotalContribution = totalContribution,
                TotalDisbursement = totalDisbursement,
                AvailabelBalance = (totalContribution - totalDisbursement)

            };
            return result;
        }
    }
}
