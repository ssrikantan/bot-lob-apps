using InsurancePolicyInfoBot.Common;
using InsurancePolicyInfoBot.Models;
using InsurancePolicyInfoBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InsurancePolicyInfoBot
{
    [Serializable]
    public class SigninForm
    {
        [Prompt("What is Microsoft Account with which you registered on our web site? {||}")]
        public string MicrosoftAccount { get; set; }

        public static IForm<SigninForm> BuildForm()
        {
            return new FormBuilder<SigninForm>()
                .Message("I would need some information before we get started.")
                .Field(nameof(MicrosoftAccount))
                .Build();
        }


        private static async Task<UserProfile> LoadBotUserState(string microsoftId)
        {
            SearchService search = new SearchService(ServiceParameters.PolicyUserProfileSearchNs,
            ServiceParameters.PolicyUserProfileSearchIndexName, ServiceParameters.PolicyUserProfileSearchApiKey);
            return await search.GetUserProfile(microsoftId);

        }
    }
}