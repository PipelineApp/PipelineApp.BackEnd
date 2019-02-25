﻿// <copyright file="IAuthService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Models.Configuration;
    using Models.DomainModels;
    using Models.DomainModels.Auth;
    using Repositories;

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
        /// The task result contains an <see cref="AuthenticationSuccessResult"/> object containing
        /// authentication tokens for the authenticated user.
        /// </returns>
        Task<AuthenticationSuccessResult> AuthenticateUser(string username, string password, HttpClient client, AppSettings config);

        /// <summary>
        /// Fetches a refreshed access token associated with the given refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="client">The HTTP client for communication with auth server.</param>
        /// <param name="config">The app config.</param>
        /// <returns>
        /// A task respresenting the asynchronous operations.
        /// The task result contains an <see cref="AuthenticationSuccessResult"/> object
        /// containing updated authentication tokens.
        /// </returns>
        Task<AuthenticationSuccessResult> GetRefreshedToken(string refreshToken, HttpClient client, AppSettings config);

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

        /// <summary>
        /// Registers a user account in the application.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password">The account password.</param>
        /// <param name="userManager"></param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="email">The account email.</param>
        /// <param name="dateOfBirth">The account date of birth.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="client">The HTTP client for communication with auth server.</param>
        /// <param name="config">The app config.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task<UserEntity> Signup(UserEntity user, string password, UserManager<UserEntity> userManager);

        Task AssertUserInformationDoesNotExist(string email, UserManager<UserEntity> userManager);
        Task AddUserToRole(UserEntity user, string role, UserManager<UserEntity> userManager);
    }
}
