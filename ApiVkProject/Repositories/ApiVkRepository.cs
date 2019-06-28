using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiVkProject.Models;
using VkNet.Utils;

namespace ApiVkProject.Repositories
{
    public class ApiVkRepository
    {
        public ApiVkRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void UpdateUsers(VkCollection<VkNet.Model.User> subscribers)
        {
            foreach (User user in _context.Users)
            {
                if (!subscribers.Any(e => e.Id == user.Id))
                {
                    _context.Users.Remove(user);
                }
            }

            foreach (var subscriber in subscribers)
            {
                User user = _context.Users.Find(subscriber.Id);
                if (user == null)
                {
                    _context.Add(ConvertUser(subscriber));
                }
                else
                {
                    _context.Update(user);
                }
            }
        }

        private ApplicationContext _context;

        private User ConvertUser(VkNet.Model.User vkUser)
        {
            User user = new User
            {
                Id = vkUser.Id,
                FirstName = vkUser.FirstName,
                LastName = vkUser.LastName,
                Photo = vkUser.Photo100.ToString()
            };

            return user;
        }
    }
}
