using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject Lamp;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Lamp.GetComponent<LampTrigger>().DoorCanOpen && collision.gameObject.tag == "Player")
        {
            Lamp.GetComponent<LampTrigger>().OpenDoor();
        }
    }
}
