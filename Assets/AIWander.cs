using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWander : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    public float timer, wanderTime;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetMouseButtonDown(2))  
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SetAITargetLocation(hit.point);
        }
    }
    timer += Time.deltaTime;

    Wander();
}


private void SetAITargetLocation(Vector3 targetLocation)
{
    _navMeshAgent.SetDestination(targetLocation);
}


private void Wander()
{
    if (timer >= wanderTime)
  {
    Vector2 wanderTarget = Random.insideUnitCircle * wanderTime;
    Vector3 wanderPos3D = new Vector3(x:transform.position.x + wanderTarget.x, transform.position.y,
    z:transform.position.z + wanderTarget.y);
    SetAITargetLocation(wanderPos3D);
    timer = 0;
  }
 }
}