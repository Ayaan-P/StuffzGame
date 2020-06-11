using UnityEngine;

public class EnemyPlatform : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startpos;

    public Vector3 nextPos;

    // Start is called before the first frame update
    private void Start()
    {
        nextPos = startpos.position;
    }

    // FixedUpdate() is called on a fixed timer
    private void FixedUpdate()
    {
        nextPos = pos2.position;
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }
}