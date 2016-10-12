using InsurancePolicyInfoBot.Common;
using InsurancePolicyInfoBot.Models;
using InsurancePolicyInfoBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InsurancePolicyInfoBot
{
    /// <summary>
    /// This class implements the Luis Dialog integration with the Bot Application. All user conversations are interpreted here and then a 
    /// subsequent call executed on Azure Search to surface the information
    /// </summary>
    [LuisModel("34302853-8cc7-4230-82bb-c2a16d0033a7", "84f8545836a449d0bb09b2aaf68ab417")]
    [Serializable]
    public class PolicyInfoDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            // Before any request is processed, obtain the identity of the Bot user
            if (UserIdentified(context))
            {
                string message = $"Sorry I did not understand. pl review and resend the message ";
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                CheckUserIdentity(context);
            }
        }

        /// <summary>
        /// Retrieves all the Policy requests in Azure Search for the user identified in the current Bot conversation
        /// The CustomerId is implictly passed in the query to ensure content of the current user alone is retrieved
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [LuisIntent("GetPolicyRequests")]
        public async Task GetPolicyRequests(IDialogContext context, LuisResult result)
        {
            if (UserIdentified(context))
            {
                string msftid = GetBotuserMicrosoftId(context);
                string command = string.Empty;
                //Set the Microsoft Account of the Bot user implicitly to return only the relevant records
                //command += "CustomerId:(Crm0101)";
                command += "CustomerId:(" + msftid + ")";
                string filter = string.Empty;
                SearchService searchService = new SearchService(ServiceParameters.PolicyRequestsSearchServiceNs,
                    ServiceParameters.PolicyRequestsSearchServiceIndexName, ServiceParameters.PolicyRequestsSearchServiceApiKey);
                string message = await searchService.GetPolicyRequests(command, filter);
                //string message = $"Get Policy requests invoked for "+msftid;
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                CheckUserIdentity(context);
            }
        }

        /// <summary>
        /// Retrieves all Policy Application requests, in Azure Search for the user identified in the current Bot conversation, that are pending approval
        /// The CustomerId is implictly passed in the query to ensure content of the current user alone is retrieved
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [LuisIntent("GetPendingPolicyRequests")]
        public async Task GetPendingPolicyRequests(IDialogContext context, LuisResult result)
        {
            if (UserIdentified(context))
            {
                //telemetry.TrackTrace("Interpreted a GetPendingPolicyRequests request from Bot User ..");
                string command = string.Empty;
                string msftid = GetBotuserMicrosoftId(context);
                command += "CustomerId:(" + msftid + ") -PolicyStatus:(Approved)";
                string filter = string.Empty;
                SearchService searchService = new SearchService(ServiceParameters.PolicyRequestsSearchServiceNs,
                    ServiceParameters.PolicyRequestsSearchServiceIndexName, ServiceParameters.PolicyRequestsSearchServiceApiKey);
                string message = await searchService.GetPolicyRequests(command, filter);

                //string message = $"Get Pending Policy Requests method is called " + msftid;
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                CheckUserIdentity(context);
            }
        }

        /// <summary>
        /// Retrieves the Policy Request details in Azure Search for a Policy request Id provided by the user in the current Bot conversation
        /// The CustomerId is implictly passed in the query to ensure content of the current user alone is retrieved
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [LuisIntent("GetPolicyRequestDetails")]
        public async Task GetPolicyRequestDetails(IDialogContext context, LuisResult result)
        {
            if (UserIdentified(context))
            {
                //telemetry.TrackTrace("Interpreted a GetPolicyRequestDetails request from Bot User ..");
                string msftid = GetBotuserMicrosoftId(context);
                string command = string.Empty;
                command += "CustomerId:(" + msftid + ") AND PolicyRequestId:(" + result.Entities[0].Entity + ")";
                string filter = string.Empty;
                SearchService searchService = new SearchService(ServiceParameters.PolicyRequestsSearchServiceNs,
                    ServiceParameters.PolicyRequestsSearchServiceIndexName, ServiceParameters.PolicyRequestsSearchServiceApiKey);
                string message = await searchService.GetPolicyRequests(command, filter);

                //string message = $"Get Policy Request details method is called " + msftid;
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                CheckUserIdentity(context);
            }
        }

        /// <summary>
        /// Retrieves Policy Application Request status for the Bot user based on the type of Insurance Policy, be it Vehicle, Home or Life Insurance
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [LuisIntent("GetPolicyRequestStatusByType")]
        public async Task GetPolicyRequestStatusByType(IDialogContext context, LuisResult result)
        {
            if (UserIdentified(context))
            {
                // telemetry.TrackTrace("Interpreted a GetPolicyRequestStatusByType request from Bot User ..");
                string msftid = GetBotuserMicrosoftId(context);
                string command = string.Empty;
                command += "CustomerId:(" + msftid + ") AND PolicyType:(\'" + result.Entities[0].Entity + " " + result.Entities[1].Entity + "\')";
                //command += "CustomerId:(Crm0101) AND PolicyType:('vehicle insurance')";
                string filter = string.Empty;
                SearchService searchService = new SearchService(ServiceParameters.PolicyRequestsSearchServiceNs,
                    ServiceParameters.PolicyRequestsSearchServiceIndexName, ServiceParameters.PolicyRequestsSearchServiceApiKey);
                string message = await searchService.GetPolicyRequests(command, filter);

                //string message = $"Get Policy Requests Status by Type method is called " + msftid;
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                CheckUserIdentity(context);
            }
        }


        /// <summary>
        /// Gets the Policy Documents issued to the Bot user, after approval of the requests, based on search criteria
        /// entered by the User. A keyword based search and a property based search is implemented
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [LuisIntent("GetPolicyDocument")]
        public async Task GetPolicyDocument(IDialogContext context, LuisResult result)
        {
            if (UserIdentified(context))
            {
                //telemetry.TrackTrace("Interpreted a GetPolicyDocument request from Bot User ..");
                string msftid = GetBotuserMicrosoftId(context);
                string command = string.Empty;
                string filter = string.Empty;
                command = SetDocumentSearchParameters(result);
                SearchService searchService = new SearchService(ServiceParameters.PolicyDocumentsSearchNs,
                    ServiceParameters.PolicyDocumentsSearchServiceIndexName, ServiceParameters.PolicyDocumentsSearchServiceApiKey);
                string message = await searchService.GetPolicyDocuments(command, filter);

                //string message = $"Get Policy Documents method is called " + msftid;
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                CheckUserIdentity(context);
            }
        }

        /// <summary>
        /// This action is called when the Bot user asks for a Site inspection visit to be scheduled for the Car, before the Insurance policy
        /// can be generated
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        [LuisIntent("ScheduleSiteVisit")]
        public async Task ScheduleInspection(IDialogContext context, LuisResult result)
        {
            if (UserIdentified(context))
            {
                PromptDialog.Confirm(context, ConfirmInspection, "Should I go ahead and schedule the inspection?", promptStyle: PromptStyle.None);
            }
            else
            {
                CheckUserIdentity(context);
            }
        }


        private bool UserIdentified(IDialogContext context)
        {
            IBotDataBag botstate = context.PrivateConversationData;
            bool flag = false;
            botstate.TryGetValue<bool>("ProfileComplete", out flag);
            return flag;
        }

        /// <summary>
        /// This is invoked once for the Bot user to identify who he/she is. Uses a Form based dialog to capture the information
        /// </summary>
        /// <param name="context"></param>
        private void CheckUserIdentity(IDialogContext context)
        {
                var form = new FormDialog<SigninForm>(
                        new SigninForm(),
                        SigninForm.BuildForm,
                        FormOptions.PromptInStart);
                context.Call<SigninForm>(form, SignUpComplete);
           
        }

        /// <summary>
        /// Action executed when the user provides the Microsoft Account that identified him/her on the Bot. It is used to
        /// capture the detailed User Profile from Azure Search, whcih is then stored in the Conversation state in the Bot
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task SignUpComplete(IDialogContext context, IAwaitable<SigninForm> result)
        {
            SigninForm profileIdentityForm = null;
            try
            {
                profileIdentityForm = await result;
            }
            catch (OperationCanceledException)
            {
            }

            if (profileIdentityForm == null)
            {
                await context.PostAsync("You have not provided your Microsoft Account. We really need to have this information before we get started.");
            }
            else
            {
                string message = string.Empty;
                context.PrivateConversationData.SetValue<string>("MicrosoftAccount", profileIdentityForm.MicrosoftAccount);
                UserProfile profile = await LoadBotUserState(profileIdentityForm.MicrosoftAccount);
                if (profile == null)
                {
                    message = $"Sorry, we could not locate your profile in our system. Please check and try again with the right Microsoft Account";
                }
                else
                {
                    context.PrivateConversationData.SetValue<bool>("ProfileComplete", true);
                    context.PrivateConversationData.SetValue<string>("FirstName", profile.FirstName);
                    context.PrivateConversationData.SetValue<string>("LastName", profile.LastName);
                    context.PrivateConversationData.SetValue<string>("Address", profile.Address);
                    context.PrivateConversationData.SetValue<string>("Phone", profile.Phone);
                    context.PrivateConversationData.SetValue<string>("CustomerId", profile.CustomerId);
                    message = $"Thanks {profile.LastName},{profile.FirstName} for identifying yourself! \n\n Let me know how I could assist you.";
                }
                await context.PostAsync(message);
            }

            context.Wait(MessageReceived);
        }

        /// <summary>
        /// An acknowledgement message that is sent to the Bot user for the Site inspection visit. It retrieves the Address and contact
        /// information from the conversation state
        /// </summary>
        /// <param name="context"></param>
        /// <param name="confirmation"></param>
        /// <returns></returns>
        public async Task ConfirmInspection(IDialogContext context, IAwaitable<bool> confirmation)
        {
            if (await confirmation)
            {
                UserProfile profile = GetBotuser(context);
                await context.PostAsync($"Ok, Site inspection scheduled for " +DateTime.Now.AddDays(1).Date.ToShortDateString() +" at 11:00 AM. The address we have is:\n"+
                    profile.Address+" , and your contact number as per our records is : "+profile.Phone);
            }
            else
            {
                await context.PostAsync("Ok! I did not schedule the site inspection visit. Let me know when you are ready for it!");
            }

            context.Wait(MessageReceived);
        }


        private string GetBotuserMicrosoftId(IDialogContext context)
        {
            IBotDataBag databag = context.PrivateConversationData;
            return databag.Get<string>("CustomerId");
        }

        private static async Task<UserProfile> LoadBotUserState(string microsoftId)
        {
            SearchService search = new SearchService(ServiceParameters.PolicyUserProfileSearchNs,
            ServiceParameters.PolicyUserProfileSearchIndexName, ServiceParameters.PolicyUserProfileSearchApiKey);
            string command = "MicrosoftId:(" + microsoftId + ")";
            return await search.GetUserProfile(command);

        }

        /// <summary>
        /// Create the User Profile object based on the property values stored in the Bot conversation state for this user
        /// to be used when scheduling a Site inspection visit, for e.g.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private UserProfile GetBotuser(IDialogContext context)
        {
            IBotDataBag databag = context.PrivateConversationData;
            UserProfile profile = new UserProfile();
            try
            {
                profile.Address = databag.Get<string>("Address");
                profile.Phone = databag.Get<string>("Phone");
                profile.FirstName = databag.Get<string>("FirstName");
                profile.LastName= databag.Get<string>("FirstName");
                profile.MicrosoftId = databag.Get<string>("MicrosoftAccount");
            }
            catch (Exception ex)
            {
               // telemetry.TrackTrace("Exception getting Bot state in Dialog Class .." + ex.StackTrace);
                return null;
            }
            return profile;
        }

        /// <summary>
        /// Used to perform a property based search for documents in Azure Search, along with Full text search within the documents
        /// using the Lucene Syntax
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string SetDocumentSearchParameters(LuisResult request)
        {
            string command = string.Empty;
            foreach (EntityRecommendation curEntity in request.Entities)
            {
                switch (curEntity.Type)
                {
                    case "PolicyNumber":
                        {
                            command += " filename:(" + request.Entities[0].Entity + ") ";
                            break;
                        }
                    case "keyword":
                        {
                            command += " " + request.Entities[0].Entity + " ";
                            break;
                        }
                    case "builtin.datetime.date":
                        {
                            char[] delimiterChars = { '|', ',', '.', ':', '\t' };
                            //command += " lastmoddate:(" + request.Entities[0].Resolution + ") ";
                            List<string> allvalues = curEntity.Resolution.Values.ToList<string>();
                            string val = allvalues[0];
                            string[] vals = val.Split(delimiterChars);
                            command += " lastmoddate:(" + vals[0] + ") ";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return command;
        }
    }
}