using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private float timeDisappearing;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<NewPlayer>())
        {
            StartCoroutine(DisappearingTime());
        }
    }

    IEnumerator DisappearingTime()
    {
        yield return new WaitForSeconds(timeDisappearing);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        yield return new WaitForSeconds(timeDisappearing * 3f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
