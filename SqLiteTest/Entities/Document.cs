using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SqLiteTest.Entities
{
    public class Document
    {
        public virtual Guid Id { get; protected internal set; }
        public virtual string Name { get; set; }
        public virtual long ContentLength { get; set; }
        public virtual string ContentType { get; set; }
    }

    public class DocumentMap : ClassMap<Document>
    {
        public DocumentMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Name);
            Map(x => x.ContentLength);
            Map(x => x.ContentType);
        }
    }
}