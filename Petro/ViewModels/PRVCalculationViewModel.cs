using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Petro.Models;

namespace Petro.ViewModels
{
    public class PRVCalculationViewModel
    {
        private readonly PRVCalculationService _calculationService;
        private readonly PRVParemetersModel _parameters = new PRVParemetersModel();
        private readonly IWebHostEnvironment _environment;

        public PRVStringResourceModel StringResources { get; private set; } = new PRVStringResourceModel();

        public PRVCalculationViewModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _calculationService = new PRVCalculationService();
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

        public double MudWeight
        {
            get => _parameters.MudWeight;
            set
            {
                _parameters.MudWeight = value;
                OnPropertyChanged();
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

        // Load string resources from JSON
        public async Task LoadResourcesAsync()
        {
            try
            {
                string filePath = Path.Combine(_environment.WebRootPath, "Resources", "Strings", "en-US", "prvpage.json");
                var jsonString = await File.ReadAllTextAsync(filePath);
                StringResources = JsonSerializer.Deserialize<PRVStringResourceModel>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new PRVStringResourceModel();

                OnPropertyChanged(nameof(StringResources));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading resources: {ex.Message}");
                // Use default resources if loading fails
                StringResources = new PRVStringResourceModel();
            }
        }


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
