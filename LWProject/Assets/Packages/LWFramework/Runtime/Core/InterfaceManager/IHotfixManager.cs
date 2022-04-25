using System.Collections;
using System.Reflection;

namespace LWFramework.Core
{
    public enum HotfixCodeRunMode
    {
#if ILRUNTIME
        ByILRuntime = 0,
#endif
        ByReflection = 1,
        ByCode = 2,
    }
    public interface IHotfixManager
    {
        /// <summary>
        /// 使用协程加载脚本
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        IEnumerator IE_LoadScript(HotfixCodeRunMode mode);
        /// <summary>
        /// 使用异步加载脚本
        /// </summary>
        /// <param name="mode"></param>
        Cysharp.Threading.Tasks.UniTaskVoid LoadScriptAsync(HotfixCodeRunMode mode);
    }
}