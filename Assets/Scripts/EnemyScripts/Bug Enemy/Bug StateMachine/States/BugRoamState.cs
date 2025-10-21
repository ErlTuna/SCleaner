using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BugRoamState : BaseState
{
    BoxCollider2D spawnArea;
    BoundPositions areaBounds;

    public BugRoamState(GameObject owner, Rigidbody2D rb2D, NavMeshAgent agent, BoxCollider2D spawnArea) : base(owner, rb2D, agent)
    {
        this.spawnArea = spawnArea;
        areaBounds = new BoundPositions(this.spawnArea);
    }

    public override void OnEnter()
    {
        Debug.Log("Entered roam state");
    }

    public override void OnExit()
    {
        //no op
    }

    public override void StateUpdate()
    {
        if (!agent.enabled) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (RandomPointWithinRectangle(spawnArea.transform.position, out Vector2 newPoint))
            {
                Debug.DrawRay(newPoint, Vector2.up, Color.blue, 1.0f);
                agent.SetDestination(newPoint);
            }

        }

        if (agent.hasPath)
        {
            RotateTowardsPoint(agent.steeringTarget);
        }
            
    }

    void FaceTarget()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            // Face in the direction of movement
            owner.transform.right = velocity.normalized;
        }
    }



    public override void StateFixedUpdate()
    {
        //no op
    }

    bool RandomPoint(Vector2 center, float range, out Vector2 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 randomPoint = center + Random.insideUnitCircle * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector2.zero;
        return false;
    }

    void RotateTowardsPoint(Vector3 point)
    {

        Vector2 pointDirection = (point - owner.transform.position).normalized;
        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg - 90f;
        owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    bool RandomPointWithinRectangle(Vector2 center, out Vector2 result)
    {

        float randomX = Random.Range(areaBounds.left, areaBounds.right);
        float randomY = Random.Range(areaBounds.top, areaBounds.bottom);

        //Vector2 randomPoint = center + new Vector2(randomX, randomY);
        Vector2 randomPoint = new Vector2(randomX, randomY);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            //Debug.Log(hit.position);
            return true;
        }


        result = Vector2.zero;
        return false;
    }
}
