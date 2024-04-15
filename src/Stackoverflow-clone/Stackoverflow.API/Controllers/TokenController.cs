using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Stackoverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Stackoverflow.API.Controllers
{
    [Route("v3/[controller]")]
    [ApiController]

    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(IConfiguration config,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ILogger<TokenController> logger)
        {
            _configuration = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string email, string password)
        {
            if (email != null && password != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                _logger.LogInformation($"User found. {user.PasswordHash}:", user.PasswordHash);
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                var LockedOut = await _userManager.IsLockedOutAsync(user);
                if (LockedOut)
                {
                    // User account is locked out
                    _logger.LogWarning("User account locked out. {Email}", email);
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
                    return BadRequest("Your account is locked. Please try again later or contact support for assistance.");
                }
                else
                {
                    if (result != null && result.Succeeded)
                    {
                        var claims = (await _userManager.GetClaimsAsync(user)).ToArray();
                        var token = await _tokenService.GetJwtToken(claims,
                                _configuration["Jwt:Key"],
                                _configuration["Jwt:Issuer"],
                                _configuration["Jwt:Audience"]
                            );

                        return Ok(token);
                    }
                    else
                    {
                        return BadRequest("Invalid credentials");
                    }
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
