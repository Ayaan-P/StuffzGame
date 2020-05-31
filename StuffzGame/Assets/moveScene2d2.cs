using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moveScene2d2 : MonoBehaviour
{
    // Start is called before the first frame update
	[SerializeField] private string outsidePlace;
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("GameObject1 collided with " + other.name);
		if(other.CompareTag("Player"))
		{
			SceneManager.LoadScene(outsidePlace);
		}
	}
}
