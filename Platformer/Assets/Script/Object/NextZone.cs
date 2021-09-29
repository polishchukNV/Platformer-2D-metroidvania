using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextZone : MonoBehaviour
{
    [SerializeField] private string levelName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<NewPlayer>())
        {
            LevelLoader.SwitchToScene(levelName);
        }
        
    }
}
