using IdentityServer4.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.IdentiryServerEvents
{
    public class OtpValidationFailure : Event
    {
        public OtpValidationFailure(string message) : base(
            "Invalid Operation" , 
            "Abnormal behavior for the 'OTP Verification' action.", 
            EventTypes.Failure, 
            9999, 
            message)
        {
        }
    }
}
