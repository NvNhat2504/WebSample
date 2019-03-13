using Microsoft.EntityFrameworkCore;
using WebSample.API.Models;

namespace WebSample.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}
        public DbSet<Value> Values {get;set;}
    }
}