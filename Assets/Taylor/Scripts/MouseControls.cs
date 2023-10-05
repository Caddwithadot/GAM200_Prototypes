using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    [SerializeField] private RayLight rayLight;
    private Transform player;

    public bool useFOV = false;
    public float startAngle = 60f;
    public float startDist = 50f;
    public float endAngle = 30f;
    public float endDist = 100f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = new Vector3(worldMousePosition.x, worldMousePosition.y, 0);
        transform.right = direction - transform.position;

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

            if (Input.GetMouseButtonUp(1))
            {
                rayLight.SetFOV(startAngle);
                rayLight.SetViewDistance(startDist);
            }

            if (Input.GetMouseButtonDown(1))
            {
                rayLight.SetFOV(endAngle);
                rayLight.SetViewDistance(endDist);
            }
        }
    }
}
