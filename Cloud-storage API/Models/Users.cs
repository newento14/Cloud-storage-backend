using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Cloud_storage_API.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(64)]
        public string Password { get; set; }
        [MaxLength(255)]
        [DefaultValue("")]
        public string Avatar { get; set; }
        [DefaultValue(10737418240)]
        public long StorageSize { get; set; }
        [DefaultValue(0)]
        public long StorageUsed { get; set; }
    }
}
