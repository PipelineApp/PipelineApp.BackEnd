namespace PipelineApp.BackEnd.Infrastructure.Data.Entities.EntityCollections
{
    using System.Collections.Generic;

    public class PipelineEntityDataCollection
    {
        public PipelineEntity Pipeline { get; set; }

        public List<FandomEntity> Fandoms { get; set; }

        public List<PersonaEntity> Personas { get; set; }
    }
}
