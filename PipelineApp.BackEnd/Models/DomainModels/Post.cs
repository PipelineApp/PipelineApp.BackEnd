using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipelineApp.BackEnd.Models.DomainModels
{
    public class Post
    {
        /// <summary>
        /// Gets or sets the post's unique identifier.
        /// </summary>
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Post ParentPost { get; set; }

        public Post RootPost { get; set; }
    }
}
