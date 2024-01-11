using System;
using UnityEngine;

public class Ball_Script : MonoBehaviour
{
    public GM_Script G_Manager;
    public Rigidbody2D Ball_Rb;
    public Vector2 const_vel;
    public float ball_knockback = 0.1f;
    public Transform Rackets;

    private void Start()
    {
        InitializeComponents();
    }

    private void Update()
    {
        HandleBoundaryCollision();
        HandleRacketCollision();
        UpdateBallVelocity();
    }

    private void InitializeComponents()
    {
        Ball_Rb = GetComponent<Rigidbody2D>();
        const_vel = Ball_Rb.velocity;
    }

    private void HandleBoundaryCollision()
    {
        float boundaryY = 4.5f;
        float knockbackY = Math.Sign(transform.position.y) * ball_knockback;

        if (transform.position.y >= boundaryY || transform.position.y <= -boundaryY)
        {
            AdjustVelocityOnBoundaryCollision(knockbackY);
            ClampBallPosition();
        }
    }

    private void AdjustVelocityOnBoundaryCollision(float knockbackY)
    {
        const_vel.x += Math.Sign(transform.position.x) * ball_knockback;
        const_vel.y = -const_vel.y + knockbackY;
    }

    private void ClampBallPosition()
    {
        float clampedY = (transform.position.y >= 4.5f) ? 4.46f : -4.46f;
        transform.position = new Vector3(transform.position.x, clampedY, 0);
    }

    private void HandleRacketCollision()
    {
        float racketThreshold = 0.02f;

        if (IsOutsideRacketBounds(racketThreshold))
        {
            RemoveBall();
        }
    }

    private bool IsOutsideRacketBounds(float threshold)
    {
        return transform.position.x > -Rackets.position.x + threshold
            || transform.position.x < Rackets.position.x - threshold;
    }

    private void RemoveBall()
    {
        G_Manager.ball_objects.Remove(gameObject);
        Destroy(gameObject, 0.2f);
        enabled = false;
    }

    private void UpdateBallVelocity()
    {
        Ball_Rb.velocity = const_vel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Racket"))
        {
            HandleRacketHit();
        }
        else if (collision.gameObject.CompareTag("PowerUps"))
        {
            G_Manager.TakeEffect(collision.gameObject.name);
        }
    }

    private void HandleRacketHit()
    {
        G_Manager.ScoreIncrease();
        float knockbackX = Math.Sign(const_vel.x) * ball_knockback;

        const_vel.x = -const_vel.x + knockbackX;
        transform.position += new Vector3(-knockbackX, 0, 0);
    }
}
