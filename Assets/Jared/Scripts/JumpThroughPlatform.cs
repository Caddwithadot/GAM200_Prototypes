using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughPlatform : MonoBehaviour
{
    public GameObject Player = null;
    public float Offset = -0.25f;
    public bool HoldingDown = false;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            HoldingDown = true;
        }
        else
        {
            HoldingDown = false;
        }

        if (Player.transform.position.y + Offset >= transform.position.y && !HoldingDown)
        {
            Physics2D.IgnoreLayerCollision(9, 10, false);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
        }
    }
}
