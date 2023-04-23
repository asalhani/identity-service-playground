using IdentityService.DataAccess.Abstract.Models;
using IdentityService.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Business.Abstract
{
    public interface IUserMessageManager
    {
        string SendUserMessage(UserMessageModel userMessage, UserMessageTemplateTypes templateType);
        UserMessageModel GetByToken(string token);
        void InvalidateToken(string token);
    }
}
