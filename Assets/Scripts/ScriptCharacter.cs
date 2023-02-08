using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptCharacter : MonoBehaviour
{
    public int speed = 1;

    public int totaltrash = 4;
    private int trashpicked = 0;

    public new Rigidbody2D rigidbody;
    public GameObject Canvas;

    private bool cooldown = false;
    private bool opentrash = false;
    private bool colliding = false;
    private bool hidden = false;
    private bool isfading = false;

    public GameObject cam;
    private GameObject detectedobject = null;
    public GameObject fadebox;
    

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        { speed = 6; }

        else if (speed != 3)
        { speed = 3; }

        if (!hidden && !isfading && GameObject.Find("Canvas").GetComponent<ScriptUI>().done)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            { rigidbody.velocity = new Vector2(speed, 0); }

            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            { rigidbody.velocity = new Vector2(-speed, 0); }

            else
            { rigidbody.velocity = new Vector2(0, 0); }
        }

        else
            { rigidbody.velocity = new Vector2(0, 0); }

        if (Input.GetKey(KeyCode.Space) && cooldown == false && colliding)
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

            else if (detectedobject.transform.name.StartsWith("trash") && trashpicked != totaltrash)
            {
                trashpicked += 1;
                Destroy(detectedobject.gameObject);

                StartCooldown(0.2f);
            }

            else if (detectedobject.transform.name == "Trashcan")
            {
                if (opentrash && trashpicked == totaltrash)
                {
                    trashpicked = 0;
                    print("slängde sopor");

                    Canvas.GetComponent<ScriptUI>().UpdateProgress(1);

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

                    print("öppnade luckan");

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
                }
            }
        }
    }

    private void Start()
    {
        StartCoroutine(Fade());
        InvokeRepeating("CheckCamera", 0.5f, 0.5f);
    }

    IEnumerator Fade()
    {
        isfading = true;
        Color fadecolor = fadebox.GetComponent<Image>().color;
        fadecolor.a = 1;

        fadebox.GetComponent<Image>().color = fadecolor;

        yield return new WaitForSeconds(0.02f);

        while (fadecolor.a > 0)
        {
            yield return new WaitForSeconds(0.01f);
            fadecolor.a -= 0.1f;
            fadebox.GetComponent<Image>().color = fadecolor;
        }
        isfading = false;
    }

    private void CheckCamera()
    {
        //om karaktären är mer än 6.2 enheter bort från kamerans mittpunkt så kommer kameran flyttas 12 enheter i den riktningen
        if (cam.transform.position.x + 6.2f < transform.position.x){cam.transform.position = new Vector3(cam.transform.position.x + 12, cam.transform.position.y, -10); StartCoroutine("Fade"); }
        else if (cam.transform.position.x - 6.2f > transform.position.x){cam.transform.position = new Vector3(cam.transform.position.x - 12, cam.transform.position.y, -10);StartCoroutine("Fade"); }
    }

    private void StartCooldown(float duration)
    {
        //funktion för att istället för att skriva samma två rader av kod hela tiden kunna korta ner det
        cooldown = true;
        Invoke("Cooldown", duration);
    }

    private void Cooldown()
    { 
        //eftersom den kallas med en Invoke() kommer cooldown avaktiveras efter önskad tid
        cooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedobject = collision.gameObject;
        colliding = true;

        GameObject.Find("popupSpace").transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + 1.5f, -1);
        GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = true;

        if (collision.transform.name == "triggerzone")
        {
            GameObject.Find("Stalker").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectedobject == collision.gameObject)
        {
            colliding = false;
            detectedobject = null;
            GameObject.Find("popupSpace").GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
