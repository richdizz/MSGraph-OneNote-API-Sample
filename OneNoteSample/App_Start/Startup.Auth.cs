using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.IdentityModel.Claims;
using OneNoteSample.Utils;

namespace OneNoteSample
{
    public partial class Startup
    {
        public async Task ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = SettingsHelper.ClientId,
                    Authority = SettingsHelper.AzureADAuthority,
                    PostLogoutRedirectUri = SettingsHelper.LogoutUri,
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                    {
                        // instead of using the default validation (validating against a single issuer value, as we do in line of business apps),  
                        // we inject our own multitenant validation logic 
                        ValidateIssuer = false,
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        AuthorizationCodeReceived = async (context) =>
                        {
                            var code = context.Code;
                            ClientCredential credential = new ClientCredential(SettingsHelper.ClientId, SettingsHelper.ClientSecret);
                            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                            AuthenticationContext authContext = new AuthenticationContext(SettingsHelper.AzureADAuthority, null);
                            AuthenticationResult result = await authContext.AcquireTokenByAuthorizationCodeAsync(
                            code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, SettingsHelper.GraphResource);

                            //cache the token in session state
                            HttpContext.Current.Session[SettingsHelper.UserTokenCacheKey] = result;
                        }
                    }
                });
        }
    }
}
