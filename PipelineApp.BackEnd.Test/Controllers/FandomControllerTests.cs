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
            _mockFandomService = new Mock<IFandomService>();
            _mockFandomRepository = new Mock<IFandomRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<FandomDto>(It.IsAny<Fandom>()))
                .Returns((Fandom model) => new FandomDto
                {
                    Name = model.Name,
                    Id = model.Id
                });
            _mockMapper.Setup(m => m.Map<Fandom>(It.IsAny<FandomDto>()))
                .Returns((FandomDto dto) => new Fandom
                {
                    Name = dto.Name,
                    Id = dto.Id
                });
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
            public async Task ReturnsOkWithFandomListWhenNoExceptionsThrown()
            {
                // Arrange
                var fandoms = new List<Fandom> { new Fandom { Id = Guid.NewGuid() }, new Fandom { Id = Guid.NewGuid() } };
                _mockFandomService
                    .Setup(s => s.GetAllFandoms(_mockFandomRepository.Object, _mockMapper.Object))
                    .ReturnsAsync(fandoms);

                // Act
                var result = await Controller.Get();
                var data = ((OkObjectResult)result).Value as List<FandomDto>;

                // Assert
                data.Should().NotBeNull();
                data.Should().HaveCount(2);
                data.Should().Contain(f => f.Id == fandoms[0].Id);
                data.Should().Contain(f => f.Id == fandoms[1].Id);
            }
        }
    }
}
