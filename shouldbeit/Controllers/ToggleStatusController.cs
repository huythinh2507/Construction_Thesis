using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Thesis_web.Data;

namespace Thesis_web.Controllers
{
    public class ToggleStatusController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id, string status)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var applicant = await context.Applicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }

            applicant.Status = status;
            context.Applicants.Update(applicant);
            await context.SaveChangesAsync();

            return Ok();
        }
      
    }
}
