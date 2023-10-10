using Cloud_storage_API.Db;
using Cloud_storage_API.Models;
using Cloud_storage_API.Models.Dtos;
using Cloud_storage_API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

namespace Cloud_storage_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly ITokenRepository _tokenRepository;

        public UserController(IUserRepository repo, ITokenRepository tokenRepository)
        {
            _repo = repo;
            _tokenRepository = tokenRepository;
        }


        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration(
            [FromBody] UserRegistrationDto userRegistration)
        {
            if (await _repo.GetByEmailAsync(userRegistration.Email) != null)
            {
                return BadRequest("email alredy used");
            }

            var user = new Users
            {
                Email = userRegistration.Email,
                Password = HashPassword(userRegistration.Password),
                StorageSize = 10737418240,
                Avatar = "",
                StorageUsed = 0
            };

            
            await _repo.RegisterAsync(user);
            CreateFolder(user);
            return Ok(user.AsDto());
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(
            [FromBody] UserLoginDto request)
        {
            var user = await _repo.GetByEmailAsync(request.Email);

            if (user != null)
            {
                if (user.Password == HashPassword(request.Password))
                {
                    var response = new UserLoginResponseDto()
                    {
                        User = new UserResponse()
                        {
                            Id = user.Id,
                            Email = user.Email,
                            Avatar = user.Avatar,
                            StorageSize = user.StorageSize,
                            StorageUsed = user.StorageUsed
                        },
                        Token = _tokenRepository.CreateToken(user)
                    };
                    return Ok(response);
                }
                return NotFound("Password is incorrect");
            }
            return NotFound("Email is incorrect");
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == "")
            {
                return Unauthorized();
            }
            var userId = _tokenRepository.ValidateToken(token.Split(' ')[1]);
            if (userId == -1)  
                return NotFound();

            var user = await _repo.GetByIdAsync(userId);

            string newToken;
            if (user != null)
            {
                var response = new UserLoginResponseDto()
                {
                    User = new UserResponse()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Avatar = user.Avatar,
                        StorageSize = user.StorageSize,
                        StorageUsed = user.StorageUsed
                    },
                    Token = _tokenRepository.CreateToken(user)
                };
                return Ok(response);
            }
            return NotFound();
        }

        [NonAction]
        private string HashPassword(string Password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [NonAction]
        private async void CreateFolder(Users user)
        {
            var id = await _repo.GetByEmailAsync(user.Email);
            Directory.CreateDirectory("D:\\storage" + $"\\{id.Id}");
        }
    }
}
