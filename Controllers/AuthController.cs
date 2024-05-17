using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Service;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(ITokenService tokenService,

         IConfiguration configuration,
          ILogger<AuthController> logger,
         RoleManager<IdentityRole> roleManager,
         UserManager<ApplicationUser> userManager
          )
        {

            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim> {
                 new Claim(ClaimTypes.Name, user.UserName!),
                 new Claim(ClaimTypes.Email, user.Email!),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                 out int refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo

                });


            }

            return Unauthorized();

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDTO model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName!);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User already exists"
                });

            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName

            };

            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User creation failed."
                });

            }

            return Ok(new Response
            {
                Status = "Success",
                Message = "User creation successfully!"
            });


        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModelDTO tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");

            }

            string? accessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));
            string refreshToken = tokenModel.RefreshToken ?? throw new ArgumentException(nameof(tokenModel));

            var principal = _tokenService.GetClaimsPrincipalFromExpiredToken(accessToken!, _configuration);

            if (principal == null)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });

        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<ActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest("Invalid user name");
            }

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<ActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return StatusCode(StatusCodes.Status200OK,
                     new Response { Status = "Success", Message = $"Role {roleName} added successfully" });

                }
                else
                {
                    _logger.LogInformation(1, "Roles Added");
                    return StatusCode(StatusCodes.Status400BadRequest,
                     new Response { Status = "Error", Message = $"Issue adding the new {roleName} role" });
                }

            }

            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Role already exist." });

        }


        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                    return StatusCode(StatusCodes.Status200OK,
                     new Response { Status = "Success", Message = $"User {user.Email} added to the {roleName} role" });

                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to add user {user.Email} to the {roleName} role");
                    return StatusCode(StatusCodes.Status400BadRequest,
                     new Response { Status = "Error", Message = $"Error Unable to add user {user.Email} to the {roleName} role" });
                }

            }

            return BadRequest(new { error = "Unable to find user" });

        }

    }

}