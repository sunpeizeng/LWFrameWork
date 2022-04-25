using UnityEngine;
using LitJson;
using LWFramework.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class ConfigDataTool
{
    public static string ConfigFilePath = Application.streamingAssetsPath;
    /// <summary>
    /// 读取文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">文件名</param>
    ///  <param name="path">本地路径</param>
    /// <param name="isEnc">是否加密</param>
    /// <returns></returns>
    public static T ReadData<T>(string fileName, string path = null, bool isEnc = true)
    {
        string fullPath = path == null ? ConfigFilePath + "/" + fileName : path + "/" + fileName;
        try
        {        
            //LWDebug.Log($"ReadData:{ typeof(T).FullName} 加载路径 :: { fullPath}");
            string dataStr = FileTool.ReadFromFile(fullPath);
            if (isEnc)
            {
                dataStr = AESUtility.AesDecrypt(dataStr, KeyHelp.Instance.AesKey);
            }
            return JsonMapper.ToObject<T>(dataStr);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ReadData:{fullPath}:路径中没有 {fileName}文件");
            Debug.LogError(e.Message);
            return default;
        }
    }
    /// <summary>
    /// 读取网络文件文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">文件名</param>
    /// <param name="url">网络路径</param>
    /// <param name="isEnc">是否加密</param>
    /// <returns></returns>
    public static async UniTask<T> ReadDataAsync<T>(string fileName, string url = null, bool isEnc = true)
    {
        string fullPath = url == null ? ConfigFilePath + "/" + fileName : url + "/" + fileName;
        try
        {           
            //LWDebug.Log($"ReadDataAsync:{ typeof(T).FullName} 加载路径 :: { fullPath}");
            UnityWebRequest _www = UnityWebRequest.Get(fullPath);
            _www.downloadHandler = new DownloadHandlerBuffer();
            await _www.SendWebRequest();
            string dataStr = _www.downloadHandler.text;
            if (isEnc)
            {
                dataStr = AESUtility.AesDecrypt(dataStr, KeyHelp.Instance.AesKey);
            }

            return JsonMapper.ToObject<T>(dataStr);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ReadDataAsync:{fullPath}:路径中没有 {fileName}文件");
            Debug.LogError(e.Message);
            return default;
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="path">路径</param>
    /// <param name="data">数据对象</param>
    /// <param name="isEnc">是否加密</param>
    public static void Create(string fileName, object data, bool isEnc = true, string path = null)
    {
        string fullPath = path == null ? ConfigFilePath + "/" + fileName : path + "/" + fileName;
        string dataStr = JsonMapper.ToJson(data);
        if (isEnc)
        {
            dataStr = AESUtility.AesEncrypt(dataStr, KeyHelp.Instance.AesKey);
        }
        if (FileTool.ExistsFile(fullPath))
        {
            FileTool.DeleteFile(fullPath);
        }
        FileTool.CreateFile(fullPath);
        FileTool.WriteToFile(fullPath, dataStr);
        Debug.Log("资源保存路径 ::" + ConfigFilePath + "/" + fileName);
    }
    /// <summary>
    /// 删除文件
    /// </summary>
    public static void Delete(string fileName)
    {
        if (FileTool.ExistsFile(fileName, ConfigFilePath))
        {
            FileTool.DeleteFile(fileName, ConfigFilePath);
        }
    }
}
