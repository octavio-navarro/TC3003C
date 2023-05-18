using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 60.0f;
    [SerializeField] private float lifeTime = 3.0f;
   
    [SerializeField] private int damage = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;   
    }

    private void OnCollisionEnter(Collision other) 
    {
        print(other.gameObject.tag);
        
        if (other.gameObject.CompareTag("Tank")) 
        {
            other.gameObject.GetComponent<TankController>().TakeDamage(damage);
        }
        else if (other.gameObject.CompareTag("Enemy")) 
        {
            other.gameObject.GetComponent<TankFSM>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
