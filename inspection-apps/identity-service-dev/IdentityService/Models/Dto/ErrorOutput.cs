using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public class ErrorOutput : ApiResultBase
    {
        public new bool Success { get; } = false;
        public Guid ErrorId { get; set; }
        public string Message { get; set; }
    }
}
