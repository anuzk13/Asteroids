using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString("0000000");
    }
}
