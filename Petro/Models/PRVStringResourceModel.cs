namespace Petro.Models
{
    public class PRVStringResourceModel
    {
        public string PageTitle { get; set; } = "PRV Calculator";

        public InputParametersModel InputParameters { get; set; } = new InputParametersModel();

        public ResultsModel Results { get; set; } = new ResultsModel();

        public FormulaModel Formula { get; set; } = new FormulaModel();

        public class InputParametersModel
        {
            public string Title { get; set; } = "Input Parameters";
            public string FlowRate { get; set; } = "Flow Rate (gpm)";
            public string MinFlowRate { get; set; } = "Min Flow Rate (gpm)";
            public string MaxFlowRate { get; set; } = "Max Flow Rate (gpm)";
            public string MudWeight { get; set; } = "Mud Weight (sg)";
            public string AvailableArea { get; set; } = "Available Discharge Area (in²)";
            public string CapacityCorrectionFactor { get; set; } = "Capacity Correction Factor (Kw)";
            public string CoefficientOfDischarge { get; set; } = "Coefficient of Discharge (Kd)";
            public string ViscosityCorrectionFactor { get; set; } = "Viscosity Correction Factor (Kv)";
            public string CombinationCorrectionFactor { get; set; } = "Combination Correction Factor (Kc)";
            public string AbsoluteViscosity { get; set; } = "Absolute viscosity (µ)";
            public string PrvSetting { get; set; } = "PRV Setting (Set Point)";
            public string MaxHydrostaticBackpressure { get; set; } = "Max Hydrostatic Backpressure (P2)";
            public string CalculateButton { get; set; } = "Calculate";
            public string ConfirmResetButton { get; set; } = "Confirm Reset";
            public string AddMudWeight { get; set; } = "Add Mud Weight";
            public string AdvancedOptions { get; set; } = "Advanced Options";
            public string CalculationMode { get; set; } = "Calculation Mode";
            public string SingleScenarioTitle { get; set; } = "Single Scenario Calculation";
            public string SingleScenarioDescr { get; set; } = "Use one flow rate and one mud weight.";
            public string MultipleScenarioTitle { get; set; } = "Multiple Scenario Calculation";
            public string MultipleScenarioDescr { get; set; } = "Use a range of flow rates and/or mud weights.";
            public string PRVCertReqTitle { get; set; } = "PRV Certification Requirement";
            public string PRVCertReqTrue { get; set; } = "Requires Capacity Certification";
            public string PRVCertReqFalse { get; set; } = "Does Not Require Capacity Certification";
        }

        public class ResultsModel
        {
            public string Title { get; set; } = "Results";
            public string RequiredAreaPrefix { get; set; } = "Required Area: ";
            public string RequiredAreaSuffix { get; set; } = "in²";
            public string Reynolds { get; set; } = "Reynold’s Number: ";
            public string DefaultMessage { get; set; } = "Enter values and click Calculate to see results.";
            public string AdequateSizeMessage { get; set; } = "The available discharge area of {0} in² exceeds the required area of {1} in², indicating that the current PRV setup is adequately sized to handle the expected flow rate safely.";
            public string InadequateSizeMessage { get; set; } = "Warning: The available discharge area of {0} in² is less than the required area of {1} in². The current PRV setup may not be adequately sized to handle the expected flow rate safely.";
        }

        public class FormulaModel
        {
            public string Title { get; set; } = "Formula Used";
            public string Description { get; set; } = "Where:";
            public VariablesModel Variables { get; set; } = new VariablesModel();
            public string FormulaLatex { get; set; } = "\\[A = \\frac{Q}{38K_dK_wK_cK_v} \\sqrt{\\frac{G}{p_1-p_2}}\\]";

            public class VariablesModel
            {
                public string Area { get; set; } = "𝐴: Required discharge area (in²)";
                public string FlowRate { get; set; } = "𝑄: Flow rate (gpm)";
                public string CoefficientOfDischarge { get; set; } = "𝐾𝑑: Coefficient of discharge (dimensionless)";
                public string CapacityCorrectionFactor { get; set; } = "𝐾𝑤: Capacity correction factor (dimensionless)";
                public string CombinationCorrectionFactor { get; set; } = "𝐾𝑐: Combination correction factor";
                public string ViscosityCorrectionFactor { get; set; } = "𝐾𝑣: Viscosity correction factor (dimensionless)";
                public string AbsoluteViscosity { get; set; } = "µ: Absolute viscosity at the flowing temperature, centipoise";
                public string SpecificGravity { get; set; } = "𝐺: Specific gravity of the fluid (sg)";
                public string SetPressure { get; set; } = "𝑃1​: Set pressure plus overpressure (psi)";
                public string Backpressure { get; set; } = "𝑃2: Backpressure (psi)";
            }
        }
    }
}
