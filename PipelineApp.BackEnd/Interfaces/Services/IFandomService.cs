// <copyright file="IFandomService.cs" company="Blackjack Software">
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

    /// <summary>
    /// Service for data manipulation related to fandoms.
    /// </summary>
    public interface IFandomService
    {
        /// <summary>
        /// Retrieves all fandoms.
        /// </summary>
        /// <param name="repository">The graph DB client.</param>
        /// <param name="mapper">The application's object mapper.</param>
        /// <returns>A list of <see cref="Fandom"/> objects.</returns>
        Task<IEnumerable<Fandom>> GetAllFandoms(IRepository<FandomEntity> repository, IMapper mapper);
    }
}
