using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using Thesis_web.Models;
using Google.Apis.Auth.OAuth2;
using MimeKit;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Thesis_web.Data;
using Microsoft.Extensions.Options;
using AspNetCoreHero.ToastNotification.Abstractions;
using Google.Apis.Auth;

namespace Thesis_web.Controllers
{
    public class ContactController : Controller
    {
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ContactController(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		[HttpPost]
        public async Task<IActionResult> SubmitContactForm(Entries entry)
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
				var newEntry = new Entries
            {
                Id = Guid.NewGuid(),
                Name = entry.Name,
                Email = entry.Email,
                Message = entry.Message
            };

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);

            // Add the entry to the database
            context.Entries.Add(newEntry);
            await context.SaveChangesAsync();

            TempData["Success"] = "Thank you for reaching out to us! Your message has been successfully submitted. We appreciate your interest and will get back to you as soon as possible. Please note that our response time may vary, but we typically respond within 24-48 hours. We look forward to assisting you!";
            TempData.Keep("Success");
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		public async Task<IActionResult> SubmitSignupForm(Signups signups)
		{
			if (!ModelState.IsValid)
			{
				// Handle validation errors
			}
			var newSignup = new Signups
			{
				Id = Guid.NewGuid(),
				Name = signups.Name,
				Email = signups.Email,
				Password = signups.Password,
                Role = signups.Role,
				AnswerField = signups.AnswerField,
                Status = "Available",
			};

			var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
			optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
			using var context = new DatabaseContext(optionsBuilder.Options);
            var existingUser = await context.Signups.FirstOrDefaultAsync(u => u.Email == newSignup.Email);
            if (existingUser != null)
            {
                // User with the same email already exists
                TempData["DuplicateEmail"] = "This email already exists, please log in instead!";
                TempData.Keep("DuplicateEmail");
                return RedirectToAction("Register", "Account");
            }
            else
            {
                context.Signups.Add(newSignup);
                await context.SaveChangesAsync();

                TempData["Success"] = "You have signed up successfully!";
                TempData.Keep("Success");
                return RedirectToAction("Register", "Account");
            }
        }

