using FullStackTest.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FullStackTest.Services.Base
{
    public abstract class BaseApplicationContextService<TService> : BaseService<TService>
       where TService : class
    {
        protected readonly ApplicationContext _context;

        public BaseApplicationContextService(
            IConfiguration configuration,
            ILogger<TService> logger,
            ApplicationContext context)
            : base(configuration, logger)
        {
            _context = context;
        }
    }
}
