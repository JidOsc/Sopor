using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptCharacter : MonoBehaviour
{
    public int speed = 1;

    public int totaltrash = 4;
    private int trashpicked = 0;

    //variabeln ska mäta vart man är i spelet,
    //0 = tutorial 1
    //1 = tutorial 2
    //2 = plocka sopor
    //3 = stalker syns utanför fönster
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

    public GameObject tutorial1;
    public GameObject tutorial2;

    public AudioClip dooropens;
    public AudioClip doorcloses;

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
        if (Input.GetKey(KeyCode.LeftShift))
        { speed = 4; }

        else if (speed != 2)
        { speed = 2; }

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
            { rigidbody.velocity = new Vector2(0, 0); }

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
                            audio.PlayOneShot(dooropens);
                            detectedobject.GetComponent<BoxCollider2D>().enabled = false;
                        }

                        else
                        {
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
                                    door.GetComponent<BoxCollider2D>().enabled = true;
                                    door.GetComponent<AudioSource>().PlayOneShot(doorcloses);
                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(3);
                                    break;

                                case 8:
                                    Canvas.GetComponent<ScriptUI>().UpdateProgress(4);
                                    stage = 3;
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
                        //om antal skräp som plockats upp delat på skräp som ryms i soppåse går jämnt ut
                        if (opentrash && trashpicked == totaltrash)
                        {
                            trashpicked = 0;
                            print("slängde sopor");
                            GetComponent<Animator>().SetInteger("trash", 0);

                            StartCooldown(0.2f);
                        }
                        else
                        {
                            if(opentrash)   {detectedobject.GetComponent<AudioSource>().PlayOneShot(trashcancloses);}

                            else            {detectedobject.GetComponent<AudioSource>().PlayOneShot(trashcanopens);}

                            opentrash = !opentrash;

                            //debug-kod, ska tas bort
                            if (detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.red)
                            { detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white; }
                            else
                            { detectedobject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red; }

                            print("öppnade luckan");

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
            //koden kollar om karaktären är längre än 3 enheter bort från mitten av rummet
            //(specificerat i en lista längre upp och går på så sätt att utöka fritt)
            //om den inte är det kommer kameran sättas på positionen av karaktären. På det sättet följer kameran karaktären fram tills ett visst avstånd från mitten

            rainemitter.transform.position = new Vector3(transform.position.x, 2, 0);
            cam.transform.position = new Vector3(transform.position.x, 0, -10);
        }
    }

    private void Start()
    {
        StartCoroutine(Fade());
        InvokeRepeating("CheckCamera", 0.5f, 0.5f);

        stage = 0;
        tutorial1.SetActive(true);

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
        //om karaktären är mer än 6.2 enheter bort från kamerans mittpunkt så kommer kameran flyttas 12 enheter i den riktningen
        if (currentroom.x + 6.2f < transform.position.x)
        {
            roomnumber += 1;
            currentroom = rooms[roomnumber];

            cam.transform.position = new Vector3(cam.transform.position.x + 6, 0, -10);

            StartCoroutine("Fade");
        }

        else if (currentroom.x - 6.2f > transform.position.x)
        {
            //beroende på om karaktären rör sig till vänster eller höger så kommer det aktiva rummet också röra sig ditåt, viktigt för att veta kamerans relation
            roomnumber -= 1;
            currentroom = rooms[roomnumber];

            cam.transform.position = new Vector3(cam.transform.position.x - 6, 0, -10);

            StartCoroutine("Fade");
        }
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

    private void Footsteps()
    {
        if (!footcooldown)
        {
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
