using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;

namespace FreeCourse.Web.Services.Interfaces
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public ClientCredentialTokenService(
            IOptions<ServiceApiSettings> serviceApisettings,
            IOptions<ClientSettings> clientSettings, 
            IClientAccessTokenCache clientAccessTokenCache,
            HttpClient httpClient
            )
        {
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
            _serviceApiSettings = serviceApisettings.Value;
            _clientSettings = clientSettings.Value;
        }
        public async Task<string> GetToken()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken", null);
            if (currentToken != null)
                return currentToken.AccessToken;

            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var clientCredentialTokenRequeest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address = discovery.TokenEndpoint
            };

            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequeest);

            if (newToken.IsError)
                throw newToken.Exception;


            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn, null);

            return newToken.AccessToken;

        }
    }
}
