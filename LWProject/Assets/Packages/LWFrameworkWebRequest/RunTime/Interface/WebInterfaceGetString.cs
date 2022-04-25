using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace LWFramework.WebRequest {
    public class WebInterfaceGetString : BaseWebInterface
    {
        public Action<string> Handler;

        public override void OnRequestFinished(DownloadHandler handler)
        {
            if (handler == null)
            {
                Handler?.Invoke("");
            }
            else
            {
                Handler?.Invoke(handler.text);
            }
        }

        public override void OnSetDownloadHandler(UnityWebRequest request)
        {

        }

        public override string OnGetDownloadString(DownloadHandler handler)
        {
            return handler.text;
        }

        public override void Reset()
        {
            OfflineHandler = null;
            Handler = null;
        }
    }

}
