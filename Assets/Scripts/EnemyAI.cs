using UnityEngine;
using UnityEngine.AI;

public class ScaryMonster : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public float fieldOfView = 60f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (IsPlayerLooking())
        {
            // FREEZE
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            // MOVE
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

bool IsPlayerLooking()
{
    Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
    
    // Create a margin. 
    // 0.1 means the monster must be 10% away from the edge to be "seen"
    float margin = 0.1f; 

    bool onScreen = screenPoint.z > 0 && 
                    screenPoint.x > -margin && screenPoint.x < (1 + margin) && 
                    screenPoint.y > -margin && screenPoint.y < (1 + margin);

    if (onScreen)
    {
        // 3. Now check for walls
        RaycastHit hit;
        // Shoot ray from CAMERA EYES to MONSTER CENTER
        Vector3 eyePos = Camera.main.transform.position;
        if (Physics.Linecast(eyePos, transform.position, out hit))
        {
            if (hit.transform == transform)
            {
                return true; // Monster is on screen AND not behind a wall
            }
        }
    }
    return false;
}
}