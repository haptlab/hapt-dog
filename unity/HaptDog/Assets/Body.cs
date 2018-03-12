using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {
	// 左右の足
	public GameObject legLeft;
	public GameObject legRight;

	// ヒンジのジョイント。サーボモータっぽく動かせるようにする
	HingeJoint jointLegLeft;
	HingeJoint jointLegRight;

	// 現在のポジション。targetPositionに向けてcurrentPositionを動かしていく
	public float currentPositionLegLeft;
	public float currentPositionLegRight;

	// 目的のポジション。これを外部からいじる。
	float targetPositionLegLeft;
	float targetPositionLegRight;

	// 前回jointのposition設定した時間を保存する場所
	float beforeTime;

	// 初期化
	void Start () {
		jointLegLeft = legLeft.GetComponent<HingeJoint>();
		jointLegRight = legRight.GetComponent<HingeJoint>();
		currentPositionLegLeft = 0.0f;
		currentPositionLegRight = 0.0f;
		targetPositionLegLeft = 0.0f;
		targetPositionLegRight = 0.0f;
		beforeTime = Time.time;
	}

	private void FixedUpdate()
	{
		// 上下キーの情報ゲット。何もしないと0。上を押し続けると最大+1.0に。下を押し続けると最小で-1.0に。
		float x = Input.GetAxis("Vertical");
		float y = Input.GetAxis("Horizontal");

		// 上下左右キーの状態によって目的ポジションを変更
		//setHinge (targetPositionLegLeft + x * 3.0f, targetPositionLegRight + y * 3.0f);

		if (Input.GetKey (KeyCode.Space)) {
			resetPoistion ();
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void resetPoistion() {
		gameObject.transform.localPosition = new Vector3(-0.02505992f, 0.03661419f, 0.06167411f);
		gameObject.transform.localRotation = Quaternion.Euler(90f, 0f, 90f);
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
		gameObject.GetComponent<Rigidbody>().angularVelocity =
			new Vector3(0f, 0f, Random.Range(-0.5f, 0.5f));
		
		legLeft.transform.localPosition = new Vector3(-0.06755992f, 0.01849664f, 0.0376741f);
		legLeft.transform.localRotation = Quaternion.Euler(90f, 0f, -90f);
		legLeft.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

		legRight.transform.localPosition = new Vector3(0.01744008f, 0.01849664f, 0.0376741f);
		legRight.transform.localRotation = Quaternion.Euler(90f, 0f, 90f);
		legRight.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

		currentPositionLegLeft = 0.0f;
		currentPositionLegRight = 0.0f;
		targetPositionLegLeft = 0.0f;
		targetPositionLegRight = 0.0f;
		beforeTime = Time.time;

		setHingeTargetPosition ();
	}

	public void setHinge (float left, float right) {
		targetPositionLegLeft = left;
		targetPositionLegRight = right;

		// サーボモータの角度制限を-90〜+90度にする
		if (targetPositionLegLeft > 90.0f) {
			targetPositionLegLeft = 90.0f;
		} else if (targetPositionLegLeft < -90.0f) {
			targetPositionLegLeft = -90.0f;
		}

		if (targetPositionLegRight > 90.0f) {
			targetPositionLegRight = 90.0f;
		} else if (targetPositionLegRight < -90.0f) {
			targetPositionLegRight = -90.0f;
		}

		// サーボの回転速度は一定なので、定速で回転するようにする
		float diffPosition = (Time.time - beforeTime) * 100.0f;
		beforeTime = Time.time;

		if (Mathf.Abs (targetPositionLegLeft - currentPositionLegLeft) < diffPosition) {
			currentPositionLegLeft = targetPositionLegLeft;
		} else {
			if (targetPositionLegLeft > currentPositionLegLeft) {
				currentPositionLegLeft += diffPosition;
			} else {
				currentPositionLegLeft -= diffPosition;
			}
		}

		if (Mathf.Abs (targetPositionLegRight - currentPositionLegRight) < diffPosition) {
			currentPositionLegRight = targetPositionLegRight;
		} else {
			if (targetPositionLegRight > currentPositionLegRight) {
				currentPositionLegRight += diffPosition;
			} else {
				currentPositionLegRight -= diffPosition;
			}
		}

		setHingeTargetPosition ();
	}

	private void setHingeTargetPosition () {
		// currentPositionをヒンジジョイントのSpringのtargetPoistionに設定
		JointSpring hingeSpringLegLeft = jointLegLeft.spring;
		hingeSpringLegLeft.targetPosition = currentPositionLegLeft;
		jointLegLeft.spring = hingeSpringLegLeft;

		JointSpring hingeSpringLegRight = jointLegRight.spring;
		hingeSpringLegRight.targetPosition = currentPositionLegRight;
		jointLegRight.spring = hingeSpringLegRight;
	}
}
