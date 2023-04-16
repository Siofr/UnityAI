using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DFPlayerController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject clickMarker;
    private LineRenderer lineRenderer;
    public int mouseClick = 1;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        lineRenderer= GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(mouseClick))
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SetAITargetLocation(hit.point);
            }
        }

        if (Vector3.Distance(navMeshAgent.destination, transform.position) <= 1)
        {
            lineRenderer.enabled = false;
            clickMarker.SetActive(false);
            animator.SetBool("moving", false);
        }
        else if (navMeshAgent.hasPath)
        {
            DrawPath();
            animator.SetBool("moving", true);
        }
    }

    private void SetAITargetLocation(Vector3 targetLocation)
    {
        clickMarker.transform.position = new Vector3(targetLocation.x, 0.08f, targetLocation.z);
        clickMarker.SetActive(true);
        navMeshAgent.SetDestination(targetLocation);
    }

    private void DrawPath()
    {
        lineRenderer.enabled = true;
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
