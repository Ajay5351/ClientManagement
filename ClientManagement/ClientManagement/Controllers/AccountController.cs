using ClientManagement.Models;
using ClientManagement.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignUpModel signupModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _accountRepository.SignUpAsync(signupModel);

                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        Message = "User registered successfully"
                    });
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _accountRepository.LoginAsync(signInModel);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new
                    {
                        Message = "Invalid email or password"
                    });
                }

                return Ok(new
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
