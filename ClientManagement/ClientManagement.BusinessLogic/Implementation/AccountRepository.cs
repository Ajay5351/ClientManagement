using ClientManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClientManagement.BusinessLogic
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationModel> _userManager;
        private readonly SignInManager<ApplicationModel> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(
            UserManager<ApplicationModel> userManager,
            SignInManager<ApplicationModel> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
        {
            var existingUser =
                await _userManager.FindByEmailAsync(signUpModel.Email);

            if (existingUser != null)
                throw new Exception("User already exists with this email.");

            var user = new ApplicationModel
            {
                FirstName = signUpModel.FirstName.Trim(),
                LastName = signUpModel.LastName.Trim(),
                Email = signUpModel.Email.Trim(),
                UserName = signUpModel.Email.Trim()
            };

            return await _userManager.CreateAsync(
                user,
                signUpModel.Password);
        }

        public async Task<string?> LoginAsync(SignInModel signInModel)
        {
            var user =
                await _userManager.FindByEmailAsync(signInModel.Email);

            if (user == null)
                throw new Exception("User not found.");

            var result =
                await _signInManager.PasswordSignInAsync(
                    signInModel.Email,
                    signInModel.Password,
                    false,
                    false);

            if (!result.Succeeded)
                throw new Exception("Invalid email or password.");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var authSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authSigningKey,
                    SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
