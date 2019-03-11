// <copyright file="IPersonaService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
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
        Task<IEnumerable<Persona>> GetAllPersonas(Guid? userId, IPersonaRepository personaRepository, IMapper mapper);

        /// <summary>
        /// Throws an exception if the provided slug is not valid.
        /// </summary>
        /// <param name="personaSlug">The slug to be validated.</param>
        /// <param name="personaId">Optional identifier for an existing persona to which the slug may already belong.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AssertSlugIsValid(string personaSlug, Guid personaId, IPersonaRepository personaRepository);

        /// <summary>
        /// Initializes and saves a new persona object.
        /// </summary>
        /// <param name="persona">Data regarding the persona to be created.</param>
        /// <param name="userId">The unique ID of the user to which the persona should be connected.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the created persona.
        /// </returns>
        Task<Persona> CreatePersona(Persona persona, Guid? userId, IPersonaRepository personaRepository, IMapper mapper);

        /// <summary>
        /// Throws an exception if the given user does not own the given persona.
        /// </summary>
        /// <param name="personaId">The unique ID of the persona.</param>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task AssertUserOwnsPersona(Guid personaId, Guid? userId, IPersonaRepository personaRepository);

        /// <summary>
        /// Updates the passed persona.
        /// </summary>
        /// <param name="model">The model containing persona information.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <param name="mapper">The application's object mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the updated persona object.
        /// </returns>
        Task<Persona> UpdatePersona(Persona model, IPersonaRepository personaRepository, IMapper mapper);

        /// <summary>
        /// Deletes the persona associated with the given persona ID.
        /// </summary>
        /// <param name="personaId">The unique identifier of the persona to be deleted.</param>
        /// <param name="personaRepository">The persona repository.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeletePersona(Guid personaId, IPersonaRepository personaRepository);
    }
}
