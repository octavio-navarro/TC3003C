using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class TankFSM : MonoBehaviour
{
    private enum FSMStates
    {
        Patrol, Chase, Attack
    }

    [SerializeField] private FSMStates currentState = FSMStates.Patrol;
    [SerializeField] HealthBar healthBar;

    [SerializeField] private GameObject turret;

    [SerializeField] private float turretRotSpeed = 10.0f;
    
    private int health = 500;
    private Vector3 destPos;

    public GameObject bullet;
    public Transform playerTransform;
    public GameObject bulletSpawnPoint;
    public List<GameObject> pointList;

    public float currentSpeed;
    [SerializeField] private float shootRate = 5.0f;
    private float elapsedTime;

    public float patrolRadius = 10f;
    public float chaseRadius = 25f;
    public float attackRadius = 20f;
    public float playerNearRadius = 10f;

    private int index = -1;

    private NavMeshAgent navmeshAgent;


    // Start is called before the first frame update
    void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        healthBar.setMaxHealth(health);
        FindNextPoint();
    }

    void Update()
    {
        if (playerTransform == null && currentState != FSMStates.Patrol)
        {
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }
        
        switch(currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrol();
                break;
                
            case FSMStates.Chase:
                UpdateChase();
                break;
                
            case FSMStates.Attack:
                UpdateAttack();
                break;
        }
        navmeshAgent.SetDestination(destPos);
    }

    private void FindNextPoint() 
    {
        // // Debug.Log("Finding next point");
        index = Random.Range(0, pointList.Count); //(index+1)%pointList.Count;
        destPos = pointList[index].transform.position;
    }

    public void TakeDamage(int damage) 
    {
        health -= damage;
        healthBar.setHealth(health);
        if (health <= 0) 
        {
            Destroy(gameObject);
        }
    }

    void UpdatePatrol()
    {
        //Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= patrolRadius) 
        {
            // // Debug.Log("Reached the destination point -- calculating the next point");
            FindNextPoint();
        }
        //Check the distance with player tank, when the distance is near, transition to chase state
        else if(playerTransform != null)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= chaseRadius) 
            {
                // // Debug.Log("Switch to Chase state");
                currentState = FSMStates.Chase;
            }
        }

        navmeshAgent.stoppingDistance = 1;

        // //Rotate to the target point
        // Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);

        // //Go Forward
        // transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);
    }

    void UpdateChase()
    {
        destPos = playerTransform.position;

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist <= attackRadius) {
            // // Debug.Log("Switch to Attack state");
            currentState = FSMStates.Attack;
        }
        else if (dist >= chaseRadius) {
            // // Debug.Log("Switch to Patrol state");
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }


    }

    void UpdateAttack()
    {
        destPos = playerTransform.position;

        Vector3 frontVector = Vector3.forward;

        //Check the distance with the player tank
        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist < playerNearRadius) 
        {
            // Rotate target point
            // The rotation is only around the vertical axis of the tank.
            Quaternion targetRotation = Quaternion.FromToRotation(frontVector, destPos - transform.position);
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, targetRotation, Time.deltaTime * turretRotSpeed);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            // if (Mathf.Abs(Quaternion.Dot(turretRotation, turret.rotation)) > 1.0f - maxFireAimError) 
            ShootBullet();
            
        }
        //Transition to patrol is the tank become too far
        if (dist >= chaseRadius) {
            // // Debug.Log("Switch to Patrol state");
            currentState = FSMStates.Patrol;
            FindNextPoint();
        }

        navmeshAgent.stoppingDistance = 3;
    }

    void ShootBullet() 
    {
        elapsedTime += Time.deltaTime;
        // Debug.Log("Shoot rate: " +   shootRate);
        // Debug.Log("Elapsed Time: " + elapsedTime);
        if (elapsedTime >= shootRate) 
        {
            //Reset the time
            elapsedTime = 0.0f;

            Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        }
    }
}
