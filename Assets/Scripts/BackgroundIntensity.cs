using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class BackgroundIntensity : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Color spriteColour;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColour = spriteRenderer.color;
        spriteColour.a = 0f;
        spriteRenderer.color = spriteColour;
        MidiJack.MidiDriver.Instance.noteOnDelegate += MidiKeyPress;
	}

    void MidiKeyPress (MidiChannel channel, int note, float velocity)
    {
        Debug.Log("Velocity: " + velocity);
        if (spriteColour.a < velocity) {
            spriteColour.a = velocity;
        }
    }

    void Update()
    {
        spriteColour.a -= 0.01f;
        spriteRenderer.color = spriteColour;
    }
}
