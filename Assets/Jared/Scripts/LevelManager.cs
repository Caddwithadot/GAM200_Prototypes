using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] Rooms;
    public Transform Colliders;
    public Transform Hazards;

    void Start()
    {
        Rooms = GameObject.FindGameObjectsWithTag("JumpThrough");

        for (int i = 0; i < Rooms.Length; i++)
        {
            Rooms[i].AddComponent<JumpThroughParent>();
        }

        for (int i = 0; i < Colliders.childCount; i++)
        {
            Colliders.GetChild(i).gameObject.tag = "Environment";
        }

        for (int i = 0; i < Hazards.childCount; i++)
        {
            Hazards.GetChild(i).gameObject.tag = "Enemy";
        }
    }
}
