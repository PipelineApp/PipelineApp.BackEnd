// <copyright file="UserRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Neo4jClient;
    using Relationships;

    /// <summary>
    /// Extension of base repository containing methods related to
    /// user data.
    /// </summary>
    public class UserRepository : BaseRepository<UserEntity>, IUserEmailStore<UserEntity>, IUserPhoneNumberStore<UserEntity>,
        IUserTwoFactorStore<UserEntity>, IUserPasswordStore<UserEntity>, IUserRoleStore<UserEntity>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="graphClient">The graph client.</param>
        public UserRepository(IGraphClient graphClient)
            : base(graphClient)
        {
        }

        /// <inheritdoc />
        public async Task<IdentityResult> CreateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await SaveAsync(user);
            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public async Task<IdentityResult> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await DeleteAsync(user);
            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public async Task<UserEntity> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await GetByIdAsync(new Guid(userId));
            return result;
        }

        /// <inheritdoc />
        public async Task<UserEntity> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var results = await GraphClient.Cypher
                .Match($"(e:{typeof(UserEntity).Name})")
                .Where((UserEntity e) => e.NormalizedUserName == normalizedUserName)
                .Return(e => e.As<UserEntity>())
                .ResultsAsync;
            return results.SingleOrDefault();
        }

        /// <inheritdoc />
        public Task<string> GetNormalizedUserNameAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        /// <inheritdoc />
        public Task<string> GetUserIdAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        /// <inheritdoc />
        public Task<string> GetUserNameAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        /// <inheritdoc />
        public Task SetNormalizedUserNameAsync(UserEntity user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task SetUserNameAsync(UserEntity user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public async Task<IdentityResult> UpdateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await UpdateAsync(user);

            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public Task SetEmailAsync(UserEntity user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task<string> GetEmailAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        /// <inheritdoc />
        public Task<bool> GetEmailConfirmedAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        /// <inheritdoc />
        public Task SetEmailConfirmedAsync(UserEntity user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public async Task<UserEntity> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var results = await GraphClient.Cypher
                .Match($"(e:{typeof(UserEntity).Name})")
                .Where((UserEntity e) => e.NormalizedEmail == normalizedEmail)
                .Return(e => e.As<UserEntity>())
                .ResultsAsync;
            return results.SingleOrDefault();
        }

        /// <inheritdoc />
        public Task<string> GetNormalizedEmailAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        /// <inheritdoc />
        public Task SetNormalizedEmailAsync(UserEntity user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task SetPhoneNumberAsync(UserEntity user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task<string> GetPhoneNumberAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        /// <inheritdoc />
        public Task<bool> GetPhoneNumberConfirmedAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <inheritdoc />
        public Task SetPhoneNumberConfirmedAsync(UserEntity user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task SetTwoFactorEnabledAsync(UserEntity user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task<bool> GetTwoFactorEnabledAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <inheritdoc />
        public Task SetPasswordHashAsync(UserEntity user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task<string> GetPasswordHashAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        /// <inheritdoc />
        public Task<bool> HasPasswordAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        /// <inheritdoc />
        public async Task AddToRoleAsync(UserEntity userEntity, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await GraphClient.Cypher
                .Match($"(user:{typeof(UserEntity).Name})", $"(role:{typeof(RoleEntity).Name})")
                .Where((UserEntity user) => user.Id == userEntity.Id)
                .AndWhere((RoleEntity role) => role.NormalizedName == roleName)
                .Create($"(user)-[:{typeof(BelongsTo).Name}]->(role)")
                .ExecuteWithoutResultsAsync();
        }

        /// <inheritdoc />
        public async Task RemoveFromRoleAsync(UserEntity userEntity, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await GraphClient.Cypher
                .OptionalMatch($"(user:{typeof(UserEntity).Name})-[r:{typeof(BelongsTo).Name}]->(role:{typeof(RoleEntity).Name})")
                .Where((UserEntity user) => user.Id == userEntity.Id)
                .AndWhere((RoleEntity role) => role.NormalizedName == roleName)
                .Delete("r")
                .ExecuteWithoutResultsAsync();
        }

        /// <inheritdoc />
        public async Task<IList<string>> GetRolesAsync(UserEntity userEntity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await GraphClient.Cypher
                .Match($"(user:{typeof(UserEntity).Name})-[r:{typeof(BelongsTo).Name}]->(role:{typeof(RoleEntity).Name})")
                .Where((UserEntity user) => user.Id == userEntity.Id)
                .Return(role => role.As<RoleEntity>())
                .ResultsAsync;
            return result.Select(r => r.Name).ToList();
        }

        /// <inheritdoc />
        public async Task<bool> IsInRoleAsync(UserEntity userEntity, string roleName, CancellationToken cancellationToken)
        {
            var roles = await GetRolesAsync(userEntity, cancellationToken);
            return roles.Contains(roleName);
        }

        /// <inheritdoc />
        public async Task<IList<UserEntity>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await GraphClient.Cypher
                .OptionalMatch($"(user:{typeof(UserEntity).Name})-[r:{typeof(BelongsTo).Name}]->(role:{typeof(RoleEntity).Name})")
                .Where((RoleEntity role) => role.NormalizedName == roleName)
                .Return(user => user.As<UserEntity>())
                .ResultsAsync;
            return result.ToList();
        }
    }
}
