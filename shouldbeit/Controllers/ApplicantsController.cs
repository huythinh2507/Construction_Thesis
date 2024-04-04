using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_web.Data;

namespace Thesis_web.Controllers
{
    public class ApplicantsController : Controller
	{
        [HttpGet]
        public async Task<IActionResult> Applicants()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var workers = await context.Workers.ToListAsync();
            var applicants = await context.Applicants.ToListAsync();

            var viewModel = new ViewModel
            {
                Applicants = applicants,
                Workers = workers
            };
            return View(viewModel);
        }
        public ActionResult Approve(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var applicant = context.Applicants.Find(id);
            if (applicant != null)
            {
                applicant.Status = "Approved ✅";
                context.SaveChanges();
            }
            return RedirectToAction("Applicants", "Workers");
        }

    }
}
