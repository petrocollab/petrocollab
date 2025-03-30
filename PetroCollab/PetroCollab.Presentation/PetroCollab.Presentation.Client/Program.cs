using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using PetroCollab.Application.Interfaces;
using PetroCollab.Presentation.Client;
using PetroCollab.Presentation.Client.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

await builder.Build().RunAsync();
