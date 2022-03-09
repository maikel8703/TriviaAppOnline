using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
 /* Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } */

    bool onGround;
    public int sideValue;

    private void OnTriggerStay(Collider col)
    {
        if(col.tag == "Ground")
        {
            onGround = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "Ground")
        {
            onGround = false;
        }
    }

    public bool OnGround()
    {
        return onGround;
    }
}
