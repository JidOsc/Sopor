using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScriptUI : MonoBehaviour
{
    private short trashthrown;
    private string line;

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
    { }

    private void FixedUpdate()
    {
            
    }

    IEnumerator RevealText()
    {

        while(dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters < line.Length)
        {
            dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters += 1;
            yield return new WaitForSeconds(0.1f);
        }
        Invoke("ClearDialogue", 2f);
    }

    private void ClearDialogue()
    {
        line = "";
        portrait1.SetActive(false);
        dialoguetext.GetComponent<TMP_Text>().text = "";
    }

    public void UpdateProgress(short trashpicked)
    {
        trashthrown += trashpicked;
        print(trashthrown);

        switch(trashthrown)
        {
            case 2:
                ShowDialogue("Tja, du har nyss plockat din andra soppåse", 1);
                break;
        }
    }

    void ShowDialogue(string text, short portrait)
    {
        print("beep");
        switch (portrait)
        {
            case 1:
                print("boop");
                portrait1.SetActive(true);
                break;
        }

        line = text;

        dialoguetext.GetComponent<TMP_Text>().maxVisibleCharacters = 0;
    
        StartCoroutine("RevealText");

        dialoguetext.GetComponent<TMP_Text>().text = text;


    }
}
