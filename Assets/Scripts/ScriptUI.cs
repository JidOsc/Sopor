using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScriptUI : MonoBehaviour
{
    private string line = "";
    private bool skipped = false;
    private bool activedialogue = false;

    public bool importantdialogue = false;
    public bool done = true;

    public bool skipcall;

    //lista över all dialog i spelet
    string[] lines = {
        //introduktion
        "I should turn on the speakers.",

        //samtal
        "*Incoming call*",
        "For fuck sake, what is it now...",
        "Hey cutie, I'm like- stuck in traffic with a bunch of groceries!",
        "It's my roommate, Luna, cheerful and energetic as ever.",
        "Okay?",
        "You don't have to be so cold, cutie! Buuut... I do need you to do something for me.",
        "She sounds way to excited.",
        "And what’s in it for me?",
        "I have a new nomster flavor for you, just do what I tell you!",
        "She knows that is a proposal I can't say no to...",
        "Okay okay. What do you want me to do?",
        "Just pick up some of the trash in the house and take it out!",
        "What was that? A tree swaying perhaps?",
        "Luna, it's like- 11 pm, can't I do it tomorrow-",
        "Well, you're still up so liiike... why not? Unless you do not want the nomster…",
        "Fine, fine. I'll do it!",
        "Hell yeah, stay safe, cutie!",
        "Ugh. Whatever, bye...",
        "Call ends.",
        "I guess it wouldn’t hurt to clean… but did it really have to be now?",

        //olika monologer för när man plockar upp skräp
        "Ew, this stinks",
        "I guess it's about time we cleaned this out...",
        "Jesus, this looks like shit...",

        //när man försöker plocka upp mer än som får plats i soppåsen
        "I need to throw this trash bag into the bin outside",

        //man ser stalkern utanför fönstret
        "What was that?",

        //dörren stängs och man hör fotsteg
        "You already home?",
        "...",
        "Hello? ...Luna?",
        "Someone is inside the house. I should hide in the wardrobe",

        //man ser stalkern för första gången
        "FUCK-",
        "I need to get out of here.",
        "There's a bush I can hide in outdoors.",
        "Just need to wait for the right moment..."

                     };

    public GameObject dialoguetext;

    public GameObject portraitluna;
    public GameObject portraitchar;
    public GameObject portraitstal;
    public GameObject portraitphon;

    public GameObject winzone;

    public GameObject tutorial1;

    public GameObject character;
    public GameObject stalker;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dialogue(1));

        HidePortraits();

        character = GameObject.Find("CHARACTER");
        stalker = GameObject.Find("STALKER");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && activedialogue)
        {
            skipped = true;
            dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters = line.Length;
        }
    }

    public void UpdateProgress(short stage)
    {
        StartCoroutine(Dialogue(stage));
    }

    IEnumerator RevealText()
    {
        while (dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters < line.Length)
        {
            if(skipped)
            {
                break;
            }

            yield return new WaitForSeconds(0.07f);
            dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters += 1;
        }
        Invoke("ClearDialogue", 2f);
    }

    IEnumerator Dialogue(short convo)
    {
        switch(convo)
        {
            case 1:
                //introduktion
                activedialogue = importantdialogue = true;

                done = false; ShowDialogue(lines[0], 0); while (!done) { yield return null; }

                yield return new WaitForSeconds(1);
                tutorial1.SetActive(true);

                activedialogue = importantdialogue = false;
                break;

            case 2:
                if (!skipcall)
                {
                    //telefonsamtal
                    yield return new WaitForSeconds(3);
                    activedialogue = importantdialogue = true;

                    done = false; ShowDialogue(lines[1], 3); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[2], 0); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[3], 2); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[4], 0); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[5], 1); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[6], 2); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[7], 0); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[8], 1); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[9], 2); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[10], 0); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[11], 1); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[12], 2); while (!done)
                    {
                        while (stalker.transform.position.x < 14)
                        {
                            stalker.GetComponent<Rigidbody2D>().velocity = new Vector2(4, 0);
                            yield return new WaitForSeconds(0.1f);
                        }
                        stalker.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        yield return null;
                    }

                    stalker.GetComponent<SpriteRenderer>().color = Color.white;

                    done = false; ShowDialogue(lines[13], 0); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[14], 1); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[15], 2); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[16], 1); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[17], 2); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[18], 1); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[19], 3); while (!done) { yield return null; }

                    done = false; ShowDialogue(lines[20], 0); while (!done) { yield return null; }

                    activedialogue = importantdialogue = false;
                }
                break;

            case 3:
                //plocka sopor 1
                activedialogue = true;
                done = false; ShowDialogue(lines[21], 0); while (!done) { yield return null; }
                activedialogue = false;
                break;

            case 4:
                //plocka sopor 2
                activedialogue = true;
                done = false; ShowDialogue(lines[22], 0); while (!done) { yield return null; }
                activedialogue = false;
                break;

            case 5:
                //plocka sopor 3
                activedialogue = true;
                done = false; ShowDialogue(lines[23], 0); while (!done) { yield return null; }
                activedialogue = false;
                break;

            case 6:
                //plocka upp för mycket
                activedialogue = true;
                done = false; ShowDialogue(lines[24], 0); while (!done) { yield return null; }
                activedialogue = false;
                break;

            case 7:
                //stalker syns utanför fönster kort, 12 sopor totalt + går förbi garderoben
                done = false; ShowDialogue(lines[25], 1); while (!done) {
                    while (stalker.transform.position.x < 14)
                    {
                        stalker.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 0);
                        yield return new WaitForSeconds(0.1f);
                    }
                    stalker.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    yield return null; }
                break;

            case 8:
                //13 sopan
                activedialogue = true;
                importantdialogue = true;
                done = false; ShowDialogue(lines[26], 1); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[27], 0); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[28], 1); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[29], 0); while (!done) { yield return null; }

                activedialogue = false;
                importantdialogue = false;
                character.GetComponent<ScriptCharacter>().stage = 4;
                break;

            case 9:
                stalker.GetComponent<SpriteRenderer>().sortingOrder = 0;
                stalker.transform.position = new Vector2(0, -0.25f);
                stalker.GetComponent<Rigidbody2D>().velocity = new Vector2(3, 0);
                yield return new WaitForSeconds(2);

                importantdialogue = true;
                activedialogue = true;
                done = false; ShowDialogue(lines[30], 0); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[31], 0); while (!done)
                {
                    while (stalker.transform.position.x < 10)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                    yield return null;
                }

                stalker.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                stalker.GetComponent<ScriptStalker>().active = true;
                stalker.GetComponent<AudioSource>().Play();

                character.GetComponent<ScriptCharacter>().stage = 5;
                winzone.SetActive(true);

                done = false; ShowDialogue(lines[32], 0); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[33], 0); while (!done) { yield return null; }

                activedialogue = false;
                importantdialogue = false;
                break;

        }
    }

    void ShowDialogue(string text, short portrait)
    {
        switch (portrait)
        {
            case 1:
                portraitchar.SetActive(true);
                break;

            case 2:
                portraitluna.SetActive(true);
                break;

            case 3:
                portraitphon.SetActive(true);
                break;

            case 4:
                portraitstal.SetActive(true);
                break;
        }

        line = text;
        dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters = 0;
        dialoguetext.GetComponent<TMP_Text>().text = text;

        StartCoroutine(RevealText());
    }

    private void ClearDialogue()
    {
        line = "";

        HidePortraits();

        dialoguetext.GetComponent<TMP_Text>().text = "";
        done = true;
        skipped = false;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void HidePortraits()
    {
        portraitchar.SetActive(false);
        portraitluna.SetActive(false);
        portraitstal.SetActive(false);
        portraitphon.SetActive(false);
    }
}
