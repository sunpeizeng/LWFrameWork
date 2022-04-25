using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class KeyHelp : MonoSingleton<KeyHelp>
{
    [SerializeField,HorizontalGroup("AesKey")]
    private string m_AesKey;
    [SerializeField,ReadOnly, HorizontalGroup("EditorAesKey")]
    private string m_EditorAesKey;
    public string AesKey {
        get {
#if UNITY_EDITOR
            if (m_AesKey == "")
            {
                return EditorPrefs.GetString(Application.productName + "AesKey", m_AesKey);
            }
            else {
                return m_AesKey;
            }          
#else
            return m_AesKey;
#endif
        }
    }
    [OnInspectorInit]
    void ShowEditorAesKey() {
        m_EditorAesKey = AesKey;
    }
    [Button("生成"), HorizontalGroup("AesKey",Width =50)]
    void GenerateKeyBtnClick()
    {
        m_AesKey = GeneratePassword(32);
#if UNITY_EDITOR
        Debug.LogError("请立即生成一次config文件");
#endif
    }
    [Button("保存"), HorizontalGroup("AesKey", Width = 50)]
    void SaveKeyBtnClick()
    {
#if UNITY_EDITOR
        EditorPrefs.SetString(Application.productName + "AesKey", m_AesKey);
#endif
        ShowEditorAesKey();
    }
    [Button("复制"), HorizontalGroup("EditorAesKey", Width = 50)]
    void CopyEditorKeyBtnClick()
    {
#if UNITY_EDITOR
        GUIUtility.systemCopyBuffer = m_EditorAesKey;
#endif
    }
    [Button("显示备份Config")]
    void ShowConfigBtnClick()
    {
#if UNITY_EDITOR
        Debug.Log(EditorPrefs.GetString("ConfigBak"));
#endif
    }
    /// <summary>
    /// 获取随机码
    /// </summary>
    /// <param name="length">长度</param>
    /// <param name="availableChars">指定随机字符，为空，默认系统指定</param>
    /// <returns></returns>
    private string GeneratePassword(int length, string availableChars = null)
    {
        if (availableChars == null) availableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@##$%^&*()_+-=[]{}";

        var id = new char[length];
        for (var i = 0; i < length; i++)
        {
            id[i] = availableChars[Random.Range(0, availableChars.Length)];
        }

        return new string(id);
    }
}
