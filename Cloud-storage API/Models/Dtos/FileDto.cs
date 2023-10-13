using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Cloud_storage_API.Models.Dtos
{
    public class FileDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public bool isPrivate { get; set; }
        public Guid Link { get; set; }
        public string Date { get; set; }
        public bool Starred { get; set; }
    }
}
