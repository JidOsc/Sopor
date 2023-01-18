using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCharacter : MonoBehaviour
{
    public int speed = 1;
    public new Rigidbody2D rigidbody;

    int RandomNumber()
    {
        return Random.Range(0, 10);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {rigidbody.velocity = new Vector2(speed, 0);}

        else if (Input.GetKey(KeyCode.A))
        {rigidbody.velocity = new Vector2(-speed, 0);}

        else
        {rigidbody.velocity = new Vector2(0, 0);}

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
        //if(collision.transform.name == "door")
       // {rigidbody.velocity *= -1;}
    //}


}
