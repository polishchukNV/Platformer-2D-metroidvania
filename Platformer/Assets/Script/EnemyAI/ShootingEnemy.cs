using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    
    private Vector2 lookDirection;
    private float lookAngle;
    private bool active;
    
    private void Update()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0, 0, lookAngle);

        Bullet();
    }

    private void Bullet()
    {
        if (!active)
        {
            GameObject bulletClone = Instantiate(bullet);
            bulletClone.transform.position = firePoint.position;
            bulletClone.transform.rotation = Quaternion.Euler(0, 0, lookAngle);

            bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * bulletSpeed;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        active = true;
        yield return new WaitForSeconds(2f);
        active = false;
    }
}
