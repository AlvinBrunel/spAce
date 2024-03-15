using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautHelmetController : MonoBehaviour
{
    public int Health;
    public int Max_Health;

    SpriteRenderer SR;
    [SerializeField] Sprite[] HelmetSprites;
    [SerializeField] GameObject HealthBarObject;
    GameObject Boss;
    public HealthBarController HealthBar;
    Vector3 Offset;
    // Start is called before the first frame update
    void Start()
    {
        Boss = transform.parent.gameObject;
        Max_Health = Health;
        SR = GetComponent<SpriteRenderer>();

        Offset = transform.position - transform.parent.position;

        HealthBar.SetMaxHealth(Max_Health);
        HealthBar.SetHealth(Health);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y,-1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Projectile")
        {
            Hit(collision.transform.GetComponent<ProjectileController>().Damage,collision.gameObject);
        }
    }
    public void Hit(int Damage, GameObject collision)
    {
        collision.GetComponent<ProjectileController>().SpawnParticle();
        if (Boss.GetComponent<EnemyAstronautController>().Vunerable)
        {
            SR.color = Color.red;

            Health -= Damage;
            HealthBar.SetHealth(Health);
            if (Health <= 0)
            {
                DestroyHelmet();
            }
            float HealthPercentage = (float)Health / (float)Max_Health;
            Invoke("ClearHit", 0.2f);
            Debug.Log((int)Mathf.Round((Health / Max_Health) * HelmetSprites.Length) - 1);
            SR.sprite = HelmetSprites[(int)Mathf.Round(HealthPercentage * HelmetSprites.Length) - 1];
        }
    }
    private void ClearHit()
    {
        SR.color = Color.white;
    }
    private void DestroyHelmet()
    {
        Boss.GetComponent<EnemyAstronautController>().Speed *= 1.25f;
        Destroy(HealthBarObject);
        Destroy(gameObject);
    }
}
