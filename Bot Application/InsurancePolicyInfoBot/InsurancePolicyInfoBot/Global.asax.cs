using InsurancePolicyInfoBot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace InsurancePolicyInfoBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            initializeServiceParameters();
        }



        private void initializeServiceParameters()
        {
            foreach (string key in System.Configuration.ConfigurationManager.AppSettings.Keys)
            {
                switch (key)
                {
                    case "SearchPolicyRequestsNamespace":
                        {
                            ServiceParameters.PolicyRequestsSearchServiceNs = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    case "SearchPolicyDocumentsNamespace":
                        {
                            ServiceParameters.PolicyDocumentsSearchNs = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    case "SearchUserProfileNamespace":
                        {
                            ServiceParameters.PolicyUserProfileSearchNs = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    case "SearchPolicyRequestsApiKey":
                        {
                            ServiceParameters.PolicyRequestsSearchServiceApiKey = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    case "SearchPolicyDocumentsApiKey":
                        {
                            ServiceParameters.PolicyDocumentsSearchServiceApiKey = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    case "SearchUserProfileApiKey":
                        {
                            ServiceParameters.PolicyUserProfileSearchApiKey = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    case "SearchPolicyRequestsIndex":
                        {
                            ServiceParameters.PolicyRequestsSearchServiceIndexName = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    case "SearchPolicyDocumentsIndex":
                        {
                            ServiceParameters.PolicyDocumentsSearchServiceIndexName = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    case "SearchUserProfileIndex":
                        {
                            ServiceParameters.PolicyUserProfileSearchIndexName = System.Configuration.ConfigurationManager.AppSettings[key];
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            }
            //telemetry.TrackTrace("** Application Parameters - The Lui Service url: " + parameters.Luiserviceurl+ " , Index Name for Blueprints "+
            //     parameters.Blueprintindexname+", Index Name for Azure Accounts "+ parameters.Azureaccountsindexname+
            //     " , Service Name space for Blueprints "+parameters.Searchservicenamespacedocs+" , Service namespace for Azure Accounts "+parameters.Searchservicenamespace+
            //     " , API Key for Search Service Blueprints "+parameters.Searchservicedocskey+" , API Key for Search Azure Accounts "+parameters.Searchservicekey);

        }
    }
}
