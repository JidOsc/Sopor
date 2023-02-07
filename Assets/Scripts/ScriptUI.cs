using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScriptUI : MonoBehaviour
{
    private short trashthrown;
    private string line = "";

    private bool done = true;

    string[] lines = {
        "*Incoming call*",
        "Hey cutie, I'm like- stuck in traffic with a bunch of groceries!",
        "Okay?",
        "You don't have to be so cold, cutie! Buuut… I do need you to do something for me.",
        "And what’s in it for me?",
        "I have a new nomster flavor for you, just do what I tell you!",
        "Okay okay. What do you want me to do?",
        "Just pick up some of the trash in the house and take it out!",
        "Luna, it's 11 pm, can't I do it tomorrow-",
        "Well, you're still up so liiike... why not? Unless you do not want the nomster…",
        "Fine, fine. I'll do it!",
        "Hell yeah, stay safe, cutie!",
        "Ugh. Whatever, bye...",
        "Call ends.",

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
    
    public GameObject character;
    public GameObject stalker;

    // Start is called before the first frame update
    void Start()
    {
        trashthrown = 0;

        portraitchar.SetActive(false);
        portraitluna.SetActive(false);
        portraitstal.SetActive(false);

        character = GameObject.Find("CHARACTER");
        stalker = GameObject.Find("Stalker");
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateProgress(short trashpicked)
    {
        trashthrown += trashpicked;
        print(trashthrown);

        switch (trashthrown)
        {
            case 2:
                StartCoroutine(Dialogue(1));
                GameObject.Find("triggerzone").GetComponent<BoxCollider2D>().enabled = true;
                stalker.transform.position = new Vector3(4.2f, 0.9f, 0f);
                break;

            case 3:
                StartCoroutine(Dialogue(2));
                break;

            case 4:
                StartCoroutine(Dialogue(3));
                break;
        }
    }

    IEnumerator RevealText()
    {
        while (dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters < line.Length)
        {
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
                done = false;   ShowDialogue(lines[0], 3);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[1], 2);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[2], 1);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[3], 2);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[4], 1);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[5], 2);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[6], 1);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[7], 2);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[8], 1);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[9], 2);  while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[10], 1); while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[11], 2); while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[12], 1); while (!done) { yield return null; }

                done = false;   ShowDialogue(lines[13], 3); while (!done) { yield return null; }

                break;


            case 2:
                done = false; ShowDialogue(lines[14], 1); while (!done) { yield return null; }

                break;

            case 3:
                done = false; ShowDialogue(lines[15], 1); while (!done) { yield return null; }

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

        portraitchar.SetActive(false);
        portraitluna.SetActive(false);
        portraitstal.SetActive(false);

        dialoguetext.GetComponent<TMP_Text>().text = "";
        done = true;
    }
}
