namespace PipelineApp.BackEnd.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class PipelineDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<FandomDto> Fandoms { get; set; }

        public List<PersonaDto> Personas { get; set; }
    }
}
