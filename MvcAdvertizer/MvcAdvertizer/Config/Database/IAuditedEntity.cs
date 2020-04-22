using System;

namespace MvcAdvertizer.Config.Database
{
    public interface IAuditedEntity
    {        
        public DateTime? CreatedOn { get; set; }        
        public DateTime? UpdatedOn { get; set; }
    }
}
