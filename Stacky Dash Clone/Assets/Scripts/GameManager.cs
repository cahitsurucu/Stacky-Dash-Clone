using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text score;
    [SerializeField] int Score,scoreCount;
    [Header("Money")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private int money;

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

    public void increaseMoney()
    {
        money += 1;
        moneyText.text = money.ToString();
    }
}
