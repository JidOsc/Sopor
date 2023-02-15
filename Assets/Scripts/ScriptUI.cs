using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScriptUI : MonoBehaviour
{
    private string line = "";
    private bool skipped = false;
    private bool activedialogue = false;

    public bool importantdialogue = false;
    public bool done = true;

    string[] lines = {
        "I should turn on the speakers.",

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

        "Ew, this stinks",
        "I guess it's about time we cleaned this out...",
        "Jesus, this looks like shit...",

        "What was that?",

        "You already home?",
        "...",
        "Hello? ...Luna?",

        "FUCK-"
                     };

    private short linenumber;

    public GameObject dialoguetext;

    public GameObject portraitluna;
    public GameObject portraitchar;
    public GameObject portraitstal;
    public GameObject portraitphon;
    
    public GameObject character;
    public GameObject stalker;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dialogue(1));

        HidePortraits();

        character = GameObject.Find("CHARACTER");
        stalker = GameObject.Find("Stalker");
        
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
        switch (stage)
        {
            case 1:
                StartCoroutine(Dialogue(2));
                GameObject.Find("triggerzone").GetComponent<BoxCollider2D>().enabled = true;
                stalker.transform.position = new Vector3(4.2f, 0.9f, 0f);
                break;

            case 2:
                StartCoroutine(Dialogue(3));
                break;

            case 3:
                StartCoroutine(Dialogue(4));
                break;

            case 4:
                StartCoroutine(Dialogue(5));
                break;

        }
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
                activedialogue = importantdialogue = true;

                done = false; ShowDialogue(lines[0], 0); while (!done) { yield return null; }

                activedialogue = importantdialogue = false;
                break;

            case 2:
                activedialogue = importantdialogue = true;

                done = false;   ShowDialogue(lines[1], 3);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[2], 0);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[3], 2);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[4], 0);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[5], 1);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[6], 2);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[7], 0);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[8], 1);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[9], 2);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[10], 0); while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[11], 1); while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[12], 2); while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[13], 0); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[14], 1); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[15], 2); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[16], 1); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[17], 2); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[18], 1); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[19], 3); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[20], 0); while (!done) { yield return null; }

                activedialogue = importantdialogue = false;
                break;


            case 3:
                activedialogue = true;
                done = false; ShowDialogue(lines[21], 0); while (!done) { yield return null; }
                activedialogue = false;
                break;

            case 4:
                activedialogue = true;
                done = false; ShowDialogue(lines[22], 0); while (!done) { yield return null; }
                activedialogue = false;
                break;

            case 5:
                activedialogue = true;
                done = false; ShowDialogue(lines[23], 0); while (!done) { yield return null; }
                activedialogue = false;
                break;

            case 6:
                activedialogue = true;
                done = false; ShowDialogue(lines[24], 1); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[25], 0); while (!done) { yield return null; }

                done = false; ShowDialogue(lines[26], 1); while (!done) { yield return null; }

                activedialogue = false;
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

    private void HidePortraits()
    {
        portraitchar.SetActive(false);
        portraitluna.SetActive(false);
        portraitstal.SetActive(false);
        portraitphon.SetActive(false);
    }
}
