namespace Petro.Models
{
    public class PRVCalculationModel
    {
        //TODO: Redo the model
        //TODO: Put them in the right folder
        // Input values
        public double FlowRate { get; set; }
        public double MudWeight { get; set; }
        public double CapacityCorrectionFactor { get; set; } = 1.0; // Kw factor by default
        public double CoefficientOfDischarge { get; set; } = 0.65; // Kd - default
        public double ViscosityCorrectionFactor { get; set; } = 1.0; // Kv factor in your formula
        public double PrvSetting { get; set; }
        public double MaxHydrostaticBackpressure { get; set; }

        // Output values
        public double RequiredArea { get; private set; }
        public double OverPressurePrv { get; private set; }

        public bool Calculate()
        {
            // Validation
            if (FlowRate <= 0 || MudWeight <= 0 || CapacityCorrectionFactor <= 0 ||
                CoefficientOfDischarge <= 0 || ViscosityCorrectionFactor <= 0)
            {
                return false;
            }

            // Calculate P1 (Over Pressure PRV)
            OverPressurePrv = PrvSetting + (0.1 * PrvSetting);

            if (OverPressurePrv <= MaxHydrostaticBackpressure)
            {
                return false;
            }

            // Calculate the required area using the formula
            double numerator = FlowRate;
            double denominator = 38 * CapacityCorrectionFactor * CoefficientOfDischarge * ViscosityCorrectionFactor;
            double pressureTerm = Math.Sqrt(MudWeight / (OverPressurePrv - MaxHydrostaticBackpressure));

            RequiredArea = (numerator / denominator) * pressureTerm;
            return true;
        }
    }
}
