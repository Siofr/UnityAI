using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                SetAITargetLocation(hit.point);
            }
            
        }
    }

    private void SetAITargetLocation(Vector3 targetLocation)
    {
        _navMeshAgent.SetDestination(targetLocation);
    }
}
