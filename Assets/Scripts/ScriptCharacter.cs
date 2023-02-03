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
    private bool hidden = false;

    private short trashthrown = 0;

    private GameObject detectedobject = null;
    

 private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        { speed = 6; }

        else { speed = 3; }

        if (!hidden)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            { rigidbody.velocity = new Vector2(speed, 0); }

            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            { rigidbody.velocity = new Vector2(-speed, 0); }

            else
            { rigidbody.velocity = new Vector2(0, 0); }
        }

        if(Input.GetKey(KeyCode.Space) && cooldown == false && colliding)
        {
            if (detectedobject.transform.name.StartsWith("Door"))
            { detectedobject.GetComponent<BoxCollider2D>().enabled = !detectedobject.GetComponent<BoxCollider2D>().enabled;

                //debug-kod, ska tas bort
                    if (detectedobject.GetComponent<SpriteRenderer>().color == Color.white)
                    { detectedobject.GetComponent<SpriteRenderer>().color = Color.red; }
                    else
                    { detectedobject.GetComponent<SpriteRenderer>().color = Color.white; }

                StartCooldown(0.4f);
            }

            else if (detectedobject.transform.name.StartsWith("trash") && carryingtrash == false)
            {
                carryingtrash = true;
                Destroy(detectedobject.gameObject);

                StartCooldown(0.2f);
            }

            else if (detectedobject.transform.name == "Trashcan")
            {
                if (opentrash && carryingtrash)
                {
                    carryingtrash = false;
                    print("sl�ngde sopor");

                    trashthrown += 1;

                    print("har sl�ngt " + trashthrown);

                    StartCooldown(0.2f);
                }
                else
                {
                    opentrash = !opentrash;

                    //debug-kod, ska tas bort
                        if (detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.red)
                        { detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white; }
                        else
                        { detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red; }

                    print("�ppnade luckan");

                    StartCooldown(0.2f);
                }
            }

            else if (detectedobject.transform.name.StartsWith("Wardrobe") && !cooldown)
            {
                if(hidden)
                {

                    StartCooldown(0.6f);
                    hidden = false;
                    GetComponent<SpriteRenderer>().enabled = true;
                }

                else
                {
                    StartCooldown(0.6f);
                    hidden = true;
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        GameObject.Find("CAMERA").transform.position = new Vector3((transform.position.x + GameObject.Find("CAMERA").transform.position.x) / 2, 0, -10);
    }

    private void StartCooldown(float duration)
    {
        //funktion f�r att ist�llet f�r att skriva samma tv� rader av kod hela tiden kunna korta ner det
        cooldown = true;
        Invoke("Cooldown", duration);
    }

    private void Cooldown()
    { cooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("1");
        detectedobject = collision.gameObject;
        colliding = true;

        GameObject.Find("popupSpace").transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + 1.5f, -1);
        GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print("0");

        if (detectedobject == collision.gameObject)
        {
            colliding = false;
            detectedobject = null;
            GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
