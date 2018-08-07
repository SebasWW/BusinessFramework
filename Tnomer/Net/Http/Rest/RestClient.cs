using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tnomer.Net.Http
{
    public class RestClient : IDisposable
    {

        String _url;
        WebClient _wc = new WebClient();

        public RestClient(String url, IEnumerable<KeyValuePair<String, String>> headers, Encoding encoding)
        {
            _RestController(url, headers, encoding);
        }

        public RestClient(String url, IEnumerable<KeyValuePair<String, String>> headers)
        {
            _RestController(url, headers, Encoding.UTF8);
        }

        public RestClient(String url)
        {
            _RestController(url, null, Encoding.UTF8);
        }

        void _RestController(String url, IEnumerable<KeyValuePair<String, String>> headers, Encoding encoding)
        {
            _url = url;
            _wc.Encoding = encoding;
            _wc.Headers.Set("Content-Type", "application/json");

            if (headers != null)
            {
                foreach (var h in headers)
                {
                    _wc.Headers.Set(h.Key.ToString(), h.Value.ToString());
                }
            }
        }

        public async Task<String> Get()
        {
            return await _wc.DownloadStringTaskAsync(_url);
        }

        public async Task<String> Get(String parameters)
        {
            return await _wc.DownloadStringTaskAsync( _url + parameters);
        }


        public async Task<String> Get(Int32 id, String parameters)
        {
            return await _wc.DownloadStringTaskAsync(_url + "/" + id.ToString() + parameters);
        }

        public async Task Update(String model, Int32 id, String parameters)
        {
            await _wc.UploadStringTaskAsync(_url + "/" + id.ToString() + parameters, "PUT", model);
        }

        public async Task<String> Insert(String model, String parameters)
        {
            return await _wc.UploadStringTaskAsync(_url + parameters, "POST", model);
        }

        public async Task Delete(Int32 id)
        {
            await _wc.UploadStringTaskAsync(_url + "/" + id.ToString(), "DELETE", "");
        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _wc.Dispose();
                }

                disposedValue = true;
            }
        }

        // ~RestClient() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
