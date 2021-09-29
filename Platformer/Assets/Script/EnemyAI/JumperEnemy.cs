using UnityEngine;

public class JumperEnemy : Enemy
{
    [Header("Patrolling")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float cirecleRadius;
    [SerializeField] private float playerLength;
    private float moveDirection = 1;
    private bool facingRight;
    private bool checkGround, checkWall;
    private bool isPlayer;
 
    [Header("Jump Attack")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize;
    private bool isGround;

    [Header("Seeing Player")]
    [SerializeField] private float lineOfSite;
    [SerializeField] private LayerMask playerLayer;
    private bool canSeePlayer;

    [SerializeField] private float damegeImpulse;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        checkGround = Physics2D.OverlapCircle(groundCheckPoint.position, cirecleRadius, groundLayer);
        checkWall = Physics2D.OverlapCircle(wallCheckPoint.position, cirecleRadius, groundLayer);
        isGround = Physics2D.OverlapCircle(groundCheck.position, cirecleRadius, groundLayer);
        canSeePlayer = Physics2D.OverlapCircle(transform.position, lineOfSite, playerLayer);
        isPlayer = (Physics2D.OverlapCircle(playerCheck.position, playerLength, playerLayer));
        if (!canSeePlayer && isGround)
        Patrolling();
        else if((canSeePlayer))
        JumpAttack();
        
    }

   
    private void Patrolling()
    {
        rigidbody.velocity = new Vector3(moveSpeed * moveDirection, rigidbody.velocity.y);
        Flip();
    }

    private void Flip()
    {
        if ((!checkGround || checkWall) && (facingRight || !facingRight))
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
      
    }

    private void JumpAttack()
    {
        float distansFromPlayer = NewPlayer.Instance.transform.position.x - transform.position.x;

        if (isGround)
        {
            rigidbody.AddForce(new Vector2(distansFromPlayer, jumpHeight), ForceMode2D.Impulse);
        }

    }

    public void GetHurt()
    {
        if (isPlayer)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(moveDirection * -damegeImpulse * Vector2.right, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,lineOfSite);
    }

}
