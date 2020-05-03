using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rigidBody;
    Vector2 movementVector;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame (don't put anything physics related here)
    void Update()
    {
        // Input

        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");


    }

    // Like Update() but executed on fixed timer (independent of framerate)
    void FixedUpdate()
    {
        // Movement
        rigidBody.MovePosition(rigidBody.position + movementVector * moveSpeed * Time.fixedDeltaTime);

    }
}
