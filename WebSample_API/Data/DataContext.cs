using Microsoft.EntityFrameworkCore;
using WebSample_API.Models;

namespace WebSample_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}
        public DbSet<Value> Values {get;set;}
        public DbSet<User> UserNames {get;set;}
    }
}