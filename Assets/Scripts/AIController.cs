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
    private float angle;

    public float detectDistance;
    public float detectAngle;

    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshPath = new NavMeshPath();
        lineRenderer = GetComponent<LineRenderer>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = null;

        foreach (GameObject player in players)
        {
            direction = player.transform.position - transform.position;
            angle = Vector3.Angle(transform.forward, direction);

            if (angle <= detectAngle && Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit) && detectDistance >= Vector3.Distance(transform.position, player.transform.position))
            {
                target = player;
                SetAITargetLocation(hit.transform.position);
            }
        }

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
}