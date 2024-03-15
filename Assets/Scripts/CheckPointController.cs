using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float CameraSizeMultiplier;

    GameObject camera;
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().CheckPoint = this.gameObject;
            camera.GetComponent<Camera>().orthographicSize = camera.GetComponent<CameraController>().CameraSize * CameraSizeMultiplier;
        }
    }
}
