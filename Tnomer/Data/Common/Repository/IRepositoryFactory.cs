using System;
using System.Collections.Generic;
using System.Text;

namespace Tnomer.Data.Common.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TRequest, TResponse, TResponseList> Create<TRequest, TResponse, TResponseList, TParams>(TParams setup)
            where TRequest : class
            where TResponse : class, new()
            where TResponseList : class, new()
            ;
    }
}
