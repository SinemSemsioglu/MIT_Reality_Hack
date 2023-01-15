using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfinderController : MonoBehaviour
{
    NavMeshAgent agent;

    Transform destination;

    [Header("Stats")]
    [SerializeField] float speed;
    [SerializeField] float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        destination = GameObject.Find("Pathfinder Target").gameObject.transform;

        Debug.Log("Destination is" + destination);

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.position);
        agent.speed = speed;

        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}
