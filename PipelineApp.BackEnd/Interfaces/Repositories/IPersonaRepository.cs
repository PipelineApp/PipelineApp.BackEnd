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
        Task<IEnumerable<PersonaEntity>> GetByUserIdAsync(Guid? userId);

        Task<IEnumerable<PersonaEntity>> GetBySlugAsync(string slug);
    }
}
