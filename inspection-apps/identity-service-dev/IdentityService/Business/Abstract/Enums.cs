using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Business.Abstract
{
    public enum UserMessageTemplateTypes : int
    {
        ForgotPassword = 1,
    }

    public enum EmailTemplateTypes : int
    {
        ForgotPasswordInvitation = 1,
    }

    public enum OtpRequestTypes
    {
        SignIn,
        ResetPassword
    }
}
