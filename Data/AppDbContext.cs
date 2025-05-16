using Microsoft.EntityFrameworkCore;
using SimpleContactManager.Models;

namespace SimpleContactManager.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions): base(dbContextOptions){ }
        public DbSet<Contact> Contacts { get; set; }
    }
}
