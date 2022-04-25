using Cysharp.Threading.Tasks;
using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Core {
    public class LicenseServerConfig : BaseLicense
    {

        public override void Init()
        {
            base.Init();

        }
        public async override UniTask Checking()
        {
            try
            {
                IsLicensePass = false;
                // ServerConfig serverConfig = await ConfigDataTool.ReadDataAsync<ServerConfig>("project", "http://rd.unitygame.work:8080/LWProject", true);
                ServerConfig serverConfig = await ConfigDataTool.ReadDataAsync<ServerConfig>("project", "http://47.99.130.41:8080/LWProject", true);
                ServerConfigItem item = serverConfig.ConfigList.Find((ServerConfigItem find) =>
                {
                    return find.ProdouctName == Application.productName;
                });
                if (item == null)
                {
                    IsLicensePass = false;
                }
                else if (item.IsOpen == "True")
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
        
            
        }
    }
    public class ServerConfigItem
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProdouctName { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public string IsOpen { get; set; }      
    }
    public class ServerConfig 
    { 
        
        public List<ServerConfigItem> ConfigList { get; set; }
    }
}
