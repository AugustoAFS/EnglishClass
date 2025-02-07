using EnglishClass.Models;
using EnglishClass.services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishClass.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        public UserController(IUserInterface userInterface) 
        { 
            _userInterface = userInterface;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return NotFound("Dados do usuário inválidos.");
            }

            IEnumerable<User> users = await _userInterface.CreateUser(user);

            return CreatedAtAction(nameof(CreateUser), new { id = users.FirstOrDefault()?.Id }, users);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            IEnumerable<User> Users = await _userInterface.GetAllUsers();
            if (Users == null)
            {
                return NotFound("Nenhum registro Localizado!");
            }

            return Ok(Users);
        }
    }
}
