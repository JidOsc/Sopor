using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCharacter : MonoBehaviour
{
    public int speed = 1;
    public new Rigidbody2D rigidbody;

    private bool doorOut = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        { rigidbody.velocity = new Vector2(speed, 0); }

        else if (Input.GetKey(KeyCode.A))
        { rigidbody.velocity = new Vector2(-speed, 0); }

        else
        { rigidbody.velocity = new Vector2(0, 0); }


        if (Input.GetKey(KeyCode.Space) && doorOut)
        {
            GameObject.Find("doorOut").GetComponent<BoxCollider2D>().isTrigger = !GameObject.Find("doorOut").GetComponent<BoxCollider2D>().isTrigger;
        }
    }
    // Update is called once per frame
    void Update()
    {
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //if(collision.transform.name == "door")
    // {rigidbody.velocity *= -1;}
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.name == "doorOut")
        {
            GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = true;
            doorOut = true;
            print("true");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.name == "doorOut")
        {
            GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = false;
            doorOut = false;
            print("false");
        }
    }
}
