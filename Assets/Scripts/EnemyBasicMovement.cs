using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMovement : MonoBehaviour
{
    float Speed;
    [SerializeField] float JumpForce;
    Rigidbody2D RB;
    public float Distance;
    Vector2 InitialPoint;
    Vector2 Direction;
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Speed = GetComponent<EnemyController>().Speed;

        Direction = Vector2.right;
        InitialPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Direction.x * Speed * Time.deltaTime, 0, transform.position.z);
        if(Direction == Vector2.right && transform.position.x > InitialPoint.x + Distance)
        {
            ChangeDirection();
        }
        else if(Direction == Vector2.left && transform.position.x < InitialPoint.x)
        {
            ChangeDirection();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Platform")
        {
            Jump(JumpForce,Vector2.up);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        }
    }
    public void Jump(float JumpSpeed, Vector2 Direction)
    {
        RB.velocity += Direction * JumpSpeed;
    }
    void ChangeDirection()
    {
        if(Direction == Vector2.right)
        {
            Direction = Vector2.left;
        }
        else if(Direction == Vector2.left)
        {
            Direction = Vector2.right;
        }
    }
}
