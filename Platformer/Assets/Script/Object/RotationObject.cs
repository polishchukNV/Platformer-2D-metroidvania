using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    [SerializeField]private float speed;
    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, speed));
    }

}
