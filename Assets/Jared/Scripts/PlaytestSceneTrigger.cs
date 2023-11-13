using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaytestSceneTrigger : MonoBehaviour
{
    private float timer = 0f;
    private float PlayerStayTime = 0.25f;
    public bool PlayerIn;

    private void Update()
    {
        if (PlayerIn)
        {
            timer += Time.deltaTime;

            if (timer >= PlayerStayTime)
            {
                SceneManager.LoadScene(0);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer = 0f;

            PlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerIn = false;
        }
    }
}
