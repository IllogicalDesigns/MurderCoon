using UnityEngine;
using System.Collections;

public class HealthBarLook : MonoBehaviour {
	[SerializeField] private Transform camTrans;
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (camTrans.position);
	}
}
