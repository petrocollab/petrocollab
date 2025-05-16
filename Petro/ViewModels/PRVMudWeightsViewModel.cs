namespace Petro.ViewModels
{
    public class PRVMudWeightsViewModel
    {
        // List to store multiple mud weight values
        public List<decimal> MudWeights { get; set; } = new List<decimal> { 0 };

        public decimal MudWeight
        {
            get => MudWeights.Count > 0 ? MudWeights[0] : 0;
            set
            {
                if (MudWeights.Count > 0)
                    MudWeights[0] = value;
                else
                    MudWeights.Add(value);
            }
        }
    }
}
