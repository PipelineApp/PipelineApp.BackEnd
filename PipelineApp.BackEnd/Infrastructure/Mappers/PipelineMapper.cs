// <copyright file="PipelineMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Mappers
{
    using System.Linq;
    using Data.Entities;
    using Data.Entities.EntityCollections;
    using Interfaces.Mappers;
    using Models.DomainModels;
    using Models.RequestModels.Pipeline;
    using Models.ViewModels;

    /// <inheritdoc />
    public class PipelineMapper : IPipelineMapper
    {
        private readonly IFandomMapper _fandomMapper;
        private readonly IPersonaMapper _personaMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineMapper"/> class.
        /// </summary>
        /// <param name="fandomMapper">The fandom mapper.</param>
        /// <param name="personaMapper">The persona mapper.</param>
        public PipelineMapper(IFandomMapper fandomMapper, IPersonaMapper personaMapper)
        {
            _fandomMapper = fandomMapper;
            _personaMapper = personaMapper;
        }

        /// <inheritdoc />
        public PipelineDto ToDto(Pipeline pipeline)
        {
            var dto = new PipelineDto
            {
                Id = pipeline.Id,
                Name = pipeline.Name,
                Description = pipeline.Description,
                Fandoms = pipeline.Fandoms?.Select(f => _fandomMapper.ToDto(f)).ToList(),
                Personas = pipeline.Personas?.Select(p => _personaMapper.ToDto(p)).ToList()
            };
            return dto;
        }

        /// <inheritdoc />
        public Pipeline ToDomainModel(UpsertPipelineRequestModel requestModel)
        {
            var pipeline = new Pipeline
            {
                Id = requestModel.Id,
                Description = requestModel.Description,
                Name = requestModel.Name
            };
            return pipeline;
        }

        /// <inheritdoc />
        public Pipeline ToDomainModel(PipelineEntityDataCollection entities)
        {
            var pipeline = new Pipeline
            {
                Id = entities.Pipeline.Id,
                Description = entities.Pipeline.Description,
                Name = entities.Pipeline.Name,
                Fandoms = entities.Fandoms.Select(f => _fandomMapper.ToDomainModel(f)).ToList(),
                Personas = entities.Personas.Select(p => _personaMapper.ToDomainModel(p)).ToList()
            };
            return pipeline;
        }

        /// <inheritdoc />
        public Pipeline ToDomainModel(PipelineEntity entity)
        {
            var pipeline = new Pipeline
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name
            };
            return pipeline;
        }

        /// <inheritdoc />
        public PipelineEntity ToEntity(Pipeline model)
        {
            var entity = new PipelineEntity();
            entity.Id = model.Id ?? entity.Id;
            entity.Description = model.Description;
            entity.Name = model.Name;
            return entity;
        }
    }
}
