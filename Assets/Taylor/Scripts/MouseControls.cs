using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    [SerializeField] private RayLightNEW rayLight;
    private Transform player;

    public float interpolant = 300f;

    public bool useFOV = false;
    public float startAngle = 60f;
    public float startDist = 50f;
    public float endAngle = 30f;
    public float endDist = 100f;

    private bool focus = false;
    public Material lightMaterial;
    public Color unfocused;
    public Color focused;

    public bool kill = false;
    public bool checkEnemies = false;
    private GameObject[] enemies;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 targetDirection = new Vector3(worldMousePosition.x, worldMousePosition.y, 0) - transform.position;
        //transform.right = direction - transform.position;

        float targetRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        Quaternion targetRotationQuat = Quaternion.Euler(0, 0, targetRotation);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationQuat, interpolant * Time.deltaTime);

        if (transform.right.x > 0)
        {
            player.transform.localScale = new Vector2(5, 5);
        }
        else if(transform.right.x < 0)
        {
            player.transform.localScale = new Vector2(-5, 5);
        }

        if (useFOV)
        {
            rayLight.SetAimDirection(transform.right);
            rayLight.SetOrigin(transform.position);

            if (Input.GetMouseButtonDown(1))
            {
                rayLight.SetFOV(endAngle);
                rayLight.SetViewDistance(endDist);

                focus = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                rayLight.SetFOV(startAngle);
                rayLight.SetViewDistance(startDist);

                focus = false;
            }

            if (Input.GetMouseButton(0) && focus)
            {
                lightMaterial.color = focused;

                kill = true;
                checkEnemies = true;
            }

            if (Input.GetMouseButtonUp(0) || !focus)
            {
                lightMaterial.color = unfocused;

                kill = false;

                EnemyCheck();
            }
        }
    }

    public void EnemyCheck()
    {
        if (checkEnemies)
        {
            foreach (GameObject obj in enemies)
            {
                if(obj.transform.childCount > 0)
                {
                    obj.GetComponentInChildren<EnemyFill>().StopFilling();
                }
            }
            checkEnemies = false;
        }
    }
}
