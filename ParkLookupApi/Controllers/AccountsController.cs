using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using ParkLookupApi.Models;

namespace ParkLookupApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly IConfiguration _configuration;

  public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _configuration = configuration;
  }

  // POST: api/accounts/register
  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDto user)
  {
    // First, check to see if these credentials have already been registered.
    // If so, return error, otherwise continue.
    ApplicationUser userExists = await _userManager.FindByEmailAsync(user.Email);
    if (userExists != null)
    {
      return BadRequest(new { status = "Error", message = "Email is already registered." });
    }

    ApplicationUser newUser = new ApplicationUser() { Email = user.Email, UserName = user.UserName };
    IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

    if (result.Succeeded)
    {
      return Ok(new { status = "Success", message = $"User {user.UserName} has been successfully created." });
    }
    else
    {
      return BadRequest(result.Errors);
    }
  }

  // POST: api/accounts/signin
  [HttpPost("signin")]
  public async Task<IActionResult> SignIn(SignInDto userInfo)
  {
    ApplicationUser user = await _userManager.FindByEmailAsync(userInfo.Email);

    if (user != null)
    {
      var signInResult = await _signInManager.PasswordSignInAsync(user, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
      if (signInResult.Succeeded)
      {
        List<Claim> authClaims = new List<Claim>
        {
          new Claim("userId", user.Id),
          new Claim("userName", user.UserName)
        };

        var newToken = CreateToken(authClaims);

        return Ok(new { status = "Success", message = $"{userInfo.Email} signed in.", token = newToken });
      }
    }

    return BadRequest(new { status = "Error", message = "Unable to sign in." });
  }

  // Generate a user's Token
  private string CreateToken(List<Claim> authClaims)
  {
    SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

    JwtSecurityToken token = new JwtSecurityToken(
      issuer: _configuration["JWT:ValidIssuer"],
      audience: _configuration["JWT:ValidAudience"],
      expires: DateTime.Now.AddHours(8),
      claims: authClaims,
      signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
    );

    // Serialize and return our newly created Token
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}