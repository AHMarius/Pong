using UnityEngine;
using TMPro;

public class Game_Event_Script : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nextScoreText;
    public TextMeshProUGUI ballsText;
    public GameObject StartText;
    public GM_Script GM;
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
    }
}
