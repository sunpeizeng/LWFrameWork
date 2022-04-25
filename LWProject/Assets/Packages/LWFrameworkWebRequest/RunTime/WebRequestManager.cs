using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace LWFramework.WebRequest
{
    /// <summary>
    /// Web请求管理器
    /// </summary>
    public class WebRequestManager : IManager, IWebRequestManager
    {
        /// <summary>
        /// 所有网络接口
        /// </summary>
        private Dictionary<string, BaseWebInterface> WebInterfaces { get; set; } = new Dictionary<string, BaseWebInterface>();
        private bool m_IsOffline = false;
        /// <summary>
        /// 当前是否是离线状态
        /// </summary>
        public bool IsOffline
        {
            get => m_IsOffline; set => m_IsOffline = value;
        }

        private ObjectPool<WebInterfaceGetString> m_StrPool;
        private ObjectPool<WebInterfaceGetTexture2D> m_TexPool;
        private ObjectPool<WebInterfaceGetAudioClip> m_AudioPool;

        public void Init()
        {
            m_StrPool = new ObjectPool<WebInterfaceGetString>(5);
            m_TexPool = new ObjectPool<WebInterfaceGetTexture2D>(5);
            m_AudioPool = new ObjectPool<WebInterfaceGetAudioClip>(5);
        }

        public void Update()
        {
        }
    
        /// <summary>
        /// 发起网络数据请求，当前方法不需要注册
        /// </summary>
        /// <param name="interfaceUrl">请求地址</param>
        /// <param name="handler">回调函数</param>
        /// <param name="parameter"></param>
        public void SendRequestUrl(string interfaceUrl, Action<string> handler, WWWForm form,int timeout=10)
        {
            WebInterfaceGetString wi = m_StrPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            SendRequestPostForm(wi, form,timeout).Forget();
        }

        /// <summary>
        /// 发起网络数据请求，当前方法不需要注册
        /// </summary>
        /// <param name="interfaceUrl">请求地址</param>
        /// <param name="handler">回调函数</param>
        /// <param name="parameter">Json数据</param>
        public void SendRequestUrlJson(string interfaceUrl, Action<string> handler, string parameter, int timeout = 10)
        {
            WebInterfaceGetString wi = m_StrPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            SendRequestPostJson(wi, parameter, timeout).Forget();
        }
        /// <summary>
        /// 发起网络数据请求，当前方法不需要注册
        /// </summary>
        /// <param name="interfaceUrl">请求地址</param>
        /// <param name="handler">回调函数</param>
        /// <param name="parameter">拼接的字符串</param>
        public void SendRequestUrl(string interfaceUrl, Action<string> handler, int timeout = 10, params string[] parameter)
        {
            WebInterfaceGetString wi = m_StrPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            SendRequestAsync(wi, timeout, parameter).Forget();
        }
        /// <summary>
        /// 发起网络数据请求，当前方法不需要注册
        /// </summary>
        /// <param name="interfaceUrl">请求地址</param>
        /// <param name="handler">回调函数</param>
        public void SendRequestUrl(string interfaceUrl, Action<string> handler, int timeout = 10)
        {
            WebInterfaceGetString wi = m_StrPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            SendRequestAsync(wi, timeout).Forget();
        }
        /// <summary>
        /// 发起网络数据请求，当前方法不需要注册
        /// </summary>
        /// <param name="interfaceUrl">请求地址</param>
        /// <param name="handler">回调函数</param>
        public void SendRequestUrl(string interfaceUrl, Action<Texture2D> handler, int timeout = 10)
        {
            WebInterfaceGetTexture2D wi = m_TexPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            SendRequestAsync(wi, timeout).Forget();
        }
        /// <summary>
        /// 发起网络数据请求，当前方法不需要注册
        /// </summary>
        /// <param name="interfaceUrl">请求地址</param>
        /// <param name="handler">回调函数</param>
        public void SendRequestUrl(string interfaceUrl, Action<AudioClip> handler, int timeout = 10)
        {
            WebInterfaceGetAudioClip wi = m_AudioPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            SendRequestAsync(wi, timeout).Forget();
        }
        
        private async UniTaskVoid SendRequestAsync(BaseWebInterface wi, int timeout , params string[] parameter)
        {
            UIWidgetHelp.Instance.OpenLoadingView();
            StringBuilder builder = new StringBuilder();
            builder.Append(wi.Url);
            if (parameter.Length > 0)
            {
                builder.Append("?");
                builder.Append(parameter[0]);
            }
            for (int i = 1; i < parameter.Length; i++)
            {
                builder.Append("&");
                builder.Append(parameter[i]);
            }
            string url = builder.ToString();

            try
            {
                using (UnityWebRequest request = UnityWebRequest.Get(url))
                {
                    DateTime begin = DateTime.Now;

                    wi.OnSetDownloadHandler(request);
                    request.timeout = timeout;
                    await request.SendWebRequest();

                    DateTime end = DateTime.Now;

                    if (!request.isNetworkError && !request.isHttpError)
                    {

                        LWDebug.Log(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 收到回复：{4}字节  string:{5}"
                            , begin.ToString("mm:ss:fff"), wi.Name, url, end.ToString("mm:ss:fff"), request.downloadHandler.data.Length, wi.OnGetDownloadString(request.downloadHandler)));
                        wi.OnRequestFinished(request.downloadHandler);
                    }
                    else
                    {
                        LWDebug.LogError(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 网络请求出错：{4}", begin.ToString("mm:ss:fff"), wi.Name, url, end.ToString("mm:ss:fff"), request.error));

                        wi.OnRequestFinished(null);
                    }
                    request.Dispose();
                    UnspawnInterface(wi);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally {
                UIWidgetHelp.Instance.CloseLoadingView();
            }
           
           
        }

       

        private async UniTaskVoid SendRequestPostForm(BaseWebInterface wi, WWWForm form,int timeout)
        {
            UIWidgetHelp.Instance.OpenLoadingView();
            string url = wi.Url;
            try
            {
                using (UnityWebRequest request = UnityWebRequest.Post(url, form))
                {
                    DateTime begin = DateTime.Now;

                    wi.OnSetDownloadHandler(request);
                    request.timeout = timeout;
                    await request.SendWebRequest();

                    DateTime end = DateTime.Now;

                    if (!request.isNetworkError && !request.isHttpError)
                    {
                        LWDebug.Log(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 收到回复：{4}字节  string:{5}"
                            , begin.ToString("mm:ss:fff"), wi.Name, url, end.ToString("mm:ss:fff"), request.downloadHandler.data.Length, wi.OnGetDownloadString(request.downloadHandler)));
                        wi.OnRequestFinished(request.downloadHandler);
                    }
                    else
                    {
                        LWDebug.LogError(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 网络请求出错：{4}", begin.ToString("mm:ss:fff"), wi.Name, url, end.ToString("mm:ss:fff"), request.error));

                        wi.OnRequestFinished(null);
                    }
                    request.Dispose();
                    UnspawnInterface(wi);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally {
                UIWidgetHelp.Instance.CloseLoadingView();
            }
            
           
        }
        
        private async UniTaskVoid SendRequestPostJson(BaseWebInterface wi, string jsonData, int timeout)
        {
            UIWidgetHelp.Instance.OpenLoadingView();
            string url = wi.Url;
            try
            {
                using (UnityWebRequest request = UnityWebRequest.Post(url, "Post"))
                {
                    DateTime begin = DateTime.Now;
                    byte[] body = Encoding.UTF8.GetBytes(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(body);
                    request.SetRequestHeader("Content-Type", "application/json");
                    wi.OnSetDownloadHandler(request);
                    request.timeout = timeout;
                    await request.SendWebRequest();

                    DateTime end = DateTime.Now;

                    if (!request.isNetworkError && !request.isHttpError)
                    {
                        LWDebug.Log(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 收到回复：{4}字节  string:{5}"
                            , begin.ToString("mm:ss:fff"), wi.Name, url, end.ToString("mm:ss:fff"), request.downloadHandler.data.Length, wi.OnGetDownloadString(request.downloadHandler)));

                        wi.OnRequestFinished(request.downloadHandler);
                    }
                    else
                    {
                        LWDebug.LogError(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 网络请求出错：{4}", begin.ToString("mm:ss:fff"), wi.Name, url, end.ToString("mm:ss:fff"), request.error));

                        wi.OnRequestFinished(null);
                    }
                    request.Dispose();
                    UnspawnInterface(wi);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally {
                UIWidgetHelp.Instance.CloseLoadingView();
            }
            
            
        }
      
        private void UnspawnInterface(BaseWebInterface baseWebInterface)
        {
            if (baseWebInterface is WebInterfaceGetString)
            {
                m_StrPool.Unspawn(baseWebInterface as WebInterfaceGetString);
            }
            else if (baseWebInterface is WebInterfaceGetTexture2D)
            {
                m_TexPool.Unspawn(baseWebInterface as WebInterfaceGetTexture2D);
            }
            else if (baseWebInterface is WebInterfaceGetAudioClip)
            {
                m_AudioPool.Unspawn(baseWebInterface as WebInterfaceGetAudioClip);
            }
            else
            {
                baseWebInterface = null;
            }
        }


     
    }
}
