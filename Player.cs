using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Add this script to Player GameObject
    //Player should have:
    //BoxCollider2D
    //Rigidbody2D (Frezee rotation z)

    Rigidbody2D rb;

    BoxCollider2D boxCollider;

    bool blockedDown = false;
    bool blockedUp = false;
    bool blockedLeft = false;
    bool blockedRight = false;

    float moveX = 0;
    float moveY = 0;

    float speed = 4;

    float jumpForce = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        CheckCollisions();

        Move();

    }

    public void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal") * speed;

        moveY = rb.velocity.y;

        if (blockedDown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveY = jumpForce;
            }
        }

        Vector3 move = new Vector3(moveX, moveY, 0);

        rb.velocity = move;

    }
    
    private void CheckCollisions()
    {

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
        blockedDown = false;
        blockedUp = false;
        blockedLeft = false;
        blockedRight = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.tag != "platform")
                continue;

            bool collideFromLeft = false;
            bool collideFromTop = false;
            bool collideFromRight = false;
            bool collideFromBottom = false;

            float RectWidth = hit.bounds.size.x;
            float RectHeight = hit.bounds.size.y;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
            }
            Vector2 contactPoint = colliderDistance.pointA;
            Vector2 center = hit.bounds.center;

            if (contactPoint.y > center.y && //checks that circle is on top of rectangle
            (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
            {
                collideFromTop = true;
            }
            else if (contactPoint.y < center.y &&
            (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
            {
                collideFromBottom = true;
            }
            else if (contactPoint.x > center.x &&
            (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2))
            {
                collideFromRight = true;
            }
            else if (contactPoint.x < center.x &&
            (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2))
            {
                collideFromLeft = true;
            }

            if (collideFromTop)
            {
                blockedDown = true;
            }
            if (collideFromBottom)
            {
                blockedUp = true;
            }
            if (collideFromRight)
            {
                blockedLeft = true;
            }
            if (collideFromLeft)
            {
                blockedRight = true;
            }
        }
    }
}
