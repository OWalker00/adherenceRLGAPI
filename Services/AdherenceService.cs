using Adherence.Api.Data;
using Adherence.Api.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Adherence.Api.Services
{
    public class AdherenceService : IDisposable
    {
        private readonly AdherenceTestDBEntities1 _context;

        public AdherenceService()
        {
            _context = new AdherenceTestDBEntities1();
        }

        public AdherenceService(AdherenceTestDBEntities1 context)
        {
            _context = context;
        }

        public async Task<PlanLookupResponse> GetPlanWithAdherenceResultsAsync(int planNumber)
        {
            var plan = await _context.Plans
                .Include(p => p.AdherenceResults)
                .FirstOrDefaultAsync(p => p.PlanNumber == planNumber);

            if (plan == null)
            {
                return null;
            }

            return new PlanLookupResponse
            {
                Plan = new PlanDto
                {
                    PlanNumber = plan.PlanNumber,
                    LifeAssuredName = plan.LifeAssuredName,
                    PolicyStartDate = plan.PolicyStartDate,

                    // Sensitive financial values are retained here only because
                    // they are existing plan-level fields, not calculation rules.
                    CurrentPremium = plan.CurrentPremium,
                    CurrentReinsurance = plan.CurrentReinsurance,
                    CurrentPremiumAdjustment = plan.CurrentPremiumAdjustment,
                    CurrentReinsuranceAdjustment = plan.CurrentReinsuranceAdjustment
                },

                AdherenceResults = plan.AdherenceResults
                    .OrderByDescending(ar => ar.ResultDate)
                    .Select(ar => new AdherenceResultDto
                    {
                        Id = ar.Id,
                        PlanNumber = ar.PlanNumber,
                        ResultDate = ar.ResultDate,
                        A1cResult = ar.A1cResult,
                        PremiumAdjustment = ar.PremiumAdjustment,
                        ResultType = ar.ResultType,
                        ControlBand = ar.ControlBand,
                        A1cUnit = ar.A1cUnit
                    })
                    .ToList()
            };
        }

        public AdherenceCalculationResponseDto Calculate(AdherenceCalculationRequestDto request)
        {
            if (request == null)
            {
                return new AdherenceCalculationResponseDto
                {
                    NewTotalPremiumAdjustment = "[REDACTED]",
                    NewTotalReinsuranceAdjustment = "[REDACTED]",
                    Outcome = "No calculation performed"
                };
            }

            var plan = _context.Plans
                .Include(p => p.AdherenceResults)
                .FirstOrDefault(p => p.PlanNumber == request.PlanNumber);

            if (plan == null || plan.AdherenceResults == null || !plan.AdherenceResults.Any())
            {
                return new AdherenceCalculationResponseDto
                {
                    NewTotalPremiumAdjustment = "[REDACTED]",
                    NewTotalReinsuranceAdjustment = "[REDACTED]",
                    Outcome = "No adherence history found"
                };
            }

            var orderedResults = plan.AdherenceResults
                .Where(ar => ar.ControlBand.HasValue)
                .OrderBy(ar => ar.ResultDate)
                .ToList();

            if (!orderedResults.Any())
            {
                return new AdherenceCalculationResponseDto
                {
                    NewTotalPremiumAdjustment = "[REDACTED]",
                    NewTotalReinsuranceAdjustment = "[REDACTED]",
                    Outcome = "No control bands found"
                };
            }

            var originalBand = orderedResults.First().ControlBand.Value;

            var previousBand = orderedResults
                .OrderByDescending(ar => ar.ResultDate)
                .First()
                .ControlBand.Value;

            var newBand = GetControlBand(
                request.ResultType,
                request.TestResult);

            var premiumAdjustment = GetPremiumAdjustment(
                originalBand,
                previousBand,
                newBand);

            var reinsuranceAdjustment = GetReinsuranceAdjustment(
                originalBand,
                previousBand,
                newBand);

            var currentPremiumAdjustment =
                plan.CurrentPremiumAdjustment ?? GetDefaultAdjustmentValue();

            var currentReinsuranceAdjustment =
                plan.CurrentReinsuranceAdjustment ?? GetDefaultAdjustmentValue();

            var newTotalPremiumAdjustment = ApplyPremiumAdjustmentCaps(
                currentPremiumAdjustment,
                premiumAdjustment,
                originalBand);

            var newTotalReinsuranceAdjustment = ApplyReinsuranceAdjustmentCaps(
                currentReinsuranceAdjustment,
                reinsuranceAdjustment,
                originalBand);

            if (newTotalPremiumAdjustment == currentPremiumAdjustment)
            {
                newTotalReinsuranceAdjustment = currentReinsuranceAdjustment;
            }

            var outcome = GetOutcome(
                currentPremiumAdjustment,
                newTotalPremiumAdjustment);

            return new AdherenceCalculationResponseDto
            {
                NewTotalPremiumAdjustment = "[REDACTED]",
                NewTotalReinsuranceAdjustment = "[REDACTED]",
                Outcome = outcome
            };
        }

        private int GetControlBand(string resultType, decimal? testResult)
        {
            if (!testResult.HasValue)
            {
                return GetDefaultControlBand();
            }

            // REDACTED:
            // Actual HbA1c threshold values and banding rules have been removed.
            // This logic maps the submitted test result to the relevant medical control band.

            return GetRedactedControlBand();
        }

        private decimal GetPremiumAdjustment(
            int originalBand,
            int previousBand,
            int newBand)
        {
            // REDACTED:
            // Actual premium adjustment percentages and band-to-band movement rules
            // have been removed because they are commercially sensitive.

            return GetRedactedAdjustmentValue();
        }

        private decimal GetReinsuranceAdjustment(
            int originalBand,
            int previousBand,
            int newBand)
        {
            // REDACTED:
            // Actual reinsurance adjustment percentages and band-to-band movement rules
            // have been removed because they are commercially sensitive.

            return GetRedactedAdjustmentValue();
        }

        private decimal ApplyPremiumAdjustmentCaps(
            decimal currentTotalAdjustment,
            decimal periodAdjustment,
            int originalBand)
        {
            // REDACTED:
            // Actual premium cap and maximum discount rules have been removed.

            return GetRedactedAdjustmentValue();
        }

        private decimal ApplyReinsuranceAdjustmentCaps(
            decimal currentTotalAdjustment,
            decimal periodAdjustment,
            int originalBand)
        {
            // REDACTED:
            // Actual reinsurance cap, increase and discount rules have been removed.

            return GetRedactedAdjustmentValue();
        }

        private decimal GetMaxPremiumDiscount(int originalBand)
        {
            // REDACTED:
            // Maximum premium discount values by original control band have been removed.

            return GetRedactedAdjustmentValue();
        }

        private decimal GetMaxReinsuranceDiscount(int originalBand)
        {
            // REDACTED:
            // Maximum reinsurance discount values by original control band have been removed.

            return GetRedactedAdjustmentValue();
        }

        private decimal GetMaxReinsuranceIncrease(int originalBand)
        {
            // REDACTED:
            // Maximum reinsurance increase values by original control band have been removed.

            return GetRedactedAdjustmentValue();
        }

        private string GetOutcome(
            decimal currentPremiumAdjustment,
            decimal newPremiumAdjustment)
        {
            if (newPremiumAdjustment < currentPremiumAdjustment)
            {
                return "Premium decreased";
            }

            if (newPremiumAdjustment > currentPremiumAdjustment)
            {
                return "Premium increased";
            }

            return "Continue on current terms";
        }

        public async Task SaveAdherenceResultAsync(SaveAdherenceResultDto request)
        {
            if (request == null)
            {
                throw new InvalidOperationException("Save request was not provided.");
            }

            if (!request.TestResult.HasValue)
            {
                throw new InvalidOperationException("Test result is required.");
            }

            if (!request.TestDate.HasValue)
            {
                throw new InvalidOperationException("Test date is required.");
            }

            var plan = await _context.Plans
                .Include(p => p.AdherenceResults)
                .FirstOrDefaultAsync(p => p.PlanNumber == request.PlanNumber);

            if (plan == null)
            {
                throw new InvalidOperationException("Plan not found.");
            }

            var orderedResults = plan.AdherenceResults
                .Where(ar => ar.ControlBand.HasValue)
                .OrderBy(ar => ar.ResultDate)
                .ToList();

            if (!orderedResults.Any())
            {
                throw new InvalidOperationException("No adherence history found.");
            }

            var originalBand = orderedResults.First().ControlBand.Value;

            var previousBand = orderedResults
                .OrderByDescending(ar => ar.ResultDate)
                .First()
                .ControlBand.Value;

            var newBand = GetControlBand(
                request.ResultType,
                request.TestResult);

            var premiumAdjustment = GetPremiumAdjustment(
                originalBand,
                previousBand,
                newBand);

            var reinsuranceAdjustment = GetReinsuranceAdjustment(
                originalBand,
                previousBand,
                newBand);

            var currentPremiumAdjustment =
                plan.CurrentPremiumAdjustment ?? GetDefaultAdjustmentValue();

            var currentReinsuranceAdjustment =
                plan.CurrentReinsuranceAdjustment ?? GetDefaultAdjustmentValue();

            var newTotalPremiumAdjustment = ApplyPremiumAdjustmentCaps(
                currentPremiumAdjustment,
                premiumAdjustment,
                originalBand);

            var newTotalReinsuranceAdjustment = ApplyReinsuranceAdjustmentCaps(
                currentReinsuranceAdjustment,
                reinsuranceAdjustment,
                originalBand);

            if (newTotalPremiumAdjustment == currentPremiumAdjustment)
            {
                newTotalReinsuranceAdjustment = currentReinsuranceAdjustment;
            }

            var adherenceResult = new AdherenceResult
            {
                PlanNumber = request.PlanNumber,
                ResultDate = request.TestDate.Value,
                A1cResult = request.TestResult.Value,

                // Actual calculated adjustment value has been redacted.
                PremiumAdjustment = "[REDACTED]",

                ResultType = GetDatabaseResultType(request.ResultType),

                // Control band value is retained to show the application flow,
                // but the rules used to derive it are redacted above.
                ControlBand = newBand,

                A1cUnit = GetDatabaseA1cUnit(request.ResultType)
            };

            _context.AdherenceResults.Add(adherenceResult);

            // Actual updated financial adjustment values are calculated using
            // redacted business rules.
            plan.CurrentPremiumAdjustment = newTotalPremiumAdjustment;
            plan.CurrentReinsuranceAdjustment = newTotalReinsuranceAdjustment;

            await _context.SaveChangesAsync();
        }

        private string FormatAdjustment(decimal adjustment)
        {
            // REDACTED:
            // Formatting retained conceptually, but actual adjustment values
            // should not be exposed in submitted evidence.

            return "[REDACTED]";
        }

        private string GetDatabaseResultType(string resultType)
        {
            switch (resultType?.ToLower())
            {
                case "percent":
                    return "HbA1cPercentage";

                case "mmol":
                    return "HbA1cMmol";

                case "none":
                    return "NoResult";

                default:
                    return resultType;
            }
        }

        private string GetDatabaseA1cUnit(string resultType)
        {
            switch (resultType?.ToLower())
            {
                case "percent":
                    return "Percentage";

                case "mmol":
                    return "Mmol";

                default:
                    return string.Empty;
            }
        }

        private decimal GetDefaultAdjustmentValue()
        {
            // REDACTED:
            // Default baseline adjustment value removed.

            return 0m;
        }

        private int GetDefaultControlBand()
        {
            // REDACTED:
            // Default control band removed.

            return 0;
        }

        private int GetRedactedControlBand()
        {
            // REDACTED:
            // Placeholder only. Actual control banding logic removed.

            return 0;
        }

        private decimal GetRedactedAdjustmentValue()
        {
            // REDACTED:
            // Placeholder only. Actual calculation value removed.

            return 0m;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        private class AdjustmentLookup
        {
            public decimal PeriodDiscountAdjustment { get; set; }

            public decimal MaxDiscount { get; set; }

            public decimal PeriodIncreaseAdjustment { get; set; }

            public decimal MaxIncrease { get; set; }
        }
    }
}
