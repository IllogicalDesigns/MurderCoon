using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SinmpleWinCon : MonoBehaviour {
	public static int Players = 0;
	public Text youLose;
	public static int enemies = 0;
	public Text youWin;

	// Use this for initialization
	void Start () {
		GameObject[] gos1 = GameObject.FindGameObjectsWithTag ("Player");
		Players = gos1.Length;
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Enemy");
		enemies = gos.Length;
		youWin.enabled = false;
		youLose.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Players <= 0)
			youLose.enabled = true;
		if (enemies <= 0)
			youWin.enabled = true;
	}
}
