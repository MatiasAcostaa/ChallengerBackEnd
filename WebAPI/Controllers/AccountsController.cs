using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.Interfaces.IExternalServices;
using WebAPI.Infrastructure.Security;
using WebAPI.Infrastructure.Security.Interfaces;
using WebAPI.Infrastructure.Security.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenHandlerService _tokenHandlerService;
        private readonly IMailService _mailService;

        public AccountsController(UserManager<IdentityUser> userManager, ITokenHandlerService tokenHandlerService, IMailService mailService)
        {
            _userManager = userManager;
            _tokenHandlerService = tokenHandlerService;
            _mailService = mailService;
        }


        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromForm] UserResquest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser is not null)
                    return BadRequest($"El Email {user.Email} esta en uso.");

                var isCreated = await _userManager.CreateAsync(new IdentityUser
                {
                    UserName = user.Email,
                    Email = user.Email
                }, password: user.Password);

                if (isCreated.Succeeded)
                {
                    await _mailService.SendMail(to: user.Email);

                    return StatusCode(201, "Registrado con éxito");
                }
                else
                    return BadRequest(isCreated.Errors.Select(e => e.Description));
            }
            else
                return BadRequest("Error al registrarse.");
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] UserResquest user)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if(existingUser is null)
                {
                    return BadRequest(new LoginResponse
                    {
                        Login = false,
                        Errors = new List<string>()
                        {
                            "Usuario o contraseña incorrecto."
                        }
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if(isCorrect)
                {
                    var parameters = new TokenParameters
                    {
                        Id = existingUser.Id,
                        UserName = existingUser.UserName,
                        PasswordHash = existingUser.PasswordHash
                    };

                    var jwtToken = _tokenHandlerService.GenerateJwtToken(parameters);

                    return Ok(new LoginResponse
                    {
                        Login = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new LoginResponse
                    {
                        Login = false,
                        Errors = new List<string>()
                        {
                            "Usuario o contraseña incorrecto."
                        }
                    });
                }
            }
            else
            {
                return BadRequest(new LoginResponse
                {
                    Login = false,
                    Errors = new List<string>()
                    {
                        "Usuario o contraseña incorrecto."
                    }
                });
            }
        }
}
    
