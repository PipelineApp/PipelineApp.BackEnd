namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Models.DomainModels;
    using Repositories;

    public interface IPostService
    {
        Task<Post> CreateBasePost(Post post, Guid personaId, IPostRepository postRepository, IMapper mapper);
    }
}
