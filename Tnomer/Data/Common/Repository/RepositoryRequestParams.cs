using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
//using System.Linq;
using System.Text;
using Tnomer.Net.Http;

namespace Tnomer.Data.Common.Repository
{
    public class RepositoryRequestParams
    {
        public RepositoryRequestParams() { ParamsList = new ConcurrentDictionary<String, Object>(); }
        public RepositoryRequestParams(ConcurrentDictionary<String, Object> paramsList) { ParamsList = paramsList; }

        internal ConcurrentDictionary<String, Object> ParamsList;

        public void SetValue(String key, Object value)
        {
            var inc = ParamsList.AddOrUpdate(key, value,(k,v)=> value);
        }
        public Object GetValue(String key)
        {
            object o;

            ParamsList.TryGetValue(key, out o);

            return o;
        }

        public static String GetParametersString(RepositoryRequestParams parameters)
        {
            string str;
            var p = parameters?.ParamsList;

            if (p != null && p.Any())
            {
                str = "?" + String.Join("&", p.Select(t => t.Key + "=" + t.Value));
                if (str == null) throw new Exception("Неверные параметры URL.");
            }
            else str = "";

            return str;
        }

        public static T GetParameters<T>(String query)
            where T : RepositoryRequestParams, new()
        {
            if (query.Length>0 && query[0] == '?') query = query.Substring(1);

            var e = query.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(
                    t =>
                    {
                        var s = t.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length == 2)
                            return new KeyValuePair<String, Object>[] { new KeyValuePair<String, Object>(s[0], s[1]) };
                        else
                            return new KeyValuePair<String, Object>[0];
                    }
                    );

            var cd = new ConcurrentDictionary<String, Object>(e);

            return new T() { ParamsList = cd };

        }
    }
}
