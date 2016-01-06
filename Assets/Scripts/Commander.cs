using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Commander : MonoBehaviour
{
	public List<BaseUnit> ourGuys = new List<BaseUnit> ();
	public Camera camera;

	public Texture2D selectHighlight = null;
	public static Rect selection = new Rect (0, 0, 0, 0);
	private Vector3 startClick = -Vector3.one;

	private void CheckCamera ()
	{
		if (Input.GetMouseButtonDown (0)) {
			startClick = Input.mousePosition;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.tag == "Player") {	//FIX_ME\/
					BaseUnit tmpUnit = hit.collider.gameObject.GetComponent<BaseUnit> ();
					ourGuys.Add (tmpUnit);
				}
			}
		}
		else if (Input.GetMouseButtonUp (0)) {
			startClick = -Vector3.one;
		}
		if (Input.GetMouseButton (0)) {
			selection = new Rect (startClick.x, invertMouseY (startClick.y), Input.mousePosition.x - startClick.x, invertMouseY (Input.mousePosition.y) - invertMouseY (startClick.y));
			if (selection.width < 0) {
				selection.x += selection.width;
				selection.width = -selection.height;
			}
			if (selection.height < 0) {
				selection.y += selection.height;
				selection.height = -selection.height;
			}
		}
	}

	public static float invertMouseY (float y)
	{
		return Screen.height - y;
	}

	private void MoveUnits ()
	{
		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.tag != "Player") {
					for (int i = 0; i < ourGuys.Count; i++) {
						ourGuys [i].SetMyTarget (hit.point);
					}
				}
				if (hit.collider.tag != "Enemy") {
					for (int i = 0; i < ourGuys.Count; i++) {
						ourGuys [i].KillThisGuy (hit.collider);
					}
				}
			}
		}
	}

	private void OnGUI ()
	{
		if (startClick != -Vector3.one) {
			GUI.color = new Color (1, 1, 1, 0.5f);
			GUI.DrawTexture (selection, selectHighlight);
		}
	}
	
	// Update is called once per frame
	private void Update ()
	{
		CheckCamera ();
		MoveUnits ();
	}
}
