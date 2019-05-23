namespace PipelineApp.BackEnd.Models.DomainModels
{
    using System;

    public class PostVersion : Version
    {
        /// <summary>
        /// Gets or sets the post's unique identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the post's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the post's content.
        /// </summary>
        public string Content { get; set; }

        public Guid FandomId { get; set; }
    }
}
