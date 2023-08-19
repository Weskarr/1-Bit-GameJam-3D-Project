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
    seek,
    chase
}

public interface IAIState {

    void EnterState();
    void UpdateState(float deltaTime);
    void ExitState();
    void ForceExitState();
}

public class WanderState : IAIState {
    Vector3 target;

    float radius;
    float moveTime;
    Transform transform;
    NavMeshAgent agent;
    float footstepFrequency;
    float idleFrequency;
    EntitySound soundPlayer;
    public WanderState(float radius, float moveTime, Transform transform, NavMeshAgent agent, float footstepFrequency, float idleFrequency, EntitySound soundPlayer) {
        this.radius = radius;
        this.moveTime = moveTime;
        this.transform = transform;
        this.agent = agent;
        this.footstepFrequency = footstepFrequency;
        this.idleFrequency = idleFrequency;
        this.soundPlayer = soundPlayer;
    }

    public static Vector3 RandomNavmeshLocation(float radius, Transform transform) {
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
        lastFootstep = 0;
        lastIdle = 0;
        moving = true;
    }

    public void ExitState() {
        soundPlayer.StopFootstep();
        soundPlayer.StopIdle();
    }

    public void ForceExitState() {
        ExitState();
    }

    float lastTime;
    float lastFootstep;
    float lastIdle;
    bool moving;
    public void UpdateState(float deltaTime) {
        if (Time.time > lastTime + moveTime) {
            lastTime = Time.time;
            moving = !moving;
            if (moving) {
                target = RandomNavmeshLocation(radius, transform);
            } else {
                target = transform.position;
            }
            agent.SetDestination(target);
        }

        if (Time.time > lastIdle + idleFrequency) {
            lastIdle = Time.time;
            soundPlayer.PlayIdle();
        }

        if (EntityAI.NavAgentMoving(agent) && Time.time > lastFootstep + footstepFrequency) {
            lastFootstep = Time.time;
            soundPlayer.PlayFootstep();
        }
    }
}

public class SeekState : IAIState {

    public static event System.Action exit;

    float moveTime;
    NavMeshAgent agent;
    Transform player;
    float footstepFrequency;
    EntitySound soundPlayer;
    public SeekState(float moveTime, NavMeshAgent agent, Transform player, float footstepFrequency, EntitySound soundPlayer) {
        this.moveTime = moveTime;
        this.agent = agent;
        this.player = player;
        this.footstepFrequency = footstepFrequency;
        this.soundPlayer = soundPlayer;
    }

    public void EnterState() {
        agent.SetDestination(player.position);
        lastTime = Time.time;
        lastFootstep = Time.time;
    }

    public void ExitState() {
        soundPlayer.StopFootstep();
        exit?.Invoke();
    }

    public void ForceExitState() {
        soundPlayer.StopFootstep();
    }

    float lastFootstep;
    float lastTime;
    public void UpdateState(float deltaTime) {
        if (Time.time > lastTime + moveTime || EntityAI.NavAgentMoving(agent)) {
            ExitState();
        }
        if(Time.time > lastFootstep + footstepFrequency) {
            lastFootstep = Time.time;
            soundPlayer.PlayFootstep();
        }
    }
}

public class ChaseState : IAIState {

    public static event System.Action exit;

    float pauseTime;
    float chaseTime;
    float chaseRadius;
    float chaseSpeed;
    float regularSpeed;
    float chaseSoundFrequency;
    NavMeshAgent agent;
    Transform ai;
    Transform player;
    EntitySound soundPlayer;
    [Header("Wwise Events")]
    AK.Wwise.Event[] chaseSounds;
    GameObject[] chaseSoundLocations;
    float chaseWanderTime;
    public ChaseState(float pauseTime, float chaseTime, float chaseRadius, float chaseSpeed, float regularSpeed, 
        float chaseSoundFrequency, NavMeshAgent agent, Transform ai, Transform player, EntitySound soundPlayer, 
        AK.Wwise.Event[] chaseSounds, GameObject[] chaseSoundLocations, float chaseWanderTime) {
        this.pauseTime = pauseTime;
        this.chaseTime = chaseTime;
        this.chaseRadius = chaseRadius;
        this.chaseSpeed = chaseSpeed;
        this.regularSpeed = regularSpeed;
        this.chaseSoundFrequency = chaseSoundFrequency;
        this.agent = agent;
        this.ai = ai;
        this.player = player;
        this.soundPlayer = soundPlayer;
        this.chaseSounds = chaseSounds;
        this.chaseSoundLocations = chaseSoundLocations;
        this.chaseWanderTime = chaseWanderTime;
    }

