using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SqLiteTest.Entities
{
    public class User
    {
        public virtual Guid Id { get; protected internal set; }
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Name).Length(50).Not.Nullable();
            Map(x => x.Password).Length(50).Not.Nullable();
        }
    }
}