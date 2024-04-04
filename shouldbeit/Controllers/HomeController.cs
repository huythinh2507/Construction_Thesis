using Microsoft.AspNetCore.Mvc;
using Thesis_web.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thesis_web.Data;
using Thesis_web.Controllers;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Thesis_web.Controllers
{
    public class HomeController : Controller
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            ViewBag.Message = "security";
            ViewBag.MyFavoriteColor = "red";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AboutUs()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                TempData["NotLoggedIn"] = "Please log in to proceed!";
                TempData.Keep("NotLoggedIn");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                // Redirect authenticated users to the Dashboard page
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ContactUs()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                TempData["NotLoggedIn"] = "Please log in to proceed!";
                TempData.Keep("NotLoggedIn");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                // Redirect authenticated users to the Dashboard page
                return View();
            }
        }

        public async Task<IActionResult> Services()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);

            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                TempData["NotLoggedIn"] = "Please log in to proceed!";
                TempData.Keep("NotLoggedIn");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                // Redirect authenticated users to the Dashboard page
                return View();
            }
        }


        public async Task<IActionResult> CustomerSupport()
        {
            return View();
        }

        public async Task<IActionResult> FAQ()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Booking()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);

            var bookings = await context.Booking.ToListAsync();
            return View(bookings);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var booking = await context.Booking.FindAsync(id);
            context.Booking.Remove(booking);
            await context.SaveChangesAsync();
            return RedirectToAction("Booking", "Home");
        }
        public async Task SetViewBagCounts(DatabaseContext context)
        {
            ViewBag.BuildersCount = await context.Workers.CountAsync(w => w.Position == "Builder");
            ViewBag.ApplicantsCount = await context.Applicants.CountAsync();
            ViewBag.WorkersCount = await context.Workers.CountAsync();
            ViewBag.FeedbacksCount = await context.Entries.CountAsync();
            ViewBag.BookingCount = await context.Booking.CountAsync();
            ViewBag.UsersCount = await context.Signups.CountAsync();
            var email = _httpContextAccessor.HttpContext.Session.GetString("email");
            var role = _httpContextAccessor.HttpContext.Session.GetString("user_role");
            ViewBag.JobsCount = await context.Booking.
                CountAsync(w => w.User_email != email && w.Workers.Contains(role) && w.Status == "Pending⏳");
        }

        public IActionResult Error404() 
        {
            return View();
        }

        public async Task<IActionResult> Account()
		{

            return View();
		}
        [HttpGet]
        public async Task<IActionResult> Project() 
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                TempData["NotLoggedIn"] = "Please log in to proceed!";
                TempData.Keep("NotLoggedIn");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                // Redirect authenticated users to the Dashboard page
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Service_details() 
        {
           
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);

            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                TempData["NotLoggedIn"] = "Please log in to proceed!";
                TempData.Keep("NotLoggedIn");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                // Redirect authenticated users to the Dashboard page
                return View();
            }
        }

        public async Task<IActionResult> Terms_Of_Service() 
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Application()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                TempData["NotLoggedIn"] = "Please log in to proceed!";
                TempData.Keep("NotLoggedIn");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                // Redirect authenticated users to the Dashboard page
                return View();
            }

        }

        public async Task<IActionResult> Modify_profile()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            var bookings = await context.Signups.ToListAsync();
            return View(bookings);
        }

        public async Task<IActionResult> MyBooking()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            await SetViewBagCounts(context);
            var bookings = await context.Booking.ToListAsync();
            return View(bookings);
        }

        public IActionResult AddWorker() 
        {
            return View();
        }
		public IActionResult ForgotPw()
		{
			return View();
		}
		[HttpGet]
        public IActionResult Pricing() 
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                TempData["NotLoggedIn"] = "Please log in to proceed!";
                TempData.Keep("NotLoggedIn");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                // Redirect authenticated users to the Dashboard page
                return View();
            }
        }
        [HttpGet]
        public int GetBookingCount()
        {

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            // Replace this with your actual logic to get the booking count
            return context.Booking.Count();
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var applicants = await context.Signups.ToListAsync();
            await SetViewBagCounts(context);
            return View(applicants);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var booking = await context.Signups.FindAsync(id);
            context.Signups.Remove(booking);
            await context.SaveChangesAsync();
            return RedirectToAction("Users", "Home");
        }

        public ActionResult Approve(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var applicant = context.Booking.Find(id);
            if (applicant != null)
            {
                applicant.Status = "Approved ✅";
                context.SaveChanges();
            }
            return RedirectToAction("Booking", "Home");
        }

        public ActionResult Accept(Guid id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var jobs = context.Booking.Find(id);
            var role = _httpContextAccessor.HttpContext.Session.GetString("user_role");

            if (jobs != null)
            {
                if (role == "Builder" && jobs.requiredBuilders > 0)
                {
                    jobs.requiredBuilders--;
                }
                if (role == "Designer" && jobs.requiredDesigners > 0)
                {
                    jobs.requiredDesigners--;
                }
                if (role == "Operator" && jobs.requiredOperators > 0)
                {
                    jobs.requiredOperators--;
                }
                if (role == "Product Manager" && jobs.requiredPManagers > 0)
                {
                    jobs.requiredPManagers--;
                }
                if (jobs.requiredBuilders == 0 && jobs.requiredDesigners == 0
                    && jobs.requiredOperators == 0 && jobs.requiredPManagers == 0)
                {
                    jobs.Status = "Finish ✅";
                }
                else
                {
                    jobs.Status = "Enrolled ✅";
                }
                context.SaveChanges();
            }
            return RedirectToAction("Booking", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
