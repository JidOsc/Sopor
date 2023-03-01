using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptCharacter : MonoBehaviour
{
    public int speed = 1;

    public int totaltrash = 4;
    private int trashpicked = 0;

    //variabeln ska m�ta vart man �r i spelet,
    //0 = tutorial 1
    //1 = tutorial 2
    //2 = plocka sopor
    //3 = stalker syns utanf�r f�nster
    //4 = stalker �r i huset
    //5 = stalker jagar spelare
    public short stage = 0;

    private bool footcooldown = false;

    public new Rigidbody2D rigidbody;
    public AudioSource ljud;
    public GameObject Canvas;

    private bool cooldown = false;
    private bool opentrash = false;
    private bool colliding = false;
    public bool hidden = false;
    private bool isfading = false;
    private short totaltrashpicked = 0;

    public GameObject cam;
    public GameObject fadebox;
    public GameObject rainemitter;
    public GameObject jumpscare;
    public GameObject door;
    public GameObject stalker;
    public GameObject winzone;

    public GameObject tutorial1;
    public GameObject tutorial2;

    public GameObject victorytext;
    public GameObject victorybutton;
    public GameObject restartbutton;

    public AudioClip dooropens;
    public AudioClip doorcloses;

    public Sprite doorclosed;
    public Sprite dooropen;

    public Sprite trashcanclosed;
    public Sprite trashcanopen;

    public AudioClip trashcanopens;
    public AudioClip trashcancloses;

    private List<GameObject> discoveredobjects = new List<GameObject> { };


    Vector3[] rooms =
    {
        new Vector3(-4, 0, -10),
        new Vector3(8, 0, -10),
        new Vector3(20, 0, -10)
    };

    public AudioClip[] stepsoutside =
    {};

    public AudioClip[] stepsinside =
    {};

    

    private Vector3 currentroom;
    private short roomnumber;


    private void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.LeftShift))
        //{ speed = 4; }

        //else if (speed != 2)
        //{ speed = 2; }

        if (!hidden && !isfading && !GameObject.Find("Canvas").GetComponent<ScriptUI>().importantdialogue)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            { 
                rigidbody.velocity = new Vector2(speed, 0);
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<Animator>().SetBool("walking", true);
                Footsteps();
                
            }

            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            { 
                rigidbody.velocity = new Vector2(-speed, 0);
                GetComponent<SpriteRenderer>().flipX = true;
                GetComponent<Animator>().SetBool("walking", true);
                Footsteps();
            }

            else
            { 
                rigidbody.velocity = new Vector2(0, 0);
                GetComponent<Animator>().SetBool("walking", false);
            }
        }

        else
        {
            GetComponent<Animator>().SetBool("walking", false);
            rigidbody.velocity = new Vector2(0, 0); 
        }

        if (colliding)
        {
            if (discoveredobjects.Contains(GameObject.Find("STALKER")) && !hidden && stage == 5)
            {
                GameOver();
            }

            else if (Input.GetKey(KeyCode.E) && cooldown == false)
            {
                foreach (GameObject detectedobject in discoveredobjects)
                {
                    if (detectedobject.transform.name.StartsWith("Door"))
                    {
                        AudioSource audio = detectedobject.GetComponent<AudioSource>();

                        Door(detectedobject, detectedobject.GetComponent<BoxCollider2D>().enabled);

                        StartCooldown(0.4f);
                        break;
                    }

                    else if (detectedobject.transform.name.StartsWith("trash") && trashpicked != totaltrash)
                    {
                        if (trashpicked % 6 != 0 || trashpicked == 0)
                        {
                            trashpicked += 1;
                            totaltrashpicked += 1;

                            switch (totaltrashpicked)
                            {
                                case 1:
                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(3);
                                    break;

                                case 5:
                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(4);
                                    break;

                                case 9:
                                    Door(door, false);
                                    break;

                                case 12:
                                    stalker.transform.position = new Vector3(10, -0.25f, 0);
                                    stalker.GetComponent<SpriteRenderer>().color = Color.white;
                                    stage = 3;

                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(5);
                                    break;

                                case 13:
                                    //stalker b�rjar jaga
                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(8);
                                    break;
                            }

                            discoveredobjects.Remove(detectedobject);
                            Destroy(detectedobject.gameObject);

                            GetComponent<Animator>().SetInteger("trash", trashpicked);

                            StartCooldown(0.2f);
                            break;
                        }

                        else
                        {
                            Canvas.GetComponent<ScriptUI>().UpdateProgress(6);
                        }
                    }

                    else if (detectedobject.transform.name == "Trashcan")
                    {
                        //om antal skr�p som plockats upp delat p� skr�p som ryms i sopp�se g�r j�mnt ut
                        if (opentrash && trashpicked == totaltrash)
                        {
                            trashpicked = 0;
                            print("sl�ngde sopor");
                            GetComponent<Animator>().SetInteger("trash", 0);

                            StartCooldown(0.2f);
                        }
                        else
                        {
                            if(opentrash)   {detectedobject.GetComponent<AudioSource>().PlayOneShot(trashcancloses); detectedobject.GetComponent<SpriteRenderer>().sprite = trashcanclosed; }

                            else            {detectedobject.GetComponent<AudioSource>().PlayOneShot(trashcanopens); detectedobject.GetComponent<SpriteRenderer>().sprite = trashcanopen; }

                            opentrash = !opentrash;

                            StartCooldown(0.2f);
                        }
                        break;
                    }

                    else if (detectedobject.transform.name.StartsWith("Hide") && !cooldown)
                    {
                        if(stage == 4)
                        {
                            Canvas.GetComponent<ScriptUI>().UpdateProgress(9);
                            stage = 6;
                        }
                        
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

                    else if (detectedobject.transform.name == "Speaker" && !cooldown)
                    {
                        if(stage == 1)
                        {
                            stage = 2;
                            tutorial2.SetActive(false);
                            GameObject.Find("Canvas").GetComponent<ScriptUI>().UpdateProgress(2);
                        }

                        detectedobject.GetComponent<Animator>().SetBool("music", true);
                        detectedobject.GetComponent<AudioSource>().Play();
                        StartCooldown(0.6f);
                        Invoke("TurnoffStereo", 43);
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

        stage = 0;

        roomnumber = 1;
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

    public void Footsteps()
    {
        if (!footcooldown)
        {
            //varje gång cooldownen har löpt ut spelas ett random ljud från en lista för variation
            //beroende på vart man är i relation till punkt x2 kommer ljudet låta annorlunda

            if (transform.position.x < 2)
            {
                ljud.PlayOneShot(stepsoutside[Random.Range(0, 5)]);
            }

            else
            { ljud.PlayOneShot(stepsinside[Random.Range(0, 5)]); }

            footcooldown = true;
            Invoke("FootCooldown", 0.6f);
        }
    }
    private void FootCooldown()
    {
        footcooldown = false;
    }

    private void TurnoffStereo()
    {
        GameObject.Find("Speaker").GetComponent<Animator>().SetBool("music", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        discoveredobjects.Add(collision.gameObject);
        colliding = true;

        if(stage == 0 && collision.transform.name == "Speaker")
        {
            stage = 1;
            tutorial1.SetActive(false);
            tutorial2.SetActive(true);
        }

        if(stage == 5 && collision.transform.name == "winzone")
        {
            Escape();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //plocka bort objektet som man l�mnar ur listan
        discoveredobjects.Remove(collision.gameObject);

        if (collision.gameObject.name == "HideWardrobe" && stage == 3)
        {
            print("usch");
            Canvas.GetComponent<ScriptUI>().UpdateProgress(7);
            stage = 6;
        }

        //om listan �r tom s� kolliderar inte karakt�ren l�ngre
        if (discoveredobjects.Count == 0)
        {
            colliding = false;
        }
    }

    public void GameOver()
    {
        jumpscare.SetActive(true);
        cam.GetComponent<AudioSource>().Play();
        restartbutton.SetActive(true);
    }

    public void Door(GameObject door, bool open)
    {
        AudioSource audio = door.GetComponent<AudioSource>();

        if (open)
        {
            door.GetComponent<SpriteRenderer>().sprite = dooropen;
            door.GetComponent<SpriteRenderer>().sortingOrder = 1;
            audio.PlayOneShot(dooropens);
            door.GetComponent<BoxCollider2D>().enabled = false;
        }

        else
        {
            door.GetComponent<SpriteRenderer>().sprite = doorclosed;
            door.GetComponent<SpriteRenderer>().sortingOrder = -1;
            audio.PlayOneShot(doorcloses);
            door.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void Escape()
    {
        print("du flydde");
        stalker.SetActive(false);
        GameObject.Find("fadebox").GetComponent<Image>().color = new Color(0, 0, 0, 235);
        victorytext.SetActive(true);
        victorybutton.SetActive(true);
        
    }

    public void TryAgain()
    {
        Fade();
        transform.position = new Vector2(13, -0.25f);
        hidden = true;
        GetComponent<SpriteRenderer>().enabled = false;
        Canvas.GetComponent<ScriptUI>().UpdateProgress(9);
        restartbutton.SetActive(false);
    }
}
