using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		public Animator animator;
		public GameObject d;
		public string pokemon_n;
		public float radius = 3f;
		public float attackradius = 1f;
		IAstarAI ai;
		void Start()
		{
			target = GameObject.FindWithTag("Player").transform;
		}
		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update () {

			float distance = Vector2.Distance(target.position,transform.position);
				if(distance<=attackradius)
				{
					GameObject Encounter_Data = Instantiate(d);
					DontDestroyOnLoad(Encounter_Data);
					// Encounter_Data.GetComponent<EncounterData>().pokemon_name = gameObject.GetComponent<SpriteSwap>().pokemon_name;
					 Encounter_Data.GetComponent<EncounterData>().pokemon_name = pokemon_n;
					SceneManager.LoadScene(1);
				}
				if (target != null && ai != null && distance<=radius) 
				{
					ai.destination = target.position;
					int lookAxis;
					if(Mathf.Abs(target.position.x-transform.position.x)>Mathf.Abs(target.position.y-transform.position.y))
						lookAxis = 0;
					else
						lookAxis = 1;
					if(lookAxis==1)
					{
						if(target.position.y>transform.position.y)
							animator.SetInteger("Direction", 1);
						else
							animator.SetInteger("Direction", 0);
					}
					else
					{
						if(target.position.x>transform.position.x)
							animator.SetInteger("Direction", 3);
						else
							animator.SetInteger("Direction", 4);
					}
					

				}

			
			
		}
	}
}
