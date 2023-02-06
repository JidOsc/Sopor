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
        "Nu har du slängt 2 soppåsar!",
        "3 soppåsar har slängts",
        "Vem är där?"
                     };


    private short linenumber;

    public GameObject dialoguetext;
    public GameObject portrait1;

    // Start is called before the first frame update
    void Start()
    {
        trashthrown = 0;
        //transform.GetChild(0).GetComponent<TMP_Text>().maxVisibleCharacters = 3;

        portrait1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && line != "")
        {
            dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters = line.Length;
        }
    
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    public void UpdateProgress(short trashpicked)
    {
        trashthrown += trashpicked;
        print(trashthrown);

        switch (trashthrown)
        {
            case 2:
                StartCoroutine(Dialogue(1));
                break;

            case 3:
                StartCoroutine(Dialogue(2));
                break;
        }
    }

    IEnumerator Dialogue(short convonumber)
    {
        switch(convonumber)
        {
            case 1:
                yield return new WaitForSeconds(1);
                ShowDialogue(lines[0], 1);

                while (dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters < line.Length)
                {
                    yield return new WaitForSeconds(0.5f);
                    dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters += 1;
                }
                Invoke("ClearDialogue", 2f);

                break;

            case 2:
                yield return new WaitForSeconds(1);
                ShowDialogue(lines[1], 1);

                while (dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters < line.Length)
                {
                    yield return new WaitForSeconds(0.5f);
                    dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters += 1;
                }
                Invoke("ClearDialogue", 2f);

                ShowDialogue(lines[2], 1);

                while (dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters < line.Length)
                {
                    yield return new WaitForSeconds(0.5f);
                    dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters += 1;
                }
                Invoke("ClearDialogue", 2f);

                break;
        }
    }

    void ShowDialogue(string text, short portrait)
    {
        switch (portrait)
        {
            case 1:
                print("boop");
                portrait1.SetActive(true);
                break;

            case 2:
                print("biip");
                portrait1.SetActive(true);
                break;
        }

        line = text;
        dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters = 0;

        dialoguetext.GetComponent<TMP_Text>().text = text;
    }

    private void ClearDialogue()
    {
        line = "";
        portrait1.SetActive(false);
        dialoguetext.GetComponent<TMP_Text>().text = "";
    }
}
