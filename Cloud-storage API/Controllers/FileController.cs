using Cloud_storage_API.Models;
using Cloud_storage_API.Models.Dtos;
using Cloud_storage_API.Repositories.Interface;
using Cloud_storage_API.Repositories.Source;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Xml.Linq;

namespace Cloud_storage_API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : Controller
    {
        private const string mainPath = "D:\\storage\\";
        private readonly IFilesRepository _fileRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITokenRepository _tokenRepository;

        public FileController(IFilesRepository repo, IUserRepository userRepo, ITokenRepository tokenRepository)
        {
            _fileRepo = repo;
            _userRepo = userRepo;
            _tokenRepository = tokenRepository;
        }


        [HttpPost]
        [Route("folder")]
        public async Task<IActionResult> CreateFolder(PathRequest request)
        {
            var pathId = request.PathId.Split("\\");
            int parrentId = 0;
            if (pathId.Length > 2)
                parrentId = Convert.ToInt32(pathId[pathId.Length - 2]);

            var path = mainPath + request.PathName;
            if (System.IO.File.Exists(path))
                return Conflict("folder with this name alredy exist");

            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == "")
                return Unauthorized();

            var id = _tokenRepository.ValidateToken(token.Split(' ')[1]);
            if (id == -1)
                return NotFound();

            var piece = path.Split('\\');
            var name = piece[piece.Length - 1];
            var file = new Models.Files()
            {
                Date = DateTime.Now.ToString(),
                Name = name,
                Type = "dir",
                UserId = id,
                Link = Guid.NewGuid(),
                Size = 0,
                parrentId = parrentId,
                Private = true,
                Starred = false
            };
            await _fileRepo.AddFileAsync(file);
            var folder = Directory.CreateDirectory(mainPath + request.PathName);
            var fileId = await _fileRepo.GetLastIdAsync();
            var response = new FileDto()
            {
                Id = fileId,
                Date = file.Date,
                Name = name,
                Starred = file.Starred,
                Link = file.Link,
                Private = file.Private,
                Size = file.Size,
                Type = file.Type,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("getFiles")]
        public async Task<IActionResult> GetFiles(PathRequest request)
        {
            var path = request.PathId.Split("\\");
            int parrentId = 0;
            if (path.Length > 2)
                parrentId = Convert.ToInt32(path[path.Length - 2]);

            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == "")
                return Unauthorized();

            var id = _tokenRepository.ValidateToken(token.Split(' ')[1]);
            if (id == -1)
                return NotFound();

            IEnumerable<Models.Files> files;
            if (parrentId == 0)
                files = await _fileRepo.FindByUserIdAsync(id);
            else
                files = await _fileRepo.FindByParentIdAsync(Convert.ToInt32(parrentId));

            files = files.OrderByDescending(x => x.Starred)
                .ThenBy(x => x.Type == "dir" ? 0 : 1)
                .ToList();

            List<FileDto> response = new List<FileDto>();
            response.Capacity = files.Count();
            foreach (var file in files)
            {
                response.Add(new FileDto()
                {
                    Id = file.Id,
                    Date = file.Date,
                    Name = file.Name,
                    Type = file.Type,
                    Link = file.Link,
                    Size = file.Size,
                    Private = file.Private,
                    Starred = file.Starred,
                });
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("setStarred")]
        public async Task<IActionResult> SetStarred([FromBody]SetStarredRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == "")
                return Unauthorized();

            var id = _tokenRepository.ValidateToken(token.Split(' ')[1]);
            if (id == -1)
                return NotFound();
            await _fileRepo.SetStarredAsync(request.id, request.state);
            return Ok();
        }

        [HttpPost]
        [Route("upload")]
        [RequestSizeLimit(10737418240)]
        public async Task<IActionResult> UploadFile([FromForm]UploadFileRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            var id = _tokenRepository.ValidateToken(token.Split(' ')[1]);
            if (id == -1)
                return NotFound();

            if (request.storageUsed + request.file.Length > request.storageSize)
                return BadRequest("Storage is full");

            if (request.file == null || request.file.Length == 0)
                return BadRequest("File is empty");

            try
            {
                string fullpath;
                if (request.pathName != null)
                    fullpath = Path.Combine(mainPath, id.ToString(), request.pathName, request.file.FileName);
                else
                    fullpath = Path.Combine(mainPath, id.ToString(), request.file.FileName);

                if (System.IO.File.Exists(fullpath))
                    return Conflict("file with this name alredy exist");

                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    await request.file.CopyToAsync(stream);
                }
                var user = await _userRepo.GetByIdAsync(id);
                await _userRepo.UpdateStorageUsed(user, request.file.Length);

                int parrentId = 0;
                string[] items;
                if (request.pathId != null)
                {
                    items = request.pathId.Split('\\');
                    parrentId = Convert.ToInt32(items[items.Length - 1]);
                }

                items = request.file.FileName.Split('.');
                string type = items[items.Length - 1];

                var dbFile = new Files()
                {
                    Date = DateTime.Now.ToString(),
                    Name = request.file.FileName,
                    Type = type,
                    UserId = id,
                    Link = Guid.NewGuid(),
                    Size = request.file.Length,
                    parrentId = parrentId,
                    Private = true,
                    Starred = false
                };

                await _fileRepo.AddFileAsync(dbFile);

                var response = new FileDto()
                {
                    Id = dbFile.Id,
                    Date = dbFile.Date,
                    Name = dbFile.Name,
                    Type = dbFile.Type,
                    Link = dbFile.Link,
                    Size = dbFile.Size,
                    Private = dbFile.Private,
                    Starred = dbFile.Starred,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
    public class UploadFileRequest
    {
        public IFormFile file {  get; set; }
        public string? pathId { get; set; }
        public string? pathName { get; set; }
        public long storageSize { get; set; }
        public long storageUsed { get; set; }
    }
    public class SetStarredRequest
    {
        public int id { get; set; }
        public bool state { get; set; }
    }
    public class PathRequest
    {
        public string PathId { get; set; }
        public string PathName { get; set; }
    }
}
