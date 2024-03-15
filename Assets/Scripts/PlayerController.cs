using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float Speed = 20f;
    [SerializeField] float JumpForce = 100f;

    int Health = 3;
    int Max_Health;
    public HealthBarController HealthBar;


    [SerializeField] GameObject Particle_Explosion;
    GameObject Cam;

    bool OnGround = false;
    KeyCode Key_Dash;

    public GameObject CheckPoint;

    public CharacterController CC;
    public Animation anim;


    Rigidbody2D RB;
    SpriteRenderer SR;

    [SerializeField] AudioSource AS;
    [SerializeField] AudioClip[] AC;
    // Start is called before the first frame update
    void Start()
    {
        Max_Health = Health;
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        anim.Play("Player Idle");
        Cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnGround == true)
        {
            AS.clip = AC[1];
            AS.Play();
            Jump(JumpForce,Vector2.up);
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * Speed * Time.deltaTime, 0, transform.position.z);
            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Platform")
        {
            OnGround = true;
        }
        if(collision.transform.tag == "EnemyProjectile")
        {
            OnDeath();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform")
        {
            OnGround = false;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag == "EnemyProjectile")
        {
            OnDeath();
        }
    }
    public void Jump(float JumpSpeed, Vector2 Direction)
    {
        RB.velocity += Direction * JumpSpeed;
    }
    public void Hit(int Damage)
    {
        SR.color = Color.red;

        Health -= Damage;
        if (Health <= 0)
        {
            OnDeath();
        }
        float HealthPercentage = (float)Health / (float)Max_Health;
        Invoke("ClearHit", 0.3f);
    }
    private void ClearHit()
    {
        SR.color = Color.white;
    }

    public void OnDeath()
    {
        if (SceneManager.GetActiveScene().name == "Boss Fight")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            AS.clip = AC[0];
            AS.Play();
            GameObject PlayerExplosion = Instantiate(Particle_Explosion, transform.position, transform.rotation);
            ParticleSystem.MainModule PS = PlayerExplosion.GetComponent<ParticleSystem>().main;
            PS.startColor = Color.red;
            RB.velocity = Vector2.zero;
            transform.position = new Vector3(CheckPoint.transform.position.x, CheckPoint.transform.position.y, transform.position.z);
        }
    }
}
