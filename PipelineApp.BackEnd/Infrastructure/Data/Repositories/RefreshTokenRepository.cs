// <copyright file="RefreshTokenRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces.Repositories;
    using Neo4jClient;
    using Relationships;

    /// <inheritdoc />
    public class RefreshTokenRepository : BaseRepository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
        /// </summary>
        /// <param name="graphClient">The graph client.</param>
        public RefreshTokenRepository(GraphClient graphClient)
            : base(graphClient)
        {
        }

        /// <inheritdoc />
        public async Task SaveRefreshTokenForUser(RefreshTokenEntity refreshToken, UserEntity userEntity)
        {
            await GraphClient.Cypher
                .Match($"(user:{typeof(UserEntity).Name})")
                .Where((UserEntity user) => user.Id == userEntity.Id)
                .Create($"(token:{typeof(RefreshTokenEntity).Name} {{newToken}})<-[:{typeof(IsValidatedBy).Name}]-(user)")
                .WithParam("newToken", refreshToken)
                .ExecuteWithoutResultsAsync();
        }

        /// <inheritdoc />
        public async Task<UserEntity> GetValidUserForToken(string refreshToken)
        {
            var now = DateTime.UtcNow;
            var result = await GraphClient.Cypher
                .OptionalMatch(
                    $"(user:{typeof(UserEntity).Name})-[:{typeof(IsValidatedBy).Name}]->(token:{typeof(RefreshTokenEntity).Name})")
                .Where((RefreshTokenEntity token) => token.Token == refreshToken && token.ExpiresUtc > now)
                .Return((user, token) => user.As<UserEntity>())
                .ResultsAsync;
            return result.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<RefreshTokenEntity> GetByTokenString(string refreshToken)
        {
            var result = await GraphClient.Cypher
                .OptionalMatch($"(token:{typeof(RefreshTokenEntity).Name})")
                .Where((RefreshTokenEntity token) => token.Token == refreshToken)
                .Return(token => token.As<RefreshTokenEntity>())
                .ResultsAsync;
            return result.FirstOrDefault();
        }
    }
}
