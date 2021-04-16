using System;
using System.Collections.Generic;

namespace NH.Example.Web.Models
{
    public class Role
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}