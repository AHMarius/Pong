using System.Collections;
using UnityEngine;

public class Ball_Behaviour : MonoBehaviour
{
    public GameObject BallObj;
    public GameObject BallParent;
    public GM_Script G_Manager;
    public Transform Racket;
    public float ball_speed = 3f;

    public void SpawnBall(float timeInterval, Vector3? position = null)
    {
        Vector3 spawnPosition = position ?? Vector3.zero;

        GameObject ballInstance = Instantiate(
            BallObj,
            spawnPosition,
            Quaternion.identity,
            BallParent.transform
        );

        Ball_Script ballScript = ballInstance.GetComponent<Ball_Script>();
        ballScript.G_Manager = G_Manager;
        ballScript.Rackets = Racket;

        G_Manager.ball_objects.Add(ballInstance);
        StartCoroutine(SetBallVelocity(ballInstance, timeInterval));
    }

    private IEnumerator SetBallVelocity(GameObject ball, float timeInterval)
    {
        yield return new WaitForSeconds(timeInterval);

        float randomX = UnityEngine.Random.Range(-100f, 100f);
        float randomY = UnityEngine.Random.Range(-100f, 100f);

        Vector2 velocity = new Vector2(
            Mathf.Sign(randomX) * ball_speed * UnityEngine.Random.Range(1.5f, 2f),
            Mathf.Sign(randomY) * ball_speed * UnityEngine.Random.Range(1.5f, 2f)
        );

        ball.GetComponent<Ball_Script>().const_vel = velocity;
    }
}
