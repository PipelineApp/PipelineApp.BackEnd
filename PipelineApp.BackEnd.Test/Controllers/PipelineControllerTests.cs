// <copyright file="PipelineControllerTests.cs" company="Blackjack Software">
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
    using BackEnd.Infrastructure.Data.Entities;
    using BackEnd.Infrastructure.Exceptions.Pipeline;
    using FluentAssertions;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.DomainModels;
    using Models.RequestModels.Pipeline;
    using Models.ViewModels;
    using Moq;
    using TestHelpers;
    using Xunit;

    [Trait("Class", "PipelineController")]
    public class PipelineControllerTests : ControllerTests<PipelineController>
    {
        private readonly Mock<IPipelineService> _mockPipelineService;
        private readonly Mock<IPipelineRepository> _mockPipelineRepository;
        private readonly Mock<IMapper> _mockMapper;

        public PipelineControllerTests()
        {
            var mockLogger = new Mock<ILogger<PipelineController>>();
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<PipelineDto>(It.IsAny<Pipeline>()))
                .Returns((Pipeline model) => new PipelineDto
                {
                    Id = model.Id,
                    Description = model.Description,
                    Name = model.Name
                });
            _mockMapper.Setup(m => m.Map<Pipeline>(It.IsAny<UpsertPipelineRequestModel>()))
                .Returns((UpsertPipelineRequestModel dto) => new Pipeline
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    Name = dto.Name
                });
            _mockPipelineService = new Mock<IPipelineService>();
            _mockPipelineRepository = new Mock<IPipelineRepository>();
            Controller = new PipelineController(mockLogger.Object, _mockPipelineService.Object, _mockPipelineRepository.Object, _mockMapper.Object);
            InitControllerContext();
        }

        public class GetAllPipelines : PipelineControllerTests
        {
            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedExceptionThrown()
            {
                // Arrange
                _mockPipelineService
                    .Setup(s => s.GetAllPipelines(Constants.UserId, _mockPipelineRepository.Object, _mockMapper.Object))
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
                var pipelines = new List<Pipeline>
                {
                    new Pipeline()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Pipeline 1"
                    },
                    new Pipeline()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Pipeline 2"
                    }
                };
                _mockPipelineService
                    .Setup(s => s.GetAllPipelines(Constants.UserId, _mockPipelineRepository.Object, _mockMapper.Object))
                    .ReturnsAsync(pipelines);

                // Act
                var result = await Controller.Get();
                var resultData = ((OkObjectResult)result).Value as List<PipelineDto>;

                // Assert
                resultData.Should().NotBeNull();
                resultData.Count.Should().Be(2);
                resultData.Should().Contain(f => f.Name == "Pipeline 1");
                resultData.Should().Contain(f => f.Name == "Pipeline 2");
            }
        }

        public class Post : PipelineControllerTests
        {
            private readonly UpsertPipelineRequestModel _validRequest;

            public Post()
            {
                _validRequest = new UpsertPipelineRequestModel
                {
                    Name = "My Test Pipeline"
                };
            }

            [Fact]
            public async Task ReturnsBadRequestWhePersonaIsInvalid()
            {
                // Arrange
                var pipeline = new Mock<UpsertPipelineRequestModel>();
                var exception = new InvalidPipelineException(new List<string> { "error1", "error2" });
                pipeline.Setup(c => c.AssertIsValid()).Throws(exception);

                // Act
                var result = await Controller.Post(pipeline.Object);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                _mockPipelineService.Verify(s => s.CreatePipeline(It.IsAny<Pipeline>(), Constants.UserId, _mockPipelineRepository.Object, _mockMapper.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPipelineService.Setup(s => s.CreatePipeline(It.IsAny<Pipeline>(), Constants.UserId, _mockPipelineRepository.Object, _mockMapper.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.Post(_validRequest);

                // Assert
                result.Should().BeOfType<ObjectResult>();
                ((ObjectResult)result).StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Arrange
                _mockPipelineService.Setup(s =>
                        s.CreatePipeline(It.IsAny<Pipeline>(), Constants.UserId, _mockPipelineRepository.Object, _mockMapper.Object))
                    .ReturnsAsync((Pipeline model, Guid? userId, IRepository<PipelineEntity> repo, IMapper mapper) => model);

                // Act
                var result = await Controller.Post(_validRequest);
                var body = ((OkObjectResult)result).Value as PipelineDto;

                // Assert
                result.Should().BeOfType<OkObjectResult>();
                body?.Name.Should().Be("My Test Pipeline");
            }
        }

        public class Put : PipelineControllerTests
        {
            private readonly UpsertPipelineRequestModel _validRequest;
            private readonly Guid _pipelineId;

            public Put()
            {
                _pipelineId = Guid.NewGuid();
                _validRequest = new UpsertPipelineRequestModel
                {
                    Name = "My Test Pipeline"
                };
            }

            [Fact]
            public async Task ReturnsBadRequestWhenPipelineIsInvalid()
            {
                // Arrange
                var pipeline = new Mock<UpsertPipelineRequestModel>();
                var exception = new InvalidPipelineException(new List<string> { "error1", "error2" });
                pipeline.Setup(c => c.AssertIsValid()).Throws(exception);

                // Act
                var result = await Controller.Put(_pipelineId, pipeline.Object);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                _mockPipelineService.Verify(s => s.UpdatePipeline(It.IsAny<Pipeline>(), _mockPipelineRepository.Object, _mockMapper.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsBadRequestWhenPipelineDoesNotExistForUser()
            {
                // Arrange
                _mockPipelineService
                    .Setup(s => s.AssertUserOwnsPipeline(_pipelineId, Constants.UserId, _mockPipelineRepository.Object))
                    .Throws<PipelineNotFoundException>();

                // Act
                var result = await Controller.Put(_pipelineId, _validRequest);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                _mockPipelineService.Verify(s => s.UpdatePipeline(It.IsAny<Pipeline>(), _mockPipelineRepository.Object, _mockMapper.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPipelineService.Setup(s => s.UpdatePipeline(It.IsAny<Pipeline>(), _mockPipelineRepository.Object, _mockMapper.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.Put(_pipelineId, _validRequest);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Arrange
                _mockPipelineService.Setup(s =>
                        s.UpdatePipeline(It.IsAny<Pipeline>(), _mockPipelineRepository.Object, _mockMapper.Object))
                    .ReturnsAsync((Pipeline model, IRepository<PipelineEntity> repo, IMapper mapper) => model);

                // Act
                var result = await Controller.Put(_pipelineId, _validRequest);
                var body = ((OkObjectResult)result).Value as PipelineDto;

                // Assert
                result.Should().BeOfType<OkObjectResult>();
                body?.Id.Should().Be(_pipelineId);
            }
        }

        public class Delete : PipelineControllerTests
        {
            private readonly Guid _pipelineId;

            public Delete()
            {
                _pipelineId = Guid.NewGuid();
            }

            [Fact]
            public async Task ReturnsNotFoundWhenPipelineDoesNotExistForUser()
            {
                // Arrange
                _mockPipelineService
                    .Setup(s => s.AssertUserOwnsPipeline(_pipelineId, Constants.UserId, _mockPipelineRepository.Object))
                    .Throws<PipelineNotFoundException>();

                // Act
                var result = await Controller.Delete(_pipelineId);

                // Assert
                result.Should().BeOfType<NotFoundObjectResult>();
                _mockPipelineService.Verify(s => s.DeletePipeline(_pipelineId, _mockPipelineRepository.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPipelineService.Setup(s => s.DeletePipeline(_pipelineId, _mockPipelineRepository.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.Delete(_pipelineId);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Act
                var result = await Controller.Delete(_pipelineId);

                // Assert
                result.Should().BeOfType<OkResult>();
            }
        }

        public class AddPersona : PipelineControllerTests
        {
            private readonly Guid _personaId;
            private readonly Guid _pipelineId;

            public AddPersona()
            {
                _personaId = Guid.NewGuid();
                _pipelineId = Guid.NewGuid();
            }

            [Fact]
            public async Task ReturnsNotFoundWhenPipelineDoesNotExistForUser()
            {
                // Arrange
                _mockPipelineService
                    .Setup(s => s.AssertUserOwnsPipeline(_pipelineId, Constants.UserId, _mockPipelineRepository.Object))
                    .Throws<PipelineNotFoundException>();

                // Act
                var result = await Controller.AddPersona(_pipelineId, _personaId);

                // Assert
                result.Should().BeOfType<NotFoundObjectResult>();
                _mockPipelineService.Verify(s => s.AddTrackedPersona(_pipelineId, _personaId, _mockPipelineRepository.Object, _mockMapper.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPipelineService.Setup(s => s.AddTrackedPersona(_pipelineId, _personaId, _mockPipelineRepository.Object, _mockMapper.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.AddPersona(_pipelineId, _personaId);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Act
                var result = await Controller.AddPersona(_pipelineId, _personaId);

                // Assert
                result.Should().BeOfType<OkResult>();
            }
        }

        public class AddFandom : PipelineControllerTests
        {
            private readonly Guid _fandomId;
            private readonly Guid _pipelineId;

            public AddFandom()
            {
                _fandomId = Guid.NewGuid();
                _pipelineId = Guid.NewGuid();
            }

            [Fact]
            public async Task ReturnsNotFoundWhenPipelineDoesNotExistForUser()
            {
                // Arrange
                _mockPipelineService
                    .Setup(s => s.AssertUserOwnsPipeline(_pipelineId, Constants.UserId, _mockPipelineRepository.Object))
                    .Throws<PipelineNotFoundException>();

                // Act
                var result = await Controller.AddFandom(_pipelineId, _fandomId);

                // Assert
                result.Should().BeOfType<NotFoundObjectResult>();
                _mockPipelineService.Verify(s => s.AddTrackedFandom(_pipelineId, _fandomId, _mockPipelineRepository.Object, _mockMapper.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPipelineService.Setup(s => s.AddTrackedFandom(_pipelineId, _fandomId, _mockPipelineRepository.Object, _mockMapper.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.AddFandom(_pipelineId, _fandomId);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Act
                var result = await Controller.AddFandom(_pipelineId, _fandomId);

                // Assert
                result.Should().BeOfType<OkResult>();
            }
        }

        public class RemovePersona : PipelineControllerTests
        {
            private readonly Guid _personaId;
            private readonly Guid _pipelineId;

            public RemovePersona()
            {
                _personaId = Guid.NewGuid();
                _pipelineId = Guid.NewGuid();
            }

            [Fact]
            public async Task ReturnsNotFoundWhenPipelineDoesNotExistForUser()
            {
                // Arrange
                _mockPipelineService
                    .Setup(s => s.AssertUserOwnsPipeline(_pipelineId, Constants.UserId, _mockPipelineRepository.Object))
                    .Throws<PipelineNotFoundException>();

                // Act
                var result = await Controller.RemovePersona(_pipelineId, _personaId);

                // Assert
                result.Should().BeOfType<NotFoundObjectResult>();
                _mockPipelineService.Verify(s => s.RemoveTrackedPersona(_pipelineId, _personaId, _mockPipelineRepository.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPipelineService.Setup(s => s.RemoveTrackedPersona(_pipelineId, _personaId, _mockPipelineRepository.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.RemovePersona(_pipelineId, _personaId);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Act
                var result = await Controller.RemovePersona(_pipelineId, _personaId);

                // Assert
                result.Should().BeOfType<OkResult>();
            }
        }

        public class RemoveFandom : PipelineControllerTests
        {
            private readonly Guid _fandomId;
            private readonly Guid _pipelineId;

            public RemoveFandom()
            {
                _fandomId = Guid.NewGuid();
                _pipelineId = Guid.NewGuid();
            }

            [Fact]
            public async Task ReturnsNotFoundWhenPipelineDoesNotExistForUser()
            {
                // Arrange
                _mockPipelineService
                    .Setup(s => s.AssertUserOwnsPipeline(_pipelineId, Constants.UserId, _mockPipelineRepository.Object))
                    .Throws<PipelineNotFoundException>();

                // Act
                var result = await Controller.RemoveFandom(_pipelineId, _fandomId);

                // Assert
                result.Should().BeOfType<NotFoundObjectResult>();
                _mockPipelineService.Verify(s => s.RemoveTrackedFandom(_pipelineId, _fandomId, _mockPipelineRepository.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPipelineService.Setup(s => s.RemoveTrackedFandom(_pipelineId, _fandomId, _mockPipelineRepository.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.RemoveFandom(_pipelineId, _fandomId);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Act
                var result = await Controller.RemoveFandom(_pipelineId, _fandomId);

                // Assert
                result.Should().BeOfType<OkResult>();
            }
        }
    }
}
