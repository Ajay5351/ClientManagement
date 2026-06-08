using ClientManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace ClientManagement.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignupAsync(SignupModel signupModel);
    }
}
