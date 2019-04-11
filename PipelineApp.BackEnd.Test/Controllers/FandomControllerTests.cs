// <copyright file="FandomControllerTests.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using BackEnd.Controllers;
    using FluentAssertions;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.DomainModels;
    using Models.ViewModels;
    using Moq;
    using TestHelpers;
    using Xunit;

    [Trait("Class", "FandomController")]
    public class FandomControllerTests : ControllerTests<FandomController>
    {
        private readonly Mock<IFandomService> _mockFandomService;
        private readonly Mock<IFandomRepository> _mockFandomRepository;
        private readonly Mock<IMapper> _mockMapper;

        public FandomControllerTests()
        {
            var mockLogger = new Mock<ILogger<FandomController>>();
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<FandomDto>(It.IsAny<Fandom>()))
                .Returns((Fandom model) => new FandomDto
                {
                    Id = model.Id,
                    Name = model.Name
                });
            _mockMapper.Setup(m => m.Map<Fandom>(It.IsAny<FandomDto>()))
                .Returns((FandomDto dto) => new Fandom
                {
                    Id = dto.Id,
                    Name = dto.Name
                });
            _mockFandomService = new Mock<IFandomService>();
            _mockFandomRepository = new Mock<IFandomRepository>();
            Controller = new FandomController(mockLogger.Object, _mockMapper.Object, _mockFandomService.Object, _mockFandomRepository.Object);
            InitControllerContext();
        }

        public class GetAllFandoms : FandomControllerTests
        {
            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedExceptionThrown()
            {
                // Arrange
                _mockFandomService
                    .Setup(s => s.GetAllFandoms(_mockFandomRepository.Object, _mockMapper.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.Get();

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenNoExceptionsThrown()
            {
                // Arrange
                var fandoms = new List<Fandom>
                {
                    new Fandom()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Fandom 1"
                    },
                    new Fandom()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Fandom 2"
                    }
                };
                _mockFandomService
                    .Setup(s => s.GetAllFandoms(_mockFandomRepository.Object, _mockMapper.Object))
                    .ReturnsAsync(fandoms);

                // Act
                var result = await Controller.Get();
                var resultData = ((OkObjectResult)result).Value as List<FandomDto>;

                // Assert
                resultData.Should().NotBeNull();
                resultData.Count.Should().Be(2);
                resultData.Should().Contain(f => f.Name == "Fandom 1");
                resultData.Should().Contain(f => f.Name == "Fandom 2");
            }
        }
    }
}
