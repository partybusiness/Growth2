using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineSegment {

	public float length = 0.1f;
	public float width = 0.0f;
	public float maxWidth = 0.1f;
	public float maxLength = 1.3f;

	public float angle = 0;

	public int leftVIndex = 0;
	public int rightVIndex = 1;

	public VineSegment() {
		angle = Random.Range (-30f, 30f);
	}

	public void Grow(float deltaRate) {
		width = Mathf.MoveTowards(width,maxWidth,deltaRate * 0.3f);
		length = Mathf.MoveTowards(length,maxLength,deltaRate);
	}

}
