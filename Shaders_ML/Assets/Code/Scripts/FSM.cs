using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    private enum FSMStates
    {
        Patrol, Chase, Aim, Shoot, Evade
    }

    [SerializeField]
    private FSMStates currentState = FSMStates.Patrol;

    public GameObject bullet;
    public Transform playerTransform;
    public GameObject bulletSpawnPoint;

    public List<GameObject> pointList;

    private int health = 100;

    public float curSpeed, targetSpeed;
    public float rotSpeed = 150.0f;
    public float turretRotSpeed = 10.0f;
    public float maxForwardSpeed = 30.0f;
    public float maxBackwardSpeed = -30.0f;
    public float shootRate = 0.5f;
    private float elapsedTime;

    public float chaseRadius = 200f;
    public float AttackRadius = 100f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrol();
                break;
                
            case FSMStates.Chase:
                UpdateChase();
                break;
                
            case FSMStates.Aim:
                UpdateAim();
                break;

            case FSMStates.Shoot:
                UpdateShoot();
                break;

            case FSMStates.Evade:
                UpdateEvade();
                break;
        }
    }

    void UpdatePatrol()
    {

    }

    void UpdateChase()
    {

    }

    void UpdateAim()
    {

    }

    void UpdateShoot()
    {

    }

    void UpdateEvade()
    {

    }

    private void FixedUpdate() 
    {
        
    }
}
