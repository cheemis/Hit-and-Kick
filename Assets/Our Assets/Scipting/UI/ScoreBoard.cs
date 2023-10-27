using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public int currentScore = -1;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        currentScore = -1;
        UpdateScore();
    }

    public void UpdateScore()
    {
        currentScore++;
        scoreText.text = "Score: <color=green> " + currentScore + "</color>";

    }
}
