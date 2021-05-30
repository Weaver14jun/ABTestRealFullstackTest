using FullStackTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackTest.Services
{
    public interface IUsersSender
    {
        List<User> Send();
    }
}
