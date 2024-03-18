using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WalletKata.Models;
using WalletKata.Services.Interfaces;

namespace WalletKata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Genero endpoint CreateUser por si es necesario generar un nuevo usuario para las pruebas.
        /// </summary>
        /// <param name="username"></param>
        /// <returns> Retorno user id  ya que es el parametro que solicitan los endpoint de Wallet.</returns>
        [HttpPost]
        public async Task<ActionResult<object>> CreateUser(string username)
        {
            try
            {
                var userId = await _userService.CreateUser(username);
                return Ok(new { userId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
