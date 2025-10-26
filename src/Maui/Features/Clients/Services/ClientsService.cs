using System.Collections.Immutable;
using System.Net.Http.Json;
using System.Text.Json;
using Maui.Features.Clients.Models;
using Maui.Features.Clients.Requests;
using Maui.Shared.Models;
using OneOf.Types;

namespace Maui.Features.Clients.Services;

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

    public async Task<OneOf<ImmutableArray<Client>, ErrorMessage>> GetClients()
    {
        var response = await _httpClient.GetAsync(ClientsRoute);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorMessage>(SerializerOptions);
            return error ?? new ErrorMessage("Failed to load clients.");
        }

        var clients = await response.Content.ReadFromJsonAsync<ImmutableArray<Client>?>(SerializerOptions);
        
        return clients ?? ImmutableArray<Client>.Empty; 
    }

    public async Task<OneOf<None, ErrorMessage>> CreateClient(Guid clientId, CreateClientRequest request)
    {
        var httpContent = JsonContent.Create(request, options: SerializerOptions);
        var response = await _httpClient.PostAsync(ClientsRoute + clientId, httpContent);
        
        if (response.IsSuccessStatusCode) return new None();
        
        var error = await response.Content.ReadFromJsonAsync<ErrorMessage>(SerializerOptions);
        return error ?? new ErrorMessage("Failed to create client.");
    }
    
    public async Task<OneOf<None, ErrorMessage>> UpdateClient(Guid clientId, UpdateClientRequest request)
    {
        var httpContent = JsonContent.Create(request, options: SerializerOptions);
        var response = await _httpClient.PutAsync(ClientsRoute + clientId, httpContent);
        
        if (response.IsSuccessStatusCode) return new None();

        var error = await response.Content.ReadFromJsonAsync<ErrorMessage>(SerializerOptions);
        return error ?? new ErrorMessage("Failed to update client.");
    }

    public async Task<OneOf<None, ErrorMessage>> DeleteClient(Guid clientId)
    {
        var response = await _httpClient.DeleteAsync(ClientsRoute + clientId);
        
        if (response.IsSuccessStatusCode) return new None();
        
        var error = await response.Content.ReadFromJsonAsync<ErrorMessage>(SerializerOptions);
        return error ?? new ErrorMessage("Failed to delete client.");
    }
}