using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCharacter : MonoBehaviour
{
    public int speed = 1;
    public new Rigidbody2D rigidbody;

    private bool cooldown = false;
    private bool carryingtrash = false;
    private bool opentrash = false;
    private bool colliding = false;

    private GameObject detectedobject = null;
    

 private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        { speed = 6; }

        else { speed = 3; }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        { rigidbody.velocity = new Vector2(speed, 0); }

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { rigidbody.velocity = new Vector2(-speed, 0); }

        else
        { rigidbody.velocity = new Vector2(0, 0); }


        if(Input.GetKey(KeyCode.Space) && cooldown == false && colliding)
        {
            if (detectedobject.transform.name.StartsWith("door"))
            { detectedobject.GetComponent<BoxCollider2D>().enabled = !detectedobject.GetComponent<BoxCollider2D>().enabled;

                if (detectedobject.GetComponent<SpriteRenderer>().color == Color.white)
                { detectedobject.GetComponent<SpriteRenderer>().color = Color.red; }
                else
                { detectedobject.GetComponent<SpriteRenderer>().color = Color.white; }

                cooldown = true;
                Invoke("Cooldown", 0.4f); }

            else if (detectedobject.transform.name.StartsWith("trash") && carryingtrash == false)
            {
                carryingtrash = true;
                Destroy(detectedobject.gameObject);
                cooldown = true;
                Invoke("Cooldown", 0.2f);
            }

            else if (detectedobject.transform.name == "soptunna")
            {
                if (opentrash && carryingtrash)
                {
                    carryingtrash = false;
                    print("slängde sopor");
                }
                else
                {
                    opentrash = !opentrash;
                    if (detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.red)
                    { detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white; }

                    else
                    { detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red; }

                    print("öppnade luckan");

                    cooldown = true;
                    Invoke("Cooldown", 0.2f);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Main Camera").transform.position = new Vector3((transform.position.x + GameObject.Find("Main Camera").transform.position.x) / 2, 0, -10);
    }

    private void Cooldown()
    { cooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedobject = collision.gameObject;
        colliding = true;

        GameObject.Find("popupSpace").transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + 1.5f, -1);
        GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
        detectedobject = null;
        GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = false;}
}
