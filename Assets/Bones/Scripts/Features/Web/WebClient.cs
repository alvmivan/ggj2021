using System;
using System.Collections.Generic;

namespace Bones.Scripts.Features.Web
{
    public static class WebClient
    {
        private static readonly Lazy<InternalWebClient> Client = new Lazy<InternalWebClient>(()=>new InternalWebClient());
        
        public static IObservable<RequestInfo> Get(string server, bool bits = false)
        {
            return Client.Value.Get(server, bits);
        }

        public static IObservable<RequestInfo> Delete(string server,  bool bits = false)
        {
            return Client.Value.Delete(server, bits);
        }

        public static IObservable<RequestInfo> Post(string server, string json, bool bits = false)
        {
            return Client.Value.Post(server, json, bits);
        }

        public static IObservable<RequestInfo> Put(string server, string json, bool bits = false)
        {
            return Client.Value.Put(server, json, bits);
        }
    }
}