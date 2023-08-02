using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text score;
    [SerializeField] int Score;
    [SerializeField] int scoreCount;

    public void increaseScore()
    {
        Score += scoreCount;
        score.text = Score.ToString();
    }

    public void decreaseScore()
    {
        Score -= scoreCount;
        score.text = Score.ToString();
    }
}
