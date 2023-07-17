using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using Web.Models.Response;

namespace Web.Controllers
{
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

		public IHttpClientFactory HttpClientFactory => _httpClientFactory;

		public IConfiguration Configuration => _configuration;

		public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;

        public async Task<EndpointResponse<TOutput>> GetAsync<TOutput>(
			string endpoint, string? token = null)
        {
			var response = new EndpointResponse<TOutput>();

            var client = this.HttpClientFactory.CreateClient("webApi");
            if (token is not null)
            {
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);
            }
            var httpResponse = await client.GetAsync(endpoint);
			response.IsSuccess = httpResponse.IsSuccessStatusCode;
			if (httpResponse.IsSuccessStatusCode)
			{
				response.Value = await httpResponse.Content.ReadFromJsonAsync<TOutput>();
            }
			else
			{
				response.Error = await httpResponse.Content.ReadAsStringAsync();
			}

            return response;
        }

        public async Task<EndpointResponse<TOutput>> PostAsync<TInput,TOutput>(
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
                response.Error = await httpResponse.Content.ReadAsStringAsync();
            }

			return response;
		}

        public async Task<EndpointResponse<TOutput>> PostMultipartAsync<TInput, TOutput>(
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
            
            //var files = new List<FormFile>();

			using var content = new MultipartFormDataContent();
            foreach (var property in properties)
            {
                if (property is not null)
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
                        FormFile file = property.GetValue(input) as FormFile;
                       
                        if (file is not null)
                        {
                            var streamContent = new StreamContent(file.OpenReadStream());
                            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                            content.Add(streamContent, file.Name, file.FileName);
                        }
                    }
                }
            }

            //foreach (var file in files)
            //{
            //    var stream = file.OpenReadStream();
            //    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            //    content.Add(new StreamContent(stream), file.Name, file.FileName);
            //}

            request.Content = content;

            var httpResponse = await client.SendAsync(request);
            response.IsSuccess = httpResponse.IsSuccessStatusCode;
            if (httpResponse.IsSuccessStatusCode)
            {
                response.Value = await httpResponse.Content.ReadFromJsonAsync<TOutput>();
            }
            else
            {
                response.Error = await httpResponse.Content.ReadAsStringAsync();
            }

            return response;
        }

        public string? GetJwtToken()
		{
            var cookieName = this._configuration["Backend:JwtTokenName"] ?? string.Empty;
            var jwtToken = this._httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
			return jwtToken;
		}
	}
}
