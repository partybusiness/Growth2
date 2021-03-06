﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

	[SerializeField]
	SpriteRenderer spriteDisplay;

	[SerializeField]
	Sprite[] sprites;

	[SerializeField]
	Texture[] normalSprites;

	int lastIndex = -1;

	public void SetPosition(Vector3 position, Vector3 direction) {
		Quaternion rotation = Quaternion.LookRotation (Vector3.forward, direction);
		transform.localRotation = rotation;
		transform.localPosition = position;
	}

	public bool SetGrowth(float newGrowth) {
		int index = Mathf.Clamp(Mathf.FloorToInt (newGrowth * sprites.Length),0,sprites.Length-1);
		if (lastIndex != index) {
			spriteDisplay.sprite = sprites [index];
			spriteDisplay.material.SetTexture ("_BumpMap", normalSprites [index]);
			lastIndex = index;
			if (index == sprites.Length-1) {
				StartCoroutine (ShootPollen ());
				return true;
			}
		}
		return false;
	}

	IEnumerator ShootPollen() {
		yield return new WaitForSeconds (0f);
		for (int i = 0; i < 3; i++) {
			yield return new WaitForSeconds (0.02f);
			PollenParticles.instance.EmitPollen (transform.TransformPoint(new Vector3(-1,8,0)), transform.up, spriteDisplay.color);
			PollenParticles.instance.EmitPollen (transform.TransformPoint(new Vector3(-1,8,0)), transform.up, spriteDisplay.color);
		}
	}

	public void SetScale(float newScale) {
		transform.localScale = Vector3.one * Mathf.Max(0f,newScale) * 0.4f;
	}

	public void SetColor(Color newColor) {
		spriteDisplay.color = newColor;
	}

}
