using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrierController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().OnDeath();
        }
    }
}
