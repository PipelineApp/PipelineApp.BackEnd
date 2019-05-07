// <copyright file="CreateRootPostRequestModel.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.RequestModels.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Exceptions.Post;

    /// <summary>
    /// Request model containing data about a user's request to create a new root post.
    /// </summary>
    public class CreateRootPostRequestModel
    {
        /// <summary>
        /// Gets or sets the post's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the post's content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the ID of the fandom the post belongs to.
        /// </summary>
        public Guid FandomId { get; set; }

        /// <summary>
        /// Throws an exception if the post is not valid.
        /// </summary>
        /// <exception cref="InvalidPostException">Thrown if the post is not valid.</exception>
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
