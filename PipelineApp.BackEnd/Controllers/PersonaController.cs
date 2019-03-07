// <copyright file="PersonaController.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Data.Entities;
    using Infrastructure.Exceptions.Persona;
    using Interfaces;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.DomainModels;
    using Models.ViewModels;
    using Newtonsoft.Json;

    /// <summary>
    /// Controller class for behavior related to persona data.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PersonaController : BaseController
    {
        private readonly ILogger<PersonaController> _logger;
        private readonly IPersonaService _personaService;
        private readonly IPersonaRepository _personaRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonaController"/> class.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="personaService">The persona service.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <param name="mapper">The mapper.</param>
        public PersonaController(
            ILogger<PersonaController> logger,
            IPersonaService personaService,
            IPersonaRepository personaRepository,
            IMapper mapper)
        {
            _logger = logger;
            _personaService = personaService;
            _personaRepository = personaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Processes a request for all fandoms.
        /// </summary>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a list of <see cref="PersonaDto" /> objects in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful retrieval of persona information</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<PersonaDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation($"Received request to get list of available personas for user {UserId}.");
                var personas = await _personaService.GetAllPersonas(UserId, _personaRepository, _mapper);
                var result = personas.Select(_mapper.Map<PersonaDto>).ToList();
                _logger.LogInformation(
                    $"Processed request to get list of available personas for user {UserId}. Found {result.Count} personas.");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Processes a request to create a new persona for the current user.
        /// </summary>
        /// <param name="personaDto">View model containing information about persona to be created.</param>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a <see cref="PersonaDto" /> object in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful creation of persona</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PersonaDto))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] PersonaDto personaDto)
        {
            try
            {
                _logger.LogInformation(
                    $"Received request to create a persona belonging to user {UserId}. Request: {personaDto}");
                personaDto.AssertIsValid();
                await _personaService.AssertSlugIsValid(personaDto.Slug, personaDto.Id, _personaRepository);
                var persona = _mapper.Map<Persona>(personaDto);
                persona.UserId = UserId;
                var result = await _personaService.CreatePersona(persona, _personaRepository, _mapper);
                _logger.LogInformation(
                    $"Processed request to create a persona belonging to user {UserId}. Created {result}");
                return Ok(result);
            }
            catch (InvalidPersonaException e)
            {
                _logger.LogWarning($"User {UserId} attempted to create invalid persona: {personaDto}");
                return BadRequest(e.Errors);
            }
            catch (PersonaSlugExistsException e)
            {
                _logger.LogWarning($"User {UserId} attempted to create persona with existing slug: {personaDto}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Processes a request to update an existing persona for the current user.
        /// </summary>
        /// <param name="personaId">The unique ID of the persona to be updated.</param>
        /// <param name="personaDto">View model containing information about persona to be updated.</param>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a <see cref="PersonaDto" /> object in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful update of persona</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPut]
        [Route("{personaId}")]
        [ProducesResponseType(200, Type = typeof(PersonaDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put(string personaId, [FromBody]PersonaDto personaDto)
        {
            try
            {
                _logger.LogInformation($"Received request to update persona {personaId} for user {UserId}. Request body: {JsonConvert.SerializeObject(personaDto)}");
                personaDto.AssertIsValid();
                personaDto.UserId = UserId;
                await _personaService.AssertUserOwnsPersona(personaId, UserId, _personaRepository);
                var model = _mapper.Map<Persona>(personaDto);
                var updatedPersona = await _personaService.UpdatePersona(model, _personaRepository, _mapper);
                _logger.LogInformation($"Processed request to update persona {personaId} for user {UserId}. Result body: {JsonConvert.SerializeObject(updatedPersona)}");
                return Ok(_mapper.Map<PersonaDto>(updatedPersona));
            }
            catch (PersonaSlugExistsException)
            {
                _logger.LogWarning($"User {UserId} attempted to set persona {personaId} to existing slug {personaDto.Slug}.");
                return BadRequest("A persona already exists with this slug.");
            }
            catch (InvalidPersonaException)
            {
                _logger.LogWarning($"User {UserId} attempted to update invalid persona {JsonConvert.SerializeObject(personaDto)}.");
                return BadRequest("The supplied persona is invalid.");
            }
            catch (PersonaNotFoundException)
            {
                _logger.LogWarning($"User {UserId} attempted to update persona {personaDto.Id} illegally.");
                return BadRequest("You do not have permission to update this persona.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error updating public view {JsonConvert.SerializeObject(personaDto)}: {e.Message}", e);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
