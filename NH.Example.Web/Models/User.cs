using System;

namespace NH.Example.Web.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool Active { get; set; }
    }
}