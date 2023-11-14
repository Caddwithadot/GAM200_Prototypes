using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LampTrigger : MonoBehaviour
{
    private MouseControls mouseControls;
    public Transform Light;
    public GameObject Door;
    public Tilemap Cable;

    private Vector3 MinScale = new Vector3(0f, 0f, 0f);
    public Vector3 MaxScale = new Vector3(0.5f, 0.5f, 0f);
    public Vector3 ScaleIncrement = new Vector3(0.001f, 0.001f, 0f);
    public Color LitColor = new Color(0.5f, 1f, 1f);
    public Color LightColor = new Color(0.5f, 1f, 1f, 0.4f);

    public bool FullyLit = false;
    public bool LightDown = true;
    public bool DoorSFXPlayed = false;
    public bool DoorCanOpen = false;
    public bool LampSFXPlayed = false;

    private float LightDownTimer = 0f;
    public float LightDownTime = 1f;

    public AudioSource LampAS;
    public AudioSource LampChargeAS;
    public AudioSource DoorAS;
    public AudioClip LampSFX;
    public AudioClip DoorSFX;

    void Start()
    {
        mouseControls = GameObject.Find("MouseControls").GetComponent<MouseControls>();

        for (int i = 0; i < Door.transform.childCount; i++)
        {
            Door.transform.GetChild(i).gameObject.layer = 7;
            Door.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = LitColor;
        }
    }

    void Update()
    {
        ClampLightScale();

        if (LightDown)
        {
            if (FullyLit == false)
            {
                LightDownTimer += Time.deltaTime;
            }

            if (LightDownTimer > LightDownTime)
            {
                Light.transform.localScale -= ScaleIncrement;
            }
        }

        if (Light.transform.localScale == MaxScale)
        {
            FullyLit = true;
            FullyLitUp();
        }

        if (Light.transform.localScale.x > MinScale.x && Light.transform.localScale.y > MinScale.y && Light.transform.localScale.x < MaxScale.x && Light.transform.localScale.y < MaxScale.y)
        {
            LampChargeAS.enabled = true;
        }
        else
        {
            LampChargeAS.enabled = false;
        }
    }

    public void LightUp()
    {
        Light.transform.localScale += ScaleIncrement;
    }

    public void ClampLightScale()
    {
        Vector3 LightScale = Light.localScale;

        LightScale.x = Mathf.Clamp(LightScale.x, MinScale.x, MaxScale.x);
        LightScale.y = Mathf.Clamp(LightScale.y, MinScale.y, MaxScale.y);

        Light.localScale = LightScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Light" && mouseControls.kill)
        {
            LightUp();

            LightDown = false;
            LightDownTimer = 0f;        
        }
        else
        {
            LightDown = true;
        }
    }

    public void FullyLitUp()
    {
        Light.gameObject.GetComponent<SpriteRenderer>().color = LightColor;
        Cable.color = LitColor;

        if (!LampSFXPlayed)
        {
            LampAS.PlayOneShot(LampSFX);
            LampSFXPlayed = true;
        }

        DoorCanOpen = true;
    }

    public void OpenDoor()
    {
        for (int i = 0; i < Door.transform.childCount; i++)
        {
            Door.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            Door.transform.GetChild(i).GetComponent<Animator>().SetTrigger("DoorOpen");
        }

        if (DoorSFXPlayed == false)
        {
            DoorAS.PlayOneShot(DoorSFX);
            DoorSFXPlayed = true;
        }
    }
}