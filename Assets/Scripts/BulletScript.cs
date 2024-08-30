using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 60f || transform.position.y < -60f || transform.position.x > 60f || transform.position.x < -60f)
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
