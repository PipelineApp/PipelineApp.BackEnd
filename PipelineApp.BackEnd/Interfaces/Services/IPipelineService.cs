// <copyright file="IPipelineService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
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
        /// The created pipeline.
        /// </returns>
        Pipeline CreatePipeline(Pipeline pipeline, Guid? userId, IPipelineRepository repository, IMapper mapper);

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

        /// <summary>
        /// Fetches all pipelines belonging to the given user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose pipelines should be retrieved.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains a list of <see cref="Pipeline"/> objects belonging to the
        /// given user.
        /// </returns>
        Task<IEnumerable<Pipeline>> GetAllPipelines(Guid? userId, IPipelineRepository pipelineRepository, IMapper mapper);

        /// <summary>
        /// Updates the passed pipeline.
        /// </summary>
        /// <param name="model">The model containing pipeline information.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <param name="mapper">The application's object mapper.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains the updated pipeline object.
        /// </returns>
        Task<Pipeline> UpdatePipeline(Pipeline model, IPipelineRepository pipelineRepository, IMapper mapper);

        /// <summary>
        /// Deletes the pipeline with the passed ID.
        /// </summary>
        /// <param name="pipelineId">The unique identifier of the pipeline to be deleted.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeletePipeline(Guid pipelineId, IPipelineRepository pipelineRepository);

        /// <summary>
        /// Removes the given persona from the given pipeline.
        /// </summary>
        /// <param name="pipelineId">The pipeline which should stop tracking the persona.</param>
        /// <param name="personaId">The persona which should be removed as a tracked item from the pipeline.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveTrackedPersona(Guid pipelineId, Guid personaId, IPipelineRepository pipelineRepository);

        /// <summary>
        /// Removes the given fandom from the given pipeline.
        /// </summary>
        /// <param name="pipelineId">The pipeline which should stop tracking the fandom.</param>
        /// <param name="fandomId">The fandom which should be removed as a tracked item from the pipeline.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveTrackedFandom(Guid pipelineId, Guid fandomId, IPipelineRepository pipelineRepository);
    }
}
