using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsurancePolicyInfoBot.Common
{
    public class ServiceParameters
    {
        public static string PolicyRequestsSearchServiceNs;// = "policyservices";
        public static string PolicyRequestsSearchServiceIndexName;// = "policydataindex";
        public static string PolicyRequestsSearchServiceApiKey;// = "F9070F2C467C1E395EC28D36E96E0794";


        public static string PolicyDocumentsSearchNs;// = "policyservices";
        public static string PolicyDocumentsSearchServiceIndexName;// = "policydocuments-index";
        public static string PolicyDocumentsSearchServiceApiKey;// = "F9070F2C467C1E395EC28D36E96E0794";


        public static string PolicyUserProfileSearchNs;// = "policyservices";
        public static string PolicyUserProfileSearchIndexName;// = "customerdataindex";
        public static string PolicyUserProfileSearchApiKey;// = "F9070F2C467C1E395EC28D36E96E0794";
    }
}