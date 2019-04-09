﻿// <copyright file="BaseControllerTests.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.Controllers
{
    using System;
    using System.Security.Claims;
    using BackEnd.Controllers;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestHelpers;
    using Xunit;

    [Trait("Class", "BaseController")]
    public class BaseControllerTests : ControllerTests<BaseController>
    {
        private readonly MockChildController _childController;

        public BaseControllerTests()
        {
            _childController = new MockChildController();
        }

        public class Props : BaseControllerTests
        {
            [Fact]
            public void UserIdReturnsClaimsPrincipalUserId()
            {
                // Arrange
                var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Constants.UserId)
                }));
                _childController.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                };

                // Act
                var result = _childController.RetrieveUserId();

                // Assert
                result.Should().Be(Constants.UserId);
            }

            [Fact]
            public void UserIdReturnsNullIfNoNameIdentifierPresent()
            {
                // Arrange
                var user = new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>()));
                _childController.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                };

                // Act
                var userId = _childController.RetrieveUserId();

                // Assert
                userId.Should().BeNull();
            }
        }
    }
}
