// <copyright file="IAuthService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System.Threading.Tasks;
    using Infrastructure.Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Models.Configuration;
    using Models.ViewModels.Auth;
    using Repositories;

    /// <summary>
    /// Service for data manipulation relating to user authentication.
    /// </summary>
    public interface IAuthService
    {
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

        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">A username string.</param>
        /// <param name="userManager">The user manager.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="UserEntity"/> associated with the given username.
        /// </returns>
        Task<UserEntity> GetUserByUsername(string username, UserManager<UserEntity> userManager);

        /// <summary>
        /// Validates a password for a given user.
        /// </summary>
        /// <param name="user">The user to be validated against.</param>
        /// <param name="password">The password to be validated.</param>
        /// <param name="userManager">The user manager.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task ValidatePassword(UserEntity user, string password, UserManager<UserEntity> userManager);

        /// <summary>
        /// Generates a JWT for the given user's claims.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>
        /// An <see cref="AuthToken"/> containing the JWT information.
        /// </returns>
        AuthToken GenerateJwt(UserEntity user, UserManager<UserEntity> userManager, AppSettings config);

        /// <summary>
        /// Generates a refresh token for the given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="config">The user manager.</param>
        /// <param name="refreshTokenRepository">The refresh token repository.</param>
        /// <returns>
        /// An <see cref="AuthToken" /> containing the refresh token information information.
        /// </returns>
        AuthToken GenerateRefreshToken(UserEntity user, AppSettings config, IRefreshTokenRepository refreshTokenRepository);

        /// <summary>
        /// Gets the user with whom the given refresh token is associated.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="refreshTokenRepository">The refresh token repository.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <see cref="IdentityUser"/> associated with the given refresh token.
        /// </returns>
        Task<UserEntity> GetUserForRefreshToken(string refreshToken, IRefreshTokenRepository refreshTokenRepository);

        /// <summary>
        /// Revokes the given refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to be revoked.</param>
        /// <param name="refreshTokenRepository">The refresh token repository.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task RevokeRefreshToken(string refreshToken, IRefreshTokenRepository refreshTokenRepository);
    }
}
