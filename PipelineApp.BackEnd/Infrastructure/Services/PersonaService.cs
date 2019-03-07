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
        public async Task<IEnumerable<Persona>> GetAllPersonas(string userId, IPersonaRepository repository, IMapper mapper)
        {
            var personaEntities = await repository.GetByUserIdAsync(userId);
            return personaEntities.Select(mapper.Map<Persona>).ToList();
        }

        /// <inheritdoc />
        public Task AssertSlugIsValid(string personaSlug, string personaId, IPersonaRepository personaRepository)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Persona> CreatePersona(Persona persona, IPersonaRepository personaRepository, IMapper mapper)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task AssertUserOwnsPersona(string personaId, string userId, IPersonaRepository personaRepository)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Persona> UpdatePersona(Persona model, IPersonaRepository personaRepository, IMapper mapper)
        {
            throw new System.NotImplementedException();
        }
    }
}
