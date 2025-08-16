using UnityEngine;
using UnityEngine.AI;

public class RoamState : BaseState
{
    BoxCollider2D spawnArea;
    BoundPositions areaBounds;
    Vector2 pointDirection;
    public RoamState(GameObject enemy, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, BoxCollider2D spawnArea) : base(enemy, player, rb2D, agent){
        this.spawnArea = spawnArea;
        areaBounds = new BoundPositions(this.spawnArea);
     }

    public override void OnEnter()
    {
        //no op
    }

    public override void OnExit()
    {
        //no op
    }

    public override void StateUpdate()
    {
        if(!agent.enabled) return;
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            if (RandomPointWithinRectangle(spawnArea.transform.position, out Vector2 point)) 
            {
                Debug.DrawRay(point, Vector2.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
            RotateTowardsPoint(point);
            //InstantTurn();
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
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector2.zero;
        return false;
    }

    private void RotateTowardsPoint(Vector3 point){
        
        CalcPointDirection(point);
        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
        enemy.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void CalcPointDirection(Vector3 point){
        pointDirection = (point - enemy.transform.position).normalized;
    }

    bool RandomPointWithinRectangle(Vector2 center, out Vector2 result){

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

    void InstantTurn(){
        //When on target -> dont rotate!
    if ((agent.destination - enemy.transform.position).magnitude < 0.1f) return; 
    
    Vector3 direction = (agent.destination - enemy.transform.position).normalized;
    Debug.Log(direction);
    Quaternion  qDir= Quaternion.LookRotation(direction);
    enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, qDir, Time.deltaTime * 5f);
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

}
