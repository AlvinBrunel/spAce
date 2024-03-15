using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    GameObject Camera;
    public float speed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.Find("Main Camera");
        if (transform.name == "Explosion")
        {
            Camera.GetComponent<ScreenShakeController>().Shake("Big");
        }
        else
        {
            Camera.GetComponent<ScreenShakeController>().Shake("Small");
        }
        Invoke("DestroyParticle", 1.25f);
    }
    void DestroyParticle()
    {
        Destroy(this.gameObject);
    }
}