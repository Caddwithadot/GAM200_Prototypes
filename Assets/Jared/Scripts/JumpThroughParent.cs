using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughParent : MonoBehaviour
{
    public Transform Player;
    public GameObject CurrentJTP;
    public int CurrentJTPIndex = 0;
    public float Offset = -0.5f;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = 10;
        }
    }

    private void Update()
    {
        FindClosestChild();

        if (CurrentJTP != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i) == CurrentJTP.transform)
                {
                    CurrentJTPIndex = i;
                    break;
                }
            }
        }

        if (CurrentJTP != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (CurrentJTPIndex == i)
                {
                    if(CurrentJTP.GetComponent<JumpThroughPlatform>() == null)
                    {
                        CurrentJTP.AddComponent<JumpThroughPlatform>();
                    }
                }
                else
                {
                    if(transform.GetChild(i).GetComponent<JumpThroughPlatform>() != null)
                    {
                        Destroy(transform.GetChild(i).GetComponent<JumpThroughPlatform>());
                    }
                }
            }
        }
    }

    private void FindClosestChild()
    {
        float minDistance = float.MaxValue;

        foreach (Transform child in transform)
        {
            float distanceToPlayerY = Mathf.Abs(child.position.y - Player.position.y);

            if (distanceToPlayerY + Offset < minDistance)
            {
                minDistance = distanceToPlayerY;
                CurrentJTP = child.gameObject;
            }
        }
    }
}
