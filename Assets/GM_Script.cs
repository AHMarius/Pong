using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM_Script : MonoBehaviour
{
    public GameObject Racket_L;
    public GameObject Racket_R;
    public GameObject pw_obj;
    public int Score = 0;
    public int allowed_Balls = 3;
    public Ball_Behaviour BBh;
    public List<GameObject> ball_objects = new List<GameObject>();
    public List<Sprite> powerUp_sprites = new List<Sprite>();
    public Racket_Movement R_M;

    public bool game_state = true;
    public bool end_game = false;
    public int total_balls = 1;
    public int total_powerup = 0;
    private string[] powerUps =
    {
        "ExpandPU",
        "ShrinkPU",
        "MoreBallsPU",
        "LessBallsPU",
        "SpeedUpPU",
        "SpeedDownPU",
        "ChangeControlsPU",
        "UnsyncControlsPU"
    };
    private GameObject powerUpTop,
        powerUpBottom;
    public int current_x = 2;
    private int score_increment = 1;
    private bool change_control = false;
    private bool unsync_control = false;

    private void Update()
    {
        EndGame();
        HandleBallSpawn();
        HandleGameState();
        CheckScoreForPowerUpSpawn();
    }

    public void ScoreIncrease()
    {
        Score += score_increment;
    }

    private void HandleBallSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Space) && allowed_Balls > 0)
        {
            BBh.SpawnBall(0.5f, RandomPosition());
            allowed_Balls--;
            total_balls++;
        }
    }

    private void Reset()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void EndGame()
    {
        if (allowed_Balls == 0 && ball_objects.Count == 0)
            end_game = true;
        if (Input.GetKeyDown(KeyCode.R))
            Reset();
        if (end_game && Input.GetKeyDown(KeyCode.Escape))
            Reset();
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), 0);
    }

    private void HandleGameState()
    {
        if (game_state && Input.anyKey)
        {
            game_state = false;
            BBh.SpawnBall(0.1f);
        }
    }

    private void CheckScoreForPowerUpSpawn()
    {
        int targetScore = Mathf.FloorToInt(
            Mathf.Log10(Mathf.PI * Mathf.Pow(2, current_x)) * 5 * current_x
        );
        if (Score >= targetScore)
        {
            SpawnPowerUps();
            current_x++;
        }
    }

    private void SpawnPowerUps()
    {
        DestroyExistingPowerUps();
        SpawnIndividualPowerUpAtPosition(3f, out powerUpTop);
        SpawnIndividualPowerUpAtPosition(-3f, out powerUpBottom);
    }

    private void DestroyExistingPowerUps()
    {
        Destroy(powerUpTop);
        Destroy(powerUpBottom);
    }

    private void SpawnIndividualPowerUpAtPosition(float yPosition, out GameObject powerUp)
    {
        powerUp = Instantiate(
            pw_obj,
            new Vector3(UnityEngine.Random.Range(-3f, 3f), yPosition, 0),
            Quaternion.identity
        );
        AssignRandomSpriteAndNameToPowerUp(powerUp);
    }

    private void AssignRandomSpriteAndNameToPowerUp(GameObject powerUp)
    {
        int index1 = Random.Range(0, powerUp_sprites.Count);
        int index2;
        do
        {
            index2 = Random.Range(0, powerUp_sprites.Count);
        } while (index1 == index2);

        powerUp.GetComponent<SpriteRenderer>().sprite = powerUp_sprites[index1];
        powerUp.name = powerUps[index1];
    }

    public void TakeEffect(string powerUpName)
    {
        allowed_Balls++;
        total_powerup++;
        switch (powerUpName)
        {
            case "ExpandPU":
                ApplyExpandPowerUp();
                break;
            case "ShrinkPU":
                ApplyShrinkPowerUp();
                break;
            case "MoreBallsPU":
                AddBallsPowerUp();
                break;
            case "LessBallsPU":
                RemoveBallsPowerUp();
                break;
            case "SpeedUpPU":
                SpeedUpPowerUp();
                break;
            case "SpeedDownPU":
                SpeedDownPowerUp();
                break;
            case "ChangeControlsPU":
                ChangeControlsPowerUp();
                break;
            case "UnsyncControlsPU":
                UnsyncControlsPowerUp();
                break;
            default:
                break;
        }
        DestroyExistingPowerUps();
    }

    private void ApplyExpandPowerUp()
    {
        if (R_M.racket_speed_t > 2f)
        {
            AdjustRacketScale(0.4f);
            R_M.racket_speed_t -= 1f;
        }
    }

    private void ApplyShrinkPowerUp()
    {
        if (Racket_R.transform.localScale.y > 1f)
        {
            AdjustRacketScale(-0.4f);
            R_M.racket_speed_t += 1f;
        }
    }

    private void AdjustRacketScale(float scaleYAdjustment)
    {
        Racket_L.transform.localScale += new Vector3(0, scaleYAdjustment, 0);
        Racket_R.transform.localScale += new Vector3(0, scaleYAdjustment, 0);
    }

    private void AddBallsPowerUp()
    {
        allowed_Balls += 2;
    }

    private void RemoveBallsPowerUp()
    {
        allowed_Balls = Mathf.Max(allowed_Balls - 2, 0);
    }

    private void SpeedUpPowerUp()
    {
        foreach (var ball in ball_objects)
        {
            var ballScript = ball.GetComponent<Ball_Script>();
            var currentVelocity = ballScript.Ball_Rb.velocity;
            ballScript.Ball_Rb.velocity = new Vector2(
                currentVelocity.x / 1.5f,
                currentVelocity.y / 1.5f
            );
        }
        BBh.ball_speed += 1f;
    }

    private void SpeedDownPowerUp()
    {
        foreach (var ball in ball_objects)
        {
            var ballScript = ball.GetComponent<Ball_Script>();
            var currentVelocity = ballScript.Ball_Rb.velocity;
            ballScript.Ball_Rb.velocity = new Vector2(
                Mathf.Max(currentVelocity.x * 1.5f, 0),
                Mathf.Max(currentVelocity.y * 1.5f, 0)
            );
        }
        if (BBh.ball_speed >= 2f)
            BBh.ball_speed -= 1f;
    }

    private void ChangeControlsPowerUp()
    {
        R_M.movement_dir = -R_M.movement_dir;
        if (!change_control)
        {
            score_increment++;
            change_control = true;
        }
    }

    private void UnsyncControlsPowerUp()
    {
        R_M.movment_sync = -R_M.movment_sync;
        if (!unsync_control)
        {
            score_increment++;
            unsync_control = true;
        }
    }
}
