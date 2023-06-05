
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;



public class BananaCollectorEnemy : Agent
{

    [Header("Suelo y materiales")]
    [SerializeField] private MeshRenderer floorObject;
    [SerializeField] private Material looseMaterial;
    [SerializeField] private Material winMaterial;

    [Header("Par�metros de entrenamiento")]

    [SerializeField] private List<GameObject> goalTransformList;
    [SerializeField] private List<GameObject> failTransformList;
    [SerializeField] private float speed = 1f;
    

    private bool isAgentPaused = false;
    private float pauseTimer = 3f;



    public override void Initialize()
    {
        RandomAgentPosition();
        goalTransformList = new List<GameObject>();
        GameObject[] initialObjects = GameObject.FindGameObjectsWithTag("Goal");
        goalTransformList.AddRange(initialObjects);

        failTransformList = new List<GameObject>();
        GameObject[] failObjects = GameObject.FindGameObjectsWithTag("Fail");
        failTransformList.AddRange(failObjects);
    }




    public override void OnEpisodeBegin()
    {

        RandomAgentPosition();



    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isAgentPaused)
        {
            // Si el agente est� pausado, no realizar ninguna acci�n
            return;
        }

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

        foreach (GameObject goal in goalTransformList)
        {
            // Agregar la posici�n del objeto con el tag deseado como observaci�n
            sensor.AddObservation(goal.transform.position);
            // Puedes agregar m�s observaciones seg�n tus necesidades, como la rotaci�n, tama�o, etc.
        }

        foreach (GameObject fail in failTransformList)
        {
            // Agregar la posici�n del objeto con el tag deseado como observaci�n
            sensor.AddObservation(fail.transform.position);
            // Puedes agregar m�s observaciones seg�n tus necesidades, como la rotaci�n, tama�o, etc.
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {


            Globals.winsRedTeam += 1;

            other.gameObject.SetActive(false);
            RandomAgentPosition();
            SetReward(3f);



            Globals.PrintStats();

        }
        if (other.tag == "Fail")
        {
            Globals.loosesRedTeam += 1;
            failTransformList.Remove(other.gameObject);
            other.gameObject.SetActive(false);
            if (!isAgentPaused)
            {
                isAgentPaused = true;
                StartCoroutine(PauseAgent());
            }
            Globals.PrintStats();


            SetReward(-3f);

        }
        if (other.tag == "Wall")
        {
            Globals.loosesRedTeam += 1;




            Globals.PrintStats();
            SetReward(-10f);
            RandomAgentPosition();

        }


    }

    public void RandomAgentPosition()
    {
        transform.localPosition = new Vector3(UnityEngine.Random.Range(-6f, 5f), 2.5f, UnityEngine.Random.Range(-2f, 6f));
    }


    private IEnumerator PauseAgent()
    {

        yield return new WaitForSeconds(pauseTimer);

        // Realizar acciones despu�s de la pausa de 3 segundos
        // Por ejemplo, puedes reanudar el movimiento del agente o ejecutar alguna otra l�gica.

        isAgentPaused = false; // Reiniciar la variable de pausa
    }
}
