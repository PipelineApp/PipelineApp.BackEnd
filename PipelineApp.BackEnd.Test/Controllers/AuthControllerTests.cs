// <copyright file="AuthControllerTests.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using BackEnd.Controllers;
    using BackEnd.Infrastructure.Data.Constants;
    using BackEnd.Infrastructure.Data.Entities;
    using BackEnd.Infrastructure.Exceptions.Account;
    using FluentAssertions;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models.Configuration;
    using Models.RequestModels.Auth;
    using Models.ViewModels.Auth;
    using Moq;
    using TestHelpers;
    using Xunit;

    [Trait("Class", "AuthController")]
    public class AuthControllerTests : ControllerTests<AuthController>
    {
        private readonly Mock<UserManager<UserEntity>> _mockUserManager;
        private readonly AppSettings _mockConfig;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;

        public AuthControllerTests()
        {
            var mockLogger = new Mock<ILogger<AuthController>>();
            var userStoreMock = new Mock<IUserStore<UserEntity>>();
            _mockUserManager = new Mock<UserManager<UserEntity>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _mockAuthService = new Mock<IAuthService>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockConfig = new AppSettings();
            var configWrapper = new Mock<IOptions<AppSettings>>();
            configWrapper.SetupGet(c => c.Value).Returns(_mockConfig);
            Controller = new AuthController(mockLogger.Object, configWrapper.Object, _mockAuthService.Object, _mockUserManager.Object, _mockRefreshTokenRepository.Object);
        }

        public class CreateToken : AuthControllerTests
        {
            [Fact]
            public async Task ReturnsBadRequestWhenUserNotFound()
            {
                // Arrange
                var model = new LoginRequest
                {
                    Username = "my-username",
                    Password = "my-password"
                };
                _mockAuthService.Setup(s => s.GetUserByUsername("my-username", _mockUserManager.Object)).Throws<UserNotFoundException>();

                // Act
                var result = await Controller.CreateToken(model);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                _mockAuthService.Verify(s => s.GenerateJwt(It.IsAny<UserEntity>(), _mockUserManager.Object, _mockConfig), Times.Never);
                _mockAuthService.Verify(s => s.GenerateRefreshToken(It.IsAny<UserEntity>(), _mockConfig, _mockRefreshTokenRepository.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsBadRequestWhenCredentialsAreInvalid()
            {
                // Arrange
                var model = new LoginRequest
                {
                    Username = "my-username",
                    Password = "my-password"
                };
                var user = new UserEntity { UserName = "my-username", Id = Guid.NewGuid() };
                _mockAuthService.Setup(s => s.GetUserByUsername("my-username", _mockUserManager.Object)).ReturnsAsync(user);
                _mockAuthService.Setup(s => s.ValidatePassword(user, "my-password", _mockUserManager.Object)).Throws<InvalidCredentialException>();

                // Act
                var result = await Controller.CreateToken(model);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                _mockAuthService.Verify(s => s.GenerateJwt(It.IsAny<UserEntity>(), _mockUserManager.Object, _mockConfig), Times.Never);
                _mockAuthService.Verify(s => s.GenerateRefreshToken(It.IsAny<UserEntity>(), _mockConfig, _mockRefreshTokenRepository.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedExceptionThrown()
            {
                // Arrange
                var model = new LoginRequest
                {
                    Username = "my-username",
                    Password = "my-password"
                };
                _mockAuthService
                    .Setup(s => s.GenerateJwt(It.IsAny<UserEntity>(), _mockUserManager.Object, _mockConfig))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.CreateToken(model);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenNoExceptionsThrown()
            {
                // Arrange
                var model = new LoginRequest
                {
                    Username = "my-username",
                    Password = "my-password"
                };
                var token = new AuthToken
                {
                    Token = "token",
                    Expiry = 12345
                };
                var refreshToken = new AuthToken
                {
                    Token = "refreshtoken",
                    Expiry = 54321
                };
                _mockAuthService
                    .Setup(s => s.GenerateJwt(It.IsAny<UserEntity>(), _mockUserManager.Object, _mockConfig))
                    .Returns(token);
                _mockAuthService
                    .Setup(s => s.GenerateRefreshToken(It.IsAny<UserEntity>(), _mockConfig, _mockRefreshTokenRepository.Object))
                    .ReturnsAsync(refreshToken);

                // Act
                var result = await Controller.CreateToken(model);
                var tokenCollection = ((OkObjectResult)result).Value as AuthTokenCollection;

                // Assert
                tokenCollection.Should().NotBeNull();
                tokenCollection?.Token.Should().Be(token);
                tokenCollection?.RefreshToken.Should().Be(refreshToken);
            }
        }

        public class RefreshToken : AuthControllerTests
        {
            [Fact]
            public async Task ReturnsInvalidTokenWhenTokenIsInvalid()
            {
                // Arrange
                var model = new RefreshTokenRequest
                {
                    RefreshToken = "refreshtoken"
                };
                _mockAuthService
                    .Setup(s => s.GetUserForRefreshToken(model.RefreshToken, _mockRefreshTokenRepository.Object))
                    .Throws<InvalidRefreshTokenException>();

                // Act
                var result = await Controller.RefreshToken(model);

                // Assert
                result.Should().BeOfType<StatusCodeResult>();
                ((StatusCodeResult)result).StatusCode.Should().Be(498);
                _mockAuthService.Verify(s => s.GenerateJwt(It.IsAny<UserEntity>(), _mockUserManager.Object, _mockConfig), Times.Never);
                _mockAuthService.Verify(s => s.GenerateRefreshToken(It.IsAny<UserEntity>(), _mockConfig, _mockRefreshTokenRepository.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedExceptionThrown()
            {
                // Arrange
                var model = new RefreshTokenRequest()
                {
                    RefreshToken = "refreshtoken"
                };
                _mockAuthService
                    .Setup(s => s.GetUserForRefreshToken(model.RefreshToken, _mockRefreshTokenRepository.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.RefreshToken(model);

                // Assert
                result.Should().BeOfType<ObjectResult>();
                ((ObjectResult)result).StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenNoExceptionsThrown()
            {
                // Arrange
                var model = new RefreshTokenRequest()
                {
                    RefreshToken = "refreshtoken"
                };
                var token = new AuthToken
                {
                    Token = "token",
                    Expiry = 12345
                };
                var refreshToken = new AuthToken
                {
                    Token = "refreshtoken",
                    Expiry = 54321
                };
                _mockAuthService
                    .Setup(s => s.GenerateJwt(It.IsAny<UserEntity>(), _mockUserManager.Object, _mockConfig))
                    .Returns(token);
                _mockAuthService
                    .Setup(s => s.GenerateRefreshToken(It.IsAny<UserEntity>(), _mockConfig, _mockRefreshTokenRepository.Object))
                    .ReturnsAsync(refreshToken);

                // Act
                var result = await Controller.RefreshToken(model);
                var tokenCollection = ((OkObjectResult)result).Value as AuthTokenCollection;

                // Assert
                tokenCollection.Should().NotBeNull();
                tokenCollection?.Token.Should().Be(token);
                tokenCollection?.RefreshToken.Should().Be(refreshToken);
            }
        }

        public class RevokeToken : AuthControllerTests
        {
            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                var model = new RefreshTokenRequest
                {
                    RefreshToken = "refreshtoken"
                };
                _mockAuthService.Setup(s => s.RevokeRefreshToken("refreshtoken", _mockRefreshTokenRepository.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.RevokeToken(model);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenNoExceptionsThrown()
            {
                // Arrange
                var model = new RefreshTokenRequest()
                {
                    RefreshToken = "refreshtoken"
                };

                // Act
                var result = await Controller.RevokeToken(model);

                // Assert
                result.Should().BeOfType<OkResult>();
            }
        }

        public class Register : AuthControllerTests
        {
            private readonly RegisterRequest _validRequest;

            public Register()
            {
                _validRequest = new RegisterRequest
                {
                    Email = "me@me.com",
                    Password = "my-password",
                    ConfirmPassword = "my-password",
                    DateOfBirth = DateTime.Now
                };
            }

            [Fact]
            public async Task ReturnsBadRequestWhenRegistrationIsInvalid()
            {
                // Arrange
                var model = new Mock<RegisterRequest>();
                var exception = new InvalidRegistrationException(new List<string> { "error1", "error2" });
                model.Setup(m => m.AssertIsValid()).Throws(exception);

                // Act
                var result = await Controller.Register(model.Object);
                var body = ((BadRequestObjectResult)result).Value as List<string>;

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                body.Should().HaveCount(2);
                _mockAuthService.Verify(
                    s => s.Signup(It.IsAny<UserEntity>(), It.IsAny<string>(), _mockUserManager.Object), Times.Never);
                _mockAuthService.Verify(
                    s => s.AddUserToRole(It.IsAny<UserEntity>(), It.IsAny<string>(), _mockUserManager.Object),
                    Times.Never);
            }

            [Fact]
            public async Task ReturnsBadRequestWhenUserInformationExists()
            {
                // Arrange
                var exception = new InvalidRegistrationException(new List<string> { "error1", "error2" });
                _mockAuthService.Setup(s =>
                        s.AssertUserInformationDoesNotExist("me@me.com", _mockUserManager.Object))
                    .Throws(exception);

                // Act
                var result = await Controller.Register(_validRequest);
                var body = ((BadRequestObjectResult)result).Value as List<string>;

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                body.Should().HaveCount(2);
                _mockAuthService.Verify(
                    s => s.Signup(It.IsAny<UserEntity>(), It.IsAny<string>(), _mockUserManager.Object), Times.Never);
                _mockAuthService.Verify(
                    s => s.AddUserToRole(It.IsAny<UserEntity>(), It.IsAny<string>(), _mockUserManager.Object),
                    Times.Never);
            }

            [Fact]
            public async Task ReturnsBadRequestWhenUserCreationFails()
            {
                // Arrange
                var exception = new InvalidRegistrationException(new List<string> { "error1", "error2" });
                _mockAuthService.Setup(s =>
                        s.Signup(
                            It.Is<UserEntity>(u => u.Email == "me@me.com"),
                            "my-password",
                            _mockUserManager.Object))
                    .Throws(exception);

                // Act
                var result = await Controller.Register(_validRequest);
                var body = ((BadRequestObjectResult)result).Value as List<string>;

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                body.Should().HaveCount(2);
                _mockAuthService.Verify(
                    s => s.AddUserToRole(It.IsAny<UserEntity>(), It.IsAny<string>(), _mockUserManager.Object),
                    Times.Never);
            }

            [Fact]
            public async Task ReturnsBadRequestWhenUserRoleCreationFails()
            {
                // Arrange
                var exception = new InvalidAccountInfoUpdateException(new List<string> { "error1", "error2" });
                _mockAuthService.Setup(s =>
                        s.AddUserToRole(It.IsAny<UserEntity>(), Roles.USER, _mockUserManager.Object))
                    .Throws(exception);

                // Act
                var result = await Controller.Register(_validRequest);
                var body = ((BadRequestObjectResult)result).Value as List<string>;

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                body.Should().HaveCount(2);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedExceptionThrown()
            {
                // Arrange
                _mockAuthService.Setup(s =>
                        s.Signup(It.IsAny<UserEntity>(), It.IsAny<string>(), _mockUserManager.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.Register(_validRequest);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenNoExceptionsThrown()
            {
                // Act
                var result = await Controller.Register(_validRequest);

                // Assert
                result.Should().BeOfType<OkResult>();
            }
        }
    }
}
