using System;
using System.Collections.Generic;
using System.Text;
using Tnomer.Data.Common.Repository;

namespace Tnomer.Net.Http.Rest
{
    public class RestRepositoryFactory : IRepositoryFactory
    {
        RestClientConfig _setup;
        public RestRepositoryFactory(RestClientConfig setup)
        {
            _setup = setup;
        }
        public IRepository<TRequest, TResponse, TResponseList> Create<TRequest, TResponse, TResponseList, TParams>(TParams resource)
            where TRequest : class
            where TResponse : class, new()
            where TResponseList : class, new()
        {
            // Type tLogger = loggers[typeof(RestRepository<TRequest, TResponse, TResponseList>)];
            return null;// new RestRepository<TRequest, TResponse, TResponseList>(_setup, resource.ToString());
        }
    }
}
