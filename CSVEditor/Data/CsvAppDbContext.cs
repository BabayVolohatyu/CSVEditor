using CSVEditor.Models;
using Microsoft.EntityFrameworkCore;

namespace CSVEditor.Data
{
    public class CsvAppDbContext : DbContext
    {
        public CsvAppDbContext(DbContextOptions<CsvAppDbContext> options) : base(options) { }

        public DbSet<Entity> Entities { get; set; }
    }
}
