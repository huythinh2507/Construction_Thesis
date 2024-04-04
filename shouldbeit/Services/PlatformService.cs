using System.Collections.Generic;
using System.Linq;
using Thesis_web.Data;
using Thesis_web.Models;

namespace Thesis_web.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly DatabaseContext _context;

        public PlatformService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Workers> GetAll()
        {
            return _context.Workers.ToList();
        }  

        public void Add(Workers newWorker)
        {
            _context.Workers.Add(newWorker);
            _context.SaveChanges();
        }
    }
}