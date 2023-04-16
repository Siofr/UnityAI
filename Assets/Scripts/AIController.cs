using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private NavMeshPath _navMeshPath;
    private LineRenderer lineRenderer;
    private Vector3 direction;
    private bool hasTarget = false;
    private int currentPoint;
    private float angle;

    public float detectDistance;
    public float detectAngle;

    public GameObject[] waypoints;
    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshPath = new NavMeshPath();
        lineRenderer = GetComponent<LineRenderer>();

        // Create a list of all gameobjects tagged player
        players = GameObject.FindGameObjectsWithTag("Player");

        // Run patrol at start
        Patrol();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();

        if (!hasTarget && !_navMeshAgent.hasPath)
        {
            Patrol();
        }

        // If the AI has a path create the line renderer else disable it
        if (_navMeshAgent.hasPath)
        {
            CreateAIPath();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void SetAITargetLocation(Vector3 targetLocation)
    {
        _navMeshAgent.SetDestination(targetLocation);
    }

    // Creates the path in game view from the nav mesh path using a line renderer
    private void CreateAIPath()
    {
        lineRenderer.enabled = true;
        NavMesh.CalculatePath(transform.position, _navMeshAgent.destination, NavMesh.AllAreas, _navMeshPath);
        lineRenderer.positionCount = _navMeshPath.corners.Length;
        lineRenderer.SetPositions(_navMeshPath.corners);
    }

    private void ChasePlayer()
    {
        GameObject target = null;

        foreach (GameObject player in players)
        {
            direction = player.transform.position - transform.position;
            angle = Vector3.Angle(transform.forward, direction);

            // If player is in fov, send out a raycast to see if they are obstructed by an object, and if they are within the view distance, set the player as a target
            if (angle <= detectAngle && Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit) && detectDistance >= Vector3.Distance(transform.position, player.transform.position))
            {
                target = hit.transform.gameObject;
            }

            if (target != null)
            {
                SetAITargetLocation(target.transform.position);
                hasTarget = true;
            } 
            else
            {
                hasTarget = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Kill(other.gameObject));
        }
    }

    // Kill the object, wait 3 seconds, and then respawn it
    IEnumerator Kill(GameObject target)
    {
        target.SetActive(false);
        yield return new WaitForSeconds(3);
        target.SetActive(true);
    }

    private void Patrol()
    {
        // Current point = current destination, if destination is reach increment current point by 1, use % waypoint.Length so it stays within the list range
        SetAITargetLocation(waypoints[currentPoint].transform.position);
        currentPoint = (currentPoint + 1) % waypoints.Length;
    }
}