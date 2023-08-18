using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TheFirstPerson;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;




enum State {
    wander,
    seek
}

public interface IAIState {

    void EnterState();
    void UpdateState();
    void ExitState();
    void ForceExitState();
}

public class WanderState : IAIState {
    Vector3 target;

    float radius;
    float moveTime;
    Transform transform;
    NavMeshAgent agent;
    public WanderState(float radius, float moveTime, Transform transform, NavMeshAgent agent) {
        this.radius = radius;
        this.moveTime = moveTime;
        this.transform = transform;
        this.agent = agent;
    }

    public Vector3 RandomNavmeshLocation(float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void EnterState() {
        lastTime = 0;
        moving = true;
    }

    public void ExitState() {
        return;
    }

    public void ForceExitState() {
        return;
    }

    float lastTime;
    bool moving;
    public void UpdateState() {
        if (Time.time > lastTime + moveTime) {
            lastTime = Time.time;
            moving = !moving;
            if (moving) {
                target = RandomNavmeshLocation(radius);
            } else {
                target = transform.position;
            }
            agent.SetDestination(target);
        }
    }
}

public class SeekState : IAIState {

    public static event System.Action exit;

    float moveTime;
    NavMeshAgent agent;
    Transform player;
    public SeekState(float moveTime, NavMeshAgent agent, Transform player) {
        this.moveTime = moveTime;
        this.agent = agent;
        this.player = player;
    }

    public void EnterState() {
        agent.SetDestination(player.position);
        lastTime = Time.time;
    }

    public void ExitState() {
        exit?.Invoke();
    }

    public void ForceExitState() {
        return;
    }

    float lastTime;
    public void UpdateState() {
        if (Time.time > lastTime + moveTime || EntityAI.NavAgentMoving(agent)) {
            ExitState();
        }
    }
}

public class EntityAI : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    private NavMeshAgent myNavAgent;

    [SerializeField]
    private GameObject player;

    public float randomRadius;
    public float attackRadius;

    public float moveTime;

    State state = State.wander;
    IAIState[] states;


    void Start()
    {
        animator = GetComponent<Animator>();
        List<IAIState> s = new List<IAIState> {
            new WanderState(randomRadius, moveTime, transform, myNavAgent),
            new SeekState(moveTime, myNavAgent, player.transform)
        };
        states = s.ToArray();
        SeekState.exit += ExitSeekState;
    }

    void ChangeState(State newState) {
        Debug.Log("Changing from state " + state + " to " + newState);
        states[(int)state].ForceExitState();
        state = newState;
        states[(int)state].EnterState();
    }

    private void Update() {
        switch (state) {
            case State.wander:
                if (player.GetComponent<FPSController>().moving && Vector3.Distance(transform.position, player.transform.position) < attackRadius) {
                    ChangeState(State.seek);
                }
                break;
            case State.seek:
                break;
        }
        states[(int)state].UpdateState();
    }

    void ExitSeekState() {
        ChangeState(State.wander);
    }

    public static bool NavAgentMoving(NavMeshAgent agent) {
        return agent.velocity.magnitude < 0.1f;
    }
}
