using System.Text.Json;
using Blazor.Clients.Requests;
using Blazor.Core.Models;
using Blazor.Shared.Results;

namespace Blazor.Clients.Services;

public sealed class ClientsService : IClientsService
{
    private readonly HttpClient _httpClient;
    private const string ClientsRoute = "clients/";
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ClientsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OneOf<List<Client>, Error>> GetClients()
    {
        var response = await _httpClient.GetAsync(ClientsRoute);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
            return error ?? new Error("Failed to load clients.");
        }

        var clients = await response.Content.ReadFromJsonAsync<List<Client>?>(SerializerOptions);
        
        return clients ?? []; 
    }

    public async Task<OneOf<Success, Error>> CreateClient(CreateClientRequest request)
    {
        var httpContent = JsonContent.Create(request, options: SerializerOptions);
        var response = await _httpClient.PostAsync(ClientsRoute, httpContent);
        
        if (response.IsSuccessStatusCode) return StaticResults.Success;
        
        var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
        return error ?? new Error("Failed to create client.");
    }
    
    public async Task<OneOf<Success, Error>> UpdateClient(Guid clientId, UpdateClientRequest request)
    {
        var httpContent = JsonContent.Create(request, options: SerializerOptions);
        var response = await _httpClient.PutAsync(ClientsRoute + clientId, httpContent);
        
        if (response.IsSuccessStatusCode) return StaticResults.Success;

        var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
        return error ?? new Error("Failed to update client.");
    }

    public async Task<OneOf<Success, Error>> DeleteClient(Guid clientId)
    {
        var response = await _httpClient.DeleteAsync(ClientsRoute + clientId);
        
        if (response.IsSuccessStatusCode) return StaticResults.Success;
        
        var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
        return error ?? new Error("Failed to delete client.");
    }
    
    public async Task<OneOf<Client, Error>> GetClientById(Guid clientId)
    {
        var response = await _httpClient.GetAsync(ClientsRoute + clientId);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(SerializerOptions);
            return error ?? new Error("Failed to load client details.");
        }

        var client = await response.Content.ReadFromJsonAsync<Client>(SerializerOptions);
        
        if (client is null) return new Error("Failed to deserialize client.");
        return client;
    }
}