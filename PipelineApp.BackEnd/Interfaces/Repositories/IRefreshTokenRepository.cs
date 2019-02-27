// <copyright file="IRefreshTokenRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Repositories
{
    using System.Threading.Tasks;
    using Infrastructure.Data.Entities;

    /// <summary>
    /// Extension of base repository containing methods related to
    /// refresh token data.
    /// </summary>
    public interface IRefreshTokenRepository : IRepository<RefreshTokenEntity>
    {
        /// <summary>
        /// Saves a refresh token in the database and associates it with a given user.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="user">The user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveRefreshTokenForUser(RefreshTokenEntity refreshToken, UserEntity user);

        /// <summary>
        /// Retrieves the user validated by the given refresh token,
        /// or null if the token does not exist or has expired.
        /// </summary>
        /// <param name="refreshToken">The token to search for.</param>
        /// <returns>The user validated by the given refresh token, or null if the token does not exist or has expired.</returns>
        Task<UserEntity> GetValidUserForToken(string refreshToken);
    }
}
