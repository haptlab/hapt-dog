using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAgent : Agent {
	public GameObject body;
	float beforeZ;

	public override List<float> CollectState()
	{
		List<float> state = new List<float>();

		//state.Add(body.transform.localPosition.z);
		state.Add(body.transform.localRotation.eulerAngles.x);
		state.Add(body.transform.localRotation.eulerAngles.y);
		state.Add(body.transform.localRotation.eulerAngles.z);

		Body b = body.GetComponent<Body> ();
		if (b != null) {
			Monitor.Log ("LegLeft", b.currentPositionLegLeft / 90.0f, MonitorType.slider, body.gameObject.transform);
			Monitor.Log ("LegRigth", b.currentPositionLegRight / 90.0f, MonitorType.slider, body.gameObject.transform);
		}

		return state;
	}

	public override void AgentStep(float[] act)
	{
		// アクションする
		if (brain.brainParameters.actionSpaceType == StateType.continuous) {
			float actionLeft = act[0];
			if (actionLeft > 1f)
			{
				actionLeft = 1f;
			}
			if (actionLeft < -1f)
			{
				actionLeft = -1f;
			}
			actionLeft *= 90.0f;

			float actionRight = act[1];
			if (actionRight > 1f)
			{
				actionRight = 1f;
			}
			if (actionRight < -1f)
			{
				actionRight = -1f;
			}
			actionRight *= 90.0f;

			Debug.Log (act[0] + ", " + act[1]);

			Body b = body.GetComponent<Body> ();
			if (b != null) {
				b.setHinge (actionLeft, actionRight);
			}
		}

		// 報酬決める
		// 遠くに行けば行くほど良い
//		if (body.transform.localPosition.z > 1) {
//			done = true;
//			reward = 1f;
//		} else {
//			float positionDiffZ = body.transform.localPosition.z - beforeZ;
//			beforeZ = body.transform.localPosition.z;
//			if (positionDiffZ < -1) {
//				reward = -1f;
//			} else if (positionDiffZ > 1) {
//				reward = 1f;
//			} else {
//				reward = positionDiffZ;
//			}
//		}
//
//
//		// ひっくり返ったら駄目
//		if (body.transform.localRotation.eulerAngles.x > -100 && body.transform.localRotation.eulerAngles.x < -80) {
//			done = true;
//			reward = -1f;
//		}

		//Debug.Log (body.transform.localRotation.eulerAngles.x);

		// ひっくり返ったらOK
		if (done == false) {
			float rotateX = body.transform.localRotation.eulerAngles.x;
			if (rotateX > 260 && rotateX < 280) {
				done = true;
				reward = 1f;
			} else if (rotateX > 180 && rotateX <= 260) {
				reward = (rotateX - 180) / 90f * 0.5f; //0.5f;
				if (reward > 1.0f) {
					reward = 1.0f;
				}
			} else if (rotateX >= 280 && rotateX <= 360) {
				reward = (360 - rotateX) / 90f * 0.5f; //0.5f;
				if (reward > 1.0f) {
					reward = 1.0f;
				}
			} else {
				reward = -1f;
			}
			Monitor.Log ("Reward", reward, MonitorType.slider, body.gameObject.transform);
		}
	}

	public override void AgentReset()
	{
		Body b = body.GetComponent<Body> ();
		if (b != null) {
			b.resetPoistion ();
		}
	}

	public override void AgentOnDone()
	{

	}
}
