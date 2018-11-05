using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class Logo : MonoBehaviour {

    public float timeUntilFadeIn = 10F;
    public float fadeSpeed = 0.01F;

    SpriteRenderer spriteRenderer;
    Color colour;
    float timer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colour = spriteRenderer.color;
        MidiJack.MidiDriver.Instance.noteOnDelegate += MidiKeyPress;
    }


    void MidiKeyPress(MidiChannel channel, int note, float velocity)
    {
        timer = Time.time + timeUntilFadeIn;
    }


    // Update is called once per frame
    void Update () {
		if (Time.time > timer)
        {
            FadeIn();
        } else
        {
            FadeOut();
        }
	}

    void FadeIn ()
    {
		colour.a = Mathf.Clamp01(colour.a+fadeSpeed);
        spriteRenderer.color = colour;
    }

    void FadeOut()
    {
		colour.a = Mathf.Clamp01(colour.a-fadeSpeed);
        spriteRenderer.color = colour;
    }
}
