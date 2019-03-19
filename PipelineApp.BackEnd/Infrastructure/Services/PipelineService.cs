// <copyright file="PipelineService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Entities;
    using Data.Relationships;
    using Exceptions.Pipeline;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    /// <inheritdoc />
    public class PipelineService : IPipelineService
    {
        /// <inheritdoc />
        public async Task<Pipeline> CreatePipeline(Pipeline pipeline, Guid? userId, IPipelineRepository repository, IMapper mapper)
        {
            if (userId == null)
            {
                throw new ArgumentException("User ID cannot be null.");
            }
            var entity = mapper.Map<PipelineEntity>(pipeline);
            var result = await repository.CreateWithInboundRelationshipAsync<Manages, UserEntity>(entity, userId.Value);
            return mapper.Map<Pipeline>(result);
        }

        /// <inheritdoc />
        public async Task AssertUserOwnsPipeline(Guid pipelineId, Guid? userId, IPipelineRepository repository)
        {
            if (userId == null)
            {
                throw new ArgumentException("User ID cannot be null.");
            }
            var entities = await repository.GetByUserIdAsync(userId);
            if (entities.All(e => e.Pipeline.Id != pipelineId))
            {
                throw new PipelineNotFoundException();
            }
        }

        /// <inheritdoc />
        public async Task AddTrackedPersona(Guid pipelineId, Guid personaId, IPipelineRepository pipelineRepository, IMapper mapper)
        {
            await pipelineRepository.AddOutboundRelationshipAsync<Tracks, PersonaEntity>(pipelineId, personaId);
        }

        /// <inheritdoc />
        public async Task AddTrackedFandom(Guid pipelineId, Guid fandomId, IPipelineRepository pipelineRepository, IMapper mapper)
        {
            await pipelineRepository.AddOutboundRelationshipAsync<Tracks, FandomEntity>(pipelineId, fandomId);
        }

        public async Task<IEnumerable<Pipeline>> GetAllPipelines(Guid? userId, IPipelineRepository pipelineRepository, IMapper mapper)
        {
            var result = await pipelineRepository.GetByUserIdAsync(userId);
            return mapper.Map<List<Pipeline>>(result);
        }
    }
}
