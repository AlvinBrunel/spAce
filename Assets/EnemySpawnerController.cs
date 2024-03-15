using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] int SpawnedEnemies = 0;
    [SerializeField] GameObject Particle_Explosion;
    [SerializeField] AudioSource AS;

    int Health = 5;

    GameObject Player;
    SpriteRenderer SR;
    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();

        Invoke("SpawningEnemy", 4f);
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        AS.volume = ASVolume(Vector2.Distance(transform.position, Player.transform.position));
    }
    void SpawningEnemy()
    {
        if (SpawnedEnemies < 3 && Vector2.Distance(gameObject.transform.position,Player.transform.position) < 10f)
        {
            GameObject SpawnEnemy = Instantiate(Enemy, transform.position, Quaternion.Euler(0, 0, 0));
            Physics2D.IgnoreCollision(SpawnEnemy.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            SpawnEnemy.name = "Enemy";
        }
        Invoke("SpawningEnemy", 4f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Projectile" && (gameObject.name != "Astronaut Boss" || GameObject.Find("Helmet") == null))
        {
            Destroy(collision.gameObject);
            Hit();
        }
    }
    void OnDeath()
    {
        GameObject EnemyExplosion = Instantiate(Particle_Explosion, transform.position, transform.rotation);
        ParticleSystem.MainModule PS = EnemyExplosion.GetComponent<ParticleSystem>().main;
        PS.startColor = Color.red;
        Destroy(this.gameObject);
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
