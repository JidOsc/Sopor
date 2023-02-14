using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStalker : MonoBehaviour
{
    private bool cooldown = false;

    private Vector2 direction = Vector2.right;

    public float huntspeed = 3;
    public float huntdistance = 1;

    public GameObject Character;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (SeesPlayer() && !Character.GetComponent<ScriptCharacter>().hidden)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(DirToCharacter() * huntspeed, 0);
        }

        else if (!cooldown)
        {
            int randint = Random.Range(1, 3);

            switch (randint)
            {
                case 1:
                    direction = Vector2.right;
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * 3 * DirToCharacter();
                    break;

                case 2:
                    direction = Vector2.left;
                    GetComponent<Rigidbody2D>().velocity = Vector2.left * 3 * DirToCharacter();
                    break;
            }
            cooldown = true;
            Invoke("Cooldown", 0.5f);
        }
    }

    private void Cooldown()
    {
        cooldown = false;
    }

    private bool SeesPlayer()
    {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(position + direction*huntdistance, position);

        if(hit)
        { return (hit.collider.transform.name == "CHARACTER"); }

        else { return (false); }
    }

    private float DirToCharacter()
    {
        return Mathf.Clamp(Character.transform.position.x - transform.position.x, -1, 1);
    }
}
