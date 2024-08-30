using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class BasicEnemyScript : MonoBehaviour
{
    Animator animator;
    EnemyBehaviour script;
    SpriteRenderer sr;
    EnemyBehaviour enemyScript;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        script = transform.parent.GetComponentInParent<EnemyBehaviour>();
        sr = GetComponent<SpriteRenderer>();
        enemyScript = GetComponentInParent<EnemyBehaviour>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(script.isMoving == true)
        {
            animator.SetInteger("Movement", 2);
        }
        if(script.isMoving == false) 
        {
            animator.SetInteger("Movement", 0);
        }

        if ((transform.parent.transform.position.x - player.transform.position.x) < 0 && !sr.flipX)
        {
            sr.flipX = true;
        }
        else if ((transform.parent.transform.position.x - player.transform.position.x) > 0 && sr.flipX)
        {
            sr.flipX = false;
        }
    }
}
