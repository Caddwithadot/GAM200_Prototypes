using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] Rooms;

    void Start()
    {
        Rooms = GameObject.FindGameObjectsWithTag("JumpThrough");

        for (int i = 0; i < Rooms.Length; i++)
        {
            Rooms[i].AddComponent<JumpThroughParent>();
        }


        for (int i = 0; i < GameObject.FindGameObjectWithTag("Hazards").transform.childCount; i++)
        {
            GameObject.FindGameObjectWithTag("Hazards").transform.GetChild(i).tag = "Enemy";
        }
    }
}
