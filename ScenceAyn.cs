using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;

static public  class ScenceAyn 
{
    static public void LoadSceneAsyn(string scenceName,Action call)
    {
        AsyncOperation opration = SceneManager.LoadSceneAsync(scenceName);
        opration.completed += (action) =>
        {
            call?.Invoke(); //true的时候调用
        };
    }

}
