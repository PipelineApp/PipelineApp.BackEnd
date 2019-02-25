namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Neo4jClient;

    public class RoleRepository : BaseRepository<RoleEntity>, IRoleStore<RoleEntity>
    {
        private readonly string _connectionString;

        public RoleRepository(GraphClient graphClient) : base(graphClient)
        {
        }

        public async Task<IdentityResult> CreateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await SaveAsync(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await UpdateAsync(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await DeleteAsync(role);
            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(RoleEntity role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(RoleEntity role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<RoleEntity> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await GetByIdAsync(new Guid(roleId));
        }

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

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}
