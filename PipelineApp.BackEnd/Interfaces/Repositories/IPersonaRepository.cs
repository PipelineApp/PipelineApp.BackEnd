// <copyright file="IPersonaRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Data.Entities;

    /// <summary>
    /// Client class for interactions with a graph database specific to persona data.
    /// </summary>
    public interface IPersonaRepository : IRepository<PersonaEntity>
    {
        /// <summary>
        /// Fetches all personas matching a given slug.
        /// </summary>
        /// <param name="personaSlug">The slug to search by.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains a list of all personas matching the given slug.
        /// (Should contain only one item or be empty.)
        /// </returns>
        Task<IEnumerable<PersonaEntity>> GetBySlug(string personaSlug);

        /// <summary>
        /// Updates the specified persona.
        /// </summary>
        /// <param name="id">The unique ID of the persona to be updated.</param>
        /// <param name="entity">The information with which the entity should be updated.</param>
        /// <returns>The updated persona entity.</returns>
        Task<PersonaEntity> Update(string id, PersonaEntity entity);
    }
}
