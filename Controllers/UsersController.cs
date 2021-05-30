using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullStackTest.Controllers.Base;
using FullStackTest.Models;
using FullStackTest.Services;
using FullStackTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace FullStackTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<UsersController>
    {
        private readonly ILogger<UsersController> _logger;
        protected readonly ApplicationContext _context;
        private readonly UserSenderService _userSenderService;

        public UsersController(
            UserSenderService userSenderService,
            IConfiguration config,
            ILogger<UsersController> logger
            )
            : base(config, logger)
        {
            _logger = logger;
            _userSenderService = userSenderService;
        }

        [Route("users")]
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userSenderService.Send();
        }

        [Route("userssave")]
        [HttpPost]
        public async Task PostUsers([FromBody] IEnumerable<UserViewModel> users)
        {
            await _userSenderService.Save(users.ToList());
        }

        [Route("userscalculate")]
        [HttpGet]
        public async Task<double> Calculate()
        {
            return await _userSenderService.CalculateRetention();
        }

        [Route("usersalive")]
        [HttpGet]
        public async Task<IEnumerable<UserAlive>> UsersAlive()
        {
            return await _userSenderService.GetUsersAlive();
        }
    }
}
