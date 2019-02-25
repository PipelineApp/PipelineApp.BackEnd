// <copyright file="AuthService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Entities;
    using Exceptions.Account;
    using Interfaces;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Identity;
    using Models.Configuration;
    using Models.DomainModels;
    using Models.DomainModels.Auth;

    /// <inheritdoc cref="IAuthService"/>
    public class AuthService : IAuthService
    {
        /// <inheritdoc />
        public async Task<AuthenticationSuccessResult> AuthenticateUser(string username, string password, HttpClient client, AppSettings config)
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
                var failureResult = await response.Content.ReadAsAsync<AuthenticationFailureResult>();
                throw new InvalidCredentialException(failureResult.ErrorDescription);
            }

            var successResult = await response.Content.ReadAsAsync<AuthenticationSuccessResult>();
            return successResult;
        }

        /// <inheritdoc />
        public async Task<AuthenticationSuccessResult> GetRefreshedToken(string refreshToken, HttpClient client, AppSettings config)
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
            var result = await response.Content.ReadAsAsync<AuthenticationSuccessResult>();
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

        /// <inheritdoc />
        public async Task<UserEntity> Signup(UserEntity user, string password, UserManager<UserEntity> userManager)
        {
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidRegistrationException(result.Errors.Select(e => e.Description).ToList());
            }

            return user;
        }

        public async Task AssertUserInformationDoesNotExist(string email, UserManager<UserEntity> userManager)
        {
            var userByEmail = await userManager.FindByEmailAsync(email);
            if (userByEmail != null)
            {
                throw new InvalidRegistrationException(new List<string> { "Error creating account. An account with some or all of this information may already exist." });
            }
        }

        public async Task AddUserToRole(UserEntity user, string role, UserManager<UserEntity> userManager)
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidAccountInfoUpdateException(roleResult.Errors.Select(e => e.Description).ToList());
            }
        }
    }
}
