using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ImplementaPost.DTO;
using Microsoft.Extensions.Logging;

namespace ImplementaPost.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private const string API_URL = "https://jsonplaceholder.typicode.com/posts/";
        private readonly HttpClient _httpClient;

        public JsonplaceholderAPIIntegration(ILogger<JsonplaceholderAPIIntegration> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    public async Task<List<PostDTO>> GetAllPosts()
    {
        string requestUrl = $"{API_URL}";
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PostDTO>>() ?? new List<PostDTO>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error al llamar a la API: {ex.Message}");
            throw;
        }
    }

    public async Task<PostDTO> GetPostDetails(int id)
    {
        string requestUrl = $"{API_URL}/{id}";
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PostDTO>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error al llamar a la API: {ex.Message}");
            throw;
        }
    }

    public async Task<PostDTO> CreatePost(PostDTO newPost)
    {
        string requestUrl = API_URL;
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUrl, newPost);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PostDTO>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error al crear el post: {ex.Message}");
            throw;
        }
    }

    public async Task<PostDTO> UpdatePost(PostDTO updatedPost)
    {
        string requestUrl = $"{API_URL}/{updatedPost.id}";
        try
        {
            string updatedPostJson = JsonSerializer.Serialize(updatedPost);
            var content = new StringContent(updatedPostJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(requestUrl, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PostDTO>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error al actualizar el post: {ex.Message}");
            throw;
        }
    }

    public async Task<HttpResponseMessage> DeletePostAsync(int id)
    {
        string requestUrl = $"{API_URL}/{id}";
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error al eliminar el post: {ex.Message}");
            throw;
        }
    }
}}