using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteScript : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sr;
    float inputX, inputY;
    float moveX, moveY;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        //movement needs to !=0 but if x or y is neg then x+y=0 so this is my fix, the real x and y are used for flipping
        moveX = inputX;
        moveY = inputY;
        if (moveX < 0)
        {
            moveX = inputX * -1;
        }
        if (moveY < 0)
        {
            moveY = inputY * -1;
        }
        animator.SetInteger("Movement", (int)(moveX + moveY));
        // Flips player sprite based on input, could be condensed but might leave for clarity
        if(inputX < 0 && !sr.flipX)
        {
            sr.flipX = inputX < 0 && !sr.flipX;
        }
        else if(inputX > 0 && sr.flipX)
        {
            sr.flipX = false;
        }
    }
}
