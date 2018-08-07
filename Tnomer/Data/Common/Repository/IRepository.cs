using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tnomer.Data.Common.Repository
{
    public interface IRepository<TRequest, TResponse, TResponseList>
        where TRequest : class
        where TResponse : class, new()
        where TResponseList : class, new()
    {
        Task<TResponseList> GetAsync();
        Task<TResponseList> GetAsync(RepositoryRequestParams args);
        Task<TResponse> GetAsync(int id);
        Task<TResponse> GetAsync(int id, RepositoryRequestParams args);
        Task<TResponse> InsertAsync(TRequest item);
        Task<TResponse> InsertAsync(TRequest item, RepositoryRequestParams args);
        Task RemoveAsync(int id);
        Task UpdateAsync(int id, TRequest item);
        Task UpdateAsync(int id, TRequest item, RepositoryRequestParams args);
    }
}

