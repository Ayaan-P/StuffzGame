using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
public class PokemonController : MonoBehaviour
{
    private Animator animator;
    public float radius = 3f;
    public int maxMovementSpeed = 5;
    private Pokemon wildPokemon;
    private GameObject playerGameObject;
    private bool isPatrolSet;
    private Patrol patrol;
    private AIPath aiPath;
    private const float MAX_POKEMON_BASE_SPEED = 160f;

    private void Start()
    {
        this.animator = this.GetComponent<Animator>();
        isPatrolSet = false;
        patrol = this.GetComponent<Patrol>();
        aiPath = this.GetComponent<AIPath>();
    }

    private void Update()
    {
        SetAnimatorSpriteDirection();

        if (playerGameObject != null)
        {
            var target = playerGameObject.transform;
            float distance = Vector2.Distance(target.position, this.transform.position);
            if (distance <= radius)
            {
                isPatrolSet = false;
                patrol.targets = new Vector3[] { target.position };
                patrol.delay = 0;
               
            }
            else
            {
                if (!isPatrolSet || patrol.targets.Length == 1)
                {
                    SetPatrol();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.Equals(playerGameObject))
        {
            EncounterData encounterData = EncounterData.Instance;
            if(encounterData != null)
            {
                encounterData.SetWildPokemonEncounter(wildPokemon);
                SceneLoader sceneLoader = new SceneLoader();
                sceneLoader.LoadBattle();
            }
            else
            {
                Debug.LogError($"{typeof(EncounterData)} is null");
            }
        }
    }

    public void InitWildPokemonData(GameObject playerGameObject, Pokemon wildPokemon)
    {
        this.playerGameObject = playerGameObject;
        this.wildPokemon = wildPokemon;
        int speedStat = this.wildPokemon.GetStat(StatName.SPEED).BaseValue;
        this.GetComponent<AIPath>().maxSpeed = speedStat / MAX_POKEMON_BASE_SPEED * maxMovementSpeed;
        this.GetComponent<Rigidbody2D>().mass = this.wildPokemon.BasePokemon.Weight;
    }

    private void SetAnimatorSpriteDirection()
    {

        Vector3 velocity = aiPath.velocity;

        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y) && Mathf.Abs(velocity.x) > float.Epsilon && Mathf.Abs(velocity.y) > float.Epsilon)
        {
            if (velocity.x > 0f)
            {
                animator.SetInteger("Direction", 3);
            }
            else
            {
                animator.SetInteger("Direction", 4);
            }
        }
        else
        {
            if (velocity.y > 0f)
            {
                animator.SetInteger("Direction", 1);
            }
            else
            {
                animator.SetInteger("Direction", 0);
            }
        }
    }

    private void SetPatrol()
    {
        const int numPatrolPoints = 5;
        const float patrolDistance = 6f;
        const float patrolDelay = 2f;
        Vector3[] randomPositions = new Vector3[numPatrolPoints];
        for (int i = 0; i < numPatrolPoints; i++)
        {
            float randX = Random.value * patrolDistance;
            float randY = Random.value * patrolDistance;
            Vector3 delta = randX > randY ? new Vector3(randX, 0) : new Vector3(0, randY);
            Vector3 newPosition = this.transform.position + delta;
            randomPositions[i] = newPosition;
        }
        patrol.targets = randomPositions;
        patrol.delay = patrolDelay;

        this.isPatrolSet = true;
    }
}