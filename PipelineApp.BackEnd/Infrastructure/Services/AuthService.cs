﻿// <copyright file="AuthService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Authentication;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Data.Entities;
    using Data.Relationships;
    using Data.Requests;
    using Exceptions.Account;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using Models.Configuration;
    using Models.ViewModels.Auth;

    /// <inheritdoc cref="IAuthService"/>
    public class AuthService : IAuthService
    {
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

        /// <inheritdoc />
        public async Task AssertUserInformationDoesNotExist(string email, UserManager<UserEntity> userManager)
        {
            var userByEmail = await userManager.FindByEmailAsync(email);
            if (userByEmail != null)
            {
                throw new InvalidRegistrationException(new List<string> { "Error creating account. An account with some or all of this information may already exist." });
            }
        }

        /// <inheritdoc />
        public async Task AddUserToRole(UserEntity user, string role, UserManager<UserEntity> userManager)
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidAccountInfoUpdateException(roleResult.Errors.Select(e => e.Description).ToList());
            }
        }

        /// <inheritdoc />
        public async Task<UserEntity> GetUserByUsername(string username, UserManager<UserEntity> userManager)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        /// <inheritdoc />
        public async Task ValidatePassword(UserEntity user, string password, UserManager<UserEntity> userManager)
        {
            var verificationResult = await userManager.CheckPasswordAsync(user, password);
            if (!verificationResult)
            {
                throw new InvalidCredentialException();
            }
        }

        /// <inheritdoc />
        public AuthToken GenerateJwt(UserEntity user, UserManager<UserEntity> userManager, AppSettings config)
        {
            var claims = GetUserClaims(user);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Auth.Key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(config.Auth.AccessExpireMinutes);
            var token = new JwtSecurityToken(
                config.Auth.Issuer,
                config.Auth.Audience,
                claims,
                expires: expiry,
                signingCredentials: creds);
            var jwtString = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthToken(jwtString, expiry);
        }

        /// <inheritdoc />
        public async Task<AuthToken> GenerateRefreshToken(UserEntity user, AppSettings config, IRefreshTokenRepository refreshTokenRepository)
        {
            var now = DateTime.UtcNow;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Auth.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(config.Auth.RefreshExpireMinutes);
            var jwt = new JwtSecurityToken(
                config.Auth.Issuer,
                config.Auth.Audience,
                claims,
                expires: expiry,
                signingCredentials: signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var entity = new RefreshTokenEntity
            {
                Token = token,
                IssuedUtc = now,
                ExpiresUtc = expiry
            };
            var request = new CreateNodeRequest<RefreshTokenEntity>(entity)
                .WithInboundRelationshipFrom<IsValidatedBy>(user.Id);
            refreshTokenRepository.CreateWithRelationships(request);
            return new AuthToken(token, expiry);
        }

        /// <inheritdoc />
        public async Task<UserEntity> GetUserForRefreshToken(string refreshToken, IRefreshTokenRepository refreshTokenRepository)
        {
            var user = await refreshTokenRepository.GetValidUserForToken(refreshToken);
            if (user == null)
            {
                throw new InvalidRefreshTokenException();
            }
            return user;
        }

        /// <inheritdoc />
        public async Task RevokeRefreshToken(string refreshToken, IRefreshTokenRepository refreshTokenRepository)
        {
            var token = await refreshTokenRepository.GetByTokenString(refreshToken);
            if (token != null)
            {
                await refreshTokenRepository.DeleteAsync(token.Id);
            }
        }

        private static IEnumerable<Claim> GetUserClaims(UserEntity user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };
            return claims;
        }
    }
}
