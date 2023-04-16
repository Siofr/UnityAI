using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class DFAiController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public float timer;
    public float wanderTime = 5;
    public GameObject[] players;
    public float viewDistance = 5;
    public bool followingPlayer;
    public GameObject viewRadius;

    // Start is called before the first frame update
    void Start()
    {
        viewRadius.transform.localScale = new Vector3(viewDistance, 0.01f, viewDistance);
        navMeshAgent = GetComponent<NavMeshAgent>();
        players = GameObject.FindGameObjectsWithTag("Player");
        Gizmos.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject Player in players)
        {
            if (Vector3.Distance(gameObject.transform.position, Player.transform.position) <= viewDistance / 2f)
            {
                followingPlayer = true;
                FollowPlayer(Player);
            }
            else
            {
                followingPlayer = false;
            }
        }

        Wander();
        if (!followingPlayer)
        {
            timer += Time.deltaTime;
        }
    }

    private void SetAITargetLocation(Vector3 targetLocation)
    {
        navMeshAgent.SetDestination(targetLocation);
    }

    private void Wander()
    {
        if (timer >= wanderTime)
        {
            Vector2 wanderTarget = (Random.insideUnitCircle) * wanderTime;
            Vector3 wanderPos3D = new Vector3(transform.position.x + wanderTarget.x, transform.position.y, transform.position.z + wanderTarget.y);
            SetAITargetLocation(wanderPos3D);
            timer = 0;
        }
    }

    private void FollowPlayer(GameObject player)
    {
        SetAITargetLocation(player.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, viewDistance / 2f);
    }
}
