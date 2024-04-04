using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_web.Data;

namespace Thesis_web.Controllers
{
    public class GetWorkersController : Controller
    {
        private readonly DatabaseContext _context;

        public GetWorkersController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetWorkers(string location)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var workers = await context.Workers.Where(w => w.Location == location).ToListAsync();
            return Json(workers);
        }


    }
}
