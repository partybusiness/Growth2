using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaf : MonoBehaviour {
	
	public enum LeafSide
	{
		none,
		left,
		right
	}

	[SerializeField]
	SpriteRenderer spriteRenderer;

	[SerializeField]
	Sprite leftSprite;

	[SerializeField]
	Sprite rightSprite;

	[SerializeField]
	Material leftMaterial;

	[SerializeField]
	Material rightMaterial;

	private LeafSide _leafSide;

	public LeafSide leafSide {
		get {
			return _leafSide;
		}

		set {
			_leafSide = value;
			switch (value) {
			case LeafSide.left:
				spriteRenderer.sprite = leftSprite;
				spriteRenderer.sharedMaterial = leftMaterial;
				break;
			case LeafSide.right:
				spriteRenderer.sprite = rightSprite;
				spriteRenderer.sharedMaterial = rightMaterial;
				break;
			}
		}
	}

	public float scale {
		
		set {
			transform.localScale = Vector3.one * value;
		}
	}

	public void SetPosition(Vector3 position, Vector3 direction) {
		Quaternion rotation = Quaternion.LookRotation (Vector3.forward, direction);
		transform.localRotation = rotation;
		transform.localPosition = position;
	}

}
