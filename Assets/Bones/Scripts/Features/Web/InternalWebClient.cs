using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Bones.Scripts.Features.Web
{
    class InternalWebClient
    {
        public IObservable<RequestInfo> Get(string server, bool bits = false)
        {
            return ObservableRequest(WebMethod.Get, server, bits);
        }
        public IObservable<RequestInfo> Post(string server, string json, bool bits = false)
        {
            return ObservableRequest(WebMethod.Post, server, bits, json);
        }
        public IObservable<RequestInfo> Delete(string server,  bool bits = false)
        {
            return ObservableRequest(WebMethod.Delete, server, bits);
        }
        public IObservable<RequestInfo> Put(string server, string json, bool bits = false)
        {
            return ObservableRequest(WebMethod.Put, server, bits, json);
        }

        private IObservable<RequestInfo> ObservableRequest(WebMethod webMethod, string server,
            bool downloadBits, string data = "")
        {
            UnityWebRequest www;
            switch (webMethod)
            {
                case WebMethod.Post:
                    www = UnityWebRequest.Post(server, data);
                    break;
                case WebMethod.Put:
                    www = UnityWebRequest.Put(server, data);
                    break;
                case WebMethod.Delete:
                    www = UnityWebRequest.Delete(server);
                    break;
                case WebMethod.Get:
                    www = UnityWebRequest.Get(server);
                    break;
                default:
                    www = UnityWebRequest.Get(server);
                    break;
            }

            Debug.Log($" --- Web Request --- {server}   {data}");
            
            IEnumerator RequestRoutine(UnityWebRequest req) { yield return req.SendWebRequest(); }
            
            return RequestRoutine(www)
                .ToObservable()
                .Do(_ =>
                {
                    if (www.isNetworkError)
                    {
                        throw new Exception("Newtwork error " + www.error + " " + server);
                    }

                    if (www.isHttpError)
                    {
                        throw new Exception("HTTP error " + www.error + " " + server);
                    }
                })
                .Select(_ => new RequestInfo
                {
                    data = downloadBits ? www.downloadHandler.data : new byte[0],
                    text = www.downloadHandler.text
                })
                .Do(info => Debug.Log($" --- Web Response --- {server}  {info.text}"))
                .ObserveOnMainThread()
                .First();
        }
    }
}