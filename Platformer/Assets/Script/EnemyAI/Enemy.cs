using System.Collections;
using UnityEngine;


[System.Serializable]
public abstract class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbod;

    [SerializeField] private bool itFlies;
    [SerializeField] private Material materialGlare;
    [SerializeField] private float forceImpact;
    public int health;
    public GameObject deathParticles;
    public ParticleSystem shockParticles; 
    private Material materialDefault;


    private void Start()
    {
        rigidbod = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialDefault = spriteRenderer.material;
    }

    public virtual void TakeDamege(int damege, Vector2 imapactPosition)
    {
        health -= damege;
        Die();
        shockParticles.Emit(Random.Range(8, 15));
        StartCoroutine(GlareEnemy());

        if (rigidbod != null)
        {
            rigidbod.velocity = Vector3.zero;
            rigidbod.AddForce(imapactPosition * forceImpact, ForceMode2D.Impulse);
        }
    }

    public void Die()
    {
        if (health <= 0)
        {
            deathParticles.SetActive(true);
            deathParticles.transform.parent = transform.parent;
            Destroy(gameObject);
        }
    }


    private IEnumerator GlareEnemy()
    {
        spriteRenderer.material = materialGlare;
        yield return new WaitForSeconds(.3f);
        spriteRenderer.material = materialDefault;
    }
}
