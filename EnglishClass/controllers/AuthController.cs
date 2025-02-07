using EnglishClass.services.UserService;
using EnglishClass.services.Auth;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using EnglishClass.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserInterface _userInterface;
    private readonly AuthUserService _authService;

    public AuthController(IUserInterface userInterface, AuthUserService authService)
    {
        _userInterface = userInterface;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Busca o usuário pelo Username
        var user = await _userInterface.GetUserByUsernameAsync(request.Username);

        if (user == null)
        {
            return Unauthorized("Usuário ou senha inválidos.");
        }

        // Verifica se a senha é válida
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Unauthorized("Usuário ou senha inválidos.");
        }

        // Gera o JWT Token
        var token = _authService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }
}
