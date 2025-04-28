namespace Petro.Models
{
    public class PRVCalculationService
    {

        public (bool Success, double RequiredArea, double OverPressurePrv, string ErrorMessage) Calculate(PRVParemetersModel parameters)
        {
            // Ensure there's at least one mud weight
            if (parameters.MudWeights == null || parameters.MudWeights.Count == 0)
                return (false, 0, 0, "At least one Mud Weight must be provided.");
            // Validation
            if (parameters.FlowRate <= 0)
                return (false, 0, 0, "Max Pump Rate must be greater than zero.");

            if (parameters.MudWeights[0] <= 0)
                return (false, 0, 0, "Mud Weight must be greater than zero.");

            if (parameters.CapacityCorrectionFactor <= 0)
                return (false, 0, 0, "Capacity Correction Factor must be greater than zero.");

            if (parameters.CoefficientOfDischarge <= 0)
                return (false, 0, 0, "Coefficient of Discharge must be greater than zero.");

            if (parameters.ViscosityCorrectionFactor <= 0)
                return (false, 0, 0, "Viscosity Correction Factor must be greater than zero.");

            double overPressurePrv = parameters.PrvSetting + (0.1 * parameters.PrvSetting);

            if (overPressurePrv <= parameters.MaxHydrostaticBackpressure)
                return (false, 0, 0, "P1 (Over Pressure PRV) must be greater than P2 (Max Hydrostatic Backpressure).");

            // Calculate the required area using the formula
            double numerator = parameters.FlowRate;

            double denominator = 38 * parameters.CapacityCorrectionFactor *
                               parameters.CoefficientOfDischarge *
                               parameters.ViscosityCorrectionFactor;

            double pressureTerm = Math.Sqrt(parameters.MudWeights[0] /
                               (overPressurePrv - parameters.MaxHydrostaticBackpressure));

            double requiredArea = (numerator / denominator) * pressureTerm;

            return (true, requiredArea, overPressurePrv, string.Empty);
        }
    }
}
