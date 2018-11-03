﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrower : MonoBehaviour {

	public Mesh vineMesh;
	private MeshFilter mf;
	private MeshRenderer mr;

	[SerializeField]
	Material vineMaterial;

	[SerializeField]
	List<VineSegment> vineSegments;

	[SerializeField]
	List<Vector3> vertices;
	List<Vector2> uvs;
	[SerializeField]
	List<int> triangles;

	[SerializeField]
	float growthRate = 2f;

	void Start () {
		vineMesh = new Mesh ();
		mf = gameObject.AddComponent<MeshFilter> ();
		mr = gameObject.AddComponent<MeshRenderer> ();
		mr.sharedMaterial = vineMaterial;
		mf.sharedMesh = vineMesh;
		vertices = new List<Vector3> ();
		triangles = new List<int> ();
		vineSegments = new List<VineSegment> ();
		FirstSegment ();
		AddSegment ();
	}

	private void FirstSegment() {
		vineSegments.Add (new VineSegment ());
		vertices.Add (new Vector3 ());
		vertices.Add (new Vector3 ());
	}

	private void AddSegment() {
		vineSegments.Add (new VineSegment ());
		//two triangles
		triangles.Add (vertices.Count);
		triangles.Add (vertices.Count-1);
		triangles.Add (vertices.Count-2);

		triangles.Add (vertices.Count);
		triangles.Add (vertices.Count+1);
		triangles.Add (vertices.Count-1);
		//two vertices
		vertices.Add (new Vector3 ());
		vertices.Add (new Vector3 ());
	}

	private void GenVine() {
		var pos = Vector3.zero;
		var direction = Vector3.up;
		int i = 0;
		Quaternion rightAngle = Quaternion.Euler (0, 0, -90f);
		Quaternion leftAngle = Quaternion.Euler (0, 0, 90f);
		for (i = 0; i < vineSegments.Count-1; i++) {
			var segment = vineSegments [i];
			vertices [i * 2] = pos + leftAngle * direction * segment.width;
			vertices [i * 2 + 1] = pos + rightAngle * direction * segment.width;
			pos += direction * segment.length;
			direction = Quaternion.Euler (0, 0, segment.angle) * direction;
		}
		//last segment
		vertices [i * 2] = pos + Vector3.left * vineSegments [i].width;
		vertices [i * 2 + 1] = pos + Vector3.right * vineSegments [i].width;

		WrapUp ();
	}

	private void GrowVines() {
		for (int i = 0; i < vineSegments.Count; i++) {
			var segment = vineSegments [i];
			segment.Grow (Time.deltaTime * growthRate);
		}
		if (vineSegments [vineSegments.Count - 1].width > vineSegments [0].maxWidth * 0.25f) {
			AddSegment ();
		}
		growthRate *= 1f - Time.deltaTime * 0.01f;
	}

	private void WrapUp() {
		vineMesh.SetVertices (vertices);
		vineMesh.SetTriangles (triangles, 0);
		vineMesh.RecalculateNormals ();
		vineMesh.RecalculateBounds ();
	}

	void Update () {
		GrowVines ();
		GenVine ();
	}
}
