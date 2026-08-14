using System;

namespace Adherence.Api.Models
{
    public class AdherenceResultDto
    {
        public int Id { get; set; }
        public int PlanNumber { get; set; }
        public DateTime ResultDate { get; set; }
        public decimal A1cResult { get; set; }
        public string PremiumAdjustment { get; set; }
        public string ResultType { get; set; }
        public int? ControlBand { get; set; }
        public string A1cUnit { get; set; }
    }
}
