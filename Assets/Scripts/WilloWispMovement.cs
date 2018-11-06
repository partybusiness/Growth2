using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilloWispMovement : MonoBehaviour {

    [SerializeField]
    protected WilloWispMovement nextWilloWisp;

    [SerializeField]
    Rect bounds;


    void Start () {
		
	}

	void Update () {
		var tempMove = new Vector3 (
			(Mathf.PerlinNoise (0f, Time.time*0.1f) - 0.5f)*3f,
			(Mathf.PerlinNoise (5.3f, Time.time*0.1f) - 0.5f)*0.7f,
			               0);
        if (tempMove.magnitude > 0.35f)
            transform.Translate(tempMove * Time.deltaTime * 5f);
        else
            return;

        var pos = transform.position;

        if (!bounds.Contains(pos))
        {
            Instantiate(nextWilloWisp);
            Destroy(gameObject);
        }

	}
}
