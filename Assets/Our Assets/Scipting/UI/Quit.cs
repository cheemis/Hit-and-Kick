using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    public void OnExitGame()
    {
       #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
       #else
        Application.Quit();
       #endif
    }
}
