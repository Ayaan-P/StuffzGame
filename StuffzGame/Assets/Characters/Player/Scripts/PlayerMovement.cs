using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int currentAxis = 0;
    public Rigidbody2D rigidBody;
    public Animator animator;
    private Vector2 movementVector;
    public Vector2 lookingInDirection;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame (don't put anything physics related here)
    private void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");
        if (movementVector != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movementVector.x);
            animator.SetFloat("Vertical", movementVector.y);
        }
        animator.SetFloat("Speed", movementVector.sqrMagnitude);

        int lookAxis = (Mathf.Abs(movementVector.x) > Mathf.Abs(movementVector.y)) ? 0 : 1;

        if (lookAxis == 1)
        {
            if (movementVector.y >= 0)
                lookingInDirection.y = 1f;
            else
                lookingInDirection.y = -1f;
        }
        else
        {
            if (movementVector.x >= 0)
                lookingInDirection.x = 1f;
            else
                lookingInDirection.x = -1f;
        }
    }

    // Like Update() but executed on fixed timer (independent of framerate)
    private void FixedUpdate()
    {
        // Movement
        rigidBody.MovePosition(rigidBody.position + movementVector * moveSpeed * Time.fixedDeltaTime);
    }
}