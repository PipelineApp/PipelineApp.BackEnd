// <copyright file="AuthService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using Exceptions.Account;
    using Interfaces.Services;
    using Models.Configuration;
    using Models.DomainModels;
    using Models.ViewModels.Auth;

    /// <inheritdoc cref="IAuthService"/>
    public class AuthService : IAuthService
    {
        /// <inheritdoc />
        public async Task<AuthenticationResult> AuthenticateUser(string username, string password, HttpClient client, AppSettings config)
        {
            var body = new
            {
                grant_type = "password",
                client_id = config.Auth.ClientId,
                client_secret = config.Auth.ClientSecret,
                audience = config.Auth.ApiIdentifier,
                scope = "offline_access",
                username,
                password
            };
            var response = await client.PostAsJsonAsync("oauth/token", body);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidCredentialException();
            }
            var result = await response.Content.ReadAsAsync<AuthenticationResult>();
            return result;
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> GetRefreshedToken(string refreshToken, HttpClient client, AppSettings config)
        {
            var body = new
            {
                grant_type = "refresh_token",
                client_id = config.Auth.ClientId,
                client_secret = config.Auth.ClientSecret,
                refresh_token = refreshToken
            };
            var response = await client.PostAsJsonAsync("oauth/token", body);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidRefreshTokenException();
            }
            var result = await response.Content.ReadAsAsync<AuthenticationResult>();
            return result;
        }

        /// <inheritdoc />
        public async Task RevokeRefreshToken(string refreshToken, HttpClient client, AppSettings config)
        {
            var body = new
            {
                client_id = config.Auth.ClientId,
                client_secret = config.Auth.ClientSecret,
                token = refreshToken
            };
            var response = await client.PostAsJsonAsync("oauth/revoke", body);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidRefreshTokenException();
            }
        }
    }
}
