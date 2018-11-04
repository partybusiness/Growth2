using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollenParticles : MonoBehaviour {

	ParticleSystem system;

	ParticleSystem.EmitParams particleEmit;

	public static PollenParticles instance;

	void Start () {
		system = GetComponent<ParticleSystem> ();
		particleEmit = new ParticleSystem.EmitParams ();
		particleEmit.startLifetime = 0.5f;
		particleEmit.startSize = 0.1f;
		instance = this;
	}

	public void EmitPollen(Vector3 position, Vector3 direction, Color color) {
		particleEmit.position = position;
		particleEmit.velocity = Quaternion.Euler(0,0,Random.Range(-45f,45f))* (direction + Random.insideUnitSphere * 0.5f)*Random.Range(4f,7f);
		particleEmit.startColor = color;
		particleEmit.startSize = Random.Range(0.06f,0.25f);
		particleEmit.startLifetime = Random.Range (0.2f, 0.5f);
		system.Emit(particleEmit,1);
	}

}
