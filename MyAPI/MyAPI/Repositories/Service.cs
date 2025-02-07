using MyAPI.Model;
using MyAPI.Repositories;

namespace MyAPI.Repositories
{
    public class Service
    {
        public UserService User { get; set; }
        public ContributionService Contribution { get; set; }
        public WithdrawService Withdraw { get; set; }

        public Service(UserService userService, ContributionService contributionService, WithdrawService withdrawService)
        {
            User = userService;
            Contribution = contributionService;
            Withdraw = withdrawService;
        }
    }
}
