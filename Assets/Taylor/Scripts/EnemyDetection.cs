using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public bool detected;
    public List<string> ignoreTags;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ignoreTags.Contains(collision.tag))
        {
            detected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!ignoreTags.Contains(collision.tag))
        {
            detected = false;
        }
    }
}
