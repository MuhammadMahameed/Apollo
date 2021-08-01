using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Models;
using Microsoft.EntityFrameworkCore;

namespace Apollo.Services
{
    public class UserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }

        public ArrayList GetAllUsers()
        {
            ArrayList users = new ArrayList();

            foreach (User user in _context.User.ToList())
            {
                users.Add(new
                {
                    id = user.Id,
                    name = user.Username,
                    password = user.Password,
                    email = user.EmailAdress,
                    age = user.Age,
                    roleType = user.RoleType
                });
            }

            return users;
        }

        public ArrayList FilterUsers(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new ArrayList();
            }

            var strToLower = str.ToLower();

            var matchingUsers = _context.User
                .Where(s => s.Username.ToLower().Contains(strToLower) ||
                            s.Password.ToLower().Contains(strToLower) ||
                            s.EmailAdress.ToLower().Contains(strToLower) ||
                            s.Age.ToString().ToLower().Contains(strToLower) ||
                            (s.RoleType == UserType.Admin && "admin".Contains(strToLower)) ||
                            (s.RoleType == UserType.Client && "client".Contains(strToLower)))
                .ToList();

            ArrayList matchingUsersList = new ArrayList();

            foreach (User user in matchingUsers)
            {
                matchingUsersList.Add(new
                {
                    id = user.Id,
                    name = user.Username,
                    password = user.Password,
                    email = user.EmailAdress,
                    age = user.Age,
                    roleType = user.RoleType.ToString()
                });
            }

            return matchingUsersList;
        }
    }
}
