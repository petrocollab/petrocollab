using System.Text.Json;
using Petro.Models;

namespace Petro.ViewModels
{
    public class HomeViewModel
    {
        private readonly IWebHostEnvironment _environment;
        //private readonly HttpClient _httpClient;
        public string Title { get; private set; } = "Loading...";
        public string Description { get; private set; } = "Loading...";

        public HomeViewModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task LoadDataAsync()
        {
            try
            {
                string filePath = Path.Combine(_environment.WebRootPath, "Resources", "Strings", "en-US", "home.json");
                var jsonString = await File.ReadAllTextAsync(filePath);
                var myData = JsonSerializer.Deserialize<HomeTextModel>(jsonString);

                Title = myData?.Title ?? "Default Title";
                Description = myData?.Description ?? "Default Description";
            }
            catch (Exception ex)
            {
                Title = "Default Exception";
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
