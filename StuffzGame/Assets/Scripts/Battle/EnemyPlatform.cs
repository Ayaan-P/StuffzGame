using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatform : MonoBehaviour
{
    public Transform pos1,pos2;
    public float speed;
    public Transform startpos;

    public Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = startpos.position;
    }

    // Update is called once per frame
    void Update()
    {
        nextPos= pos2.position;
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed*Time.deltaTime);
    }
}
