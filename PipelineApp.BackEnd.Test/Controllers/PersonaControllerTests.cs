// <copyright file="PersonaControllerTests.cs" company="Blackjack Software">
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
    using BackEnd.Infrastructure.Exceptions.Persona;
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

    [Trait("Class", "PersonaController")]
    public class PersonaControllerTests : ControllerTests<PersonaController>
    {
        private readonly Mock<IPersonaService> _mockPersonaService;
        private readonly Mock<IPersonaRepository> _mockPersonaRepository;
        private readonly Mock<IMapper> _mockMapper;

        public PersonaControllerTests()
        {
            var mockLogger = new Mock<ILogger<PersonaController>>();
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<PersonaDto>(It.IsAny<Persona>()))
                .Returns((Persona model) => new PersonaDto
                {
                    Id = model.Id,
                    Description = model.Description,
                    PersonaName = model.PersonaName,
                    Slug = model.Slug
                });
            _mockMapper.Setup(m => m.Map<Persona>(It.IsAny<PersonaDto>()))
                .Returns((PersonaDto dto) => new Persona
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    PersonaName = dto.PersonaName,
                    Slug = dto.Slug
                });
            _mockPersonaService = new Mock<IPersonaService>();
            _mockPersonaRepository = new Mock<IPersonaRepository>();
            Controller = new PersonaController(mockLogger.Object, _mockPersonaService.Object, _mockPersonaRepository.Object, _mockMapper.Object);
            InitControllerContext();
        }

        public class GetAllPersonas : PersonaControllerTests
        {
            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedExceptionThrown()
            {
                // Arrange
                _mockPersonaService
                    .Setup(s => s.GetAllPersonas(Constants.UserId, _mockPersonaRepository.Object, _mockMapper.Object))
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
                var personas = new List<Persona>
                {
                    new Persona()
                    {
                        Id = Guid.NewGuid(),
                        PersonaName = "Persona 1"
                    },
                    new Persona()
                    {
                        Id = Guid.NewGuid(),
                        PersonaName = "Persona 2"
                    }
                };
                _mockPersonaService
                    .Setup(s => s.GetAllPersonas(Constants.UserId, _mockPersonaRepository.Object, _mockMapper.Object))
                    .ReturnsAsync(personas);

                // Act
                var result = await Controller.Get();
                var resultData = ((OkObjectResult)result).Value as List<PersonaDto>;

                // Assert
                resultData.Should().NotBeNull();
                resultData.Count.Should().Be(2);
                resultData.Should().Contain(f => f.PersonaName == "Persona 1");
                resultData.Should().Contain(f => f.PersonaName == "Persona 2");
            }
        }

        public class Post : PersonaControllerTests
        {
            private readonly PersonaDto _validRequest;

            public Post()
            {
                _validRequest = new PersonaDto
                {
                    PersonaName = "Test Persona",
                    Slug = "test-persona",
                    Description = "My test persona."
                };
            }

            [Fact]
            public async Task ReturnsBadRequestWhePersonaIsInvalid()
            {
                // Arrange
                var persona = new Mock<PersonaDto>();
                var exception = new InvalidPersonaException(new List<string> { "error1", "error2" });
                persona.Setup(c => c.AssertIsValid()).Throws(exception);

                // Act
                var result = await Controller.Post(persona.Object);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                _mockPersonaService.Verify(s => s.CreatePersona(It.IsAny<Persona>(), Constants.UserId, _mockPersonaRepository.Object, _mockMapper.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsBadRequestWhenSlugExists()
            {
                // Arrange
                _mockPersonaService.Setup(s => s.CreatePersona(It.IsAny<Persona>(), Constants.UserId, _mockPersonaRepository.Object, _mockMapper.Object))
                    .Throws<PersonaSlugExistsException>();

                // Act
                var result = await Controller.Post(_validRequest);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPersonaService.Setup(s => s.CreatePersona(It.IsAny<Persona>(), Constants.UserId, _mockPersonaRepository.Object, _mockMapper.Object))
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
                _mockPersonaService.Setup(s =>
                        s.CreatePersona(It.IsAny<Persona>(), Constants.UserId, _mockPersonaRepository.Object, _mockMapper.Object))
                    .ReturnsAsync((Persona model, Guid? userId, IRepository<PersonaEntity> repo, IMapper mapper) => model);

                // Act
                var result = await Controller.Post(_validRequest);
                var body = ((OkObjectResult)result).Value as PersonaDto;

                // Assert
                result.Should().BeOfType<OkObjectResult>();
                body?.PersonaName.Should().Be("Test Persona");
            }
        }

        public class Put : PersonaControllerTests
        {
            private readonly PersonaDto _validRequest;
            private readonly Guid _personaId;

            public Put()
            {
                _personaId = Guid.NewGuid();
                _validRequest = new PersonaDto
                {
                    Id = _personaId,
                    PersonaName = "Test Persona",
                    Slug = "test-persona",
                    Description = "My test persona."
                };
            }

            [Fact]
            public async Task ReturnsBadRequestWhenPersonaIsInvalid()
            {
                // Arrange
                var persona = new Mock<PersonaDto>();
                var exception = new InvalidPersonaException(new List<string> { "error1", "error2" });
                persona.Setup(c => c.AssertIsValid()).Throws(exception);

                // Act
                var result = await Controller.Put(_personaId, persona.Object);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                _mockPersonaService.Verify(s => s.UpdatePersona(It.IsAny<Persona>(), _mockPersonaRepository.Object, _mockMapper.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsBadRequestWhenSlugExists()
            {
                // Arrange
                _mockPersonaService.Setup(s => s.UpdatePersona(It.IsAny<Persona>(), _mockPersonaRepository.Object, _mockMapper.Object))
                    .Throws<PersonaSlugExistsException>();

                // Act
                var result = await Controller.Put(_personaId, _validRequest);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
            }

            [Fact]
            public async Task ReturnsBadRequestWhenPersonaDoesNotExistForUser()
            {
                // Arrange
                _mockPersonaService
                    .Setup(s => s.AssertUserOwnsPersona(_personaId, Constants.UserId, _mockPersonaRepository.Object))
                    .Throws<PersonaNotFoundException>();

                // Act
                var result = await Controller.Put(_personaId, _validRequest);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
                _mockPersonaService.Verify(s => s.UpdatePersona(It.IsAny<Persona>(), _mockPersonaRepository.Object, _mockMapper.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPersonaService.Setup(s => s.UpdatePersona(It.IsAny<Persona>(), _mockPersonaRepository.Object, _mockMapper.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.Put(_personaId, _validRequest);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Arrange
                _mockPersonaService.Setup(s =>
                        s.UpdatePersona(It.IsAny<Persona>(), _mockPersonaRepository.Object, _mockMapper.Object))
                    .ReturnsAsync((Persona model, IRepository<PersonaEntity> repo, IMapper mapper) => model);

                // Act
                var result = await Controller.Put(_personaId, _validRequest);
                var body = ((OkObjectResult)result).Value as PersonaDto;

                // Assert
                result.Should().BeOfType<OkObjectResult>();
                body?.Id.Should().Be(_personaId);
            }
        }

        public class Delete : PersonaControllerTests
        {
            private readonly Guid _personaId;

            public Delete()
            {
                _personaId = Guid.NewGuid();
            }

            [Fact]
            public async Task ReturnsNotFoundWhenPersonaDoesNotExistForUser()
            {
                // Arrange
                _mockPersonaService
                    .Setup(s => s.AssertUserOwnsPersona(_personaId, Constants.UserId, _mockPersonaRepository.Object))
                    .Throws<PersonaNotFoundException>();

                // Act
                var result = await Controller.Delete(_personaId);

                // Assert
                result.Should().BeOfType<NotFoundObjectResult>();
                _mockPersonaService.Verify(s => s.DeletePersona(_personaId, _mockPersonaRepository.Object), Times.Never);
            }

            [Fact]
            public async Task ReturnsServerErrorWhenUnexpectedErrorOccurs()
            {
                // Arrange
                _mockPersonaService.Setup(s => s.DeletePersona(_personaId, _mockPersonaRepository.Object))
                    .Throws<NullReferenceException>();

                // Act
                var result = await Controller.Delete(_personaId);

                // Assert
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            }

            [Fact]
            public async Task ReturnsOkWhenRequestIsSuccessful()
            {
                // Act
                var result = await Controller.Delete(_personaId);

                // Assert
                result.Should().BeOfType<OkResult>();
            }
        }
    }
}
