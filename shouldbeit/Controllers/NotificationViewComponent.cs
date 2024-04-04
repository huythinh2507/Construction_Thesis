using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_web.Data;

namespace Thesis_web.Controllers
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly DatabaseContext _context;

        public NotificationViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var counts = new CountsModel
            {
                ApplicantsCount = await _context.Applicants.CountAsync(),
                WorkersCount = await _context.Workers.CountAsync(),
                FeedbacksCount = await _context.Entries.CountAsync(),
                BookingCount = await _context.Booking.CountAsync()
            };

            return View(counts);
        }
    }

}
