using Cysharp.Threading.Tasks;
using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Core {
    public class LicenseLocalDate : BaseLicense
    {
        private string m_EndTime = "2030-05-01 00:00:00";
        private string m_NowTime;

        public override void Init()
        {
            base.Init();
            m_NowTime = DateTool.GetNowStr();
          
        }
        public async override UniTask Checking()
        {
            try
            {
                string cfgName = KeyHelp.Instance.AesKey.Substring(0, 2) + KeyHelp.Instance.AesKey.Substring(20, 2)+ KeyHelp.Instance.AesKey.Substring(12, 2);
                LicenseDate licenseDate = await ConfigDataTool.ReadDataAsync<LicenseDate>(cfgName, null, true);
                m_EndTime = licenseDate.date;
            }
            catch (Exception)
            {
                LWDebug.Log("加载数据失败");
            }

            try
            {             
                int ret = DateTool.CompanyDate(m_NowTime, m_EndTime);
                IsLicensePass = ret < 0;
                //排除更改日期的方式
                string lastTime = PlayerPrefs.GetString("LicenseLastDate");
                if (lastTime != ""&& IsLicensePass)
                {
                    int ret2 = DateTool.CompanyDate(m_NowTime, lastTime);
                    IsLicensePass = ret2 > 0;
                }
                PlayerPrefs.SetString("LicenseLastDate", m_NowTime);
               
            }
            catch (Exception)
            {
                IsLicensePass = false;               
            }
            finally {
                await UniTask.DelayFrame(1);
            }
           
        
        }
    }
    public class LicenseDate {
        public string date { get; set; }
    }
}
