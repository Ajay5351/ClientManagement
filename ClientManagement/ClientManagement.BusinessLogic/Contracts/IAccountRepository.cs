using ClientManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace ClientManagement.BusinessLogic
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
        Task<string> LoginAsync(SignInModel signInModel);
    }
}
