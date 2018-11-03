using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawner : MonoBehaviour {

	[SerializeField]
	Vector3 minPos;

	[SerializeField]
	Vector3 maxPos;

	[SerializeField]
	PlantGrower plantPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
			var newPlant = Instantiate (plantPrefab);
			newPlant.plantSeed = Random.Range (0f, 20f);
			newPlant.seedSpeed = Random.Range (0.2f, 0.6f);
			newPlant.flowerScale = Random.Range (0.5f, 1.5f);
			newPlant.SetFlowerColour(Random.ColorHSV(0,1,0.6f,1f,0.7f,1f));
			newPlant.transform.position = Vector3.Lerp(minPos,maxPos, Random.Range(0f,1f));
		}
	}
}
