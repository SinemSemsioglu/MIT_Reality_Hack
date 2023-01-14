using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfinderController : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] Vector3 destination;

    [Header("Stats")]
    [SerializeField] float speed;
    [SerializeField] float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination);
        agent.speed = speed;

        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}
