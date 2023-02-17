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
    //4 = stalker jagar spelare
    private short stage = 0;

    private short footstep = 0;
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

    public GameObject tutorial1;
    public GameObject tutorial2;

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
            if (discoveredobjects.Contains(GameObject.Find("STALKER")) && !hidden && stage == 4)
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

                        if (detectedobject.GetComponent<BoxCollider2D>().enabled)
                        {
                            detectedobject.GetComponent<SpriteRenderer>().sprite = dooropen;
                            detectedobject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                            audio.PlayOneShot(dooropens);
                            detectedobject.GetComponent<BoxCollider2D>().enabled = false;
                        }

                        else
                        {
                            detectedobject.GetComponent<SpriteRenderer>().sprite = doorclosed;
                            detectedobject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                            audio.PlayOneShot(doorcloses);
                            detectedobject.GetComponent<BoxCollider2D>().enabled = true;
                        }

                        StartCooldown(0.4f);
                        break;
                    }

                    else if (detectedobject.transform.name.StartsWith("trash") && trashpicked != totaltrash)
                    {
                        if (trashpicked != totaltrash)
                        {
                            trashpicked += 1;
                            totaltrashpicked += 1;

                            switch (totaltrashpicked)
                            {
                                case 1:
                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(2);
                                    break;

                                case 5:
                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(3);
                                    break;

                                case 9:
                                    door.GetComponent<BoxCollider2D>().enabled = true;
                                    door.GetComponent<AudioSource>().PlayOneShot(doorcloses);
                                    break;

                                case 12:
                                    stalker.transform.position = new Vector3(10, -0.25f, 0);
                                    stage = 3;

                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(4);
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
                            Canvas.GetComponent<ScriptUI>().UpdateProgress(5);
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

                    else if (detectedobject.transform.name == "Speaker" && !cooldown)
                    {
                        if(stage == 1)
                        {
                            stage = 2;
                            tutorial2.SetActive(false);
                            GameObject.Find("Canvas").GetComponent<ScriptUI>().UpdateProgress(1);
                        }

                        detectedobject.GetComponent<AudioSource>().Play();
                        StartCooldown(0.6f);
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

    private void Footsteps()
    {
        if (!footcooldown)
        {
            //varje g�ng cooldownen har l�pt ut kommer ett ljud spelas. Ljudet beror p� om man �r till v�nster eller h�ger om position x = 2
            //till v�nster �r utomhus, h�ger �r inomhus. Sedan finns det 6 olika ljud f�r de olika milj�erna som k�rs i en lista efter varandra
            //s� att det blir lite variation och inte samma ljud hela tiden
            footstep++;
            if (transform.position.x < 2)
            {
                switch (footstep)
                {
                    case 0:
                        ljud.PlayOneShot(stepsoutside[0]);
                        break;
                    case 1:
                        ljud.PlayOneShot(stepsoutside[1]);
                        break;
                    case 2:
                        ljud.PlayOneShot(stepsoutside[2]);
                        break;
                    case 3:
                        ljud.PlayOneShot(stepsoutside[3]);
                        break;
                    case 4:
                        ljud.PlayOneShot(stepsoutside[4]);
                        break;
                    case 5:
                        ljud.PlayOneShot(stepsoutside[5]);
                        footstep = -1;
                        break;
                }
            }

            else
                switch (footstep)
                {
                    case 0:
                        ljud.PlayOneShot(stepsinside[0]);
                        break;
                    case 1:
                        ljud.PlayOneShot(stepsinside[1]);
                        break;
                    case 2:
                        ljud.PlayOneShot(stepsinside[2]);
                        break;
                    case 3:
                        ljud.PlayOneShot(stepsinside[3]);
                        break;
                    case 4:
                        ljud.PlayOneShot(stepsinside[4]);
                        break;
                    case 5:
                        ljud.PlayOneShot(stepsinside[5]);
                        footstep = -1;
                        break;
                }
            footcooldown = true;
            Invoke("FootCooldown", 0.6f);
        }
    }
    private void FootCooldown()
    {
        footcooldown = false;
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //plocka bort objektet som man l�mnar ur listan
        discoveredobjects.Remove(collision.gameObject);

        if (collision.gameObject.name == "Wardrobe" && stage == 3)
        {
            Canvas.GetComponent<ScriptUI>().UpdateProgress(6);
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
    }
}
