namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class RoleEntity : BaseEntity
    {
        public RoleEntity() { }

        public RoleEntity(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}
