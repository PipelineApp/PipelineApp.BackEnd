// <copyright file="IPersonaService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Data.Entities;
    using Models.DomainModels;
    using Repositories;

    /// <summary>
    /// Service for data manipulation related to personas.
    /// </summary>
    public interface IPersonaService
    {
        /// <summary>
        /// Fetches all personas belonging to the given user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose personas should be retrieved.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains a list of <see cref="Persona"/> objects belonging to the
        /// given user.
        /// </returns>
        Task<IEnumerable<Persona>> GetAllPersonas(string userId, IRepository<PersonaEntity> personaRepository, IMapper mapper);

        /// <summary>
        /// Throws an exception if the provided slug is not valid.
        /// </summary>
        /// <param name="personaSlug">The slug to be validated.</param>
        /// <param name="personaId">Optional identifier for an existing persona to which the slug may already belong.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AssertSlugIsValid(string personaSlug, string personaId, IPersonaRepository personaRepository);

        /// <summary>
        /// Initializes and saves a new persona object.
        /// </summary>
        /// <param name="persona">Data regarding the persona to be created.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the created persona.
        /// </returns>
        Task<Persona> CreatePersona(Persona persona, IPersonaRepository personaRepository, IMapper mapper);
    }
}
