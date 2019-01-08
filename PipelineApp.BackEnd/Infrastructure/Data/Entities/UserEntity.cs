namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using System;
    using Neo4j.Driver.V1;
    using Providers;

    public class UserEntity : GraphEntity
    {
        public string Username { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public override void LoadRecord(IRecord record)
        {
            var root = record.GetOrDefault("v", (INode)null);
            if (root == null)
            {
                return;
            }
            Id = root.GetOrDefault<string>("id", null);
            Username = root.GetOrDefault<string>("username", null);
            DateOfBirth = root.GetOrDefault<DateTime?>("dob", null);
        }
    }
}
