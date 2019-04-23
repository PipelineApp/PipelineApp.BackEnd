// <copyright file="PersonaService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Entities;
    using Data.Relationships;
    using Exceptions.Persona;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    /// <inheritdoc />
    public class PersonaService : IPersonaService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<Persona>> GetAllPersonas(Guid? userId, IPersonaRepository repository, IMapper mapper)
        {
            var personaEntities = await repository.GetByUserIdAsync(userId);
            return personaEntities.Select(mapper.Map<Persona>).ToList();
        }

        /// <inheritdoc />
        public async Task AssertSlugIsValid(string slug, Guid personaId, IPersonaRepository personaRepository)
        {
            var slugRegex = new Regex(@"^[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*$");
            if (!slugRegex.IsMatch(slug))
            {
                throw new InvalidPersonaException(new List<string> { "The provided slug is in an invalid format." });
            }
            var existingEntities = await personaRepository.GetBySlugAsync(slug);
            if (existingEntities.Any(p => p.Id != personaId))
            {
                throw new PersonaSlugExistsException();
            }
        }

        /// <inheritdoc />
        public async Task<Persona> CreatePersona(Persona persona, Guid? userId, IPersonaRepository repository, IMapper mapper)
        {
            if (userId == null)
            {
                throw new ArgumentException("User ID cannot be null.");
            }
            var entity = mapper.Map<PersonaEntity>(persona);
            var inboundRelationships = new List<BaseRelationship>
            {
                new HasPersona {SourceId = userId.Value}
            };
            var createdEntity = repository.CreateWithRelationships(entity, inboundRelationships);
            return mapper.Map<Persona>(createdEntity);
        }

        /// <inheritdoc />
        public async Task AssertUserOwnsPersona(Guid personaId, Guid? userId, IPersonaRepository repository)
        {
            if (userId == null)
            {
                throw new ArgumentException("User ID cannot be null.");
            }
            var entities = await repository.GetByUserIdAsync(userId);
            if (entities.All(e => e.Id != personaId))
            {
                throw new PersonaNotFoundException();
            }
        }

        /// <inheritdoc />
        public async Task<Persona> UpdatePersona(Persona model, IPersonaRepository repository, IMapper mapper)
        {
            var entity = mapper.Map<PersonaEntity>(model);
            var result = await repository.UpdateAsync(entity);
            return mapper.Map<Persona>(result);
        }

        /// <inheritdoc />
        public async Task DeletePersona(Guid personaId, IPersonaRepository repository)
        {
            await repository.DeleteAsync(personaId);
        }
    }
}
