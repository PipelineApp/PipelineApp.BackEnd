namespace PipelineApp.BackEnd.Models.RequestModels.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Exceptions.Pipeline;

    public class CreatePipelineRequestModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public void AssertIsValid()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add("You must provide a name for this pipeline.");
            }

            if (errors.Any())
            {
                throw new InvalidPipelineException(errors);
            }
        }
    }
}
