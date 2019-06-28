using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ApiVkProject.Models;
using ApiVkProject.Repositories;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using VkNet.Exception;

namespace ApiVkProject.Controllers
{
    public class ApiVkController : ControllerBase
    {
        public ApiVkController(ApplicationContext context, IConfiguration config)
        {
            AppConfiguration = config;
            _context = context;
            _apiVkRepository = new ApiVkRepository(_context);

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

            _apiVkRepository.UpdateUsers(subscribers);

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
            foreach (GroupHasUser groupHasUser in _context.GroupHasUser.Where(gu => gu.GroupId == id))
            {
                try
                {
                    _vkApi.Messages.Send(new MessagesSendParams
                    {
                        UserId = groupHasUser.UserId,
                        Message = message
                    });
                }
                catch (CannotSendToUserFirstlyException) {}
            }
        }

        private readonly ApplicationContext _context;
        private VkApi _vkApi;
        private ApiVkRepository _apiVkRepository;
    }
}