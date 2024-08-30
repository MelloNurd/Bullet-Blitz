using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class EnemyBehaviour : MonoBehaviour
{

    [SerializeField] float speed, moveTowardsUpperRange, moveTowardsLowerRange, shootRange, moveAwayRange, maximumPatrolDistance, waitTime, bulletSize;
    [SerializeField] int bulletCount;
    [SerializeField] Color bulletColor;
    Vector3 patrolPoint;
    bool waitToMove;
    Vector3 moveOffset;

    Transform target;

    BulletPoolScript bulletPool;
    [SerializeField] float timeBetweenShots, bulletSpeed;
    float nextShotTime;

    public bool isMoving;

    bool canShoot;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
        bulletPool = GameObject.Find("Game Manager").GetComponent<BulletPoolScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        waitToMove = false;
        isMoving = false;

        SetNewMoveOffset();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement code
        if(Vector2.Distance(transform.position, target.position) < moveAwayRange)
        {
            // MOVE AWAY CODE
            isMoving = true;
            transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * 1.75f * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, target.position) < moveTowardsUpperRange && Vector2.Distance(transform.position, target.position) > moveTowardsLowerRange)
        {
            // MOVE TOWARDS CODE    
            isMoving = true;                                // move offset helps with them getting stuck on top of each other
            transform.position = Vector2.MoveTowards(transform.position, target.position + moveOffset, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, target.position) > moveTowardsLowerRange)
        {
            // PATROL CODE
            if (transform.position != patrolPoint)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoint, speed * Time.deltaTime);
            }
            else
            {
                if (!waitToMove)
                {
                    StartCoroutine(SetNewPatrolPoint());
                }
            }
        }
        else
        {
            isMoving = false;
        }

        // Shooting code
        // // Outside the other ifs so it is able to shoot whilst moving
        if (Vector2.Distance(transform.position, target.position) < shootRange)
        {
            // ATTACK CODE
            if (Time.time > nextShotTime && canShoot)
            {
                // SHOOT BULLET
                Shoot(bulletCount);
                nextShotTime = Time.time + timeBetweenShots;
            }
        }
    }

    void Shoot(int shotCount)
    {
        if (shotCount > 12) shotCount = 12;
        if(shotCount < 1) shotCount = 1;

        for (int i = 1; i <= shotCount; i++)
        {
            GameObject bullet = bulletPool.GetPooledObject();
            if (bullet == null) bullet = bulletPool.CreateNewObject();

            bullet.transform.localScale = Vector3.one * bulletSize;
            bullet.GetComponent<SpriteRenderer>().color = bulletColor;
            bullet.GetComponentsInChildren<Light2D>()[0].color = bulletColor;
            bullet.GetComponentsInChildren<Light2D>()[1].color = bulletColor;
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            Vector3 shootDirection = (target.transform.position - transform.position).normalized;
            int spread = ((30 * shotCount) - (30 * i)) - 15*(shotCount-1);
            Quaternion spreadAngle = Quaternion.AngleAxis(spread, new Vector3(0, 0, 1));
            Vector3 spreadDirection = spreadAngle * shootDirection;
            bullet.GetComponent<Rigidbody2D>().AddForce(spreadDirection * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    IEnumerator SetNewPatrolPoint()
    {
        waitToMove = true;
        isMoving = false;
        yield return new WaitForSeconds(waitTime);
        // do {
        float randomX = UnityEngine.Random.Range(-maximumPatrolDistance, maximumPatrolDistance);
        float randomY = UnityEngine.Random.Range(-maximumPatrolDistance, maximumPatrolDistance);
        patrolPoint = transform.position + new Vector3(randomX, randomY, 0);
        // while loop
        waitToMove = false;
        isMoving = true;
    }

    public void SetNewMoveOffset()
    {
        moveOffset = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), 0);
    }

    public IEnumerator EnableShooting()
    {
        canShoot = false;
        yield return new WaitForSeconds(3f);
        canShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, moveAwayRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveTowardsUpperRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, moveTowardsLowerRange);
    }
}
