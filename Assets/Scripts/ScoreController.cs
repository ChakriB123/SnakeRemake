using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int score;
    public int increaseScoreBy = 1;
    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        refreshUI(); 
    }
    public void incrementScore(int multiplier)
    {
        score += increaseScoreBy * multiplier;
        refreshUI();
    }
    public void refreshUI()
    {
        scoreText.text = ""+score;
    }

}