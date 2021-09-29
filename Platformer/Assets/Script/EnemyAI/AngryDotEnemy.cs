using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryDotEnemy : Enemy
{
    [SerializeField] private float speed;
    [SerializeField] private float groundLength;
    [SerializeField] private float playerLength;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Vector3 groundCheck;
    [SerializeField] private Vector3 wallCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private bool facingRight, isPlayer;
    [SerializeField] private float damegeImpulse;
    private Vector3 direction = new Vector3(1,0);
    private bool isGrounded;

    private Rigidbody2D rigibody;

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = (Physics2D.Raycast(transform.position + groundCheck, Vector2.down, groundLength, whatIsGround)) && !(Physics2D.Raycast(transform.position + wallCheck, Vector2.right, groundLength, whatIsGround));
        isPlayer = (Physics2D.OverlapCircle(playerCheck.position, playerLength, whatIsPlayer));
        Moving();
    }

    private void Moving()
    {
        rigibody.AddForce(Vector2.right * speed);
        if (!isGrounded)
        {
            Flip();
        }
        if (Mathf.Abs(rigibody.velocity.x) > maxSpeed)
        {
            rigibody.velocity = new Vector2(Mathf.Sign(rigibody.velocity.x) * maxSpeed, rigibody.velocity.y);
        }

        if (isPlayer)
        {
            maxSpeed = 10;
            speed = 8 * direction.x;
        }
        else
        {
            maxSpeed = 5;
            speed = 4.5f * direction.x;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        groundCheck = new Vector3(-groundCheck.x, 0);
        wallCheck = new Vector3(-wallCheck.x, 0);

        direction.x = -direction.x;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + groundCheck, transform.position + groundCheck + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + wallCheck, transform.position + wallCheck + direction * groundLength);
        Gizmos.DrawWireSphere(playerCheck.position, playerLength);
    }


    public void GetHurt(int launchDirection, int hitPower)
    {
        if (isPlayer)
        {
            rigibody.velocity = Vector2.zero;
            rigibody.AddForce(Mathf.Abs(speed) / speed * -damegeImpulse * Vector2.right, ForceMode2D.Impulse);
        }
    }
}
