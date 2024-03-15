using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject Player;

    [SerializeField] Vector3 Offset;

    [SerializeField] float CameraToMouseDistance;
    [SerializeField] float CameraSpeed;
    public float CameraSize;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        transform.position = Vector3.Lerp(transform.position, Player.transform.position + Offset + Vector3.ClampMagnitude(new Vector3(mousePos.x, mousePos.y, -1), CameraToMouseDistance), Time.deltaTime * CameraSpeed);
    }
}
