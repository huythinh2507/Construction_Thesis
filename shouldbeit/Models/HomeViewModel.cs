using Thesis_web.Data;

namespace Thesis_web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Workers> Workers { get; set; }
        public int BookingCount { get; set; }
    }
}
