using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;



public class MoveToGoalAgent : Agent
{

    [Header("Suelo y materiales")]
    [SerializeField] private MeshRenderer floorObject;
    [SerializeField] private Material looseMaterial;
    [SerializeField] private Material winMaterial;

    [Header("Parámetros de entrenamiento")]  

    [SerializeField] private Transform goalTransform;
    [SerializeField] private float speed = 3f;




    
    
   




    public override void OnEpisodeBegin()
    {
        Globals.generation+=1;
        transform.localPosition = new Vector3(UnityEngine.Random.Range(-6f, 5f), 2.5f, UnityEngine.Random.Range(-2f, 6f));
        goalTransform.localPosition = new Vector3(UnityEngine.Random.Range(-6f, 5f), 2.5f, UnityEngine.Random.Range(-2f, 6f));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 movementDirection = new Vector3(moveX, 0, moveZ);

        this.transform.Translate(Vector3.right * moveX * speed * Time.deltaTime);
        this.transform.Translate(Vector3.forward * moveZ * speed * Time.deltaTime);

        Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        this.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360f * Time.deltaTime);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(goalTransform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            Globals.winsBlueTeam += 1;

            

            SetReward(1f);

            floorObject.material = winMaterial;
            Globals.PrintStats();
            EndEpisode();
        }
        if (other.tag == "Wall")
        {
            Globals.loosesBlueTeam += 1;

            

            floorObject.material = looseMaterial;
            
            SetReward(-1f);
            Globals.PrintStats();
            EndEpisode();
        }

    }
}
