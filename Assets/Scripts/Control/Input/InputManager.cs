using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InputManager : MonoSingleton<InputManager>
{
    public Queue<KeyCode> cachedKeys=new Queue<KeyCode>();
    public float cachedTime=0.2f;

    private void Update()
    {
        CacheInputKey(KeyCode.A);
        CacheInputKey(KeyCode.D);
        CacheInputKey(KeyCode.Mouse0);
        CacheInputKey(KeyCode.Mouse1);
    }


    public bool ReadCachedKey(KeyCode key)
    {
        if (cachedKeys.Count < 1) { return false; }
        return cachedKeys.Peek() == key;
    }

    private void CacheInputKey(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //Debug.Log(key+"¼ü±»´¢´æ,0.2sºóÉ¾³ý");
            cachedKeys.Enqueue(key);
            StartCoroutine("DequeueKey");
        }        
    }

    private IEnumerator DequeueKey()
    {
        yield return new WaitForSeconds(cachedTime);
        cachedKeys.Dequeue();
    }
}

