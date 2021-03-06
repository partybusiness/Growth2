﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrower : MonoBehaviour {

	public Mesh vineMesh;
	private MeshFilter mf;
	private MeshRenderer mr;

	[SerializeField]
	Material vineMaterial;

	List<VineSegment> vineSegments;

	List<Vector3> vertices;
	List<Vector2> uvs;
	List<int> triangles;
	List<Color> vertexColours;

	[SerializeField]
	float growthRate = 2f;

	[SerializeField]
	Leaf leafPrefab;

	[SerializeField]
	Flower flower;

	public float flowerScale = 1f;

	[HideInInspector]
	public float plantSeed = 0f;

	[SerializeField]
	float seedMult = 6f;

	[HideInInspector]
	public float growthMult = 1f;

	float tipSeed = 0f;

	float tipCounter = 0f;

	public float maxWidth = 0.35f;

	[HideInInspector]
	public int leafCounter = -5;

	[SerializeField]
	float tipSpeed = 60f;

	[HideInInspector]
	public float seedSpeed = 0.3f;

	[SerializeField]
	float maxAngle = 10f;

	[SerializeField]
	float stemThickness = 0.35f;

	[SerializeField]
	AnimationCurve growthCurve;

	[SerializeField]
	AnimationCurve straightenCurve;

	[SerializeField]
	Color plantColour;

	[SerializeField]
	AudioClip startSound;

	[SerializeField]
	AudioClip leafSound;

	[SerializeField]
	AudioClip flowerSound;

	float goalAngle = 0f;

	static float movementSpeed = 1f;

	const float movementChange = 0.1f;

	AudioSource audioSource;

	void Start () {
		vineMesh = new Mesh ();
		mf = gameObject.AddComponent<MeshFilter> ();
		mr = gameObject.AddComponent<MeshRenderer> ();
		mr.sharedMaterial = vineMaterial;
		mf.sharedMesh = vineMesh;
		vertices = new List<Vector3> ();
		vertexColours = new List<Color> ();
		triangles = new List<int> ();
		vineSegments = new List<VineSegment> ();
		uvs = new List<Vector2> ();
		FirstSegment ();
		AddSegment (tipSeed, Leaf.LeafSide.none);
		movementSpeed += movementChange;
	}

	public void SetAudio(int note, float velocity) {
		return;
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.bypassEffects = true;
		audioSource.bypassListenerEffects = true;
		audioSource.bypassReverbZones = true;
		audioSource.dopplerLevel = 0;
		audioSource.playOnAwake = false;
		audioSource.loop = false;
		audioSource.spatialBlend = 0;

		audioSource.pitch = Mathf.Pow (1.05946f, note);
		audioSource.volume = velocity*0.1f;

		//audioSource.PlayOneShot (startSound);
	}

	private void FirstSegment() {
		vineSegments.Add (new VineSegment ());
		vertices.Add (new Vector3 ());
		vertices.Add (new Vector3 ());
		vertexColours.Add (Color.black);
		vertexColours.Add (Color.black);
		uvs.Add (new Vector2 ());
		uvs.Add (new Vector2 ());
	}

	private void AddSegment(float currentSeed, Leaf.LeafSide addLeaf) {
		var newSegment = new VineSegment ();
		vineSegments.Add ( newSegment );
		//set angle based on perlin noise?
		newSegment.angle = (Mathf.PerlinNoise (plantSeed, currentSeed) -0.5f) * maxAngle;
		newSegment.maxWidth = stemThickness;
		newSegment.leafSide = addLeaf;
		switch (addLeaf) {
		case Leaf.LeafSide.left:
		case Leaf.LeafSide.right:
			var newLeaf = Instantiate (leafPrefab, transform);
			//audioSource.PlayOneShot (leafSound);
			newLeaf.leafSide = addLeaf;
			newLeaf.scale = 0f;
			newSegment.leaf = newLeaf;
			newLeaf.colour = plantColour;
			break;
		}

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
		vertexColours.Add (plantColour);
		vertexColours.Add (plantColour);
		//two uvs
		uvs.Add (new Vector2 (0,0));
		uvs.Add (new Vector2 (1,0));
	}

	private void GenVine() {
		var pos = Vector3.zero;
		var direction = Vector3.up;
		var v = 0f;
		int i = 0;
		Quaternion rightAngle = Quaternion.Euler (0, 0, -90f);
		Quaternion leftAngle = Quaternion.Euler (0, 0, 90f);
		for (i = 0; i < vineSegments.Count-1; i++) {
			var segment = vineSegments [i];
			vertices [i * 2] = pos + leftAngle * direction * segment.width;
			vertices [i * 2 + 1] = pos + rightAngle * direction * segment.width;
			switch (segment.leafSide) {
			case Leaf.LeafSide.left:
				segment.leaf.SetPosition (vertices [i * 2], direction);
				break;
			case Leaf.LeafSide.right:
				segment.leaf.SetPosition (vertices [i * 2 + 1], direction);
				break;
			}
			pos += direction * segment.length;
			uvs [i * 2] = new Vector2(0,v);
			uvs [i * 2+1] = new Vector2(1,v);
			v += 1f;
			direction = Quaternion.Euler (0, 0, segment.angle) * direction;
		}
		//last segment
		vertices [i * 2] = pos + leftAngle * direction * vineSegments [i].width;
		vertices [i * 2 + 1] = pos + rightAngle * direction * vineSegments [i].width;
		switch (vineSegments [i].leafSide) {
		case Leaf.LeafSide.left:
			vineSegments [i].leaf.SetPosition (vertices [i * 2], direction);
			break;
		case Leaf.LeafSide.right:
			vineSegments [i].leaf.SetPosition (vertices [i * 2 + 1], direction);
			break;
		}
		flower.SetPosition ((vertices [i * 2] + vertices [i * 2+1])/2f, direction);
		flower.SetScale (Mathf.Clamp01(tipSeed) * flowerScale);
		if (flower.SetGrowth (1f - growthRate)) {
			//audioSource.PlayOneShot (flowerSound);
		}
		uvs [i * 2] = new Vector2(0,v);
		uvs [i * 2+1] = new Vector2(1,v);
		//if position is too low and pointing down, add bend towards oppsite direction

		WrapUp ();
	}

	private void GrowVines() {
		growthRate = Mathf.Max(0f,growthCurve.Evaluate (tipSeed)) * growthMult;
		if (growthRate == 0f)
			return;
		for (int i = 0; i < vineSegments.Count; i++) {
			var segment = vineSegments [i];
			segment.StraightenStem (Time.deltaTime * growthRate * straightenCurve.Evaluate(i*1f/vineSegments.Count), goalAngle);
			segment.GrowWidth (Time.deltaTime * growthRate * 0.2f);
			segment.GrowLength (Time.deltaTime * growthRate);
			segment.GrowLeaf (Time.deltaTime * growthRate * 0.7f);
		}
		tipCounter += Time.deltaTime * tipSpeed * growthRate * 0.3f;
		if (growthRate >0.1f && tipCounter >1f) {
			tipCounter -= 1f;
			Leaf.LeafSide newLeaf = Leaf.LeafSide.none;
			leafCounter++;
			if (leafCounter == 6) newLeaf = Leaf.LeafSide.left;
			if (leafCounter == 7) {
				newLeaf = Leaf.LeafSide.right;
				leafCounter = Random.Range (-4, 0);
			}
			AddSegment ( tipSeed * seedMult, newLeaf);
			//if leaf counter>1f ??
		}
		tipSeed += Time.deltaTime*seedSpeed;

		//growthRate *= 1f - Time.deltaTime * 0.05f;
	}

	private void WrapUp() {
		vineMesh.SetVertices (vertices);
		vineMesh.SetTriangles (triangles, 0);
		vineMesh.SetUVs (0, uvs);
		vineMesh.SetColors (vertexColours);
		vineMesh.RecalculateNormals ();
		vineMesh.RecalculateTangents ();
		vineMesh.RecalculateBounds ();
	}

	public void SetFlowerColour(Color newColour) {
		flower.SetColor (newColour);
	}

	public void MoveBack() {
		transform.Translate (Vector3.forward * Time.deltaTime * movementSpeed);
		//at some 
		if (transform.position.z > 35f) {
			movementSpeed -= movementChange;
			Destroy (gameObject);
		}
	}

	void Update () {
		MoveBack ();
		GrowVines ();
		if (growthRate == 0f)
			return;
		GenVine ();
	}
}
