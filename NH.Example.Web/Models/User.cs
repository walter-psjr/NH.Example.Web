using System;
using System.Collections.Generic;

namespace NH.Example.Web.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool Active { get; set; }
        public virtual IList<Role> Roles { get; set; }

        public User()
        {
            Roles = new List<Role>();
        }
    }
}