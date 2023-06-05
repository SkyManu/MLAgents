using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private List<GameObject> blueTeamAgents;
    private List<GameObject> redTeamAgents;
    private List<GameObject> goalList;
    private List<GameObject> failList;

    private float minX = -12f;
    private float maxX = 7f;
    private float minZ = -7f;
    private float maxZ = 12f;

    private float timeElapsed;
    private void Awake()
    {
        GetAllAgents();
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;

        

        if (CheckGoalList() || timeElapsed >= 150f) 
        {
            timeElapsed = 0f;
            ReactivateAllAgents();
            ReactivateBananas();
            Globals.generation += 1;
        }

    }
    private void GetAllAgents()
    {
        blueTeamAgents = new List<GameObject>();
        GameObject[] blueAgents = GameObject.FindGameObjectsWithTag("BlueAgent");
        blueTeamAgents.AddRange(blueAgents);

        redTeamAgents = new List<GameObject>();
        GameObject[] redAgents = GameObject.FindGameObjectsWithTag("RedAgent");
        redTeamAgents.AddRange(redAgents);

        goalList = new List<GameObject>();
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        goalList.AddRange(goals);

        failList = new List<GameObject>();
        GameObject[] failObjects = GameObject.FindGameObjectsWithTag("Fail");
        failList.AddRange(failObjects);


    }
    private void ReactivateAllAgents()
    {
        foreach(GameObject agent in blueTeamAgents)
        {
           
           var agentSctrip = agent.GetComponent<BananaCollector>();
            agentSctrip.EndEpisode();
           
        }
        foreach (GameObject redAgent in redTeamAgents)
        {
            var redAgentScript = redAgent.GetComponent<BananaCollectorEnemy>();
            redAgentScript.EndEpisode();
        }
    }
    private bool CheckGoalList()
    {
        foreach (GameObject obj in goalList)
        {
            if (obj.activeSelf)
            {
                return false;
            }
        }

        return true;
    }
    public void ReactivateBananas()
    {
        foreach (GameObject goal in goalList)
        {
            goal.SetActive(true);
            goal.transform.localPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), 2.5f, UnityEngine.Random.Range(minZ, maxZ));
        }
        foreach (GameObject fail in failList)
        {
            fail.SetActive(true);
            fail.transform.localPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), 2.5f, UnityEngine.Random.Range(minZ, maxZ));
        }

    }


}
