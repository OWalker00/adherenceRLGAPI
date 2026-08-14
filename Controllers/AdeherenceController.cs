using Adherence.Api.Models;
using Adherence.Api.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace Adherence.Api.Controllers
{
    [RoutePrefix("api/plans")]
    public class AdherenceController : ApiController
    {
        private readonly AdherenceService _adherenceService;

        public AdherenceController()
        {
            _adherenceService = new AdherenceService();
        }

        public AdherenceController(AdherenceService adherenceService)
        {
            _adherenceService = adherenceService;
        }

        [HttpGet]
        [Route("{planNumber}")]
        public async Task<IHttpActionResult> GetPlan(int planNumber)
        {
            var response = await _adherenceService.GetPlanWithAdherenceResultsAsync(planNumber);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("calculate")]
        public IHttpActionResult Calculate( AdherenceCalculationRequestDto request)
        {
            return Ok(_adherenceService.Calculate(request));
        }

        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> Save([FromBody] SaveAdherenceResultDto request)
        {
            await _adherenceService.SaveAdherenceResultAsync(request);

            return Ok();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _adherenceService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
