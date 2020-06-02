using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Core.AdditionalObjects
{
    public class RepresentObjectConfigurator
    {
        public int? pageNumber = 1;
        [BindRequired]
        public int? PageNumber { get => pageNumber; set { if (value != null) pageNumber = value; } }

        public int? pageSize = 5;
        [BindRequired]
        public int? PageSize { get => pageSize; set { if (value != null) pageSize = value; } }
    }
}
