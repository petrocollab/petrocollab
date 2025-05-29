namespace Petro.Models
{
    public class PRVParemetersModel
    {
        // Input values
        public double? AvailableArea { get; set; }
        public double? FlowRate { get; set; }
        public double? MinFlowRate { get; set; }
        public double? MaxFlowRate { get; set; }
        public List<double?> MudWeights { get; set; } = new List<double?> { null };
        public double CapacityCorrectionFactor { get; set; } = 1.0; // Kw factor by default
        public double CoefficientOfDischarge { get; set; } = 0.65; // Kd - default
        public double CombinationCorrectionFactor { get; set; } = 1.0; // Kc factor
        public double? AbsoluteViscosity { get; set; } // µ - to calculate Kv
        public double? PrvSetting { get; set; }
        public double? MaxHydrostaticBackpressure { get; set; }
    }
}
