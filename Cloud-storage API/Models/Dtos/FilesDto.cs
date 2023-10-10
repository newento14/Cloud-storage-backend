using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Cloud_storage_API.Models.Dtos
{
    public class FilesDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public bool Private { get; set; }
        public Guid Link { get; set; }
        public string Date { get; set; }
        public bool Starred { get; set; }
    }
}
