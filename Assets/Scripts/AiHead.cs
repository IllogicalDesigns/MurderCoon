using UnityEngine;
using System.Collections;

public class AiHead : MonoBehaviour {
	public Vector3 SightingPos;

	public void SetSightedPosition (Vector3 newSighting) {
		SightingPos = newSighting;
	}
}
