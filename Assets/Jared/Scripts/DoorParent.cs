using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DoorParent : MonoBehaviour
{
    public LampTrigger Lamp;

    private void Start()
    {
        Lamp = Lamp.GetComponent<LampTrigger>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Light" || collision.tag == "ChargeLight" || collision.tag == "PlayerAura")
        {
            Debug.Log("LIGHT");

            if (Lamp.DoorCanOpen)
            {
                Lamp.OpenDoor();
            }
        }
    }
}
