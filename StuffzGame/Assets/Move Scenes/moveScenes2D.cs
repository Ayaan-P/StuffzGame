using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moveScenes2D : MonoBehaviour
{
    // Start is called before the first frame update
	[SerializeField] private string insidePlace;
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("GameObject1 collided with " + other.name);
		if(other.CompareTag("Player"))
		{
			SceneManager.LoadScene(insidePlace);
		}
	}
}
