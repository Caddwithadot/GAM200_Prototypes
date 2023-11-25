using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    [SerializeField] private RayLightNEW rayLight;
    [SerializeField] private SuperRayLight superLight;
    private Transform player;
    private float playerX;

    public float interpolant = 20f;

    public bool canFocus = true;
    public bool focus = false;
    public bool kill = false;

    public bool checkEnemies = false;
    private GameObject[] enemies;

    private ParticleSystem ps;
    public bool useParticles;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerX = player.localScale.x;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ps = GetComponent<ParticleSystem>();
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
            player.transform.localScale = new Vector2(playerX, playerX);
        }
        else if(transform.right.x < 0)
        {
            player.transform.localScale = new Vector2(-playerX, playerX);
        }

        rayLight.SetAimDirection(transform.right);
        rayLight.SetOrigin(transform.position);
        superLight.SetAimDirection(transform.right);
        superLight.SetOrigin(transform.position);

        if (canFocus)
        {
            if (Input.GetMouseButton(1))
            {
                focus = true;

                if (useParticles)
                {
                    ps.startLifetime = 1.5f;
                    var partShape = ps.shape;
                    partShape.angle = 2;
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                focus = false;

                if (useParticles)
                {
                    ps.startLifetime = 1;
                    var partShape = ps.shape;
                    partShape.angle = 20;
                }
            }

            if (Input.GetMouseButton(0) && focus)
            {
                kill = true;
                checkEnemies = true;
            }

            if (Input.GetMouseButtonUp(0) || !focus)
            {
                kill = false;
                EnemyCheck();
            }
        }
        else
        {
            kill = false;
            focus = false;
            checkEnemies = true;
            EnemyCheck();
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
