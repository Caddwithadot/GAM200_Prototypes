using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryButtons : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}
