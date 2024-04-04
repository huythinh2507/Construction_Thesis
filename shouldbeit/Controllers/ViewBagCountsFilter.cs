using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Thesis_web.Data;
using Microsoft.EntityFrameworkCore;

namespace Thesis_web.Controllers
{
    public class ViewBagCountsFilter : ActionFilterAttribute
    {
        public readonly DatabaseContext _context;

        public ViewBagCountsFilter(DatabaseContext context)
        {
            _context = context;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                controller.ViewBag.ApplicantsCount = await _context.Applicants.CountAsync();
                controller.ViewBag.WorkersCount = await _context.Workers.CountAsync();
                controller.ViewBag.FeedbacksCount = await _context.Entries.CountAsync();
                controller.ViewBag.BookingCount = await _context.Booking.CountAsync();

            }
            await next();
        }
    }
}
