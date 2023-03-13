using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GUIDE.Repositories;
using GUIDE.Models;
using GUIDE.Services;

namespace GUIDE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] Users model)
        {
            var user = await _userRepository.Get(model.Username, model.Password);

            if (user == null)
                return (new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> CreateUser([FromBody] Users model)
        {
            var user = await _userRepository.Post(model.Username, model.Password);

            return new
            {
                user = user
            };
        }
    }
}