using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCharacter : MonoBehaviour
{
    public int speed = 1;
    public new Rigidbody2D rigidbody;

    private bool cooldown = false;

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
    }
    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Main Camera").transform.position = new Vector3((transform.position.x + GameObject.Find("Main Camera").transform.position.x) / 2, 0, -10);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name.StartsWith("door") && Input.GetKey(KeyCode.Space) && cooldown == false)
        {
            collision.GetComponent<BoxCollider2D>().enabled = !collision.GetComponent<BoxCollider2D>().enabled;
            cooldown = true;
            Invoke("Cooldown", 0.2f);
        }
    }

    private void Cooldown()
    { cooldown = false; }

    private void OnTriggerExit2D(Collider2D collision)
    {GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = false;}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("popupSpace").transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + 1.5f, -1);
        GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = true;
    }
}
