using Microsoft.EntityFrameworkCore;
using user_service.domain;

namespace user_service.infrastructure
{
    public class UserContext : DbContext
    {
        
        public DbSet<User> users { get; set; }
        public UserContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Изменю на реальные данные. Наверное
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=пароль_от_postgres");
        }
        
    }
}
