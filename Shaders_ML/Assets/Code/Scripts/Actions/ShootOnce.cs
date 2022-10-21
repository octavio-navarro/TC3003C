using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("ShootOnce")]
public class ShootOnce : GOAction
{
    [InParam("shootPoint")]
    public Transform shootPoint;

    [InParam("bullet")]
    public GameObject bullet;

    [InParam("velocity", DefaultValue = 30f)]
    public float velocity;
    
    public override void OnStart() 
    {
        if (shootPoint == null) 
        {
            shootPoint = gameObject.transform.Find("shootPoint");
            if (shootPoint == null) 
            {
                Debug.LogWarning("Shoot point not specified. ShootOnce will not work for " + gameObject.name);
            }
        }
        base.OnStart();
    }

    public override TaskStatus OnUpdate() 
    {
        if (shootPoint == null || bullet == null) return TaskStatus.FAILED;
        
        // Instantiate the bullet prefab.
        GameObject newBullet = Object.Instantiate(bullet, shootPoint.position, shootPoint.rotation * bullet.transform.rotation);
        
        // Give it a velocity
        newBullet.GetComponent<Rigidbody>().velocity = velocity * shootPoint.forward;
        // The action is completed. We must inform the execution engine.
        return TaskStatus.COMPLETED;
    }
}
