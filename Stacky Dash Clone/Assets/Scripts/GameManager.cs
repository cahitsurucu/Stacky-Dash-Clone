using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text score;
    [SerializeField] int Score,scoreCount;
    [Header("Money")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private int money;
    [Header("Finish Canvas")]
    [SerializeField] private GameObject finish;

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

    public void restart()
    {
        SceneManager.LoadScene(0);
    }

    public void setActiveFinishCanvas(bool temp)
    {
        finish.SetActive(temp);
    }
}
