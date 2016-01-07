using UnityEngine;
using System.Collections;

public class CommanderMovement : MonoBehaviour {
	public float speed = 5f;
	float v;
	float h;
	float mSW;
	float minY = 5f;
	float maxY = 15f;
	
	// Update is called once per frame
	void Update () {
		v = Input.GetAxis ("Vertical");
		h = Input.GetAxis ("Horizontal");
		mSW = Input.GetAxis ("MouseScrollWheel");
		if (transform.position.y < minY)
			mSW = Mathf.Clamp (mSW, -1, 0);
		if (transform.position.y > maxY)
			mSW = Mathf.Clamp (mSW, 0, 1);
		transform.Translate (new Vector3(-1,0,1) * -h * speed * Time.deltaTime,Space.World);
		transform.Translate (new Vector3(0.5f,0,0.5f) * v * speed * Time.deltaTime,Space.World);
		transform.Translate (Vector3.forward * mSW * 100f * speed * Time.deltaTime);
	}
}
