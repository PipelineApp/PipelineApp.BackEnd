namespace PipelineApp.BackEnd.Models.RequestModels.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Exceptions.Post;

    public class CreateBasePostRequestModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid FandomId { get; set; }

        public void AssertIsValid()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(Title) && string.IsNullOrWhiteSpace(Content))
            {
                errors.Add("You must include a title or content for your post.");
            }

            if (errors.Any())
            {
                throw new InvalidPostException(errors);
            }
        }
    }
}
