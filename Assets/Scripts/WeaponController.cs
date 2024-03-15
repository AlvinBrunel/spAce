using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    GameObject Player;
    [SerializeField] GameObject BlasterBarObject;

    [SerializeField] float GunDistance;
    [SerializeField] float GunRotationSpeed;
    [SerializeField] float GunKickBackBlast;
    [SerializeField] float FiringRate = 1f;

    float TotalTicks = 25;
    float CurrentTick = 0;

    [SerializeField] GameObject Projectile;
    GameObject Cam;
    bool ReadyToFire = true;
    Vector3 Offset;

    [SerializeField] AudioSource AS;

    // Start is called before the first frame update
    void Start()
    {
        Player = transform.parent.gameObject;
        Offset = transform.position - Player.transform.position;
        Cursor.visible = false;

        Cam = GameObject.Find("Main Camera");

    }

    // Update is called once per frame
    void Update()
    {
        //rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (transform.parent.transform.localScale.x == 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        }
        else
        {
            if (transform.parent.transform.localScale.x == -1)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.localScale = new Vector3(-1, transform.localScale.y, 1);
            }
        }

        if (mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            Offset.x = -Offset.x;
        }
        else if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, 1);
            Offset.x = -Offset.x;
        }

        transform.position = Vector3.Lerp(transform.position,Player.transform.position + Offset + Vector3.ClampMagnitude(new Vector3(mousePos.x, mousePos.y, -1), GunDistance),Time.deltaTime* GunRotationSpeed);

        if(Input.GetMouseButtonDown(0))
        {
            if(ReadyToFire)
            {
                Shoot();
                ReadyToFire = false;
                CurrentTick = 0;
            }
        }
    }

    public void Shoot()
    {
        GameObject SpawnProjectile = Instantiate(Projectile, transform.position, transform.rotation);
        SpawnProjectile.name = "Projectile";
        //BlasterBar.SetPoint(CurrentTick);
        Invoke("Ticking", FiringRate / TotalTicks);
        Cam.GetComponent<ScreenShakeController>().Shake("VerySmall");
        Player.GetComponent<PlayerController>().Jump(GunKickBackBlast, ((Player.transform.position + Offset) - transform.position));
        transform.position += ((Player.transform.position + Offset) - transform.position)*GunKickBackBlast/10;
        AS.Play();
    }
    void Recharge()
    {
        ReadyToFire = true;
        CurrentTick = 0;
    }
    void Ticking()
    {
        CurrentTick++;
        BlasterBarObject.GetComponent<Image>().fillAmount = CurrentTick/TotalTicks;
        if (CurrentTick >= TotalTicks)
        {
            Recharge();
        }
        else
        {
            Invoke("Ticking", 0.05f);
        }
    }
}
