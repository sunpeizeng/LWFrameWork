using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.WebRequest {
    public interface IWebRequestManager
    {
        bool IsOffline { get; set; }

        void SendRequestUrl(string interfaceUrl, Action<string> handler, WWWForm form, int timeout = 10);
        void SendRequestUrlJson(string interfaceUrl, Action<string> handler, string parameter, int timeout = 10);
        void SendRequestUrl(string interfaceUrl, Action<string> handler, int timeout = 10);
        void SendRequestUrl(string interfaceUrl, Action<Texture2D> handler, int timeout = 10);
        void SendRequestUrl(string interfaceUrl, Action<AudioClip> handler, int timeout = 10);
        void SendRequestUrl(string interfaceUrl, Action<string> handler, int timeout = 10, params string[] parameter);
    }

}
