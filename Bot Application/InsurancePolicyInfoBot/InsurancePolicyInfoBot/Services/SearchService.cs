using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Threading.Tasks;
using InsurancePolicyInfoBot.Models;

namespace InsurancePolicyInfoBot.Services
{
    /// <summary>
    /// Implements the integration with Azure Search to surface Policy Application requests, Customer profile information and the Policy documents that are 
    /// generated
    /// </summary>
    public class SearchService
    {
        private string SearchNameSpace;
        private string SearchIndexName;
        private string SearchApiKey;
        public SearchService(string searchns, string searchindex, string apikey)
        {
            SearchNameSpace = searchns;
            SearchIndexName = searchindex;
            SearchApiKey = apikey;
        }


        public async Task<UserProfile> GetUserProfile(string command)
        {
            // Setting the Bot user's Microsoft Account in the request to retrieve the relevant records
            DocumentSearchResult<UserProfile> response = null;
            IList<SearchResult<UserProfile>> results = null;
            try
            {
                SearchServiceClient serviceClient = new SearchServiceClient(SearchNameSpace, new SearchCredentials(SearchApiKey));
                ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(SearchIndexName);

                // Execute search based on search text
                var sp = new SearchParameters();
                sp.QueryType = QueryType.Full;
                sp.SearchMode = SearchMode.All;
                sp.Top = 500;
                response = await indexClient.Documents.SearchAsync<UserProfile>(command, sp);
                results = response.Results;
            }
            catch (Exception)
            {
                return null;
            }
            if (results.Count == 0)
            {
                return null;
            }
            UserProfile userProfile = results[0].Document;
            return userProfile;
        }

        /// <summary>
        /// Retrieves the Policy Application requests for the Bot User
        /// </summary>
        /// <param name="command">Lucene query </param>
        /// <param name="filter">any filter criteria to be applied on the results</param>
        /// <returns></returns>
        public async Task<string> GetPolicyRequests(string command, string filter = null)
        {
            string searchResponse = string.Empty;
            int counter = 1;
            DocumentSearchResult<PolicyRequest> response = null;
            IList<SearchResult<PolicyRequest>> results = null;
            try
            {
                SearchServiceClient serviceClient = new SearchServiceClient(SearchNameSpace, new SearchCredentials(SearchApiKey));
                ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(SearchIndexName);

                // Execute search based on search text and optional filter
                var sp = new SearchParameters();
                if (!String.IsNullOrEmpty(filter))
                    sp.Filter = filter;
                sp.QueryType = QueryType.Full;
                sp.SearchMode = SearchMode.All;
                sp.Top = 500;
                response = await indexClient.Documents.SearchAsync<PolicyRequest>(command, sp);
                results = response.Results;
            }
            catch (Exception ex)
            {
                searchResponse = "Error retrieving Policy Requests information :" + ex.Message;
                return searchResponse;
            }
            if (results.Count == 0)
            {
                searchResponse = "I could not find any Policy Requests matching the criteria";
                return searchResponse;
            }
            searchResponse += "Found **" + results.Count + "** Policy Application Requests(s) in all ..\n\n";

            //Group the results by Insurance Policy type, using Linq
            IEnumerable<IGrouping<string, SearchResult<PolicyRequest>>> groupedAccounts = results.GroupBy(x => x.Document.PolicyType);
            foreach (IGrouping<string, SearchResult<PolicyRequest>> eachGroup in groupedAccounts)
            {
                counter = 1;
                searchResponse += "Here is/are the " + eachGroup.Count<SearchResult<PolicyRequest>>() + " " + eachGroup.Key + " Policy Request(s) \n\n";
                foreach (SearchResult<PolicyRequest> result in eachGroup)
                {
                    searchResponse += (counter) + ". **" + result.Document.PolicyRequestId + "**" +
                    " on the insured :**" + result.Document.InsuredIdentifier + "** for an amount of :**" + 
                    result.Document.Currency + " " + result.Document.AssessedValue + "** has status: **" + 
                    result.Document.PolicyStatus + "**\n\n";

                    counter++;
                }
            }
            return searchResponse;
        }

        /// <summary>
        /// Executes the Search for Policy Documents
        /// </summary>
        /// <param name="command"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<string> GetPolicyDocuments(string command, string filter = null)
        {
            string searchResponse = string.Empty;
            int counter = 1;
            DocumentSearchResult<PolicyDocument> response = null;
            IList<SearchResult<PolicyDocument>> results = null;
            try
            {
                SearchServiceClient serviceClient = new SearchServiceClient(SearchNameSpace, new SearchCredentials(SearchApiKey));
                ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(SearchIndexName);

                // Execute search based on search text and optional filter
                var sp = new SearchParameters();
                if (!String.IsNullOrEmpty(filter))
                    sp.Filter = filter;
                sp.QueryType = QueryType.Full;
                sp.SearchMode = SearchMode.All;
                sp.Top = 500;
                response = await indexClient.Documents.SearchAsync<PolicyDocument>(command, sp);
                results = response.Results;
            }
            catch (Exception ex)
            {
                searchResponse = "Error retrieving Policy Documents information :" + ex.Message;
                return searchResponse;
            }
            if (results.Count == 0)
            {
                searchResponse= "No Policy Documents found matching the criteria. Please review the criteria and resubmit";
                return searchResponse;
            }
            searchResponse += "Found **" + results.Count + "** Policy Document(s) matching the criteria..\n\n";

            foreach (SearchResult<PolicyDocument> result in results)
            {
                searchResponse += counter + ". Link: [" + result.Document.filename + "](" + result.Document.fileurl + ") \n";
                counter++;
            }
            return searchResponse;
        }
    }
}