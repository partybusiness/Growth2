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

	private LeafSide _leafSide;

	public LeafSide leafSide {
		get {
			return _leafSide;
		}

		set {
			_leafSide = value;
			switch (value) {
			case LeafSide.left:
				transform.localScale = new Vector3 (-1, 1, 1);
				break;
			case LeafSide.right:
				transform.localScale = new Vector3 (1, 1, 1);
				break;
			}
		}
	}

	public float scale {
		
		set {
			switch (_leafSide) {
			case LeafSide.left:
				transform.localScale = new Vector3 (-1, 1, 1) * value;
				break;
			case LeafSide.right:
				transform.localScale = new Vector3 (1, 1, 1) * value;
				break;
			}
		}
	}

	public void SetPosition(Vector3 position, Vector3 direction) {
		Quaternion rotation = Quaternion.LookRotation (Vector3.forward, direction);
		transform.localRotation = rotation;
		transform.localPosition = position;
	}


}
