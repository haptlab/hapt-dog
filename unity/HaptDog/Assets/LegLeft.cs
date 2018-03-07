using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegLeft : MonoBehaviour {
	// ヒンジのジョイント。サーボモータっぽく動かせるようにする
	HingeJoint joint;

	// 現在のポジション。targetPositionに向けてcurrentPositionを動かしていく
	float currentPosition;

	// 目的のポジション。これを外部からいじる。
	float targetPosition;

	// 前回jointのposition設定した時間を保存する場所
	float beforeTime;

	// 初期化
	void Start () {
		joint = gameObject.GetComponent<HingeJoint>();
		currentPosition = 0.0f;
		targetPosition = 0.0f;
		beforeTime = Time.time;
	}

	private void FixedUpdate()
	{
		// 上下キーの情報ゲット。何もしないと0。上を押し続けると最大+1.0に。下を押し続けると最小で-1.0に。
		float x = Input.GetAxis("Vertical");

		// 上下キーの状態によって目的ポジションを変更
		targetPosition += x * 3.0f;

		// サーボモータの角度制限を-90〜+90度にする
		if (targetPosition > 90.0f) {
			targetPosition = 90.0f;
		} else if (targetPosition < -90.0f) {
			targetPosition = -90.0f;
		}

		// サーボの回転速度は一定なので、定速で回転するようにする
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

		// currentPositionをヒンジジョイントのSpringのtargetPoistionに設定
		JointSpring hingeSpring = joint.spring;
		hingeSpring.targetPosition = currentPosition;
		joint.spring = hingeSpring;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
