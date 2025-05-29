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

        private string _calculationMode = "single"; // Default to single scenario
        public bool PRVCertificationRequired { get; set; } = false; // Default to false

        public PRVStringResourceModel StringResources { get; private set; } = new PRVStringResourceModel();

        public PRVCalculationViewModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _calculationService = new PRVCalculationService();

            // Ensure MudWeights has at least one value
            if (_parameters.MudWeights.Count == 0)
            {
                _parameters.MudWeights = new List<double?> { null };
            }
        }

        public double? AvailableArea
        {
            get => _parameters.AvailableArea;
            set
            {
                _parameters.AvailableArea = value;
                OnPropertyChanged();
            }
        }

        public double? FlowRate
        {
            get => _parameters.FlowRate;
            set
            {
                _parameters.FlowRate = value;
                OnPropertyChanged();
            }
        }

        public double? MinFlowRate
        {
            get => _parameters.MinFlowRate;
            set
            {
                _parameters.MinFlowRate = value;
                OnPropertyChanged();
            }
        }

        public double? MaxFlowRate
        {
            get => _parameters.MaxFlowRate;
            set
            {
                _parameters.MaxFlowRate = value;
                OnPropertyChanged();
            }
        }

        public List<double?> MudWeights
        {
            get => _parameters.MudWeights;
        }

        public void UpdateMudWeight(int index, double? value)
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
                _parameters.MudWeights.Add(null); // Default value
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

        public double CombinationCorrectionFactor
        {
            get => _parameters.CombinationCorrectionFactor;
            set
            {
                _parameters.CombinationCorrectionFactor = value;
                OnPropertyChanged();
            }
        }

        public double? AbsoluteViscosity
        {
            get => _parameters.AbsoluteViscosity;
            set
            {
                _parameters.AbsoluteViscosity = value;
                OnPropertyChanged();
            }
        }

        public double? PrvSetting
        {
            get => _parameters.PrvSetting;
            set
            {
                _parameters.PrvSetting = value;
                OnPropertyChanged();
            }
        }

        public double? MaxHydrostaticBackpressure
        {
            get => _parameters.MaxHydrostaticBackpressure;
            set
            {
                _parameters.MaxHydrostaticBackpressure = value;
                OnPropertyChanged();
            }
        }

        public string CalculationMode
        {
            get => _calculationMode;
            set
            {
                if (_calculationMode != value)
                {
                    _calculationMode = value;

                    // When switching to single mode, keep only the first mud weight
                    if (_calculationMode == "single" && _parameters.MudWeights.Count > 1)
                    {
                        var firstValue = _parameters.MudWeights.FirstOrDefault();
                        _parameters.MudWeights.Clear();
                        _parameters.MudWeights.Add(firstValue);
                        OnPropertyChanged(nameof(MudWeights));
                    }

                    OnPropertyChanged();
                }
            }
        }

        // Output properties
        public double RequiredArea { get; private set; }
        public string AreaComparisonMessage { get; private set; }
        public string Reynolds { get; private set; } = string.Empty;
        public bool IsAreaAdequate { get; private set; }
        public bool HasAreaComparison => AvailableArea.HasValue && AvailableArea > 0 && CalculationPerformed;

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
                    CalculationPerformed = true;
                    HasError = false;
                    ErrorMessage = string.Empty;
                    Reynolds = string.Empty;

                    if (result.r > 0)
                    {
                        Reynolds = StringResources.Results.Reynolds + result.r.ToString("F0");
                    }

                    // Compare Required Area with Available Area if provided
                    if (AvailableArea.HasValue && AvailableArea > 0)
                    {
                        IsAreaAdequate = AvailableArea.Value >= RequiredArea;
                        // Format the message with values
                        AreaComparisonMessage = string.Format(
                            IsAreaAdequate
                                ? StringResources.Results.AdequateSizeMessage
                                : StringResources.Results.InadequateSizeMessage,
                            AvailableArea.Value.ToString("F2"),
                            RequiredArea.ToString("F2"));
                    }
                    else
                    {
                        AreaComparisonMessage = string.Empty;
                    }
                }
                else
                {
                    HasError = true;
                    ErrorMessage = result.ErrorMessage;
                    CalculationPerformed = false;
                    AreaComparisonMessage = string.Empty;
                }

                // Notify UI of changes
                OnPropertyChanged(nameof(RequiredArea));
                OnPropertyChanged(nameof(HasError));
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(CalculationPerformed));
                OnPropertyChanged(nameof(AreaComparisonMessage));
                OnPropertyChanged(nameof(IsAreaAdequate));
                OnPropertyChanged(nameof(HasAreaComparison));
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = $"Error in calculation: {ex.Message}";
                CalculationPerformed = false;
                AreaComparisonMessage = string.Empty;

                OnPropertyChanged(nameof(HasError));
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(CalculationPerformed));
                OnPropertyChanged(nameof(AreaComparisonMessage));
                OnPropertyChanged(nameof(HasAreaComparison));
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
