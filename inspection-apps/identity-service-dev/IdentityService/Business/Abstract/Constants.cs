using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Business.Abstract
{
    public struct EmailPlaceholders
    {
        public const string ReciverAddress = "ReciverAddress";
        public const string Token = "token";
        public const string RootSiteUrl = "rootSiteUrl";
        public const string UserNinLastFourDigits = "userNinLast4Digit";
        public const string AssociationName = "associationName";
        public const string OrganizationName = "orgName";
        public const string DelegatedNinLast4Digit = "delegatedNinLast4Digit";
        public const string LicenseNumber = "LicenseNumber";
        public const string NumberOfDays = "NumberOfDays";
        public const string RequestId = "requestId";
        public const string RequestTitle = "requestTitle";
        public const string InvoiceNumber = "invoiceNumber";
    }

    public struct Members
    {
        public const string InspectionCenterClaim = "inspection_center_id";
    }
}
