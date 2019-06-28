using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiVkProject.Models;

namespace ApiVkProject.Repositories
{
    public class GroupRepository
    {
        public GroupRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<User> GetMembers(int groupId)
        {
            List<User> users = new List<User>();
            foreach (GroupHasUser groupHasUser in _context.GroupHasUser.Where(gu => gu.GroupId == groupId))
            {
                users.Add(_context.Users.Find(groupHasUser.UserId));
            }
            return users;
        }

        public List<long> GetMemberIds(int groupId)
        {
            List<long> userIds = new List<long>();
            foreach (GroupHasUser groupHasUser in _context.GroupHasUser.Where(gu => gu.GroupId == groupId))
            {
                userIds.Add(groupHasUser.UserId);
            }
            return userIds;
        }

        public void UpdateGroupHasUser(int groupId, long[] selectedUsers)
        {
            foreach (GroupHasUser groupHasUser in _context.GroupHasUser.Where(gu => gu.GroupId == groupId))
            {
                if (!selectedUsers.Any(userId => userId == groupHasUser.UserId))
                {
                    _context.GroupHasUser.Remove(groupHasUser);
                }
            }

            foreach (long userId in selectedUsers)
            {
                GroupHasUser groupHasUser = _context.GroupHasUser.Find(groupId, userId);
                if (groupHasUser == null)
                {
                    _context.Add(new GroupHasUser { GroupId = groupId, UserId = userId });
                }
            }
        }

        private readonly ApplicationContext _context;
    }
}
