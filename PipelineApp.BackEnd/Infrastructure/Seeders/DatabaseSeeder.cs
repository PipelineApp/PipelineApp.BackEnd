// <copyright file="DatabaseSeeder.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Seeders
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Data;
    using Data.Constants;
    using Data.Entities;
    using Interfaces;
    using Interfaces.Repositories;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Seeder class used to initialize database content for a blank database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabaseSeeder
    {
        private readonly IFandomRepository _repository;
        private readonly RoleManager<RoleEntity> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSeeder"/> class.
        /// </summary>
        /// <param name="repository">The fandom repository.</param>
        /// <param name="roleManager">The role manager.</param>
        public DatabaseSeeder(IFandomRepository repository, RoleManager<RoleEntity> roleManager)
        {
            _repository = repository;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Seeds the database with fandom vertices.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task Seed()
        {
            await InitFandoms();
            await InitRoles();
        }

        private async Task InitRoles()
        {
            var userRole = await _roleManager.FindByNameAsync(Roles.USER);
            if (userRole == null)
            {
                userRole = new RoleEntity(Roles.USER);
                await _roleManager.CreateAsync(userRole);
            }
        }

        private async Task InitFandoms()
        {
            var fandomsCount = await _repository.Count();
            if (fandomsCount > 0)
            {
                return;
            }
            _repository.CreateWithRelationships(new FandomEntity { Name = "Star Trek" });
            _repository.CreateWithRelationships(new FandomEntity { Name = "Mass Effect" });
            _repository.CreateWithRelationships(new FandomEntity { Name = "Dragon Age" });
        }
    }
}
