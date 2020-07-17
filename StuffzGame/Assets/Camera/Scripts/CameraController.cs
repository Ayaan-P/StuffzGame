using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        GameObject player = Player.Instance.gameObject;
        if(player == null)
        {
            Debug.LogError($"{typeof(CameraController)}: Player is null");
        }
        else
        {
            target = player.transform;
        }

    }
    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        }
    }
}