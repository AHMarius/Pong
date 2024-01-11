using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Game_Event_Script : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nextScoreText;
    public TextMeshProUGUI ballsText;
    public GameObject StartText;
    public GM_Script GM;
    public List<GameObject> gameObjects = new List<GameObject>();
    private int score;

    private void Update()
    {
        UpdateScoreDisplay();
        UpdateNextScoreDisplay();
        UpdateLeftBalls();
        CheckGameState();
    }

    private void UpdateScoreDisplay()
    {
        score = GM.Score;
        scoreText.text = score.ToString("000");
    }

    private void UpdateNextScoreDisplay()
    {
        nextScoreText.text =
            "Next power-up: "
            + Mathf
                .FloorToInt(Mathf.Log10(Mathf.PI * Mathf.Pow(2, GM.current_x)) * 5 * GM.current_x)
                .ToString("000");
    }

    private void UpdateLeftBalls()
    {
        string displayText;
        if (GM.allowed_Balls != 0)
        {
            displayText = new string('●', GM.allowed_Balls);
            ballsText.color = Color.white;
        }
        else
        {
            displayText = "●";
            ballsText.color = Color.red;
        }
        ballsText.text = displayText;
    }

    private void CheckGameState()
    {
        if (!GM.game_state)
            StartText.SetActive(false);
        if (GM.end_game)
        {
            StartText.SetActive(true);
            foreach (var gmObj in gameObjects)
            {
                gmObj.SetActive(false);
            }
            StartText.GetComponent<TextMeshProUGUI>().text =
                "Joc terminat! \n"
                + "Mingi folosite: "
                + GM.total_balls.ToString("000")
                + "\n"
                + "Upgrade-uri: "
                + GM.total_powerup.ToString("000")
                + "\n"
                + "Scor final: "
                + score.ToString("000")
                + "\n"
                + "Reset - [ESC]";
        }
    }
}
