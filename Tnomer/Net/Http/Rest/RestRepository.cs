using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tnomer.Data.Common.Repository;

namespace Tnomer.Net.Http.Rest
{
    public class RestRepository<TRequest, TResponse, TResponseList> : IRepository<TRequest, TResponse, TResponseList>
           where TRequest : class, new()
          where TResponse : class, new()
          where TResponseList : class, new()
    {

        RestClient _ctrl;

        public RestRepository(RestClientConfig config, String resource)
        {
            _ctrl = new RestClient
                (
                    config.ServiceResourceUrl + "//" + resource,
                    config.Headers,
                    config.Encoding
                );
        }

        public async Task<TResponseList> GetAsync()
        {
            var response = await _ctrl.Get();

            var model = JsonConvert.DeserializeObject<TResponseList>(response);

            return model;
        }

        public async Task<TResponseList> GetAsync(RepositoryRequestParams parameters)
        {
            var response = await _ctrl.Get(RepositoryRequestParams.GetParametersString(parameters));

            return JsonConvert.DeserializeObject<TResponseList>(response);
        }


        public async Task<TResponse> GetAsync(Int32 id, RepositoryRequestParams parameters)
        {
            var response = await _ctrl.Get(id, RepositoryRequestParams.GetParametersString(parameters));

            var model = JsonConvert.DeserializeObject<TResponse>(response);

            return model;
        }

        public async Task<TResponse> GetAsync(Int32 id)
        {
            return await GetAsync(id, null);
        }


        public async Task<TResponse> InsertAsync(TRequest item, RepositoryRequestParams parameters)
        {
            var request = JsonConvert.SerializeObject(item);

            var model = JsonConvert.DeserializeObject<TResponse>(await _ctrl.Insert(request, RepositoryRequestParams.GetParametersString(parameters)));

            return model;
        }

        public async Task<TResponse> InsertAsync(TRequest item)
        {
            return await InsertAsync(item, null);
        }


        public async Task UpdateAsync(Int32 id, TRequest item, RepositoryRequestParams parameters)
        {
            var model = JsonConvert.SerializeObject(item);

            await _ctrl.Update(model, id, RepositoryRequestParams.GetParametersString(parameters));
        }

        public async Task UpdateAsync(Int32 id, TRequest item)
        {
            await UpdateAsync(id, item, null);
        }


        public async Task RemoveAsync(Int32 id)
        {
            await _ctrl.Delete(id);
        }
    }
}
