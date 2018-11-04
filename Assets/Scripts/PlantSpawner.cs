using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class PlantSpawner : MonoBehaviour {

	[SerializeField]
	Vector3 minPos;

	[SerializeField]
	Vector3 maxPos;


	Dictionary<int, PlantGrower> lastPlants;

	KeyCode[] allKeys;

	[SerializeField]
	int numberOfKeys=100;

	[SerializeField]
	PlantGrower[] octavePlants;

	[System.Serializable]
	public class ColourRange {
		public Color colorOne;
		public Color colorTwo;
	}

	[SerializeField]
	ColourRange[] colours;

	[System.Serializable]
	public class SimKey {
		public KeyCode key;
		public int keyIndex;
	}

	[SerializeField]
	SimKey[] simKeys;

	// Use this for initialization
	void Start () {
		lastPlants = new Dictionary<int, PlantGrower> ();
		allKeys = new KeyCode[] {
			KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F
		};
	}

	void SpawnFlower(int keyIndex, float velocity) {
		int octave = Mathf.FloorToInt(keyIndex*1f / 12);
		int keyInOctave = keyIndex % 12;

		var newPlant = Instantiate (octavePlants[octave]);
		newPlant.plantSeed = Random.Range (0f, 20f);
		newPlant.seedSpeed = 0.3f;
		newPlant.growthMult = velocity*3f;
		newPlant.flowerScale = Mathf.Lerp (0.5f, 1.5f, velocity); //adjust this according to attack?
		newPlant.SetFlowerColour(Color.Lerp(colours[keyInOctave].colorOne,colours[keyInOctave].colorTwo,Random.Range(0f,1f)));
		//newPlant.leafSpacing = Random.Range (-8, -3);
		newPlant.leafCounter = Random.Range (-10, -3);
		newPlant.transform.position = Vector3.Lerp(minPos,maxPos, 
			Mathf.InverseLerp(36,96,keyIndex)
			+Random.Range(-0.1f,0.1f));//adjust this position?
		lastPlants.Add(keyIndex, newPlant);
	}

	void ReleaseFlower(int keyIndex) {
		if (lastPlants.ContainsKey(keyIndex) && lastPlants[keyIndex]!=null) {
			lastPlants[keyIndex].seedSpeed = 0.9f;
			lastPlants.Remove(keyIndex);
		}
	}

	void CheckKey(int keyIndex) {
		if (MidiMaster.GetKeyDown(keyIndex)) {
			Debug.Log ("Key = " + keyIndex);
			SpawnFlower (keyIndex, MidiMaster.GetKey (keyIndex));
		}
		if (MidiMaster.GetKeyUp (keyIndex)) {
			ReleaseFlower (keyIndex);
		}
	}

	void CheckSimKey(SimKey simkey) {
		if (Input.GetKeyDown(simkey.key)) {
			SpawnFlower (simkey.keyIndex, Random.Range(0f,1f));
		}
		if (Input.GetKeyUp (simkey.key)) {
			ReleaseFlower (simkey.keyIndex);
		}
	}


	void Update () {

		for (int keyIndex = 0; keyIndex < numberOfKeys; keyIndex++) {
			CheckKey (keyIndex);
		}

		foreach (var simKey in simKeys) {
			CheckSimKey (simKey);
		}

	}
}
