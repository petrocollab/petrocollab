namespace Petro.Models
{
    public class PRVParemetersModel
    {
        // Input values
        public double? FlowRate { get; set; }
        public List<double?> MudWeights { get; set; } = new List<double?> { null };
        public double CapacityCorrectionFactor { get; set; } = 1.0; // Kw factor by default
        public double CoefficientOfDischarge { get; set; } = 0.65; // Kd - default
        public double ViscosityCorrectionFactor { get; set; } = 1.0; // Kv factor
        public double? PrvSetting { get; set; }
        public double? MaxHydrostaticBackpressure { get; set; }
    }
}
