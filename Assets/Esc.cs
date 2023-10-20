using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Esc : MonoBehaviour
{
    public int SceneNumber;
    public void EscButton(int SceneNumber)
    {
        SceneManager.LoadScene(SceneNumber);
    }
    public void EscKey(int SceneNumber)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneNumber);
        }
    }
}
