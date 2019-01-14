// <copyright file="UserEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using System;
    using Neo4j.Driver.V1;
    using Providers;

    /// <summary>
    /// Data-layer representation of a user.
    /// </summary>
    public class UserEntity : GraphEntity
    {
        /// <summary>
        /// Gets or sets the user's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user's date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <inheritdoc />
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
