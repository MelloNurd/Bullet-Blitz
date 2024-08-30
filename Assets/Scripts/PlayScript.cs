using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayScript : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = text.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPointerEnter(PointerEventData eventData)
    {
        text.color = new Color(169, 169, 169, 255);
    }

    void OnPointerExit(PointerEventData eventData)
    {
        text.color = new Color(67, 67, 67, 255);
    }
}
