using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform followTarget;

    private float nextUpdateTime;
    private float updateInterval = 0.5f;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followTarget == null) {
            agent.isStopped = true;
            return;
        }
        if(Time.time > nextUpdateTime) {
            if(agent.isStopped) {
                agent.isStopped = false;
            }
            nextUpdateTime = Time.time + updateInterval;
            agent.SetDestination(followTarget.position);
        }
    }
}
