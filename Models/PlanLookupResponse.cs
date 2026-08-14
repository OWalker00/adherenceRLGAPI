using System.Collections.Generic;

namespace Adherence.Api.Models
{
    public class PlanLookupResponse
    {
        public PlanDto Plan { get; set; }
        public IEnumerable<AdherenceResultDto> AdherenceResults { get; set; }
    }
}
