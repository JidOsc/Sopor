using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStalker : MonoBehaviour
{
    private bool cooldown = false;

    private Vector2 direction = Vector2.right;

    public float huntspeed = 3;
    //vilken hastighet ska stalkern r�ra sig med n�r den ser spelaren

    public float huntdistance = 1;
    //vilket avst�nd ska stalkern kunna se spelaren p�

    public GameObject Character;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (SeesPlayer() && !Character.GetComponent<ScriptCharacter>().hidden)
        {
            print("karakt�r");
            GetComponent<Rigidbody2D>().velocity = new Vector2(DirToCharacter() * huntspeed, 0);
        }

        else if (!cooldown)
        {
            switch (Random.Range(1, 3))
            {
                case 1:
                    direction = Vector2.right;
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * 3;
                    break;

                case 2:
                    direction = Vector2.left;
                    GetComponent<Rigidbody2D>().velocity = Vector2.left * 3;
                    break;
            }
            cooldown = true;
            Invoke("Cooldown", 0.7f);
        }
    }

    private void Cooldown()
    {
        cooldown = false;
    }

    private bool SeesPlayer()
    {
        print("varde ljus");
        //skickar en str�le i riktningen som den kollar ett avst�nd som �r konfigurerbart
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(position + (direction*huntdistance), position, LayerMask.GetMask("Character"));

        if(hit)
        { return (hit.collider.transform.name == "CHARACTER"); }

        else { return (false); }
    }

    private float DirToCharacter()
    {
        return Mathf.Clamp(Character.transform.position.x - transform.position.x, -1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.name.StartsWith("Door") && !collision.GetComponent<BoxCollider2D>().enabled)
        {
            collision.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
