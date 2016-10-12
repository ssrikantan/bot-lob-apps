using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsurancePolicyInfoBot.Models
{
    public class UserProfile
    {
        public string CustomerId;
        public string MicrosoftId;
        public string SlackId;
        public string Address;
        public string FirstName;
        public string LastName;
        public string Phone;
        public bool ProfileComplete;
    }
}