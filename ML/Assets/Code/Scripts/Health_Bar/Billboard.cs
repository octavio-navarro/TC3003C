using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform camTransform;

    // Update is called once per frame
    private void LateUpdate() 
    {
        transform.LookAt(transform.position + camTransform.forward);        
    }
}
