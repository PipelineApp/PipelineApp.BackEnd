// <copyright file="PersonaService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Entities;
    using Exceptions.Persona;
    using Interfaces;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    /// <inheritdoc />
    public class PersonaService : IPersonaService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<Persona>> GetAllPersonas(string userId, IRepository<PersonaEntity> personaRepository, IMapper mapper)
        {
            var entities = await personaRepository.GetAll(userId);
            return entities.Select(mapper.Map<Persona>);
        }

        /// <inheritdoc />
        public async Task AssertSlugIsValid(string personaSlug, string personaId, IPersonaRepository personaRepository)
        {
            var existingEntities = await personaRepository.GetBySlug(personaSlug);
            if (existingEntities.Any(e => e.Id != personaId))
            {
                throw new PersonaSlugExistsException();
            }
        }

        /// <inheritdoc />
        public async Task<Persona> CreatePersona(Persona persona, IPersonaRepository personaRepository, IMapper mapper)
        {
            var entity = mapper.Map<PersonaEntity>(persona);
            var result = await personaRepository.Create(entity);
            return mapper.Map<Persona>(result);
        }

        /// <inheritdoc />
        /// <exception cref="PersonaNotFoundException">Thrown if the persona does not exist or does not belong to the given user.</exception>
        public async Task AssertUserOwnsPersona(string personaId, string userId, IPersonaRepository personaRepository)
        {
            var persona = await personaRepository.GetById(personaId);
            if (persona == null || persona.UserId != userId)
            {
                throw new PersonaNotFoundException();
            }
        }

        /// <inheritdoc />
        /// <exception cref="PersonaSlugExistsException">Thrown if the user is attempting to update
        /// the persona to a slug already in use by another persona.</exception>
        public async Task<Persona> UpdatePersona(Persona model, IPersonaRepository personaRepository, IMapper mapper)
        {
            var entity = mapper.Map<PersonaEntity>(model);
            var existingEntities = await personaRepository.GetBySlug(model.Slug);
            var existingEntity = existingEntities.FirstOrDefault();
            if (existingEntity != null && existingEntity.Id != model.Id)
            {
                throw new PersonaSlugExistsException();
            }
            var result = await personaRepository.Update(model.Id, entity);
            return mapper.Map<Persona>(result);
        }
    }
}
