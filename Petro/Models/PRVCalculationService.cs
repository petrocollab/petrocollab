namespace Petro.Models
{
    public class PRVCalculationService
    {

        public (bool Success, double RequiredArea, double OverPressurePrv, string ErrorMessage, double r) Calculate(PRVParemetersModel parameters)
        {
            // Calculate Kv factor
            double kv = 1.0; // Default value if viscosity or area data is unavailable
            double r = 0;

            // Ensure there's at least one mud weight
            if (parameters.MudWeights == null || parameters.MudWeights.Count == 0)
                return (false, 0, 0, "At least one Mud Weight must be provided.", r);
            // Validation
            if (parameters.FlowRate <= 0)
                return (false, 0, 0, "Max Pump Rate must be greater than zero.", r);

            if (parameters.MudWeights[0] <= 0)
                return (false, 0, 0, "Mud Weight must be greater than zero.", r);

            if (parameters.CapacityCorrectionFactor <= 0)
                return (false, 0, 0, "Capacity Correction Factor must be greater than zero.", r);

            if (parameters.CoefficientOfDischarge <= 0)
                return (false, 0, 0, "Coefficient of Discharge must be greater than zero.", r);

            if (parameters.CombinationCorrectionFactor <= 0)
                return (false, 0, 0, "Combination Correction Factor must be greater than zero.", r);



            if (parameters.AbsoluteViscosity.HasValue && parameters.AbsoluteViscosity > 0 &&
                parameters.AvailableArea.HasValue && parameters.AvailableArea > 0)
            {
                // Calculate Reynolds number (R)
                double flowRate = parameters.FlowRate ?? 0;
                double mudWeight = (double)parameters.MudWeights[0];
                double absoluteViscosity = parameters.AbsoluteViscosity.Value;
                double availableArea = parameters.AvailableArea.Value;

                r = (flowRate * (2800 * mudWeight)) /
                          (absoluteViscosity * Math.Sqrt(availableArea));

                // Calculate Kv using the formula: Kv = (0.9935 + (2.878 / R^0.5) + (342.75/R^1.5))^-1.0
                kv = Math.Pow(0.9935 + (2.878 / Math.Sqrt(r)) + (342.75 / Math.Pow(r, 1.5)), -1.0);
            }

            double prvSetting = parameters.PrvSetting ?? 0;
            double overPressurePrv = prvSetting * 1.1;

            if (overPressurePrv <= parameters.MaxHydrostaticBackpressure)
                return (false, 0, 0, "P1 (Over Pressure PRV) must be greater than P2 (Max Hydrostatic Backpressure).", r);

            // Calculate the required area using the formula
            double numerator = parameters.FlowRate ?? 0;

            double denominator = 38 * parameters.CapacityCorrectionFactor *
                               parameters.CoefficientOfDischarge *
                               parameters.CombinationCorrectionFactor * kv;

            double maxHydrostaticBackpressure = parameters.MaxHydrostaticBackpressure ?? 0;

            double pressureTerm = Math.Sqrt((double)parameters.MudWeights[0] /
                               (overPressurePrv - maxHydrostaticBackpressure));

            double requiredArea = (numerator / denominator) * pressureTerm;

            return (true, requiredArea, overPressurePrv, string.Empty, r);
        }
    }
}
