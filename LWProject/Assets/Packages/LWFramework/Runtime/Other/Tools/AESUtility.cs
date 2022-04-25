using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AESUtility 
{
    private const string defaultKey = "iw//pKlMWBCfbs=Z+tRLCIrTHEzeed31";
    /// <summary>
    ///  AES 加密
    /// </summary>
    /// <param name="str">明文（待加密）</param>
    /// <param name="key">密文</param>
    /// <returns></returns>
    public static string AesEncrypt(string str, string Key = defaultKey)
    {
        if (string.IsNullOrEmpty(str)) return null;
        Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

        RijndaelManaged rm = new RijndaelManaged
        {
            Key = Encoding.UTF8.GetBytes(Key),
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        ICryptoTransform cTransform = rm.CreateEncryptor();
        Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    ///  AES 解密
    /// </summary>
    /// <param name="str">明文（待解密）</param>
    /// <param name="key">密文</param>
    /// <returns></returns>
    public static string AesDecrypt(string str, string Key = defaultKey)
    {
        if (string.IsNullOrEmpty(str)) return null;
        Byte[] toEncryptArray = Convert.FromBase64String(str);

        RijndaelManaged rm = new RijndaelManaged
        {
            Key = Encoding.UTF8.GetBytes(Key),
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        ICryptoTransform cTransform = rm.CreateDecryptor();
        Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Encoding.UTF8.GetString(resultArray);
    }

    /// <summary>
    /// AES 算法加密(ECB模式) 将明文加密
    /// </summary>
    /// <param name="toEncryptArray,">明文</param>
    /// <param name="Key">密钥</param>
    /// <returns>加密后base64编码的密文</returns>
    public static byte[] AesEncrypt(byte[] toEncryptArray, string Key)
    {
        try
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return resultArray;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return null;
        }
    }

    /// <summary>
    /// AES 算法解密(ECB模式) 将密文base64解码进行解密，返回明文
    /// </summary>
    /// <param name="toDecryptArray">密文</param>
    /// <param name="Key">密钥</param>
    /// <returns>明文</returns>
    public static byte[] AesDecrypt(byte[] toDecryptArray, string Key)
    {
        try
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            return resultArray;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return null;
        }
    }

    /// <summary>
    /// AES 算法加密(ECB模式) 无padding填充，用于分块解密
    /// </summary>
    /// <param name="toEncryptArray,">明文</param>
    /// <param name="Key">密钥</param>
    /// <returns>加密后base64编码的密文</returns>
    public static byte[] AesEncryptWithNoPadding(byte[] toEncryptArray, string Key)
    {
        try
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.None;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return resultArray;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return null;
        }
    }

    /// <summary>
    /// AES 算法解密(ECB模式) 无padding填充，用于分块解密
    /// </summary>
    /// <param name="toDecryptArray">密文</param>
    /// <param name="Key">密钥</param>
    /// <returns>明文</returns>
    public static byte[] AesDecryptWithNoPadding(byte[] toDecryptArray, string Key)
    {
        try
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.None;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            return resultArray;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return null;
        }
    }
    /// <summary>
    /// 获取文件的hash值
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetHash(string filePath)
    {
        var hash = SHA1.Create();
        var stream = new FileStream(filePath, FileMode.Open);
        byte[] hashByte = hash.ComputeHash(stream);
        stream.Close();
        return BitConverter.ToString(hashByte).Replace("-", "");
    }

}
