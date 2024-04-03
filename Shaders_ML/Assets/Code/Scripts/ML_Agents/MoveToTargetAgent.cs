// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class MoveToTargetAgent : Agent
// {
//     [SerializeField] private Transform targetTransform;
//     [SerializeField] private MeshRenderer floor;
//     [SerializeField] private Material positiveMaterial;
//     [SerializeField] private Material negativeMaterial;
//     [SerializeField] private float moveSpeed;

//     public override void OnEpisodeBegin()
//     {
//         base.OnEpisodeBegin();

//         transform.localPosition = Vector3.zero;
//     }

//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         base.Heuristic(actionsOut);

//         ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

//         continuousActions[0] = Input.GetAxisRaw("Horizontal");
//         continuousActions[1] = Input.GetAxisRaw("Vertical");
//     }
//     public override void CollectObservations(VectorSensor sensor)
//     {
//         base.CollectObservations(sensor);

//         sensor.AddObservation(transform.localPosition);
//         sensor.AddObservation(targetTransform.localPosition);
//     }

//     public override void OnActionReceived(ActionBuffers actions)
//     {
//         base.OnActionReceived(actions);

//         float moveX = actions.ContinuousActions[0];
//         float moveZ = actions.ContinuousActions[1];

//         transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
//     }

//     private void OnTriggerEnter(Collider other) 
//     {
//         if(other.CompareTag("Target"))
//         {
//             SetReward(1f);
//             floor.material = positiveMaterial;
//             EndEpisode();
//         }
//         if(other.CompareTag("Wall"))
//         {
//             SetReward(-1f);
//             floor.material = negativeMaterial;
//             EndEpisode();
//         }

//     }
// }
