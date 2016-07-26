using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OneNoteSample.Utils
{
    public class SettingsHelper
    {
        public static string UserTokenCacheKey
        {
            get { return "USER_TOKEN"; }
        }

        public static string ClientId
        {
            get { return ConfigurationManager.AppSettings["ida:ClientId"]; }
        }

        public static string ClientSecret
        {
            get { return ConfigurationManager.AppSettings["ida:ClientSecret"]; }
        }

        public static string AzureAdTenant
        {
            get { return ConfigurationManager.AppSettings["ida:Tenant"]; }
        }

        public static string LogoutUri
        {
            get { return ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"]; }
        }

        public static string GraphResource
        {
            get { return "https://graph.microsoft.com/"; }
        }

        public static string AzureADAuthority
        {
            get { return string.Format("https://login.microsoftonline.com/{0}/", AzureAdTenant); }
        }

        public static string ClaimTypeObjectIdentifier
        {
            get { return "http://schemas.microsoft.com/identity/claims/objectidentifier"; }
        }
    }
}