using System;

namespace NH.Example.Web.Models
{
    public class Phone
    {
        public virtual Guid Id { get; set; }
        public virtual string Number { get; set; }
    }
}