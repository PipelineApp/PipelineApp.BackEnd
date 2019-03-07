﻿// <copyright file="AuthController.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Data.Constants;
    using Infrastructure.Data.Entities;
    using Infrastructure.Exceptions.Account;
    using Interfaces;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models.Configuration;
    using Models.RequestModels;
    using Models.ViewModels.Auth;

    /// <summary>
    /// Controller class for authentication-related behavior.
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AppSettings _config;
        private readonly IAuthService _authService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="authService">The authentication service.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="refreshTokenRepository">The refresh token repository.</param>
        public AuthController(
            ILogger<AuthController> logger,
            IOptions<AppSettings> config,
            IAuthService authService,
            UserManager<UserEntity> userManager,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _logger = logger;
            _config = config.Value;
            _authService = authService;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
        }

        /// <summary>
        /// Processes an attempt to authenticate a user's login credentials
        /// and provide them with a JWT and refresh token.
        /// </summary>
        /// <param name="model">Request object containing the user's
        /// login credentials.</param>
        /// <returns>
        /// HTTP response containing information about the operation success or failure and,
        /// if successful, the JWT and refresh token in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful authentication and token generation</description></item>
        /// <item><term>400 Bad Request</term><description>Response code for invalid username or password</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost("api/auth/token")]
        [ProducesResponseType(200, Type = typeof(AuthTokenCollection))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateToken([FromBody] LoginRequest model)
        {
            try
            {
                var user = await _authService.GetUserByUsername(model.Username, _userManager);
                await _authService.ValidatePassword(user, model.Password, _userManager);
                var jwt = _authService.GenerateJwt(user, _userManager, _config);
                var refreshToken = await _authService.GenerateRefreshToken(user, _config, _refreshTokenRepository);
                return Ok(new AuthTokenCollection
                {
                    Token = jwt,
                    RefreshToken = refreshToken
                });
            }
            catch (UserNotFoundException)
            {
                _logger.LogWarning($"Login failure for {model.Username}. No user exists with this username or email address.");
                return BadRequest("Invalid username or password.");
            }
            catch (InvalidCredentialException)
            {
                _logger.LogWarning($"Login failure for {model.Username}. Error validating password.");
                return BadRequest("Invalid username or password.");
            }
            catch (Exception ex)
            {
                _logger.LogError(default(EventId), ex, $"Error creating JWT: {ex.Message}");
                return StatusCode(500, "Failed to create JWT.");
            }
        }

        /// <summary>
        /// Processes a request to refresh a user's JWT security token using a provided
        /// refresh token.
        /// </summary>
        /// <param name="model">Request object containing information about the refresh request.</param>
        /// <returns>
        /// HTTP response containing information about the operation success or failure and,
        /// if successful, the JWT and refresh token in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful token refresh</description></item>
        /// <item><term>498 Invalid Token</term><description>Response code for requests that cannot be completed due to expired refresh token</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost("api/auth/refresh")]
        [ProducesResponseType(200, Type = typeof(AuthTokenCollection))]
        [ProducesResponseType(498)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            try
            {
                _logger.LogInformation("Received token refresh request.");
                var user = await _authService.GetUserForRefreshToken(model.RefreshToken, _refreshTokenRepository);
                var jwt = _authService.GenerateJwt(user, _userManager, _config);
                var refreshToken = await _authService.GenerateRefreshToken(user, _config, _refreshTokenRepository);
                _logger.LogInformation("Processed token refresh request.");
                return Ok(new AuthTokenCollection
                {
                    Token = jwt,
                    RefreshToken = refreshToken
                });
            }
            catch (InvalidRefreshTokenException)
            {
                return StatusCode(498);
            }
            catch (Exception ex)
            {
                _logger.LogError(default(EventId), ex, $"Error refreshing JWT: {ex.Message}");
                return StatusCode(500, "Failed to create JWT.");
            }
        }

        /// <summary>
        /// Processes a request to invalidate a user's authentication refresh token.
        /// </summary>
        /// <param name="model">Request object containing the refresh token to be invalidated.</param>
        /// <returns>
        /// HTTP response containing information about the operation success or failure.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful authentication token invalidation</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost("api/auth/revoke")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest model)
        {
            try
            {
                await _authService.RevokeRefreshToken(model.RefreshToken, _refreshTokenRepository);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(default(EventId), ex, $"Error revoking JWT: {ex.Message}");
                return StatusCode(500, "Failed to revoke JWT.");
            }
        }

        /// <summary>
	    /// Processes a user's request to register a new account.
	    /// </summary>
	    /// <param name="model">Request object containing the user's account information.</param>
	    /// <returns>
	    /// HTTP response containing information about the operation success or failure.<para />
	    /// <list type="table">
	    /// <item><term>200 OK</term><description>Response code for successful account creation</description></item>
	    /// <item><term>400 Bad Request</term><description>Response code for unsuccessful account creation</description></item>
	    /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
	    /// </list>
	    /// </returns>
        [HttpPost("api/auth/register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(List<string>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                model.AssertIsValid();
                var user = new UserEntity
                {
                    UserName = model.Email,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth.GetValueOrDefault(),
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await _authService.AssertUserInformationDoesNotExist(model.Email, _userManager);
                var createdUser = await _authService.Signup(user, model.Password, _userManager);
                await _authService.AddUserToRole(createdUser, Roles.USER, _userManager);
                _logger.LogInformation(3, $"User {model.Email} created a new account with password.");
                return Ok();
            }
            catch (InvalidRegistrationException e)
            {
                _logger.LogError(e, $"Error registering user with email {model.Email}");
                return BadRequest(e.Errors);
            }
            catch (InvalidAccountInfoUpdateException e)
            {
                _logger.LogError(e, $"Error adding {model.Email} to user role.");
                return BadRequest(e.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error registering user with email {model.Email}");
                return StatusCode(500, new List<string> { "Error creating account. An account with some or all of this information may already exist." });
            }
        }

        /// <summary>
        /// Processes a user's request to receive a password reset link.
        /// </summary>
        /// <param name="model">Request object containing the details of the password reset request.</param>
        /// <returns>
        /// HTTP response containing information about the operation success or failure.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful processing of request</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost("api/auth/forgotpassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestModel model)
        {
            throw new NotImplementedException();
            /*
            try
            {
                var user = await _authService.GetUserByUsernameOrEmail(model.Email, _userManager);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var email = _emailBuilder.BuildForgotPasswordEmail(user, _config.Cors.CorsUrl, code, _config);
                await _emailClient.SendEmail(email, _config);
                return Ok();
            }
            catch (InvalidCredentialException e)
            {
                _logger.LogWarning(e, $"Password reset requested for nonexistant email {model.Email}.");

                // Prevent account scraping by not informing front-end whether user exists or not.
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error requesting password reset for {model.Email}");
                return StatusCode(500, "An unexpected error occurred.");
            }
            */
        }

        /// <summary>
        /// Processes a user's request to reset their password using a security key.
        /// </summary>
        /// <param name="model">Request object containing information about the password reset request.</param>
        /// <returns>
        /// HTTP response containing information about the operation success or failure.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful password reset</description></item>
        /// <item><term>400 Bad Request</term><description>Response code for unsuccessful password reset</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost("api/auth/resetpassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(500)]
        public Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            throw new NotImplementedException();
            /*
            try
            {
                await _authService.ResetPassword(model.Email, model.Code, model.NewPassword, model.ConfirmNewPassword, _userManager);
                return Ok();
            }
            catch (InvalidChangePasswordException e)
            {
                _logger.LogError(e, $"Error resetting password for {model.Email}: {e.Errors.Join()}");
                return BadRequest(e.Errors.Join(" "));
            }
            catch (InvalidPasswordResetTokenException e)
            {
                _logger.LogError(e, $"Error resetting password for {model.Email}: {e.Errors.Join(",")}");
                return BadRequest("This reset token is invalid. Please request a new password reset link.");
            }
            catch (InvalidCredentialException e)
            {
                _logger.LogError(e, $"Error resetting password for {model.Email}. User does not exist.");
                return BadRequest("This reset token is invalid. Please request a new password reset link.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error requesting password reset for {model.Email}");
                return StatusCode(500, "An unexpected error occurred.");
            }
            */
        }
    }
}
