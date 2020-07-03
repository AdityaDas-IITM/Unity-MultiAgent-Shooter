using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    AgentScript agentScript;
    AreaScript areaScript;
    // Update is called once per frame
    public void shoot()
    {
        
        areaScript =transform.parent.parent.GetComponentInParent<AreaScript>();
        agentScript = transform.parent.parent.GetComponent<AgentScript>();
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 15))
        {
            GameObject shot = hit.transform.gameObject;
            if (shot.CompareTag("enemy"))
            {
                areaScript.DestroyEnemy(shot);
                Debug.Log("Hit Enemy");
            }

            else
            {
                agentScript.AddReward(-0.0002f);
            }

        }

        else
        {
            agentScript.AddReward(-0.0002f);
        }
    }
}
