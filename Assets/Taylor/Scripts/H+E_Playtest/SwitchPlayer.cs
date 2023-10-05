using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayer : MonoBehaviour
{
    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject playerThree;
    public GameObject playerFour;
    public GameObject playerFive;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerOne.SetActive(true);
            playerTwo.SetActive(false);
            playerThree.SetActive(false);
            playerFour.SetActive(false);
            playerFive.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerOne.SetActive(false);
            playerTwo.SetActive(true);
            playerThree.SetActive(false);
            playerFour.SetActive(false);
            playerFive.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerOne.SetActive(false);
            playerTwo.SetActive(false);
            playerThree.SetActive(true);
            playerFour.SetActive(false);
            playerFive.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerOne.SetActive(false);
            playerTwo.SetActive(false);
            playerThree.SetActive(false);
            playerFour.SetActive(true);
            playerFive.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerOne.SetActive(false);
            playerTwo.SetActive(false);
            playerThree.SetActive(false);
            playerFour.SetActive(false);
            playerFive.SetActive(true);
        }
    }
}
