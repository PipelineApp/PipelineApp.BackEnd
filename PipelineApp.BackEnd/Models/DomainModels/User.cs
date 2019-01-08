namespace PipelineApp.BackEnd.Models.DomainModels
{
    using System;

    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
