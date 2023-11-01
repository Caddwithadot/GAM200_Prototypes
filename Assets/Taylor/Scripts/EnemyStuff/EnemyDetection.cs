using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public bool detected;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Environment")
        {
            detected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Environment")
        {
            detected = false;
        }
    }
}
