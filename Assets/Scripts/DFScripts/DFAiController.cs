using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject Player in players)
        {
            if (Vector3.Distance(gameObject.transform.position, Player.transform.position) <= viewDistance)
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
        if (followingPlayer == false)
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
            Vector2 wanderTarget = Random.insideUnitCircle * wanderTime;
            Vector3 wanderPos3D = new Vector3(transform.position.x + wanderTarget.x, transform.position.y, transform.position.z + wanderTarget.y);
            SetAITargetLocation(wanderPos3D);
            timer = 0;
        }
    }

    private void FollowPlayer(GameObject player)
    {
        SetAITargetLocation(player.transform.position);
    }
}
