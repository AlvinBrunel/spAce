using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAstronautController : MonoBehaviour
{
    Vector3 Target;
    GameObject Player;
    Vector3 CharacterSize;
    public Animator Anim;
    Rigidbody2D RB;
    [SerializeField] GameObject Cam;
    [SerializeField] GameObject HealthBarObject;

    bool isSmashing = false;
    bool isWalking = true;
    bool isCharging = false;

    public HealthBarController HealthBar;

    string[] AttackStates = {"SmashState", "ChargeState", "SpawnSlimeState"};
    float[] AttackPlayerDistance = { 3f, 8f, 5f };
    string NextState;


    public float Speed;
    
    
    float PlayerMinDistance = 2f;

    [SerializeField] GameObject SlimeShooter;
    [SerializeField] GameObject SlimeEnemy;
    [SerializeField] GameObject[] CeilingDebris;

    public int Health;
    public int Max_Health;
    public bool Vunerable = false;

    int RandNum;

    Color InitialColour;

    [SerializeField] GameObject Particle_Explosion;
    AudioSource AS;

    SpriteRenderer SR;
    // Start is called before the first frame update
    void Start()
    {
        Max_Health = Health;
        SR = GetComponent<SpriteRenderer>();
        InitialColour = SR.color;
        CharacterSize = transform.localScale;
        Player = GameObject.Find("Player");
        Target = Player.transform.position;
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        Anim.Play("AstronautBossWalking");

        for(int i=0;i<GameObject.Find("Ceiling").transform.childCount;i++)
        {
            CeilingDebris[i] = GameObject.Find("Ceiling").transform.GetChild(i).gameObject;
        }
        Cam = GameObject.Find("Main Camera");

        HealthBar.SetMaxHealth(Max_Health);
        HealthBar.SetHealth(Health);

        RandNum = Random.Range(0, AttackStates.Length);
        NextState = AttackStates[RandNum];
        PlayerMinDistance = AttackPlayerDistance[RandNum];
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector2.Distance(transform.position, Target));
        Target = Player.transform.position;
        if (isCharging)
        {
            transform.position += new Vector3(transform.localScale.x * Speed*2.5f * Time.deltaTime, 0, transform.position.z);
        }
        else if (Vector2.Distance(transform.position,Target) >= PlayerMinDistance && isWalking)
        {
            Facing();
            if (isWalking)
            {
                transform.position += new Vector3(transform.localScale.x * Speed * Time.deltaTime, 0, transform.position.z);
            }
        }
        else if (Vector2.Distance(transform.position, Target) < PlayerMinDistance && isWalking)
        {
            isWalking = false;

            Invoke(NextState, 0.5f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.tag == "Platform" || collision.transform.name == "Jump Pad") && isSmashing == true)
        {
            Cam.GetComponent<ScreenShakeController>().Shake("Medium");
            for (int i = 0; i < CeilingDebris.Length; i++)
            {
                CeilingDebris[i].GetComponent<CeilingDebrisController>().CreateDebris();
            }
            isSmashing = false;
            Vunerable = true;
            Invoke("GetUp", 4f);
        }
        if (collision.transform.name == "ChargePad" && isCharging)
        {
            Cam.GetComponent<ScreenShakeController>().Shake("Medium");
            for (int i = 0; i < CeilingDebris.Length; i++)
            {
                CeilingDebris[i].GetComponent<CeilingDebrisController>().CreateDebris();
            }
            isCharging = false;
            Anim.Play("AstronautBossInAir");
            Vunerable = true;
            Invoke("GetUp", 4f);
        }
        if (collision.transform.name == "Projectile" && GameObject.Find("Helmet") == null)
        {
            collision.gameObject.GetComponent<ProjectileController>().SpawnParticle();
            Hit();
        }
        if (collision.transform.name == "Player" && (isCharging || isSmashing))
        {
            Player.GetComponent<PlayerController>().OnDeath();
            Application.LoadLevel(Application.loadedLevel);
        }
    }
    public void Jump(float JumpSpeed, Vector2 Direction)
    {
        RB.velocity += Direction * JumpSpeed;
    }
    void Hit()
    {
        if (Vunerable)
        {
            Health--;
            HealthBar.SetHealth(Health);
            SR.color = Color.red;
            Invoke("ClearColour", 0.25f);
            if (Health <= 0)
            {
                OnDeath();
            }
        }
    }
    void ClearColour()
    {
        SR.color = Color.white;
    }
    void OnDeath()
    {
        Instantiate(Particle_Explosion, transform.position, transform.rotation);
        Destroy(HealthBarObject);
        Destroy(this.gameObject);
    }


    void Smash()
    {
        Jump(Speed * 2, Vector2.up);
        isSmashing = true;
        Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(),false);
        Anim.Play("AstronautBossInAir");
    }
    void SmashState()
    {
        Anim.Play("AstronautBossWalking");
        Smash();
    }
    void Charge()
    {
        isCharging = true;
        Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
    }
    void ChargeState()
    {
        Anim.Play("AstronautBossWalking");
        Charge();
    }
    void SpawnSlime()
    {
        GameObject BossSlime = Instantiate(SlimeEnemy, transform.position, Quaternion.Euler(0, 0, 0));
        BossSlime.transform.name = "Slime";
        BossSlime.GetComponent<EnemyController>().BossMade = true;
        Vunerable = true;
        Invoke("GetUp", 3f);
    }
    void SpawnSlimeState()
    {
        Anim.Play("AstronautBossIdle");
        Invoke("SpawnSlime", 2f);
    }
    void GetUp()
    {
        isWalking = true;
        isSmashing = false;
        isCharging = false;
        Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        Anim.Play("AstronautBossWalking");
        Facing();
        RandNum = Random.Range(0, AttackStates.Length);
        NextState = AttackStates[RandNum];
        PlayerMinDistance = AttackPlayerDistance[RandNum];
        Vunerable = false;
    }
    void Facing()
    {
        if (Target.x > transform.position.x)
        {
            transform.localScale = new Vector3(CharacterSize.x, CharacterSize.y, CharacterSize.z);
        }
        else if (Target.x < transform.position.x)
        {
            transform.localScale = new Vector3(-CharacterSize.x, CharacterSize.y, CharacterSize.z);
        }
    }
}
