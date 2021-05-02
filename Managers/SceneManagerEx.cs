using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public void LoadScene(Defines.SceneType sceneType)
    {
        CurrentScene.Clear();
        string _sceneName = System.Enum.GetName(typeof(Defines.SceneType), sceneType);

        if (!_sceneName.Contains("Scene"))
            _sceneName = $"{_sceneName}Scene";

        SceneManager.LoadScene(_sceneName);
    }


}