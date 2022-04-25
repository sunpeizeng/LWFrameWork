using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class UniTaskExtend
{
    /// <summary>
    /// 扩展的UniTask
    /// </summary>
    /// <param name="secondsDelay">秒</param>
    /// <param name="ignoreTimeScale"></param>
    /// <param name="delayTiming"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static UniTask Delay(float secondsDelay, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken))
    {
        
        int millisecondsDelay = (int)(secondsDelay*1000);
        return UniTask.Delay(millisecondsDelay, ignoreTimeScale, delayTiming, cancellationToken);
    }
}
