using NH.Example.Web.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NH.Example.Web.Mappings
{
    public class PhoneMapping : ClassMapping<Phone>
    {
        public PhoneMapping()
        {
            Id(x => x.Id, map =>
            {
                map.Column("PhoneId");
                map.Generator(Generators.Guid);
            });

            Property(x => x.Number);
        }
    }
}