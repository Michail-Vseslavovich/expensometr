using expense_service.domain.Models;
using Microsoft.EntityFrameworkCore;

namespace expense_service.infrastructure.db
{
    public class IncomeContext : DbContext
    {
        public DbSet<Income> Incomes { get; set; }
        public IncomeContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=пароль_от_postgres");
        }
    }
}
