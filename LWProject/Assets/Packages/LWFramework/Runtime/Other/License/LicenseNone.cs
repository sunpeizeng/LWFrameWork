using Cysharp.Threading.Tasks;
using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Other {
    public class LicenseNone : BaseLicense
    {
        
        public override void Init()
        {
            base.Init();

        }
        public async override UniTask Checking()
        {
            IsLicensePass = true;
            await UniTask.WaitForEndOfFrame();
        }
    }

}
