using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

	[SerializeField]
	SpriteRenderer spriteDisplay;

	[SerializeField]
	Sprite[] sprites;

	[SerializeField]
	Texture[] normalSprites;

	public void SetPosition(Vector3 position, Vector3 direction) {
		Quaternion rotation = Quaternion.LookRotation (Vector3.forward, direction);
		transform.localRotation = rotation;
		transform.localPosition = position;
	}

	public void SetGrowth(float newGrowth) {
		int index = Mathf.Clamp(Mathf.FloorToInt (newGrowth * sprites.Length),0,sprites.Length-1);
		spriteDisplay.sprite = sprites[index];
		spriteDisplay.material.SetTexture ("_BumpMap", normalSprites [index]);
	}

	public void SetScale(float newScale) {
		transform.localScale = Vector3.one * Mathf.Max(0f,newScale) * 0.4f;
	}

}
