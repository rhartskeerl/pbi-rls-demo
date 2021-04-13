using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using RlsDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RlsDemo.Services
{
    public class PbiService
    {
        private readonly AadConfiguration aadConfiguration;
        private readonly string urlPowerBiServiceApiRoot = "https://api.powerbi.com";

        public PbiService(AadConfiguration aadConfiguration)
        {
            this.aadConfiguration = aadConfiguration;
        }

        public PowerBIClient GetPowerBIClient()
        {
            var tokenCredentials = new TokenCredentials(AadService.GetAccessToken(aadConfiguration), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }

        public EmbedParams GetEmbedParams(Guid workspaceId, Guid reportId, [Optional] Guid additionalDatasetId)
        {
            PowerBIClient pbiClient = this.GetPowerBIClient();
            var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);
            var datasetIds = new List<Guid>
            {
                Guid.Parse(pbiReport.DatasetId)
            };
            if (additionalDatasetId != Guid.Empty)
            {
                datasetIds.Add(additionalDatasetId);
            }

            var embedReports = new List<EmbedReport>() {
                new EmbedReport
                {
                    ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl
                }
            };

            var embedToken = GetEmbedToken(reportId, datasetIds, workspaceId);

            var embedParams = new EmbedParams
            {
                EmbedReport = embedReports,
                Type = "Report",
                EmbedToken = embedToken
            };
            
            return embedParams;
        }

        public EmbedToken GetEmbedToken(Guid reportId, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            PowerBIClient pbiClient = this.GetPowerBIClient();
            var tokenRequest = new GenerateTokenRequestV2(

                reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },

                datasets: datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList(),

                targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null,
                identities: new List<EffectiveIdentity>() 
                { 
                    new EffectiveIdentity(
                        username: "test.2@demo.db", 
                        roles: new List<string>() { "SalesPerson"},
                        datasets: datasetIds.Select(datasetId => datasetId.ToString()).ToList()) 
                }   
            );
            var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

            return embedToken;
        }
    }
}
