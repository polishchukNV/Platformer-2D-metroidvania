using System.Collections;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float time;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 11)
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            StartCoroutine(DeadSqueeze(1.5f, 0.1f, 0.07f));
        }
        
    }

    IEnumerator DeadSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = transform.localScale;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 2)
        {
            t += Time.deltaTime / seconds;
            gameObject.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        
        Destroy(gameObject);
    }
}
