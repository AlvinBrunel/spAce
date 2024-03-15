using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] float Speed;
    Vector3 InitialPos;
    public int Damage;
    ParticleSystem PS;
    SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        InitialPos = transform.position;

        PS = GetComponent<ParticleSystem>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.right * Speed * Time.deltaTime);

        if (Vector3.Distance(InitialPos,transform.position) > 100f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform" || collision.transform.tag == "Enemy")
        {
            SpawnParticle();
        }
    }
    public void SpawnParticle()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        SR.enabled = false;
        PS.Play();
        Invoke("DestroyBullet", 0.1f);
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
