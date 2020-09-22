using MvcAdvertizer.Config.Database;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcAdvertizer.Data.Models
{
    public class Advert : IAuditedEntity
    {
        [Key]      
        public Guid? Id { get; set; }        
        
        public int Number { get; set; }
        
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        
        public string Content { get; set; }

        public string ImageHash { get; set; }
        
        public int? Rate { get; set; }
        
        public DateTime PublishingDate { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [NotMapped]
        public bool? Selected { get; set; } = false;
    }
}
