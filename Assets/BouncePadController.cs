using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadController : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] AudioSource AS;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.name == "Player")
        {
            Player.GetComponent<PlayerController>().Jump(Speed, Vector2.up);
            AS.Play();
        }
    }
}
