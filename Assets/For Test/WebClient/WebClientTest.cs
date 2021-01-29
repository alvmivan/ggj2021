using System;
using Bones.Scripts.Features.Web;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WebClient = Bones.Scripts.Features.Web.WebClient;


public class WebClientTest : MonoBehaviour
{
    public Button send;
    public TMP_Dropdown method;
    public TMP_InputField jsonInput;
    public TMP_InputField urlInput;
    public TextMeshProUGUI response;


    private void Start()
    {
        send
            .OnClickAsObservable()
            .SelectMany(_ => Req())
            .Subscribe(info =>
            {
                Debug.Log("LE RESPONSEEEE    "+info.text);
                response.text = info.text;
            }, Debug.LogError);
    }

    private IObservable<RequestInfo> Req()
    {
        return method.value == 0 ? WebClient.Get(urlInput.text) : WebClient.Post(urlInput.text, jsonInput.text);
    }
}