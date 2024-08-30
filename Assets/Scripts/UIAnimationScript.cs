using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationScript : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(StartAnim());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator StartAnim()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("MoveUI");
        Debug.Log("Triggered");
    }
}
