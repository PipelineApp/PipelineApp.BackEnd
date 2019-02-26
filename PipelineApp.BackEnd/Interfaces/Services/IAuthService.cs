// <copyright file="IAuthService.cs" company="Blackjack Software">
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
        /// Registers a user with the application.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="userManager">The user manager.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the created user entity in the database.
        /// </returns>
        Task<UserEntity> Signup(UserEntity user, string password, UserManager<UserEntity> userManager);

        /// <summary>
        /// Throws an exception if the user with the given information
        /// already exists in the database.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="userManager">The user manager.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task AssertUserInformationDoesNotExist(string email, UserManager<UserEntity> userManager);

        /// <summary>
        /// Grants the given user a particular usage role in the application.
        /// </summary>
        /// <param name="user">The user to be modified.</param>
        /// <param name="role">The name of the role to which the user should be added.</param>
        /// <param name="userManager">The user manager.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task AddUserToRole(UserEntity user, string role, UserManager<UserEntity> userManager);
    }
}
