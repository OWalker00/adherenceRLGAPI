using System;

namespace Adherence.Api.Models
{
    public class PlanDto
    {
        public int PlanNumber { get; set; }
        public string LifeAssuredName { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public decimal? CurrentPremium { get; set; }
        public decimal? CurrentReinsurance { get; set; }
        public decimal? CurrentPremiumAdjustment { get; set; }
        public decimal? CurrentReinsuranceAdjustment { get; set; }
    }
}
