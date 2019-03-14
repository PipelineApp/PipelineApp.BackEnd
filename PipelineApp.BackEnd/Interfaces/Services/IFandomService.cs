// <copyright file="IFandomService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Models.DomainModels;
    using Repositories;

    /// <summary>
    /// Service for data manipulation related to fandoms.
    /// </summary>
    public interface IFandomService
    {
        /// <summary>
        /// Gets a list of all fandoms managed by the application.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains a list of the retrieved fandoms.
        /// </returns>
        Task<IEnumerable<Fandom>> GetAllFandoms(IFandomRepository repository, IMapper mapper);
    }
}
