using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegLeft : MonoBehaviour {
	HingeJoint joint;
	float currentPosition;
	float targetPosition;
	float beforeTime;

	// Use this for initialization
	void Start () {
		joint = gameObject.GetComponent<HingeJoint>();
		currentPosition = 0.0f;
		targetPosition = 0.0f;
		beforeTime = Time.time;
	}

	private void FixedUpdate()
	{
		float x = Input.GetAxis("Vertical");

		targetPosition += x * 3.0f;

		if (targetPosition > 90.0f) {
			targetPosition = 90.0f;
		} else if (targetPosition < -90.0f) {
			targetPosition = -90.0f;
		}

		float diffPosition = (Time.time - beforeTime) * 100.0f;
		beforeTime = Time.time;

		if (Mathf.Abs (targetPosition - currentPosition) < diffPosition) {
			currentPosition = targetPosition;
		} else {
			if (targetPosition > currentPosition) {
				currentPosition += diffPosition;
			} else {
				currentPosition -= diffPosition;
			}
		}

		JointSpring hingeSpring = joint.spring;
		hingeSpring.targetPosition = currentPosition;
		joint.spring = hingeSpring;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
