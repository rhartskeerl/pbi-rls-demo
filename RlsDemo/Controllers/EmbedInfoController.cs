using Microsoft.AspNetCore.Mvc;
using RlsDemo.Models;
using RlsDemo.Services;
using System;
using System.Text.Json;

namespace RlsDemo.Controllers
{
    public class EmbedInfoController : Controller
    {
        private readonly PbiService pbiService;

        public EmbedInfoController()
        {
            AadConfiguration aadConfiguration = new AadConfiguration
            {
                AuthorityUri = Globals.AUTHORITY_URI,
                ClientId = Globals.APPLICATION_ID,
                ClientSecret = Globals.SECRET,
                Scope = new string[] { "https://analysis.windows.net/powerbi/api/.default" }
            };
            pbiService = new PbiService(aadConfiguration);
        }

        [HttpGet]
        public string GetEmbedInfo()
        {
            EmbedParams embedParams = pbiService.GetEmbedParams(new Guid(Globals.WORKSPACE_ID), new Guid(Globals.REPORT_ID));
            return JsonSerializer.Serialize<EmbedParams>(embedParams);
        }
    }
}
