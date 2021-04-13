using Microsoft.Identity.Client;
using RlsDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RlsDemo.Services
{
    public static class AadService
    {
        public static string GetAccessToken(AadConfiguration aadConfiguration)
        {
            IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder
                                                                            .Create(aadConfiguration.ClientId)
                                                                            .WithClientSecret(aadConfiguration.ClientSecret)
                                                                            .WithAuthority(aadConfiguration.AuthorityUri)
                                                                            .Build();
            AuthenticationResult authenticationResult = clientApp.AcquireTokenForClient(aadConfiguration.Scope).ExecuteAsync().Result;
            return authenticationResult.AccessToken;
        }
    }
}
