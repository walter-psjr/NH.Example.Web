using NH.Example.Web.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NH.Example.Web.Mappings
{
    public class RoleMapping : ClassMapping<Role>
    {
        public RoleMapping()
        {
            Id(x => x.Id, map =>
            {
                map.Column("RoleId");
                map.Generator(Generators.Guid);
            });

            Property(x => x.Name);

            IdBag(x => x.Users, map =>
            {
                map.Key(m => m.Column("UserId"));
                map.Table("UserRole");
                map.Id(m =>
                {
                    m.Column("Sequence");
                    m.Generator(Generators.Identity);
                });
            }, rel =>
            {
                rel.ManyToMany(map =>
                {
                    map.Column("RoleId");
                });
            });
        }
    }
}