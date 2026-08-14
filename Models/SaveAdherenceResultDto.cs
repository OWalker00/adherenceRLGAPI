using System;

namespace Adherence.Api.Models
{
    public class SaveAdherenceResultDto
    {
        public int PlanNumber { get; set; }

        public string ResultType { get; set; }

        public decimal? TestResult { get; set; }

        public DateTime? TestDate { get; set; }
    }
}
