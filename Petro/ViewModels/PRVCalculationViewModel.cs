using System.ComponentModel;
using System.Runtime.CompilerServices;
using Petro.Models;

namespace Petro.ViewModels
{
    public class PRVCalculationViewModel
    {
        private readonly PRVCalculationService _calculationService;
        private readonly PRVParemetersModel _parameters = new PRVParemetersModel();

        public PRVCalculationViewModel()
        {
            _calculationService = new PRVCalculationService();

            // Ensure MudWeights has at least one value
            if (_parameters.MudWeights.Count == 0)
            {
                _parameters.MudWeights = new List<double> { 1.20 };
            }
        }

        public double MaxPumpRate
        {
            get => _parameters.FlowRate;
            set
            {
                _parameters.FlowRate = value;
                OnPropertyChanged();
            }
        }

        public List<double> MudWeights
        {
            get => _parameters.MudWeights;
        }

        public void UpdateMudWeight(int index, double value)
        {
            if (index >= 0 && index < _parameters.MudWeights.Count)
            {
                _parameters.MudWeights[index] = value;
                OnPropertyChanged(nameof(MudWeights));
            }
        }

        public void AddMudWeight()
        {
            if (_parameters.MudWeights.Count < 5)
            {
                _parameters.MudWeights.Add(1.20); // Default value
                OnPropertyChanged(nameof(MudWeights));
            }
        }

        public void DeleteMudWeight(int index)
        {
            if (_parameters.MudWeights.Count > 1 && index >= 0 && index < _parameters.MudWeights.Count)
            {
                _parameters.MudWeights.RemoveAt(index);
                OnPropertyChanged(nameof(MudWeights));
            }
        }

        public double CapacityCorrectionFactor
        {
            get => _parameters.CapacityCorrectionFactor;
            set
            {
                _parameters.CapacityCorrectionFactor = value;
                OnPropertyChanged();
            }
        }

        public double CoefficientOfDischarge
        {
            get => _parameters.CoefficientOfDischarge;
            set
            {
                _parameters.CoefficientOfDischarge = value;
                OnPropertyChanged();
            }
        }

        public double ViscosityCorrectionFactor
        {
            get => _parameters.ViscosityCorrectionFactor;
            set
            {
                _parameters.ViscosityCorrectionFactor = value;
                OnPropertyChanged();
            }
        }

        public double PrvSetting
        {
            get => _parameters.PrvSetting;
            set
            {
                _parameters.PrvSetting = value;
                OnPropertyChanged();
            }
        }

        public double MaxHydrostaticBackpressure
        {
            get => _parameters.MaxHydrostaticBackpressure;
            set
            {
                _parameters.MaxHydrostaticBackpressure = value;
                OnPropertyChanged();
            }
        }

        // Output properties
        public double RequiredArea { get; private set; }
        public double OverPressurePrv { get; private set; }

        // UI state
        public bool CalculationPerformed { get; private set; }
        public bool HasError { get; private set; }
        public string ErrorMessage { get; private set; }

        // Calculate command
        public void CalculateRequiredArea()
        {
            try
            {
                var result = _calculationService.Calculate(_parameters);

                if (result.Success)
                {
                    RequiredArea = result.RequiredArea;
                    OverPressurePrv = result.OverPressurePrv;
                    CalculationPerformed = true;
                    HasError = false;
                    ErrorMessage = string.Empty;
                }
                else
                {
                    HasError = true;
                    ErrorMessage = result.ErrorMessage;
                    CalculationPerformed = false;
                }

                // Notify UI of changes
                OnPropertyChanged(nameof(RequiredArea));
                OnPropertyChanged(nameof(OverPressurePrv));
                OnPropertyChanged(nameof(HasError));
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(CalculationPerformed));
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = $"Error in calculation: {ex.Message}";
                CalculationPerformed = false;

                OnPropertyChanged(nameof(HasError));
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(CalculationPerformed));
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
