using System;
using System.ComponentModel.DataAnnotations;

namespace MvcAdvertizer.Data.Models
{
    public class UserAdvertsCounter
    {        
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public long Count { get; set; }
    }
}
