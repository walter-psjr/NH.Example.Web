using NH.Example.Web.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace NH.Example.Web.Mappings
{
    public class UserMapping : ClassMapping<User>
    {
        public UserMapping()
        {
            Id(x => x.Id, map =>
            {
                map.Column("UserId");
                map.Generator(Generators.Guid);
            });

            Property(x => x.UserName);
            
            Property(x => x.Active);

            Bag(x => x.Phones, map =>
            {
                map.Key(map =>
                {
                    map.Column("UserId");
                    map.NotNullable(false);
                });
                map.Cascade(Cascade.All);
                //map.Fetch(CollectionFetchMode.Subselect);
                //map.Lazy(CollectionLazy.NoLazy);
                map.Inverse(true);
            }, rel =>
            {
                rel.OneToMany();
            });

            IdBag(x => x.Roles, map =>
            {
                map.Key(m => m.Column("RoleId"));
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
                    map.Column("UserId");
                });
            });
        }
    }
}