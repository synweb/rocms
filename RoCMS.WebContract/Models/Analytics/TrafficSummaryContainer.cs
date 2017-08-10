using System;
using System.Collections.Generic;

namespace RoCMS.Web.Contract.Models.Analytics
{
    public class TrafficSummaryContainer
    {
        public TrafficSummaryContainer()
        {
            TrafficSummaryCollection = new List<TrafficSummary>();
        }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<TrafficSummary> TrafficSummaryCollection { get; private set; } 
    }
}
