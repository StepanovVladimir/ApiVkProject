using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ApiVkProject.Models;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace ApiVkProject.Controllers
{
    public class ApiVkController : ControllerBase
    {
        public ApiVkController(ApplicationContext context, IConfiguration config)
        {
            AppConfiguration = config;
            _context = context;
            _vkApi = new VkApi();
            _vkApi.Authorize(new VkNet.Model.ApiAuthParams
            {
                AccessToken = AppConfiguration["VkToken"]
            });
        }

        public IConfiguration AppConfiguration { get; set; }

        [HttpPost]
        public IActionResult UpdateUsers()
        {
            var subscribers = _vkApi.Groups.GetMembers(new GroupsGetMembersParams
            {
                GroupId = AppConfiguration["VkGroupId"],
                Fields = UsersFields.Photo100
            });

            foreach (var user in _context.Users)
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

            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        [HttpPost]
        public void SendMessageToUser(long id, string message)
        {
            _vkApi.Messages.Send(new MessagesSendParams
            {
                UserId = id,
                Message = message
            });
        }

        [HttpPost]
        public void SendMessageToGroup(int id, string message)
        {
            List<long> userIds = new List<long>();
            foreach (GroupHasUser groupHasUser in _context.GroupHasUser.Where(gu => gu.GroupId == id))
            {
                userIds.Add(groupHasUser.UserId);
            }

            _vkApi.Messages.Send(new MessagesSendParams
            {
                UserIds = userIds,
                Message = message
            });
        }

        private readonly ApplicationContext _context;
        private VkApi _vkApi;

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