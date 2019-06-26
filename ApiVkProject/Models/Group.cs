using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVkProject.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<GroupHasUser> GroupHasUser { get; set; }
    }
}
