using System.Collections;
using UnityEngine;
using System;

public class NewPlayer : MonoBehaviour
{
    [Header("Life and Energi")]
    [Range(0,10)] public int health, maxHealth;
    [Range(0,1)]public float energy,MaxEnergy;
    public event Action<int> OnHealthChange;
    public bool state;

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float dahsForce;
    private float dahsTimer = 0.6f;
    private Vector2 direction;
    private bool facingRight = true;
    private float dahsTime;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float sideSpeed;
    private float jumpDelay = 0.25f;
    private float jumpTimer;
    public float wallJumpForce;
    public float BaseSpeed;

    [Header("Components")]
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rigibody;
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private RaycastHit2D raycastHit;

    [Header("Physics")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float linearDrag = 4f;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float fallMultiplier = 5f;

    [Header("Collision")]
    public Vector3 saveDots;
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private Vector3 colliderWall;
    [SerializeField] private float groundLength = 0.6f;
    private bool onGround;
    private bool onWall;
    private float collisionRadius = 0.2f;
    private float directionX;
    private float wallDirection;
    private float positionDashY;
    private bool wallJump;

    [Header("Atack")]
    [SerializeField] private float distance;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask WhatIsEnemy;
    private Vector3 attackPosition;

    [Header("Audio")]
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip stoneSound;

    [Header("Hold")]
    [SerializeField] private float distansHold;
    [SerializeField] private float maxSpeedHold;
    private bool hold;
    private RaycastHit2D hitHold;
    private float directionHold = 1;

    [Header("Times")]
    public GameObject fightSp;

    private static NewPlayer instance;
	public static NewPlayer Instance
	{
		get
		{
			if (instance == null) instance = GameObject.FindObjectOfType<NewPlayer>();
			return instance;
		}
	}

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        saveDots = transform.position;
    }

