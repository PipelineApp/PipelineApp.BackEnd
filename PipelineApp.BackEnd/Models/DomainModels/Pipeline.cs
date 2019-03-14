namespace PipelineApp.BackEnd.Models.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Data.Entities;
    using Infrastructure.Data.Entities.EntityCollections;
    using RequestModels.Pipeline;
    using ViewModels;

    public class Pipeline
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Fandom> Fandoms { get; set; }

        public List<Persona> Personas { get; set; }

        public Pipeline(CreatePipelineRequestModel dto)
        {
            Name = dto.Name;
            Description = dto.Description;
        }

        public Pipeline(PipelineEntityDataCollection entity)
        {
            Id = entity.Pipeline?.Id;
            Name = entity.Pipeline?.Name;
            Description = entity.Pipeline?.Description;
            Fandoms = entity.Fandoms.Select(f => new Fandom(f)).ToList();
            Personas = entity.Personas.Select(p => new Persona(p)).ToList();
        }

        public Pipeline(PipelineEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
        }

        public PipelineEntity ToEntity()
        {
            var entity = new PipelineEntity();
            if (Id != null)
            {
                entity.Id = Id.GetValueOrDefault();
            }

            entity.Name = Name;
            entity.Description = Description;
            return entity;
        }

        public PipelineDto ToDto()
        {
            var dto = new PipelineDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Fandoms = Fandoms?.Select(f => f.ToDto()).ToList(),
                Personas = Personas?.Select(p => p.ToDto()).ToList()
            };
            return dto;
        }
    }
}
