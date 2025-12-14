using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalscoreText;
    public TextMeshProUGUI PlayerWonText;
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
        finalscoreText.text = "FinalScore -"+score;
    }
    public void updatePlayerWon(string name)
    {
        PlayerWonText.text = name + "Won";
    }
}