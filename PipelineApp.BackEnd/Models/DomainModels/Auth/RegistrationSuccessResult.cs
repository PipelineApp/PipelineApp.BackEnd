namespace PipelineApp.BackEnd.Models.DomainModels.Auth
{
    using Newtonsoft.Json;

    public class RegistrationSuccessResult
    {

        [JsonProperty("_id")]
        public string UserId { get; set; }

        public string Email { get; set; }
    }
}
