using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutController : MonoBehaviour
{
    ParticleSystem PS;
    [SerializeField] AudioSource AS;
    [SerializeField] float Delay;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
        Player = GameObject.Find("Player");
        Invoke("ShootSprout", Delay);
    }

    // Update is called once per frame
    void Update()
    {
        AS.volume = ASVolume(Vector2.Distance(transform.position, Player.transform.position));
    }
    void ShootSprout()
    {
        PS.Play();
        AS.Play();
        Invoke("ShootSprout", 2f);
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
            return  1 - ((dist - MinDist) / (MaxDist - MinDist));
        }
    }
}