    public void EnterState() {
        agent.SetDestination(ai.position);
        lastTime = Time.time;
        hasPaused = false;
        soundPlayer.PlayScream();
    }

    public void ExitState() {
        soundPlayer.StopScream();
        soundPlayer.StopChase();
        agent.speed = regularSpeed;
        exit?.Invoke();
    }

    public void ForceExitState() {
        soundPlayer.StopScream();
        soundPlayer.StopChase();
        agent.speed = regularSpeed;
    }

    float DistToPlayer() {
        return Vector3.Distance(player.position, ai.position);
    }

    bool wander = false;
    float lastWander;
    float lastChaseSound;
    float lastTime;
    bool hasPaused;
    public void UpdateState(float deltaTime) {
        if (!hasPaused) {
            if (Time.time > lastTime + pauseTime) {
                hasPaused = true;
                soundPlayer.PlayChase();
                agent.speed = chaseSpeed;
            }
        } else {
            if(Time.time > lastChaseSound + chaseSoundFrequency) {
                lastChaseSound = Time.time;
                int i = Random.Range(0, chaseSounds.Length);
                int j = Random.Range(0, chaseSoundLocations.Length);
                chaseSounds[i].Post(chaseSoundLocations[j]);
            }

            if(DistToPlayer() < chaseRadius / 2 && !wander) {
                Debug.Log("Chase wander time");
                wander = true;
                agent.SetDestination(WanderState.RandomNavmeshLocation(15, ai));
                lastWander = Time.time;
            }

            if (wander && Time.time > lastWander + chaseWanderTime) {
                wander = false;
            } else {
                agent.SetDestination(player.transform.position);
            }



            if (Time.time > lastTime + chaseTime && DistToPlayer() > chaseRadius) {
                ExitState();
            }
            
            
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
    public float seekRadius;
    public float chaseRadius;
    public float continueChaseRadius;

    public float moveTime;

    public float chasePauseTime;
    public float chaseTime;
    public float chaseSpeed;
    public float chaseWanderTime;

    public float catchRadius;
    public float quietTime;

    public float chaseSoundFrequency;
    [Header("Wwise Events")]
    public AK.Wwise.Event[] chaseSounds;
    public GameObject[] chaseSoundLocations;

    State state = State.wander;
    IAIState[] states;

    public float footstepFrequency;
    public float idleFrequncy;

    EntitySound soundPlayer;

    Vector3 startLocation;
    void Start()
    {
        startLocation = transform.position;
        soundPlayer = GetComponent<EntitySound>();
        soundPlayer.PlayGlow();
        animator = GetComponent<Animator>();
        List<IAIState> s = new List<IAIState> {
            new WanderState(randomRadius, moveTime, transform, myNavAgent, footstepFrequency, idleFrequncy, soundPlayer),
            new SeekState(moveTime, myNavAgent, player.transform, footstepFrequency, soundPlayer),
            new ChaseState(chasePauseTime, chaseTime, continueChaseRadius, chaseSpeed, myNavAgent.speed, chaseSoundFrequency, 
                myNavAgent, transform, player.transform, soundPlayer, chaseSounds, chaseSoundLocations, chaseWanderTime)
        };
        states = s.ToArray();
        SeekState.exit += ExitSeekState;
        ChaseState.exit += ExitChaseState;
        
    }

    void ChangeState(State newState) {
        Debug.Log("Changing from state " + state + " to " + newState);
        states[(int)state].ForceExitState();
        state = newState;
        states[(int)state].EnterState();
    }

    float quietStartTime;
    private void Update() {

        if(soundPlayer.quiet && Time.time > quietStartTime + quietTime) {
            soundPlayer.quiet = false;
        }

        switch (state) {
            case State.wander:
                if (Vector3.Distance(transform.position, player.transform.position) < chaseRadius) {
                    ChangeState(State.chase);
                }else if (player.GetComponent<FPSController>().moving && Vector3.Distance(transform.position, player.transform.position) < seekRadius) {
                    ChangeState(State.seek);
                }
                break;
                
            case State.seek:
                if (Vector3.Distance(transform.position, player.transform.position) < chaseRadius) {
                    ChangeState(State.chase);
                }
                break;
            case State.chase:
                break;
        }
        states[(int)state].UpdateState(Time.deltaTime);
    }

    void ExitSeekState() {
        ChangeState(State.wander);
    }

    void ExitChaseState() {
        ChangeState(State.wander);
    }

    public static bool NavAgentMoving(NavMeshAgent agent) {
        return agent.velocity.magnitude < 0.1f;
    }
}
