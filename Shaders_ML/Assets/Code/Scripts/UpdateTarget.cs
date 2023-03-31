using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateTarget : MonoBehaviour
{
    public Transform targetPosition;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = targetPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.destination != targetPosition.position)
            agent.destination = targetPosition.position;
    }
}
