// <copyright file="IPipelineService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Models.DomainModels;
    using Repositories;

    /// <summary>
    /// Service for data manipulation relating to user authentication.
    /// </summary>
    public interface IPipelineService
    {
        /// <summary>
        /// Initializes and saves a new pipeline object.
        /// </summary>
        /// <param name="pipeline">Data regarding the pipeline to be created.</param>
        /// <param name="userId">The unique ID of the user to which the pipeline should be connected.</param>
        /// <param name="repository">The pipeline repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the created pipeline.
        /// </returns>
        Task<Pipeline> CreatePipeline(Pipeline pipeline, Guid? userId, IPipelineRepository repository, IMapper mapper);

        /// <summary>
        /// Throws an exception if the given user does not own the given pipeline.
        /// </summary>
        /// <param name="pipelineId">The unique ID of the pipeline.</param>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task AssertUserOwnsPipeline(Guid pipelineId, Guid? userId, IPipelineRepository pipelineRepository);

        /// <summary>
        /// Adds a tracked persona to a pipeline.
        /// </summary>
        /// <param name="pipelineId">The unique identifier of the pipeline which should track the persona.</param>
        /// <param name="personaId">The unique identifier of the persona to be tracked.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddTrackedPersona(Guid pipelineId, Guid personaId, IPipelineRepository pipelineRepository, IMapper mapper);

        /// <summary>
        /// Adds a tracked fandom to a pipeline.
        /// </summary>
        /// <param name="pipelineId">The unique identifier of the pipeline which should track the persona.</param>
        /// <param name="fandomId">The unique identifier of the fandom to be tracked.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddTrackedFandom(Guid pipelineId, Guid fandomId, IPipelineRepository pipelineRepository, IMapper mapper);
    }
}