    private void Update()
    {
        bool wasOnGround = onGround;
        onGround = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer))&&!state;
        onWall = Physics2D.OverlapCircle(transform.position + colliderWall, collisionRadius, groundLayer)||
            Physics2D.OverlapCircle(transform.position - colliderWall, collisionRadius, groundLayer);
       
        if (!wasOnGround && onGround)
        {
            //анімація преземлення 
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            positionDashY = transform.position.y;
            DashTimer();
        }

        if (dahsTime >= 0)
        {
            Dash();
            dahsTime -= Time.deltaTime;
        }

        animator.SetBool("onGround", onGround);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (rigibody.velocity.x > 0.01f)
            wallDirection = 1;
        else if (rigibody.velocity.x < -0.01f)
            wallDirection = -1;


        if (direction.x != 0)
            directionX  = direction.x;

        fightSp.transform.position = transform.position+attackPosition;
    }

    private void FixedUpdate()
    {
        moveCharacter(direction.x);
        
        if (onWall && !onGround && Input.GetButtonDown("Jump"))
        {
            WallJump();
        }
        else if (onWall && !onGround)
        {
            WallSlide(); 
        }

        if (jumpTimer > Time.time && onGround)
        {
            Jump();
            PlaySoundSFX(jumpSound, 0.5f, 1f);
        }

        Hold();
        modifyPhysics();
        Atack(1);
    }

    private void moveCharacter(float horizontal)
    {
       if(!state) rigibody.AddForce(Vector2.right * horizontal * moveSpeed);

        if (((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))&&!hold)
        {
            Flip();
        }

        if (Mathf.Abs(rigibody.velocity.x) > maxSpeed)
        {
            rigibody.velocity = new Vector2(Mathf.Sign(rigibody.velocity.x) * maxSpeed, rigibody.velocity.y);
        }

        if (Math.Abs(rigibody.velocity.x) > 1 && onGround && !audioSource.isPlaying)
            PlaySoundSFX(stepSound);

                animator.SetFloat("horizontal", Mathf.Abs(rigibody.velocity.x));  //створти і підключити анімації  
        animator.SetFloat("vertical", rigibody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            StartCoroutine(Death(3f));
            Damege(1);
        }

        if (collision.gameObject.layer == 10)
        {
            Damege(1);
        }
    }

    private void Jump()
    { 
        rigibody.velocity = new Vector2(rigibody.velocity.x, 0);
        rigibody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        hold = false;
    }

    private void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rigibody.velocity.x < 0) || (direction.x < 0 && rigibody.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rigibody.drag = linearDrag;
            }
            else
            {
                rigibody.drag = 0f;
            }
            rigibody.gravityScale = 0;
        }
        else
        {
            rigibody.gravityScale = gravity;
            rigibody.drag = linearDrag * 0.15f;
            if (rigibody.velocity.y < 0)
            {
                rigibody.gravityScale = gravity * fallMultiplier/1.6f;
            }
            else if (rigibody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rigibody.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        if (directionX > 0) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }

    private void Atack(int DamegeEn)
    {
        if (Input.GetButtonDown("Fire1"))
        {

            if (direction.y == 0)
            {
                attackPosition = new Vector3(distance * directionX, 0);
            }
            else
            {
                attackPosition = new Vector3(0, distance * direction.y);
            }

           Collider2D[] enemiesToDamege = Physics2D.OverlapCircleAll(transform.position+attackPosition,attackRange,WhatIsEnemy);
           foreach(Collider2D enemy in enemiesToDamege)
           {
                if (enemy.GetComponent<Enemy>() != null)
                {
                    enemy.GetComponent<Enemy>().TakeDamege(DamegeEn,attackPosition);
                } 
           }
            PlaySoundSFX(attackSound,0.7f,1f);
        }
    }

    //-------------------------------Перенесення Об'єктів-----------------------//
    private void Hold()
    {
        if (Input.GetKeyDown(KeyCode.F) )
        {
            if (!hold)
            {
                Physics2D.queriesStartInColliders = false;
                hitHold = Physics2D.Raycast(transform.position, Vector2.right * directionX, distansHold);
                directionHold = directionX;
            }

            if (hitHold.collider != null && hitHold.transform.tag == "Dinamic")//змінити tag
            {
                hold = true;
            }           
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            hold = false;
        }

        if (hold)
        {
            hitHold.collider.gameObject.transform.position = new Vector3(transform.position.x + (hitHold.transform.lossyScale.x/2f* directionHold) , hitHold.collider.transform.position.y) + distansHold * Vector3.right * directionHold;
            maxSpeed = maxSpeedHold;
        }
        else 
        {
           maxSpeed = 7;
        }
    }

    
    private void Dash()
    {
        if (dahsTime > 0 && !state && !onWall)
        {
            rigibody.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x + dahsForce * directionX, positionDashY);
        }
    }

    private void DashTimer()
    {
        if(dahsTime <= 0)
        {
            dahsTime = dahsTimer;
        }
        else if(dahsTime > 0)
        {
            dahsTime -= Time.deltaTime;
        }
    }

    private void Damege(int damege)
    {
        health -= damege;
        if(OnHealthChange != null)
        {
            OnHealthChange.Invoke(health);
        }
    }

    private void PlaySoundSFX(AudioClip audioClip)
    {
        audioSource.pitch = (UnityEngine.Random.Range(0.6f, 1f));
        audioSource.PlayOneShot(jumpSound);
    }

    private void PlaySoundSFX(AudioClip audioClip, float minPitch, float maxPitch)
    {
        audioSource.pitch = (UnityEngine.Random.Range(minPitch, maxPitch));
        audioSource.PlayOneShot(audioClip);
    }

    private void WallSlide()
    {
        if (direction.y != 0 && !onGround)
        {
            rigibody.velocity = new Vector2(rigibody.velocity.x, sideSpeed * direction.y * 10f);
        }
        else if(!onGround && !wallJump) 
        {
            rigibody.velocity = new Vector2(rigibody.velocity.x , -sideSpeed);
        }
    }

    private void WallJump()
    {
        wallJump = true;
        rigibody.velocity = Vector3.one;
        Vector2 forceToAdd = new Vector2(jumpSpeed * wallJumpForce * -wallDirection, jumpSpeed * wallJumpForce * 1.6f);
        rigibody.AddForce(forceToAdd, ForceMode2D.Impulse);
        StartCoroutine(WallJumps(0.7f));
       
    }

    private IEnumerator WallJumps(float times)
    {
        yield return new WaitForSeconds(times);
        wallJump = false;
    }

    private IEnumerator Death(float times)
    {
        state = true;
        yield return new WaitForSeconds(times);
        transform.position = saveDots;
        state = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawWireSphere(transform.position + colliderWall, collisionRadius);
        Gizmos.DrawWireSphere(transform.position - colliderWall, collisionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * distansHold);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * distansHold);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + attackPosition, attackRange);
    }

}


public struct SaveData
{
    
}