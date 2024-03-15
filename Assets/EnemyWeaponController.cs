using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    GameObject Player;
    GameObject Enemy;
    string State = "Walking";
    string[] States = { "Idle", "Walking" };
     
    Vector3 Offset;

    [SerializeField] GameObject Projectile;
    float[] ShootForwardDirections = { 0, 180 };

    float ShootingCurve = 45f;
    float GunRotation;

    [SerializeField] float GunDistance;
    [SerializeField] float GunRotationSpeed;
    [SerializeField] float GunKickBackBlast;
    [SerializeField] float FiringRate = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Enemy = transform.parent.gameObject;
        Player = GameObject.Find("Player");
        Offset = transform.position - Player.transform.position;

        //Cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            ShootForward();
        }
    }

    void RainShots()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,0,ShootingCurve),GunRotation*Time.deltaTime);
    }
    public void ShootForward()
    {
        if (Player.transform.position.x > transform.position.x)
        {
            //transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            GameObject SpawnProjectile = Instantiate(Projectile, transform.position, Quaternion.Euler(0,0,0));

            Enemy.GetComponent<EnemyAstronautController>().Jump(GunKickBackBlast, new Vector3(-transform.parent.transform.localScale.x, 0.25f, 0));
            //transform.position += ((Player.transform.position + Offset) - transform.position) * GunKickBackBlast / 10;
        }
        else if (Player.transform.position.x < transform.position.x)
        {
            //transform.localScale = new Vector3(transform.localScale.x, -1, 1);
            GameObject SpawnProjectile = Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, 180));

            Enemy.GetComponent<EnemyAstronautController>().Jump(GunKickBackBlast, new Vector3(-transform.parent.transform.localScale.x, 0.25f, 0));
            //transform.position += ((Player.transform.position + Offset) - transform.position) * GunKickBackBlast / 10;
        }
    }
}
