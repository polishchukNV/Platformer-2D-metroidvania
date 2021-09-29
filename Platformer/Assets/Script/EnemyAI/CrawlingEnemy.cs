using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingEnemy : Enemy
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject[] wayPoint;
    private int nextWayPoint;
    private float distansToPoint;
    private bool state;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        distansToPoint = Vector2.Distance(transform.position, wayPoint[nextWayPoint].transform.position);
        if(!state)
        transform.position = Vector2.MoveTowards(transform.position, wayPoint[nextWayPoint].transform.position, moveSpeed * Time.deltaTime);

        if(distansToPoint < 0.2f)
        {
            TakeTurn();
        }
    }

    private void TakeTurn()
    {
        Vector3 objectRotate = transform.eulerAngles;
        objectRotate.z += wayPoint[nextWayPoint].transform.eulerAngles.z;
        transform.eulerAngles = objectRotate;
        ChoseNextWayPoint();
    }

    private void ChoseNextWayPoint()
    {
        nextWayPoint++;
        if(nextWayPoint == wayPoint.Length)
        {
            nextWayPoint = 0;
        }
    }

    IEnumerator States()
    {
        state = true;
        yield return new WaitForSeconds(1.2f);
        state = false;
    }

    public void GetHurt()
    {
        StartCoroutine(States());
    }

    public override void TakeDamege(int damege, Vector2 imapactPosition)
    {
        base.TakeDamege(damege, imapactPosition);
        if (GetComponent<Rigidbody2D>() == null)
            GetHurt();
    }

}
