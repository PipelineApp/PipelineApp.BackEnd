// <copyright file="RoleRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Neo4jClient;

    /// <summary>
    /// Extension of base repository containing methods related to
    /// role data.
    /// </summary>
    public class RoleRepository : BaseRepository<RoleEntity>, IRoleStore<RoleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="graphClient">The graph client.</param>
        public RoleRepository(IGraphClient graphClient)
            : base(graphClient)
        {
        }

        /// <inheritdoc />
        public Task<IdentityResult> CreateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CreateWithRelationships(role);
            return Task.FromResult(IdentityResult.Success);
        }

        /// <inheritdoc />
        public async Task<IdentityResult> UpdateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await UpdateAsync(role);
            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public async Task<IdentityResult> DeleteAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await DeleteAsync(role.Id);
            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public Task<string> GetRoleIdAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        /// <inheritdoc />
        public Task<string> GetRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        /// <inheritdoc />
        public Task SetRoleNameAsync(RoleEntity role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task<string> GetNormalizedRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        /// <inheritdoc />
        public Task SetNormalizedRoleNameAsync(RoleEntity role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public async Task<RoleEntity> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await GetByIdAsync(new Guid(roleId));
        }

        /// <inheritdoc />
        public async Task<RoleEntity> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var results = await GraphClient.Cypher
                .Match($"(e:{typeof(RoleEntity).Name})")
                .Where((RoleEntity e) => e.NormalizedName == normalizedRoleName)
                .Return(e => e.As<RoleEntity>())
                .ResultsAsync;
            return results.SingleOrDefault();
        }
    }
}
