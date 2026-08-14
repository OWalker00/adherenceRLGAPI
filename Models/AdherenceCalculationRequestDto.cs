using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adherence.Api.Models
{
    public class AdherenceCalculationRequestDto
{
    public int PlanNumber { get; set; }

    public string ResultType { get; set; }

    public decimal? TestResult { get; set; }

    public DateTime? TestDate { get; set; }
}
}
