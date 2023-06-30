using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteService
{
    public class Leaddata
    {
        public string id { get; set; } = Guid.NewGuid().ToString();

        public Customer Customer { get; set; }
       
    }

    public class Customer
    {
        public int numberOfEmployees { get; set; }
        public string azureConsumption { get; set; }
        public string language { get; set; }
        public string jobTitle { get; set; }
        public string mobilePhone { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string websiteUrl { get; set; }
        public string emailId { get; set; }
        public string country { get; set; }
        public string CountryCode { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string customerComments { get; set; }
        public string linkedinProfileURL { get; set; }
        public string primaryProduct { get; set; }
    }

   

    public class Score
    {
        public float score { get; set; }
        public DateTime scoreUpdateTimestamp { get; set; }
        public string rating { get; set; }
        public string scoreRatingReason { get; set; }
    }


}

