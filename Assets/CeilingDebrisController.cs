using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingDebrisController : MonoBehaviour
{
    ParticleSystem PS;
    [SerializeField] bool Loop;
    [SerializeField] float Delay;
    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<ParticleSystem>();

        if(Loop == true)
        {
            InvokeRepeating("CreateDebris", Delay,1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateDebris()
    {
        PS.Play();
    }
}
