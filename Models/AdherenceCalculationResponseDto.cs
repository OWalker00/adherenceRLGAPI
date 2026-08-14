using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adherence.Api.Models
{
    public class AdherenceCalculationResponseDto
    {
        public string NewTotalPremiumAdjustment { get; set; }

        public string NewTotalReinsuranceAdjustment { get; set; }

        public string Outcome { get; set; }
    }
}
