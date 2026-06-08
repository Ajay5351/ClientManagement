using ClientManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace ClientManagement.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationModel> _userManager;

        public AccountRepository(UserManager<ApplicationModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> SignupAsync(SignupModel signupModel)
        {
            var user = new ApplicationModel
            {
                FirstName = signupModel.FirstName,
                LastName = signupModel.LastName,
                Email = signupModel.Email,
                UserName = signupModel.Email
            };

            return await _userManager.CreateAsync(user, signupModel.Password);
        }
    }
}
