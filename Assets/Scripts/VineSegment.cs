using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineSegment {

	public float length = 0.0f;
	public float width = 0.0f;
	public float maxWidth = 0.35f;
	public float maxLength = 0.7f;

	public float angle = 0;

	public int leftVIndex = 0;
	public int rightVIndex = 1;

	public Leaf leaf;

	public float leafScale = 0f;

	public Leaf.LeafSide leafSide;

	public VineSegment() {
		angle = Random.Range (-30f, 30f);
	}

	public void GrowWidth(float deltaRate) {
		width = Mathf.MoveTowards(width,maxWidth,deltaRate);
	}

	public void GrowLength(float deltaRate) {
		length = Mathf.MoveTowards(length,maxLength,deltaRate);
	}

	public void GrowLeaf(float deltaRate) {
		if (leafSide == Leaf.LeafSide.none)
			return;
		leafScale = Mathf.MoveTowards(leafScale,1f,deltaRate);
		leaf.scale = leafScale;
	}

	public void StraightenStem(float deltaRate, float goalAngle) {
		angle = ((angle - goalAngle)*(1f - deltaRate)) + goalAngle;
	}
}
