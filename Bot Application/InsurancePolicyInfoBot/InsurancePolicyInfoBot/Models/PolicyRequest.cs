using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsurancePolicyInfoBot.Models
{
    public class PolicyRequest
    {
        public string PolicyRequestId;
        public string CustomerId;
        public string PolicyType;
        public DateTime ApplicationDate;
        public string PolicyStatus;
        public string InsuredIdentifier;
        public string AssessedValue;
        public string Currency;
        public DateTime PolicyStartDate;
        public DateTime PolicyExpiryDate;
        public string PolicyId;
        public string RefDocumentIds;
    }
}