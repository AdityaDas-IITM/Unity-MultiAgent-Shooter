// self play AREA script

using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.UIElements;

public class SPAreaScript : MonoBehaviour
{
    GameObject agent;

    public GameObject team_red_player;
    public GameObject team_blue_player;

    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public float EpisodeLength = 220f;

    private float startTime;

    //public List<GameObject> blue = new List<GameObject>();
    //public List<GameObject> red = new List<GameObject>();

    public int team_size = 1;

    // bl -> bottom left corner (blue spawn), tr -> top right corner (red spawn)
    float bl_x_min = -13f;
    float bl_x_max = 0f;
    float bl_z_min = -8f;
    float bl_z_max = 0f;
    float tr_x_min = 0f;
    float tr_x_max = 13f;
    float tr_z_min = 0f;
    float tr_z_max = 8f;
    float y = 1.6f;

    List<GameObject> team_red_list;
    List<GameObject> team_blue_list;
    
    private void Start()
    {
        startTime = Mathf.FloorToInt(Time.time);
        ResetArea();
    }

    public void Update()
    {
        if(Mathf.FloorToInt(Time.time) == startTime+EpisodeLength)
        {
            ResetArea();
            startTime = Mathf.FloorToInt(Time.time);
            Debug.Log("Reset");
        }
    }

    public void DestroyPerson(GameObject dest, GameObject source)
    {
        SPAgentScript agentsource = source.GetComponent<SPAgentScript>();
        SPAgentScript agentdest = dest.GetComponent<SPAgentScript>();

        if (dest.CompareTag("team_blue")) 
        {
            for (int i = 0; i<team_blue_list.Count; i++)
            {
                if(GameObject.ReferenceEquals(team_blue_list[i], dest))
                {
                    if (source.CompareTag(dest.tag))
                    {
                        agentsource.AddReward(-1f);
                    }
                    else
                    {
                        agentsource.AddReward(1f);
                        agentdest.AddReward(-1f);

                        agentdest.EndEpisode();
                        Destroy(team_blue_list[i]);
                        //team_blue_list[i].SetActive(false);
                        team_blue_list.RemoveAt(i);
                    }
                }
            }

            Debug.Log(team_blue_list.Count);

            if(team_blue_list.Count == 0)
            {
                agentsource.AddReward(5f);
                agentdest.AddReward(-5f);
                //agentsource.EndEpisode();

                for (int i= 0; i < team_red_list.Count; i++)
                {
                    SPAgentScript script = team_red_list[i].GetComponent<SPAgentScript>();
                    script.EndEpisode();
                }

                ResetArea();
            }

        }

        else if (dest.CompareTag("team_red")) 
        {
            for (int i = 0; i<team_red_list.Count; i++)
            {
                if(GameObject.ReferenceEquals(team_red_list[i], dest))
                {
                    if (source.CompareTag(dest.tag))
                    {
                        agentsource.AddReward(-1f);
                    }
                    else
                    {
                        agentsource.AddReward(1f);
                        agentdest.AddReward(-1f);

                        agentdest.EndEpisode();
                        Destroy(team_red_list[i]);
                        //team_red_list[i].SetActive(false);
                        team_red_list.Remove(team_red_list[i]);
                    }
                }
            }

            Debug.Log(team_blue_list.Count);

            if (team_red_list.Count == 0)
            {
                agentsource.AddReward(5f);
                agentdest.AddReward(-5f);
                //agentsource.EndEpisode();

                for (int i = 0; i < team_blue_list.Count; i++)
                {
                    SPAgentScript script = team_blue_list[i].GetComponent<SPAgentScript>();
                    script.EndEpisode();
                }

                ResetArea();
            }
        }
    }

    public void ResetArea()
    {
        DestroyAllPersons();
        SpawnPlayers(team_size);
        cube1.transform.localPosition = new Vector3(-10.57f, 2.7f, -4.92f);
        cube1.transform.rotation = Quaternion.Euler(0, 0, 0);
        cube2.transform.localPosition = new Vector3(-9.23f, 2.7f, 3.1f);
        cube2.transform.rotation = Quaternion.Euler(0, 0, 0);
        cube3.transform.localPosition = new Vector3(10.41f, 2.7f, -1.18f);
        cube3.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void SpawnPlayers(int team_size)
    {   
        // blue team spawn
        for (int i = 0; i < team_size; i++)
        {
            float x = UnityEngine.Random.Range(bl_x_min, bl_x_max);
            float z = UnityEngine.Random.Range(bl_z_min, bl_z_max);

            Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            Vector3 position = new Vector3(x, y, z);

            if (canSpawn(position))
            {
                GameObject newBlue = Instantiate(team_blue_player) as GameObject;
                newBlue.SetActive(true);
                newBlue.transform.SetParent(transform);
                Debug.Log(newBlue.transform.parent.name);
                newBlue.transform.localPosition = position;
                newBlue.transform.rotation = rotation;
                team_blue_list.Add(newBlue);
                //team_blue_player.transform.localPosition = position;
                //team_blue_player.transform.rotation = rotation;

                //blue[i].SetActive(true);
                //team_blue_list.Add(blue[i]);
                //blue[i].transform.localPosition = position;
                //blue[i].transform.rotation = rotation;
            }

            else
            {
                i--;
            }
        }

        // red team spawn
        for (int i = 0; i < team_size; i++)
        {
            float x = UnityEngine.Random.Range(tr_x_min, tr_x_max);
            float z = UnityEngine.Random.Range(tr_z_min, tr_z_max);

            Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            Vector3 position = new Vector3(x, y, z);

            if (canSpawn(position))
            {
                GameObject newRed = Instantiate(team_red_player) as GameObject;
                newRed.SetActive(true);
                newRed.transform.SetParent(transform);
                newRed.transform.localPosition = position;
                newRed.transform.rotation = rotation;
                team_red_list.Add(newRed);
                //team_red_player.transform.localPosition = position;
                //team_red_player.transform.rotation = rotation;

                //red[i].SetActive(true);
                //team_red_list.Add(red[i]);
                //red[i].transform.localPosition = position;
                //red[i].transform.rotation = rotation;
            }
       

            else
            {
                i--;
            }
        }
    }

    bool canSpawn(Vector3 position)
    {
        bool truth;

        GameObject dummy = new GameObject();

        GameObject instdummy = Instantiate(dummy);
        instdummy.transform.SetParent(transform);
        instdummy.transform.localPosition = position;
        Collider[] collider = Physics.OverlapSphere(instdummy.transform.position, 0.6f);

        if(collider.Length == 0)
        { 
           truth = true;
        }

        else
        {
            Destroy(instdummy);
            truth = false;
        }

        Destroy(instdummy);
        Destroy(dummy);
        return truth;   
    }

    void DestroyAllPersons()
    {
        if (team_blue_list != null)
        {
            for (int i = 0; i < team_blue_list.Count; i++)
            {
                if (team_blue_list[i] != null)
                {
                    team_blue_list[i].GetComponent<SPAgentScript>().EndEpisode();
                    Destroy(team_blue_list[i]);
                    //team_blue_list[i].SetActive(false);
                }
            }
        }

        team_blue_list = new List<GameObject>();

        if (team_red_list != null)
        {
            for (int i = 0; i < team_red_list.Count; i++)
            {
                if (team_red_list[i] != null)
                {
                    team_red_list[i].GetComponent<SPAgentScript>().EndEpisode();
                    Destroy(team_red_list[i]);
                    //team_red_list[i].SetActive(false);
                }
            }
        }

        team_red_list = new List<GameObject>();
    }
}
