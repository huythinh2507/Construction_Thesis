using Microsoft.EntityFrameworkCore;
namespace Thesis_web.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public  DbSet<Entries> Entries { get; set; }
        public  DbSet<Workers> Workers { get; set; }
        public  DbSet<Signups> Signups { get; set; }
        public DbSet<Applicants> Applicants { get; set; }
        public DbSet<Promocode> Promocode { get; set; }
        public DbSet<Booking> Booking { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    public class ContactController  
    {
        private readonly DatabaseContext _context;

        public ContactController(DatabaseContext context)
        {
            _context = context;
        }
    }
}