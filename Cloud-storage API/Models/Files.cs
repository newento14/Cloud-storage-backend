using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cloud_storage_API.Models
{
    public class Files
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Type { get; set; }
        public int Size { get; set; }
        [DefaultValue(true)]
        public bool Private { get; set; }
        [MaxLength(255)]
        public Guid Link { get; set; }
        public string Date { get; set; }
        public bool Starred { get; set; }
        [Required]
        public int UserId { get; set; }
        [DefaultValue(null)]
        public int parrentId { get; set; }
    }
}
