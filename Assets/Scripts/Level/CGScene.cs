using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CGScene : MonoBehaviour
{
    public Animator animator;

    public UnityEvent AnimFinished;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.CheckIsState("Finish"))
        {
            AnimFinished?.Invoke();
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("BossScene");
    }

}
