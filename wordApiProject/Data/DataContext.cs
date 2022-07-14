 using Microsoft.EntityFrameworkCore;

namespace wordApiProject.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        

            public DbSet<User> Users { get; set; }
            public DbSet<UserWord>UserWords { get; set; }
            public DbSet<Words> Words { get; set; }

        internal Task<bool> FindAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
    
}
