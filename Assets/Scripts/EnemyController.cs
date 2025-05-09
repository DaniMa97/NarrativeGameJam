using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    enum State { Patroling, Waiting, Chasing, Searching }

    [Tooltip("Points to which move the agent.")]
    public List<GameObject> patrolPoints = new List<GameObject>();

    [Tooltip("Amount of time waiting between patrol points.")]
    public float patrolTime;

    [Tooltip("Angle of vision of the enemy.")]
    public float angleVision = 30;

    [Tooltip("Distance vision of the enemy.")]
    public float distanceVision = 20;

    [Tooltip("Chase time after leaving enemy view")]
    public float searchTime = 2;

    public enum EnemyType { Guard, Monster}

    [Tooltip("Type of the enemy")]
    public EnemyType enemyType;

    State currentState = State.Patroling;
    Coroutine waitingRoutine = null;

    NavMeshAgent agent;
    int currentPoint = 0;
    bool forward = true;
    float currentSearchTime; 

    Transform playerPos;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.Find("Player").transform;

        if(patrolPoints.Count != 0)
        {
            agent.SetDestination(patrolPoints[0].transform.position);
        }
        currentSearchTime = searchTime;
    }

    private void Update()
    {
        if(currentState == State.Patroling)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    waitingRoutine = StartCoroutine(GoToNextPoint());
                }
            }
        }
        else if(currentState == State.Searching)
        {
            currentSearchTime -= Time.deltaTime;
            if(currentSearchTime <= 0)
            {
                agent.SetDestination(patrolPoints[currentPoint].transform.position);
                currentState = State.Patroling;
            }
        }
        if(IsPlayerDetected())
        {
            currentSearchTime = searchTime;
            if (waitingRoutine != null)
            {
                StopCoroutine(waitingRoutine);
                waitingRoutine = null;
            }
            int offset = 0;
            if (enemyType == EnemyType.Monster && !GameController.GetInstance().IsMaskOn())
            {
                offset = 10000;
            }
            else if (enemyType == EnemyType.Guard && GameController.GetInstance().IsMaskOn())
            {
                offset = -10000;
            }
            Vector3 playerRelativePos = playerPos.position;
            playerRelativePos.x += offset;
            agent.SetDestination(playerRelativePos);
            currentState = State.Chasing;
        }
        else if(currentState == State.Chasing)
        {
            currentState = State.Searching;
        }
    }

    bool IsPlayerDetected()
    {
        int offset = 0;
        if(enemyType == EnemyType.Monster && !GameController.GetInstance().IsMaskOn())
        {
            offset = -10000;
        }
        else if(enemyType == EnemyType.Guard && GameController.GetInstance().IsMaskOn())
        {
            offset = 10000;
        }
        Vector3 myPos = transform.position;
        myPos.x += offset;
        Vector3 playerDir = playerPos.position - myPos;
        float playerAngle = Vector3.Angle(transform.forward, playerDir);
        float playerDistance = Vector3.Distance(myPos, playerPos.position);

        if(playerAngle <= angleVision/2 && playerDistance <= distanceVision)
        {
            return true;
        }
        return false;
    }

    IEnumerator GoToNextPoint()
    {
        currentState = State.Waiting;
        yield return new WaitForSeconds(patrolTime);
        if(forward)
        {
            if(currentPoint + 1 >= patrolPoints.Count)
            {
                currentPoint--;
                forward = !forward;
            }
            else
            {
                currentPoint++;
            }
        }
        else
        {
            if(currentPoint - 1 < 0)
            {
                currentPoint++;
                forward = !forward;
            }
            else
            {
                currentPoint--;
            }
        }
        if(currentPoint >= 0 && currentPoint < patrolPoints.Count)
        {
            currentState = State.Patroling;
            agent.SetDestination(patrolPoints[currentPoint].transform.position);
        }
    }
}
