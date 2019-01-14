// <copyright file="UserRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    using Neo4j.Driver.V1;

    /// <summary>
    /// Repository class for data handling relating to users.
    /// </summary>
    public class UserRepository : BaseRepository<UserEntity>, IRepository<UserEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="driver">The graph db driver.</param>
        public UserRepository(IDriver driver)
            : base(driver)
        {
        }

        /// <inheritdoc />
        public async Task<UserEntity> Create(UserEntity data)
        {
            var query = $@"CREATE (v:{VertexTypes.USER} 
                        {{ 
                            id: '{data.Id}', 
                            username: '{data.Username}',
                            dob: '{data.DateOfBirth}'
                        }}) RETURN v";
            return await LoadQuerySingle(query);
        }

        /// <inheritdoc />
        public int Count(string userId)
        {
            return CountAll(VertexTypes.USER);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserEntity>> GetAll(string userId)
        {
            var query = $"MATCH (v:{VertexTypes.USER}) RETURN v";
            return await LoadQuery(query);
        }
    }
}
