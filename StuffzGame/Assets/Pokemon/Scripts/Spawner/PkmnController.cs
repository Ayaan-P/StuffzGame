using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using System.Linq;

public class PkmnController: MonoBehaviour {
		
		public GameObject player;
		public Animator animator;
		public GameObject encounter_data;
		public string pokemon_name;
		public float radius = 3f;
		public float attack_radius = 1f;
		public float escape_radius = 3f;
		public Pokemon wild_pokemon {get; set;}

		void Update ()
		{
			int speedStat = wild_pokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.SPEED).SingleOrDefault().BaseValue;
			float MAX_BASE_SPEED = 160;
			int MAX_MOVEMENT_SPEED = 5;
			gameObject.GetComponent<AIPath>().maxSpeed = speedStat/MAX_BASE_SPEED * MAX_MOVEMENT_SPEED;
			animator.speed = speedStat / MAX_BASE_SPEED;
			var target = player.transform;
			float distance = Vector2.Distance(target.position,transform.position);
			if(distance<=radius)
			{
				gameObject.GetComponent<AIDestinationSetter>().target=target;
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
				if(distance<=attack_radius)
				{

					//GameObject Encounter_Data = GameObject.Find("CurrentEncounter(Clone)");;
					
					// Encounter_Data.GetComponent<EncounterData>().pokemon_name = gameObject.GetComponent<SpriteSwap>().pokemon_name;
					encounter_data.GetComponent<EncounterData>().current_enemy = wild_pokemon;
					encounter_data.GetComponent<EncounterData>().pokemon_name = wild_pokemon.BasePokemon.Name;
					Debug.Log(encounter_data.GetComponent<EncounterData>().current_enemy.BasePokemon.Name );
					Debug.Log(pokemon_name );
					SceneManager.LoadScene(1);
				}
			}
			else if(distance>escape_radius)
			{
				gameObject.GetComponent<AIDestinationSetter>().target=null;
			}
		}
		
					


	}

