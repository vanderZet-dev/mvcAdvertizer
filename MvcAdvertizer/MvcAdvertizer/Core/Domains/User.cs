using MvcAdvertizer.Config.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Core.Domains
{
    public class User : IAuditedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
