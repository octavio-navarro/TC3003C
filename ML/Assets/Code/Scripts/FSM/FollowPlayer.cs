using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Vector3 cameraOffset;


    void OnEndGame() {
        // Don't allow any more control changes when the game ends
        this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform == null) {
            return;
        }
        // Move position of the camera to follow a target
        transform.position = playerTransform.position + cameraOffset;

        // Rotate the camera to look always at the target
        transform.LookAt(playerTransform);        

        // Transform the camera so that it is always at the back of the target
        // transform.position = playerTransform.position - playerTransform.forward * 10 + Vector3.up * 3;

    }
}
