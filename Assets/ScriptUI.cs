using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScriptUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.GetChild(0).GetComponent<TMP_Text>().maxVisibleCharacters = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        { GameObject.Find("Door").GetComponent<BoxCollider2D>().isTrigger = !GameObject.Find("Door").GetComponent<BoxCollider2D>().isTrigger; }
    }
}
