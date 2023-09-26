using Microsoft.EntityFrameworkCore;
namespace ToDoApp.Models
{
    public class AuthenticationDBContext : DbContext
    {
        public AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> options)
            : base(options)
        {
        }

        public DbSet<AuthenticationModel> authenticationModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}

