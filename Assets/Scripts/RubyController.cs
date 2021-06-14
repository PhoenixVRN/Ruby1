using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigidbody2D;
   
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal")*speed;
        float vertical = Input.GetAxis("Vertical")*speed;
        Vector2 position = rigidbody2D.position;
        position.x = position.x + horizontal * Time.deltaTime;
        position.y = position.y + vertical * Time.deltaTime;
       rigidbody2D.MovePosition(position);

    }
}
