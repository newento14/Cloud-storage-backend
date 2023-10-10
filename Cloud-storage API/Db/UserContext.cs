using Cloud_storage_API.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Cloud_storage_API.Db
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Files> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost; user=root; port=1488; password=pupaOkOk1488?; database=CloudStorage",
                                    new MySqlServerVersion(new Version(8, 0, 11)));

            optionsBuilder.UseLoggerFactory(MyLogger);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static readonly ILoggerFactory MyLogger = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name &&
            level == LogLevel.Information).AddDebug();
        });

    }
}
