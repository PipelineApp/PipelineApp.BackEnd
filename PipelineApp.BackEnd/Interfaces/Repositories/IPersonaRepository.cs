// <copyright file="IPersonaRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Data.Entities;

    /// <summary>
    /// Extension of base repository containing methods related to
    /// persona data.
    /// </summary>
    public interface IPersonaRepository : IRepository<PersonaEntity>
    {
        /// <summary>
        /// Retrieves a list of the personas owned by the given user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose personas should be retrieved.</param>
        /// <returns>A list of persona entities owned by the given user.</returns>
        Task<IEnumerable<PersonaEntity>> GetByUserIdAsync(Guid? userId);

        /// <summary>
        /// Retrieves a list of personas with the given slug. (Should only ever return one item.)
        /// </summary>
        /// <param name="slug">The slug value to be searched for.</param>
        /// <returns>A list of personas with the given slug. This list should only ever have one item.</returns>
        Task<IEnumerable<PersonaEntity>> GetBySlugAsync(string slug);
    }
}
