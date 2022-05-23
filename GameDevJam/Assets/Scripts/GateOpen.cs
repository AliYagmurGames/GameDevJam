using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    public int type;
    bool startOpen = false;

    public void rotateOpenDoor()
    {
        startOpen = true;
    }

    private void Update()
    {
        if(startOpen)
        {
            if (type == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 210, 0), 5 * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -30, 0), 5 * Time.deltaTime);
            }
        }
    }
}
