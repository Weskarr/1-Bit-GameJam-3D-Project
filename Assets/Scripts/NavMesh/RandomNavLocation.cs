using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RandomNavLocation : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent myNavAgent;

    [SerializeField]
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {

    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        myNavAgent.SetDestination(target.transform.position);
        //myNavAgent.SetDestination(RandomNavmeshLocation(20f));
    }
}
