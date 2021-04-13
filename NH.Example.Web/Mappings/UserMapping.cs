using NH.Example.Web.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NH.Example.Web.Mappings
{
    public class UserMapping : ClassMapping<User>
    {
        public UserMapping()
        {
            Id(x => x.Id, map => map.Generator(Generators.Guid));

            Property(x => x.UserName);
            
            Property(x => x.Active);
        }
    }
}