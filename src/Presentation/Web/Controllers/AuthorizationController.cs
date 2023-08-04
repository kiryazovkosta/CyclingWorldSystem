using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using Web.Models.Response;

namespace Web.Controllers
{
    using System.Text.Json;
    using Models;

    public class AuthorizationController : Controller
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public AuthorizationController(
			IHttpContextAccessor httpContextAccessor, 
			IHttpClientFactory httpClientFactory, 
			IConfiguration configuration)
		{
			_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
			_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

        private IHttpClientFactory HttpClientFactory => _httpClientFactory;

        protected IConfiguration Configuration => _configuration;

        protected IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;

        protected async Task<EndpointResponse<TOutput>> GetAsync<TOutput>(
            string endpoint,
            string? token = null,
            Dictionary<string, string>? parameters = null)
        {
			var response = new EndpointResponse<TOutput>();

            var client = this.HttpClientFactory.CreateClient("webApi");
            if (token is not null)
            {
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);
            }
            if (parameters is not null && parameters.Any())
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, parameters!);
            }

            var httpResponse = await client.GetAsync(endpoint);
			response.IsSuccess = httpResponse.IsSuccessStatusCode;
			if (httpResponse.IsSuccessStatusCode)
			{
				response.Value = await httpResponse.Content.ReadFromJsonAsync<TOutput>();
            }
			else
			{
                var errorMessage = await httpResponse.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<RestError>(errorMessage);
                response.Error = error;
			}

            return response;
        }

        protected async Task<EndpointResponse<TOutput>> PostAsync<TInput,TOutput>(
			string endpoint, TInput input, string? token = null)
		{
			var response = new EndpointResponse<TOutput>();
			var client = this.HttpClientFactory.CreateClient("webApi");
			if (token is not null)
			{
                client.DefaultRequestHeaders.Authorization
					= new AuthenticationHeaderValue("Bearer", token);
            }
			var httpResponse = await client.PostAsJsonAsync(endpoint, input);
			response.IsSuccess = httpResponse.IsSuccessStatusCode;
			if (httpResponse.IsSuccessStatusCode)
			{
                response.Value = await httpResponse.Content.ReadFromJsonAsync<TOutput>();
			}
			else
			{
                response.Error = GetError(
                    await httpResponse.Content.ReadAsStringAsync());
            }

			return response;
		}


        protected async Task<EndpointResponse<Guid>> PutAsync<TInput>(
            string endpoint, TInput model, string? token = null)
        {
            var response = new EndpointResponse<Guid>();
            if (model is not null)
            {
                var client = this.HttpClientFactory.CreateClient("webApi");
                if (token is not null)
                {
                    client.DefaultRequestHeaders.Authorization
                        = new AuthenticationHeaderValue("Bearer", token);
                }
                var httpResponse = await client.PutAsJsonAsync<TInput>(endpoint, model!);
                response.IsSuccess = httpResponse.IsSuccessStatusCode;
                if (!httpResponse.IsSuccessStatusCode)
                {
                    response.Error = GetError(
                        await httpResponse.Content.ReadAsStringAsync());
                }
            }

            return response;
        }

        public async Task DeleteAsync(
            string endpoint, Guid id, string? token = null)
        {
            var response = new EndpointResponse<Guid>();
            var client = this.HttpClientFactory.CreateClient("webApi");
            if (token is not null)
            {
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);
            }
            var httpResponse = await client.DeleteAsync(endpoint + id.ToString());
            response.IsSuccess = httpResponse.IsSuccessStatusCode;
            if (!httpResponse.IsSuccessStatusCode)
            {
                response.Error = GetError(
                    await httpResponse.Content.ReadAsStringAsync());
            }
        }

        protected async Task<EndpointResponse<TOutput>> PostMultipartAsync<TInput, TOutput>(
            string endpoint, TInput input, string? token = null)
        {
            var response = new EndpointResponse<TOutput>();
            var client = this.HttpClientFactory.CreateClient("webApi");
            if (token is not null)
            {
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            PropertyInfo[] properties = typeof(TInput)
				.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			using var content = new MultipartFormDataContent();
            foreach (var property in properties)
            {
                if (property.PropertyType.Name == "String")
                {
                    var value = property.GetValue(input);
                    if (value is not null) 
                    {
                        content.Add(new StringContent(value.ToString()!), property.Name);
                    }
                }

                if (property.PropertyType.Name == "IFormFile")
                {
                    FormFile file = (property.GetValue(input) as FormFile)!;
                    if (file is not null)
                    {
                        var streamContent = new StreamContent(file.OpenReadStream());
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        content.Add(streamContent, property.Name, file.FileName);
                    }

                }
            }

            request.Content = content;

            var httpResponse = await client.SendAsync(request);
            response.IsSuccess = httpResponse.IsSuccessStatusCode;
            if (httpResponse.IsSuccessStatusCode)
            {
                response.Value = await httpResponse.Content.ReadFromJsonAsync<TOutput>();
            }
            else
            {
                response.Error = GetError(
                    await httpResponse.Content.ReadAsStringAsync());
            }

            return response;
        }
        
        public async Task<EndpointResponse<List<string>>> PostOnlyFilesAsync(
            string endpoint, PicturesInputModel input, string? token = null)
        {
            var response = new EndpointResponse<List<string>>();
            var client = this.HttpClientFactory.CreateClient("webApi");
            if (token is not null)
            {
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
			using var content = new MultipartFormDataContent();
            foreach (var file in input.Files)
            {
                var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(streamContent, "Files", file.FileName);
            }
            
            request.Content = content;

            var httpResponse = await client.SendAsync(request);
            response.IsSuccess = httpResponse.IsSuccessStatusCode;
            
            if (httpResponse.IsSuccessStatusCode)
            {
                response.Value = await httpResponse.Content.ReadFromJsonAsync<List<string>>();
            }
            else
            {
                response.Error = GetError(
                    await httpResponse.Content.ReadAsStringAsync());
            }
            return response;
        }

        public string? GetJwtToken()
		{
            var cookieName = this._configuration["Backend:JwtTokenName"] ?? string.Empty;
            var jwtToken = this._httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
			return jwtToken;
		}

        public string CurrentUserId()
        {
            string id =
                this.HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? string.Empty;
            return id;
        }

        private RestError GetError(string message) 
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var error = JsonSerializer.Deserialize<RestError>(message, options);
            return error!;
        }
            
    }
}
