using NavMeshPlus.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : BaseState
{
    GameObject _owner;
    NavMeshAgent _agent;
    BoxCollider2D spawnArea;
    BoundPositions areaBounds;
    Vector2 pointDirection;
    Animator _animator;
    public RoamState(GameObject owner, NavMeshAgent agent, BoxCollider2D spawnArea, Animator animator = null)
    {
        _owner = owner;
        _agent = agent;
        this.spawnArea = spawnArea;
        areaBounds = new BoundPositions(this.spawnArea);
        _animator = animator;
    }

    public override void OnEnter()
    {
        Debug.Log("Entered roam state");
        _animator.SetBool("isMoving", true);
    }

    public override void OnExit()
    {
        _animator.SetBool("isMoving", false);
    }

    public override void StateUpdate()
    {
        if (_agent.enabled != true) return;

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (RandomPointWithinRectangle(spawnArea.transform.position, out Vector2 point))
            {
                Debug.DrawRay(point, Vector2.up, Color.blue, 1.0f);
                _agent.SetDestination(point);
            }
        }

        if (_agent.speed < Mathf.Epsilon)
            _animator.SetBool("isMoving", false);

        //if (agent.angularSpeed != 0)
            //FaceTarget();
    }

    void FaceTarget()
    {
        Vector3 velocity = _agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            // Face in the direction of movement
            _owner.transform.right = velocity.normalized;
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
            //float randomX = Random.Range(areaBounds.left, areaBounds.right);
            //float randomY = Random.Range(areaBounds.top, areaBounds.bottom);

            //Vector2 randomPoint = center + new Vector2(randomX, randomY);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector2.zero;
        return false;
    }

    private void RotateTowardsPoint(Vector3 point)
    {

        CalcPointDirection(point);
        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
        _owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void CalcPointDirection(Vector3 point)
    {
        pointDirection = (point - _owner.transform.position).normalized;
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

    public class BoundPositions{
        public Vector2 center;
        public readonly float left;
        public readonly float right;
        public readonly float top;
        public readonly float bottom;

        public BoundPositions(BoxCollider2D area){
            center = area.bounds.center;
            left = center.x - area.bounds.extents.x;
            right = center.x + area.bounds.extents.x;
            top = center.y + area.bounds.extents.y;
            bottom = center.y - area.bounds.extents.y;
        }
    }
