using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

	// Use this for initialization
	void Start()
	{

	}

	private void FixedUpdate()
	{
		float x = Input.GetAxis("Vertical");

		Vector3 pos = transform.position;
		pos.x += x * 0.5f;
		transform.position = pos;
	}

	// Update is called once per frame
	void Update()
	{

	}
}