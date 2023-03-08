using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStalker : MonoBehaviour
{
    private bool cooldown = false;
    public bool active = false;

    private Vector2 direction = Vector2.right;

    public float huntspeed = 3;
    //vilken hastighet ska stalkern röra sig med när den ser spelaren

    public float huntdistance = 1;
    //vilket avstånd ska stalkern kunna se spelaren på

    public GameObject Character;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().sortingOrder = 0;
        //GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void FixedUpdate()
    {
        if (active)
        {
            if (SeesPlayer() && !Character.GetComponent<ScriptCharacter>().hidden)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(DirToCharacter() * huntspeed, 0);
            }

            else if (!cooldown)
            {
                switch (Random.Range(1, 11))
                {
                    case 1:
                        GetComponent<SpriteRenderer>().flipX = true;
                        direction = Vector2.left;
                        break;

                    case 2:
                        GetComponent<SpriteRenderer>().flipX = false;
                        direction = Vector2.right;
                        break;

                    case > 2:
                        GetComponent<Rigidbody2D>().velocity = direction * 3;
                        break;
                }
                cooldown = true;
                Invoke("Cooldown", 0.7f);
            }
        }

        if(GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            GetComponent<Animator>().SetBool("walking", true);
        }

        else
        {
            GetComponent<Animator>().SetBool("walking", false);
        }
    }

    private void Cooldown()
    {
        cooldown = false;
    }

    private bool SeesPlayer()
    {
        //skickar en stråle i riktningen som den kollar ett avstånd som är konfigurerbart
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(position + (direction*huntdistance), position, LayerMask.GetMask("Character"));

        if(hit)
        { return (hit.collider.transform.name == "CHARACTER"); }

        else { return (false); }
    }

    private float DirToCharacter()
    {
        //räknar ut vilket håll karaktären är åt
        return Mathf.Clamp(Character.transform.position.x - transform.position.x, -1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.name.StartsWith("Door") && collision.GetComponent<BoxCollider2D>().enabled && active)
        {
            GameObject.Find("CHARACTER").GetComponent<ScriptCharacter>().Door(collision.gameObject, true);
        }

        else if (collision.transform.name.StartsWith("edgeborder"))
        {
            if (direction == Vector2.left)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                direction = Vector2.right;
            }

            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
                direction = Vector2.left;
            }
        }
    }
}
