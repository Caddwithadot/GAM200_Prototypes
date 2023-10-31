using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LampTrigger : MonoBehaviour
{
    private MouseControls mouseControls;
    public Transform Light;
    public GameObject Door;
    public Color LitCableColor;
    public Tilemap Cable;
    public Vector3 MinScale = new Vector3(0f, 0f, 0f);
    public Vector3 MaxScale = new Vector3(0.5f, 0.5f, 0f);
    public Vector3 ScaleIncrement = new Vector3(0.002f, 0.002f, 0f);
    public bool FullyLit = false;
    public bool LightDown = true;
    public float LightDownTimer = 0f;
    public float LightDownTime = 1f;
    public AudioClip DoorOpenSFX;
    public GameObject DoorParent;
    public bool SFXPlayed = false;

    void Start()
    {
        Door = gameObject.transform.parent.transform.GetChild(1).gameObject;
        Light = transform.GetChild(0);
        mouseControls = GameObject.Find("MouseControls").GetComponent<MouseControls>();
        DoorParent = gameObject.transform.parent.transform.GetChild(1).gameObject;

        for (int i = 0; i < Door.transform.childCount; i++)
        {
            Door.transform.GetChild(i).gameObject.layer = 7;
            Door.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 1f);
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
        if (collision.tag == "Light" && mouseControls.kill )
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
        Light.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 1f, 0.4f);
        Cable.color = LitCableColor;

        for (int i = 0; i < Door.transform.childCount; i++)
        {
            Door.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            Door.transform.GetChild(i).GetComponent<Animator>().SetTrigger("DoorOpen");
        }

        if (SFXPlayed == false)
        {
            DoorParent.GetComponent<AudioSource>().PlayOneShot(DoorOpenSFX);
            SFXPlayed = true;
        }
    }
}
