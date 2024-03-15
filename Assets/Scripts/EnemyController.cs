using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int Health;
    public int Max_Health;
    public float Speed;
    public bool BossMade;
    Color InitialColour;


    [SerializeField] AudioSource AS;
    [SerializeField] GameObject Particle_Explosion;

    GameObject Player;

    SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");

        Max_Health = Health;
        SR = GetComponent<SpriteRenderer>();
        InitialColour = SR.color;
        if(GameObject.Find("Astronaut Boss"))
        {
            Physics2D.IgnoreCollision(GameObject.Find("Astronaut Boss").GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        }
        InvokeRepeating("MovingSound", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        AS.volume = ASVolume(Vector2.Distance(transform.position, Player.transform.position));
        if (BossMade && GameObject.Find("Astronaut Boss") == null)
        {
            OnDeath();
        }
    }
    void Hit()
    {
        Health--;
        SR.color = Color.red;
        Invoke("ClearColour", 0.25f);
        if (Health <= 0)
        {
            OnDeath();
        }
    }
    void ClearColour()
    {
        SR.color = Color.white;
    }
    void OnDeath()
    {
        GameObject EnemyExplosion = Instantiate(Particle_Explosion, transform.position, transform.rotation);
        ParticleSystem.MainModule PS = EnemyExplosion.GetComponent<ParticleSystem>().main;
        PS.startColor = Color.red;
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.name == "Projectile" && (gameObject.name != "Astronaut Boss" || GameObject.Find("Helmet") == null))
        {
            Destroy(collision.gameObject);
            Hit();
        }
        if(collision.transform.name == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        }
    }
    void MovingSound()
    {
        AS.Play();
    }

    float ASVolume(float dist)
    {
        float MinDist = 1;
        float MaxDist = 14;

        if (dist < MinDist)
        {
            return 1f;
        }
        else if (dist > MaxDist)
        {
            return 0;
        }
        else
        {
            return 1 - ((dist - MinDist) / (MaxDist - MinDist));
        }
    }
}
