using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)  // constructor, recibe options como parametro y extiende de base
        {
        }

        public DbSet<AppUser> Users { get; set; } //this represent tables inside of the db, Users is my table in the db
    }
}