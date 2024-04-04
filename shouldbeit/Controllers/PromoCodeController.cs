using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thesis_web.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Thesis_web.Controllers
{
    public class PromoCodeController : Controller
    {
        private readonly DatabaseContext _context;

        public PromoCodeController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> IsPromoCodeAvailable(string code)
        {
            return await _context.Promocode.AnyAsync(pc => pc.Code == code);
        }
    }
}
