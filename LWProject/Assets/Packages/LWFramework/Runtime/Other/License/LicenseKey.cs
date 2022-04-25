using Cysharp.Threading.Tasks;
using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
namespace LWFramework.Core {
    public class LicenseKey : BaseLicense
    {

        public override void Init()
        {
            base.Init();
            

        }
        public async override UniTask Checking()
        {
            try
            {
                string m_ReadKey = ConfigDataTool.ReadData<LicenseKeyData>("a", "C:\\", true).value;
                string value = SystemInfoTool.GetGraphicsDeviceID() + Application.productName;
                if (value == m_ReadKey)
                {
                    IsLicensePass = true;
                }
                else
                {
                    IsLicensePass = false;
                }
            }
            catch (Exception)
            {

                IsLicensePass = false;
            }
            finally {
                await UniTask.WaitForEndOfFrame();
            }                             
        }

        public static void CreateKeyFile(string str) {
            ConfigDataTool.Create("a",new LicenseKeyData { value = str }, true, "D:\\");
        }

       
       
    }
    class LicenseKeyData{
        public string value;
    }
}
