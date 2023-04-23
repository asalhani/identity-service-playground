using IdentityService.Business.Abstract;
using IdentityService.DataAccess.Abstract;
using IdentityService.DataAccess.Abstract.Models;
using IdentityService.Models.Configurations;
using IdentityService.Models.Dto;
using Microsoft.Extensions.Options;
using PlatformCommons.PlatformService.Abstractions.Dto;
using PlatformCommons.PlatformService.Abstractions.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Business
{
    public class UserMessageManager : IUserMessageManager
    {
        private readonly IUserMessageRepository _userMessageDA;
        private readonly IEmailNotification _emailNotification;
        private readonly ISmsNotification _smsNotification;
        private readonly AppSettings _settings;

        public UserMessageManager(IUserMessageRepository userMessageDA,
                                  IEmailNotification emailNotification,
                                  ISmsNotification smsNotification,
                                  IOptions<AppSettings> settings)
        {
            _userMessageDA = userMessageDA;
            _emailNotification = emailNotification;
            _smsNotification = smsNotification;
            _settings = settings.Value;
        }

        public UserMessageModel GetByToken(string token)
        {
            return _userMessageDA.GetByToken(token);
        }

        public string SendUserMessage(UserMessageModel userMessage, UserMessageTemplateTypes templateType)
        {
            userMessage.IsUsed = false;
            userMessage.IsCancelled = false;
            userMessage.InvitationToken = Guid.NewGuid().ToString();
            userMessage.CreatedBy = null;
            userMessage.CreatedDate = DateTime.Now;
            userMessage.UserMessageTypeId = (int)templateType;

            // add token to the Dictionary params so it can be repleaced on body
            userMessage.EmailParams.Add(EmailPlaceholders.Token, userMessage.InvitationToken);


            // check if user already has submmited a request to reset password, if yes, then mark all of them as used
            CancelUserPreviousMessage(userMessage.UserId);

            // add new record to [EmailInvitation] table
            _userMessageDA.Add(userMessage);
            var obj = new SendEmailInput()
            {
                ToAddress = userMessage.EmailParams[EmailPlaceholders.ReciverAddress],
                TemplateId = _settings.Notifications.Templates.EmailResetPassword,
                Params = userMessage.EmailParams
            };
            NotificationOutput result = new NotificationOutput();
            try
            {
                result = _emailNotification.SendEmail(obj).Result;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            if (result.result != "success")
                throw new Exception($"Unsuccessfult attempt to send an email: {result.result}");
#if DEBUG
            return FormatEmailBody(@"<rootSiteUrl>/<token>", userMessage.EmailParams);
#else
            return "";
#endif
        }

        public void InvalidateToken(string token)
        {
            var userMessage = _userMessageDA.GetByToken(token);
            userMessage.IsUsed = true;
            userMessage.UpdatedDate = DateTime.Now;
            _userMessageDA.Update(userMessage);
        }

        private void CancelUserPreviousMessage(string userId)
        {
            // get current invitations for user with the same inivitation type
            List<UserMessageModel> preInvitations = _userMessageDA.GetAllByUserId(userId);
            if (preInvitations != null)
            {
                preInvitations.ForEach(inv =>
                {
                    inv.IsCancelled = true;
                    inv.UpdatedBy = userId;
                    inv.UpdatedDate = DateTime.Now;
                    _userMessageDA.UpdateIsCancelled(inv);
                });
            }
        }

        private string FormatEmailBody(string emailBody, Dictionary<string, string> emailParams)
        {
            foreach (var param in emailParams)
            {
                emailBody = emailBody.Replace($"<{param.Key}>", param.Value);
            }
            return emailBody;
        }
    }
}
