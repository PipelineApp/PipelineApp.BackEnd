namespace PipelineApp.BackEnd.Models.ViewModels
{
    using System;

    public class PostDto
    {
        /// <summary>
        /// Gets or sets the pipeline's unique identifier.
        /// </summary>
        public Guid? Id { get; set; }

        public Guid PersonaId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public PostDto ParentPost { get; set; }

        public PostDto RootPost { get; set; }
    }
}
