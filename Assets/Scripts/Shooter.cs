using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    
    AreaScript areaScript;
    // Update is called once per frame
    public void shoot()
    {
        
        areaScript =transform.parent.parent.GetComponentInParent<AreaScript>();
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 15))
        {
            GameObject shot = hit.transform.gameObject;
            if(shot.CompareTag("enemy"))
            {
                areaScript.DestroyEnemy(shot);
                Debug.Log("Hit Enemy");
            }

        }
        
    }
}
