namespace PipelineApp.BackEnd.Infrastructure.Data.Entities.EntityCollections
{
    using System.Collections.Generic;

    public class PostEntityDataCollection
    {
        public PostEntity Post { get; set; }

        public List<PostVersionEntity> Versions { get; set; }

        public PostEntity ParentPost { get; set; }

        public PostEntity RootPost { get; set; }
    }
}
