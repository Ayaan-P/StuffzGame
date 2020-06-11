using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 2f;
    public GameObject player;
    private void Update()
    {
        if (Input.GetAxisRaw("Submit") >= 0)
        {
            var heading = transform.position - player.GetComponent<Transform>().position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            var lookingInDirection = player.GetComponent<PlayerMovement>().lookingInDirection;

            if (distance <= radius && Input.GetAxisRaw("Submit") > 0)
            {
                if ((Mathf.Round(direction.x) == lookingInDirection.x) || (Mathf.Round(direction.y) == lookingInDirection.y))
                {
                    Debug.Log("You found a Blazikenite");
                    Destroy(gameObject);
                }
            }
        }
    }
}