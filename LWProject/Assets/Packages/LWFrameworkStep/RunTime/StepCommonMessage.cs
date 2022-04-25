using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 步骤中的常用消息
/// </summary>
public class StepCommonMessage
{  
    /// <summary>
    /// 发送章节
     /// </summary>
    public static byte StepChapter { get; }
    /// <summary>
    /// 发送章节状态
    /// </summary>
    public static byte StepChapterState { get; }
    /// <summary>
    /// 发送帮助消息文字
    /// </summary>
    public static byte StepHelp { get; }
}


/// <summary>
/// 步骤中数据获取
/// </summary>
public class StepCommonMessageKey
{
    /// <summary>
    /// 发送章节
    /// </summary>
    public static byte ChapterKey { get; }
    /// <summary>
    /// 发送章节状态
    /// </summary>
    public static byte ChapterStateKey { get; }
    /// <summary>
    /// 发送章节编号 1 -1
    /// </summary>
    public static byte ChapterIndexOffsetKey { get; }
    /// <summary>
    /// 发送帮助消息文字
    /// </summary>
    public static byte StepHelpKey { get; }
}