        [HttpPost]
		public async Task<IActionResult> SubmitSigninForm(string Email, string Password, bool rememberMe)
		{
			if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
			{
				// Handle validation errors
				return View(); // Return to the same view to display validation errors
			}

			var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
			optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
			using var context = new DatabaseContext(optionsBuilder.Options);

			// Find the user with the submitted email
			var user = await context.Signups.FirstOrDefaultAsync(u => u.Email == Email);

			if (user == null)
			{
                // Handle case when user with submitted email doesn't exist
                TempData["NoEmail"] = "This email does not exist.";
                TempData.Keep("NoEmail");
                return RedirectToAction("Register", "Account");
			}
			else if (user.Password != Password)
			{
                // Handle case when password is incorrect

                TempData["Wrongpw"] = "Wrong password, please enter again!";
                TempData.Keep("Wrongpw");
                return RedirectToAction("Register", "Account");
			}
			else
			{
				// Sign in successful, start a new session
				HttpContext.Session.SetString("user_id", user.Id.ToString());
				HttpContext.Session.SetString("username", user.Name);
                HttpContext.Session.SetString("user_role", user.Role);
                HttpContext.Session.SetString("email", user.Email);
                HttpContext.Session.SetString("password", user.Password);
				HttpContext.Session.SetString("secretanswer", user.AnswerField);


				// If "Remember Me" is checked, create a persistent cookie
				if (rememberMe)
				{
					HttpContext.Response.Cookies.Append("user_id", user.Id.ToString(), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
					HttpContext.Response.Cookies.Append("username", user.Name, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
				}
                // Redirect to a different page
                return RedirectToAction("Index", "Home");
			}
		}
		[HttpPost]
		public async Task<IActionResult> GoogleLogin(string idToken)
		{
			// Validate the ID token and retrieve the user's Google email address
			var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
			var email = payload.Email;
			var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
			optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
			using var context = new DatabaseContext(optionsBuilder.Options);
			// Find the user in your database
			var user = await context.Signups.FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
			{
				// Handle case when user with submitted email doesn't exist
				TempData["NoEmail"] = "This email does not exist.";
				TempData.Keep("NoEmail");
				return RedirectToAction("Register", "Account");
			}
			else
			{
				// Sign in successful, start a new session
				HttpContext.Session.SetString("user_id", user.Id.ToString());
				HttpContext.Session.SetString("username", user.Name);
				HttpContext.Session.SetString("user_role", user.Role);

				// Redirect to a different page
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
        public async Task<IActionResult> SubmitWorkerData(Workers x)
        {
            if (!ModelState.IsValid)
            {
                // Handle validation errors
            }
            var newWorker = new Workers
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                Position = x.Position,
                Description = x.Description,
				ImageLink = x.ImageLink,
                Phone = x.Phone,
				Location = x.Location,
				Skills = x.Skills
            };

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);

            // Add the entry to the database
            context.Workers.Add(newWorker);
            await context.SaveChangesAsync();

            TempData["Success"] = "Worker added successfully!";
            TempData.Keep("Success");

            // Redirect back to the form action
            return RedirectToAction("AddWorker", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApplication(Applicants x)
        {
            var userEmail = _httpContextAccessor.HttpContext.Session.GetString("email");

            if (!ModelState.IsValid)
            {
                // Handle validation errors
            }
            var newSignup = new Applicants
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                Position = x.Position,
                Phone = x.Phone,
                Description = x.Description,
                ImageLink = x.ImageLink,
                User_email = userEmail,

                Status = "Pending ⏳"
            };

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);

            // Add the entry to the database
            context.Applicants.Add(newSignup);
            await context.SaveChangesAsync();

            TempData["Success"] = "Thank you for applying to us, we will let you know the result as soon as possible!";
            TempData.Keep("Success");

            // Redirect back to the form action
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> BookingRequest(Booking x)
        {
            var userEmail = _httpContextAccessor.HttpContext.Session.GetString("email");
            if (!ModelState.IsValid)
            {
                // Handle validation errors
            }
            var serviceWorkers = new Dictionary<string, Dictionary<string, int>>
{
    { "Remodeling", new Dictionary<string, int> { { "Builder", 1 }, { "Designer", 1 }, { "Operator", 1 } } },
    { "Construction", new Dictionary<string, int> { { "Builder", 2 }, { "Operator", 1 } } },
    { "Repairs", new Dictionary<string, int> { { "Builder", 1 }, { "Operator", 1 } } },
    { "Design", new Dictionary<string, int> { { "Designer", 1 }, { "Product Manager", 1 }, { "Operator", 1 } } }
};
            var newBooking = new Booking
            {
                Id = Guid.NewGuid(),
                ServiceType = x.ServiceType,
                Location = x.Location,
                Workers = x.Workers,
                WorkingTime = x.WorkingTime,
                StartTime = x.StartTime,
                Phone = x.Phone,
                Note = string.IsNullOrEmpty(x.Note) ? "no" : x.Note,
                PromoCode = string.IsNullOrEmpty(x.PromoCode) ? "no" : x.PromoCode,
                WorkDate = x.WorkDate,
                TotalPrice = x.TotalPrice,
                User_email = userEmail,
                Status = "Pending ⏳",
                SubmittedDate = DateTime.Now,
                requiredBuilders = serviceWorkers[x.ServiceType].ContainsKey("Builder") ? serviceWorkers[x.ServiceType]["Builder"] : 0,
                requiredDesigners = serviceWorkers[x.ServiceType].ContainsKey("Designer") ? serviceWorkers[x.ServiceType]["Designer"] : 0,
                requiredOperators = serviceWorkers[x.ServiceType].ContainsKey("Operator") ? serviceWorkers[x.ServiceType]["Operator"] : 0,
                requiredPManagers = serviceWorkers[x.ServiceType].ContainsKey("Product Manager") ? serviceWorkers[x.ServiceType]["Product Manager"] : 0,

            };
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);

            // Add the entry to the database
            context.Booking.Add(newBooking);
            await context.SaveChangesAsync();

            TempData["Success"] = "Thank you for your booking! If you have any questions or need to make changes to your booking, please do not hesitate to contact us.";
            TempData.Keep("Success");

            // Redirect back to the form action
            return RedirectToAction("Index", "Home");
        }

        

        [HttpPost]
        public async Task<IActionResult> ChangeName(string oldName, string newName, string password)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);

            var user = await context.Signups.SingleOrDefaultAsync(u => u.Name == oldName);

            if (user == null)
            {
                TempData["Fail"] = "User not found.";
                TempData.Keep("Fail");
                return RedirectToAction("Index", "Home");
            }

            if (user.Password != password)
            {
                TempData["Fail"] = "Incorrect password.";
                TempData.Keep("Fail");
                return RedirectToAction("Modify_profile", "Home");
            }

            user.Name = newName;
            await context.SaveChangesAsync();

            TempData["Success"] = "Your name has been changed successfully!";
            TempData.Keep("Success");
            return RedirectToAction("Modify_profile", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string username, string oldPw, string newPw)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);
            var user = await context.Signups.SingleOrDefaultAsync(u => u.Name == username);


            if (user == null)
            {
                TempData["Fail"] = "User not found.";
                TempData.Keep("Fail");
                return RedirectToAction("Index", "Home");
            }

            if (user.Password != oldPw)
            {
                TempData["Fail"] = "Incorrect password.";
                TempData.Keep("Fail");
                return RedirectToAction("Modify_profile", "Home");
            }

            user.Password = newPw;
            await context.SaveChangesAsync();

            TempData["Success"] = "Your password has been changed successfully!";
            TempData.Keep("Success");
            return RedirectToAction("Modify_profile", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string username ,string secretAnswer, string newPw)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Thesis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            using var context = new DatabaseContext(optionsBuilder.Options);

            var user = await context.Signups.SingleOrDefaultAsync(u => u.Name == username);


            if (user.AnswerField != secretAnswer)
            {
                TempData["Fail"] = "Incorrect secret answer.";
                TempData.Keep("Fail");
                return RedirectToAction("Modify_profile", "Home");
            }

            user.Password = newPw;
            await context.SaveChangesAsync();

            TempData["Success"] = "Your password has been restored successfully!";
            TempData.Keep("Success");
            return RedirectToAction("Register", "Account");
        }



        public IActionResult SuccessfullyAddedWorker()
		{ 
			return View(); 
		}
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SuccessfulSignUp() 
        {
            return View();
        }

		public IActionResult SuccessfulSignIn() 
		{
			return View();		
		}
    }
}