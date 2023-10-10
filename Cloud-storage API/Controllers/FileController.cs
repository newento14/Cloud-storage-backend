using Cloud_storage_API.Models;
using Cloud_storage_API.Models.Dtos;
using Cloud_storage_API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Cloud_storage_API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : Controller
    {
        private const string mainPath = "D:\\storage\\";
        private readonly IFilesRepository _repo;
        private readonly ITokenRepository _tokenRepository;

        public FileController(IFilesRepository repo, ITokenRepository tokenRepository)
        {
            _repo = repo;
            _tokenRepository = tokenRepository;
        }


        [HttpPost]
        [Route("folder")]
        public IActionResult CreateFolder(FolderRequest request)
        {
            var path = mainPath + request.Path;
            if (System.IO.File.Exists(path))
            {
                return Conflict("folder with this name alredy created");
            }
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == "")
            {
                return Unauthorized();
            }
            var id = _tokenRepository.ValidateToken(token.Split(' ')[1]);
            if (id == -1)
                return NotFound();

            string[] piece = path.Split('\\');
            string name = piece[piece.Length - 1];
            var file = new Files()
            {
                Date = DateTime.Now.ToString(),
                Name = name,
                Type = "dir",
                UserId = id,
                Link = Guid.NewGuid(),
                Size = 0,
                parrentId = -1,
                Private = true,
                Starred = false
            };
            _repo.AddFileAsync(file);

            var folder = Directory.CreateDirectory(mainPath + request.Path);
            return Ok(folder);
        }

        [HttpGet]
        [Route("getFiles")]
        public async Task<IActionResult> GetFiles()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == "")
            {
                return Unauthorized();
            }
            var id = _tokenRepository.ValidateToken(token.Split(' ')[1]);
            if (id == -1)
                return NotFound();

            var users = await _repo.FindByUserIdAsync(id);

            List<FilesDto> response = new List<FilesDto>();
            response.Capacity = users.Count();
            foreach (var user in users)
            {
                response.Add(new FilesDto()
                {
                    Date = user.Date,
                    Name = user.Name,
                    Type = user.Type,
                    Link = user.Link,
                    Size = user.Size,
                    Private = user.Private,
                    Starred = user.Starred,
                });
            }
            return Ok(response);


        }
    }

    public class FolderRequest
    {
        public string Path { get; set; }
    }
}
