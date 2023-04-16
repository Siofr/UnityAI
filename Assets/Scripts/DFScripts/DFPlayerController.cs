using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DFPlayerController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject clickMarker;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        lineRenderer= GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SetAITargetLocation(hit.point);
            }
        }

        if (Vector3.Distance(navMeshAgent.destination, transform.position) <= 1)
        {
            Debug.Log("Click marker disabled");
            clickMarker.SetActive(false);
        }
        else if (navMeshAgent.hasPath)
        {
            DrawPath();
        }
    }

    private void SetAITargetLocation(Vector3 targetLocation)
    {
        clickMarker.transform.position = new Vector3(targetLocation.x, 0.08f, targetLocation.z);
        Debug.Log("Click marker enabled");
        clickMarker.SetActive(true);
        navMeshAgent.SetDestination(targetLocation);
    }

    private void DrawPath()
    {
        lineRenderer.positionCount = navMeshAgent.path.corners.Length;
        lineRenderer.SetPosition(0, transform.position);

        if (navMeshAgent.path.corners.Length < 2)
        {
            return;
        }

        for (int i = 1; i < navMeshAgent.path.corners.Length; i++)
        {
            Vector3 pointPosition = new Vector3(navMeshAgent.path.corners[i].x, navMeshAgent.path.corners[i].y, navMeshAgent.path.corners[i].z);
            lineRenderer.SetPosition(i, pointPosition);
        }
    }
}
