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
    }
}
