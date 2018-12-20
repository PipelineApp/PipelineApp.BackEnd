namespace PipelineApp.BackEnd.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Data.Entities;

    public interface IGraphDbClient : IDisposable
    {
        Task<Fandom> CreateFandom(string name);

        int Count(string vertexType);

        IEnumerable<T> GetAll<T>(string vertexType)
            where T : GraphEntity, new();
    }
}
