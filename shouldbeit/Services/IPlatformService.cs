using System.Collections.Generic;
using Thesis_web.Data;
using Thesis_web.Models;
namespace Thesis_web.Services
{
    public interface IPlatformService
    {
        IEnumerable<Workers> GetAll();
        void Add(Workers newWorker);
    }
}

