using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVkProject.Models
{
    public class GroupHasUser
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
