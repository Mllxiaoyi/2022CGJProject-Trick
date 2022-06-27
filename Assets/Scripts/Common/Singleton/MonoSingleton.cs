using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MonoSingleton<T>:MonoBehaviour where T: MonoSingleton<T>
{
    private static T instance;

    public static T Instance { get { return instance; } }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = (T)this;
    }

    /// <summary>
    /// 得到单例是否存在的bool值
    /// </summary>
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    /// <summary>
    /// 用来销毁单例
    /// </summary>
    protected void DestroyInstance()
    {
        if (instance == this)
        {
            Destroy(this);
        }
    }
}
