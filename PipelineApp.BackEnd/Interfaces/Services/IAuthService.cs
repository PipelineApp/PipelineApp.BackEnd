// <copyright file="IAuthService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models.Configuration;
    using Models.DomainModels;
    using Models.ViewModels.Auth;

    /// <summary>
    /// Service for data manipulation relating to user authentication.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Validates a username and password combination via communication with the auth server.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="client">The HTTP client for communication with auth server.</param>
        /// <param name="config">The app config.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains an <see cref="AuthenticationResult"/> object containing
        /// authentication tokens for the authenticated user.
        /// </returns>
        Task<AuthenticationResult> AuthenticateUser(string username, string password, HttpClient client, AppSettings config);

        /// <summary>
        /// Fetches a refreshed access token associated with the given refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="client">The HTTP client for communication with auth server.</param>
        /// <param name="config">The app config.</param>
        /// <returns>
        /// A task respresenting the asynchronous operations.
        /// The task result contains an <see cref="AuthenticationResult"/> object
        /// containing updated authentication tokens.
        /// </returns>
        Task<AuthenticationResult> GetRefreshedToken(string refreshToken, HttpClient client, AppSettings config);

        /// <summary>
        /// Revokes the given refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="client">The HTTP client for communication with auth server.</param>
        /// <param name="config">The app config.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task RevokeRefreshToken(string refreshToken, HttpClient client, AppSettings config);
    }
}
