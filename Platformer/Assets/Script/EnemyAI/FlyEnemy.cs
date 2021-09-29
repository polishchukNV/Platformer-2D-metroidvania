using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    public Enemy enemy;
    [SerializeField] private float speed, circleRadius;
    [SerializeField] private Vector3 rightCheck, roofCheck, groundCheck;
    [SerializeField] private LayerMask groundLayer, whatIsPlayer;
    [SerializeField] private bool facingRight = true, groundTouch, roofTouch, righttouch;
    [SerializeField] private float dirX = 1, dirY = 0.25f;
    [SerializeField] private float damegeImpulse;
    [SerializeField] private Vector3 direction, playerCheck;
    [SerializeField] private float playerLength;
    private Rigidbody2D rb;
    public bool isPlayer;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    { 
        isPlayer = (Physics2D.Raycast(transform.position + playerCheck, direction, playerLength, whatIsPlayer))&& (Physics2D.Raycast(transform.position - playerCheck, direction, playerLength, whatIsPlayer));
        rb.velocity = new Vector2(dirX, dirY) * speed * Time.deltaTime;
        HitDetection();

    }

    private void HitDetection()
    {
        righttouch = Physics2D.OverlapCircle(transform.position+rightCheck, circleRadius, groundLayer);
        roofTouch = Physics2D.OverlapCircle(transform.position + roofCheck, circleRadius, groundLayer);
        groundTouch = Physics2D.OverlapCircle(transform.position + groundCheck, circleRadius, groundLayer);
        HitLogic();
    }

    private void HitLogic()
    {
        if (righttouch && facingRight)
        {
            Flip();
        }
        else if (righttouch && !facingRight)
        {
            Flip();
        }

        if (roofTouch)
        {
            dirY = -0.25f;
        }
        else if (groundTouch)
        {
            dirY = 0.25f;
        }


    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        rightCheck = new Vector3(-rightCheck.x, rightCheck.y);
        roofCheck = new Vector3(-roofCheck.x, roofCheck.y);
        groundCheck = new Vector3(-groundCheck.x, groundCheck.y);
        dirX = -dirX;
        if (facingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + rightCheck, circleRadius);
        Gizmos.DrawWireSphere(transform.position + roofCheck, circleRadius);
        Gizmos.DrawWireSphere(transform.position + groundCheck, circleRadius);
        Gizmos.DrawLine(transform.position + playerCheck, transform.position + playerCheck + direction * playerLength);
        Gizmos.DrawLine(transform.position - playerCheck, transform.position - playerCheck + direction * playerLength);
    }

    public void UpdateValue(int health)
    {
        if (isPlayer) rb.AddForce(Mathf.Abs(dirX) / dirX * -damegeImpulse * Vector2.right, ForceMode2D.Impulse);
    }
}
