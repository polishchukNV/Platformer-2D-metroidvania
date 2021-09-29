using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    
    public Vector2 SaveDot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveDot = transform.position;
            NewPlayer.Instance.saveDots = transform.position;
        }
    }

}
