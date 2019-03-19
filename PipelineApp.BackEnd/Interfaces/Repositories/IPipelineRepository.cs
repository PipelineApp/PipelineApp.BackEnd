// <copyright file="IPipelineRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Data.Entities;
    using Infrastructure.Data.Entities.EntityCollections;

    /// <summary>
    /// Extension of base repository containing methods related to
    /// pipeline data.
    /// </summary>
    public interface IPipelineRepository : IRepository<PipelineEntity>
    {
        /// <summary>
        /// Retrieves a list of the pipelines owned by the given user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose pipelines should be retrieved.</param>
        /// <returns>A list of pipeline entities owned by the given user.</returns>
        Task<IEnumerable<PipelineEntityDataCollection>> GetByUserIdAsync(Guid? userId);
    }
}
