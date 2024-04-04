using Microsoft.AspNetCore.Mvc;
using Thesis_web.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thesis_web.Data;
namespace Thesis_web.Controllers
{
    public class WorkersController : Controller
	{
        public readonly IHttpContextAccessor httpContextAccessor;

        public WorkersController(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
        }


        public async Task SetViewBagCounts(DatabaseContext context)
        {
            ViewBag.ApplicantsCount = await context.Applicants.CountAsync();
            ViewBag.WorkersCount = await context.Workers.CountAsync();
            ViewBag.FeedbacksCount = await context.Entries.CountAsync();
            ViewBag.BookingCount = await context.Booking.CountAsync();
            ViewBag.UsersCount = await context.Signups.CountAsync();

        }

        [HttpGet]
        public async Task<IActionResult> Workers()
        {
            var userId = httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                TempData["NotLoggedIn"] = "Please log in to proceed!";
                TempData.Keep("NotLoggedIn");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
                using var context = new DatabaseContext(optionsBuilder.Options);
                var workers = await context.Workers.ToListAsync();
                await SetViewBagCounts(context);
                return View(workers);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Applicants()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var applicants = await context.Applicants.ToListAsync();
            await SetViewBagCounts(context);
            return View(applicants);
        }

        [HttpGet]
        public async Task<IActionResult> Feedback()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var feedbacks = await context.Entries.ToListAsync();
            await SetViewBagCounts(context);
            return View(feedbacks);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var booking = await context.Entries.FindAsync(id);
            context.Entries.Remove(booking);
            await context.SaveChangesAsync();
            return RedirectToAction("Feedback", "Workers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApplicant(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var booking = await context.Applicants.FindAsync(id);
            context.Applicants.Remove(booking);
            await context.SaveChangesAsync();
            return RedirectToAction("Applicants", "Workers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWorker(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var booking = await context.Workers.FindAsync(id);
            context.Workers.Remove(booking);
            await context.SaveChangesAsync();
            return RedirectToAction("Workers", "Workers");
        }


        public async Task<IActionResult> MyApp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            var apps = await context.Applicants.ToListAsync();
            return View(apps);
        }

    }
}
