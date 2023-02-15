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
    public bool hidden = false;
    private bool isfading = false;

    public GameObject cam;
    public GameObject fadebox;
    public GameObject rainemitter;
    public GameObject jumpscare;

    private List<GameObject> discoveredobjects = new List<GameObject> { };


    Vector3[] rooms =
    {
        new Vector3(-4, 0, -10),
        new Vector3(8, 0, -10),
        new Vector3(20, 0, -10)
    };

    private Vector3 currentroom;
    private short roomnumber;


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        { speed = 4; }

        else if (speed != 2)
        { speed = 2; }

        if (!hidden && !isfading && GameObject.Find("Canvas").GetComponent<ScriptUI>().done)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            { 
                rigidbody.velocity = new Vector2(speed, 0);
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<Animator>().SetBool("walking", true);
            }

            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            { 
                rigidbody.velocity = new Vector2(-speed, 0);
                GetComponent<SpriteRenderer>().flipX = true;
                GetComponent<Animator>().SetBool("walking", true);
            }

            else
            { 
                rigidbody.velocity = new Vector2(0, 0);
                GetComponent<Animator>().SetBool("walking", false);
            }
        }

        else
            { rigidbody.velocity = new Vector2(0, 0); }

        if (colliding)
        {
            if (discoveredobjects.Contains(GameObject.Find("STALKER")) && !hidden)
            {
                GameOver();
            }

            else if (Input.GetKey(KeyCode.E) && cooldown == false)
            {
                foreach (GameObject detectedobject in discoveredobjects)
                {
                    if (detectedobject.transform.name.StartsWith("Door"))
                    {
                        detectedobject.GetComponent<BoxCollider2D>().enabled = !detectedobject.GetComponent<BoxCollider2D>().enabled;

                        //debug-kod, ska tas bort
                        if (detectedobject.GetComponent<SpriteRenderer>().color == Color.white)
                        { detectedobject.GetComponent<SpriteRenderer>().color = Color.red; }
                        else
                        { detectedobject.GetComponent<SpriteRenderer>().color = Color.white; }

                        StartCooldown(0.4f);
                        break;
                    }

                    else if (detectedobject.transform.name.StartsWith("trash") && trashpicked != totaltrash)
                    {
                        trashpicked += 1;

                        discoveredobjects.Remove(detectedobject);
                        Destroy(detectedobject.gameObject);

                        GetComponent<Animator>().SetInteger("trash", trashpicked);

                        StartCooldown(0.2f);
                        break;
                    }

                    else if (detectedobject.transform.name == "Trashcan")
                    {
                        if (opentrash && trashpicked == totaltrash)
                        {
                            trashpicked = 0;
                            print("sl�ngde sopor");
                            GetComponent<Animator>().SetInteger("trash", 0);

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

                            print("�ppnade luckan");

                            StartCooldown(0.2f);
                        }
                        break;
                    }

                    else if (detectedobject.transform.name.StartsWith("Wardrobe") && !cooldown)
                    {
                        if (hidden)
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
                        break;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (currentroom.x - transform.position.x < 3f && currentroom.x - transform.position.x > -3f)
        {
            //koden kollar om karakt�ren �r l�ngre �n 3 enheter bort fr�n mitten av rummet
            //(specificerat i en lista l�ngre upp och g�r p� s� s�tt att ut�ka fritt)
            //om den inte �r det kommer kameran s�ttas p� positionen av karakt�ren. P� det s�ttet f�ljer kameran karakt�ren fram tills ett visst avst�nd fr�n mitten

            rainemitter.transform.position = new Vector3(transform.position.x, 2, 0);
            cam.transform.position = new Vector3(transform.position.x, 0, -10);
        }
    }

    private void Start()
    {
        StartCoroutine(Fade());
        InvokeRepeating("CheckCamera", 0.5f, 0.5f);

        roomnumber = 0;
        currentroom = rooms[roomnumber];
        

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
        //om karakt�ren �r mer �n 6.2 enheter bort fr�n kamerans mittpunkt s� kommer kameran flyttas 12 enheter i den riktningen
        if (currentroom.x + 6.2f < transform.position.x)
        {
            roomnumber += 1;
            currentroom = rooms[roomnumber];

            cam.transform.position = new Vector3(cam.transform.position.x + 6, 0, -10);

            StartCoroutine("Fade");
        }

        else if (currentroom.x - 6.2f > transform.position.x)
        {
            //beroende p� om karakt�ren r�r sig till v�nster eller h�ger s� kommer det aktiva rummet ocks� r�ra sig dit�t, viktigt f�r att veta kamerans relation
            roomnumber -= 1;
            currentroom = rooms[roomnumber];

            cam.transform.position = new Vector3(cam.transform.position.x - 6, 0, -10);

            StartCoroutine("Fade");
        }
    }

    private void StartCooldown(float duration)
    {
        //funktion f�r att ist�llet f�r att skriva samma tv� rader av kod hela tiden kunna korta ner det
        cooldown = true;
        Invoke("Cooldown", duration);
    }

    private void Cooldown()
    { 
        //eftersom den kallas med en Invoke() kommer cooldown avaktiveras efter �nskad tid
        cooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        discoveredobjects.Add(collision.gameObject);
        colliding = true;

        if (collision.transform.name == "triggerzone")
        {
            GameObject.Find("Stalker").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        discoveredobjects.Remove(collision.gameObject);

        if (discoveredobjects.Count == 0)
        {
            colliding = false;
        }
    }

    public void GameOver()
    {
        jumpscare.SetActive(true);
    }
}
