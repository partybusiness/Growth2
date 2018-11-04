using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class PlantSpawner : MonoBehaviour {

	[SerializeField]
	Vector3 minPos;

	[SerializeField]
	Vector3 maxPos;

	[SerializeField]
	PlantGrower plantPrefab;

	PlantGrower lastPlant;

	Dictionary<KeyCode, PlantGrower> lastPlants;

	KeyCode[] allKeys;

	// Use this for initialization
	void Start () {
		lastPlants = new Dictionary<KeyCode, PlantGrower> ();
		allKeys = new KeyCode[] {
			KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F
		};
	}
	
	// Update is called once per frame
	void Update () {

		if (MidiMaster.GetKeyDown (3)) {
		}
		foreach (var key in allKeys) {
			if (Input.GetKeyDown(key)) {
				var newPlant = Instantiate (plantPrefab);
				newPlant.plantSeed = Random.Range (0f, 20f);
				newPlant.seedSpeed = 0.3f;
				newPlant.flowerScale = Random.Range (0.5f, 1.5f);
				newPlant.SetFlowerColour(Random.ColorHSV(0f,0.2f,0.6f,1f,0.7f,1f));
				//newPlant.leafSpacing = Random.Range (-8, -3);
				newPlant.leafCounter = Random.Range (-10, -3);
				newPlant.transform.position = Vector3.Lerp(minPos,maxPos, Random.Range(0f,1f));
				lastPlants.Add(key, newPlant);
			}
			if (Input.GetKeyUp (key)) {
				if (lastPlants.ContainsKey(key) && lastPlants[key]!=null) {
					lastPlants[key].seedSpeed = 0.9f;
					lastPlants.Remove(key);
				}
			}
		}

	}
}
