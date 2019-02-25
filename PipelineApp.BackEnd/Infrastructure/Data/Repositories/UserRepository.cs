namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Neo4jClient;

    public class UserRepository : BaseRepository<UserEntity>, IUserStore<UserEntity>, IUserEmailStore<UserEntity>, IUserPhoneNumberStore<UserEntity>,
        IUserTwoFactorStore<UserEntity>, IUserPasswordStore<UserEntity>, IUserRepository
    {
        public UserRepository(GraphClient graphClient) : base(graphClient) { }

        public async Task<IdentityResult> CreateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await SaveAsync(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await DeleteAsync(user);
            return IdentityResult.Success;
        }

        public async Task<UserEntity> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await GetByIdAsync(new Guid(userId));
            return result;
        }

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

        public Task<string> GetNormalizedUserNameAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(UserEntity user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(UserEntity user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await UpdateAsync(user);

            return IdentityResult.Success;
        }

        public Task SetEmailAsync(UserEntity user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(UserEntity user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

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

        public Task<string> GetNormalizedEmailAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(UserEntity user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(UserEntity user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(UserEntity user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(UserEntity user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(UserEntity user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
        }
    }
}
