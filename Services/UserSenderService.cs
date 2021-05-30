using FullStackTest.Models;
using FullStackTest.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FullStackTest.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FullStackTest.Services
{
    public class UserSenderService : BaseApplicationContextService<UserSenderService>
    {
        public UserSenderService(
            IConfiguration configuration,
            ILogger<UserSenderService> logger,
            ApplicationContext context)
             : base(configuration, logger, context)
        {

        }

        public async Task<List<User>> Send()
        {
            var tempUsers = await _context.Users.ToListAsync();
            foreach(var user in tempUsers)
            {
                user.RegistrationDate = user.RegistrationDate.Date;
                user.LastActivityDate = user.LastActivityDate.Date;
            }
            return tempUsers;
        }

        public async Task Save(List<UserViewModel> users)
        {
            var currentUsers = await _context.Users.ToListAsync();
            foreach(var user in currentUsers)
            {
                var tempUser = users.SingleOrDefault(x => x.UserId == user.UserId);
                if (tempUser != null)
                {
                    user.LastActivityDate = Convert.ToDateTime(tempUser.LastActivityDate);
                    user.RegistrationDate = Convert.ToDateTime(tempUser.RegistrationDate);
                }
            }

            await _context.SaveChangesAsync(true);
        }

        public async Task<double> CalculateRetention()
        {
            var tempUsers = await _context.Users.ToListAsync();
            var retendedUsers = tempUsers.Where(x => x.RegistrationDate.AddDays(7) <= x.LastActivityDate).ToList();
            var oldUsers = tempUsers.Where(x => x.RegistrationDate.AddDays(7) <= DateTime.Now);
            var result = (double)retendedUsers.Count() / (double)(oldUsers.Count())*100;
            return result;
        }

        public async Task<List<UserAlive>> GetUsersAlive()
        {
            List <UserAlive> usersAlive= new List<UserAlive>();
            var tempUsers = await _context.Users.ToListAsync();

            foreach(var user in tempUsers)
            {
                usersAlive.Add(new UserAlive
                {
                    UserId = user.UserId,
                    DaysAlive = Convert.ToInt32((user.LastActivityDate - user.RegistrationDate).TotalDays)
                });
            }

            return usersAlive;
        }
    }
}
