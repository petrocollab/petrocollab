using PetroCollab.Presentation.Client.Models;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Net.Http;

namespace PetroCollab.Presentation.Client.ViewModels
{
    public class HomeViewModel
    {
        private readonly HttpClient _httpClient;
        public string Title { get; private set; } = "Loading...";
        public string Description { get; private set; } = "Loading...";

        public HomeViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoadDataAsync()
        {
            try
            {
                string filePath = "Resources/Strings/en-US/home.json";
                var jsonString = await _httpClient.GetStringAsync(filePath);
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
