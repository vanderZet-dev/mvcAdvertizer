using MvcAdvertizer.Config.Database;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcAdvertizer.Data.Models
{
    public class Advert : IAuditedEntity
    {
        [Key]
        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Content { get; set; }
        public byte[] Image { get; set; }

        public int Rate { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [NotMapped]
        public bool? Selected { get; set; } = false;
    }
}
