using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealLamp : MonoBehaviour
{
    private MouseControls mouseControls;
    private Transform Light;
    public Vector3 MinScale = new Vector3(0f, 0f, 0f);
    public Vector3 MaxScale = new Vector3(1f, 1f, 0f);
    public Vector3 ScaleIncrement = new Vector3(0.005f, 0.005f, 0f);
    public bool FullyLit = false;
    public bool LightDown = true;
    public float LightDownTimer = 0f;
    public float LightDownTime = 1f;
    public AudioClip HealLampSFX;
    public bool SFXPlayed = false;

    void Start()
    {
        mouseControls = GameObject.Find("MouseControls").GetComponent<MouseControls>();
        Light = transform.GetChild(0);
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
        Light.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.7f, 0f, 0.4f);
        Light.gameObject.tag = "Aura";

        if (SFXPlayed == false)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(HealLampSFX);
            SFXPlayed = true;
        }
    }
}
