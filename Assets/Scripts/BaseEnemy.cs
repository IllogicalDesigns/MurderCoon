using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour {
	bool selected = false;
	public Vector3 target;
	[SerializeField] private Renderer myRend;
	[SerializeField] private SpriteRenderer mySpriteRend;
	[SerializeField] private NavMeshAgent myAgent;
	[SerializeField] private Commander myCommander;
	[SerializeField] private float myFOV = 180f;
	//Differ this based on skill
	bool firstSelected = true;
	bool playerInSight = false;
	[SerializeField] bool holdFire = false;
	[SerializeField] private SphereCollider myCol;
	bool isAttacking = false;
	[SerializeField] private float attakTimer = 0.5f;
	[SerializeField] private float reloadimer = 0.25f;
	[SerializeField] private AudioClip testGun;
	[SerializeField] private AudioClip testReload;
	[SerializeField] private AudioClip testOutOfAmmo;
	[SerializeField] private ParticleSystem testShot;
	[SerializeField] private float clipCap = 1f;
	private float clipCurrCap;
	[SerializeField] private float ammoCap = 32f;
	private float ammoCurrCap;
	bool outOfAmmo = false;
	[SerializeField] private float damDice = 12;
	float modiAmout = 2f;

	private void Start ()
	{
		clipCurrCap = clipCap;
		ammoCurrCap = ammoCap;
	}

	IEnumerator Attack (Collider who, float attkTime, float modifiers, float damageDice)
	{
		bool tmpHit = false;
		Health tmpHealth = who.GetComponent<Health> ();
		float tmpFloat = (Random.Range (1, 20) + modifiers);
		if (tmpFloat >= tmpHealth.amourClass)
			tmpHit = true;
		isAttacking = true;
		AudioSource.PlayClipAtPoint (testGun, transform.position, 1f);
		testShot.Emit (10);
		yield return new WaitForSeconds (attkTime);
		if (tmpHit && tmpHealth != null) 
			tmpHealth.ApplyDamage ((Random.Range (1, damageDice) + modifiers));
		clipCurrCap--;
		isAttacking = false;
	}

	IEnumerator Reload (float reloadTime)
	{
		isAttacking = true;
		AudioSource.PlayClipAtPoint (testReload, transform.position, 1f);
		yield return new WaitForSeconds (reloadTime);
		if (ammoCurrCap - clipCap > 0) {
			clipCurrCap = clipCap;
			ammoCurrCap -= clipCap;
		} else {
			clipCurrCap = ammoCurrCap;
			ammoCurrCap -= clipCurrCap;
		}
		isAttacking = false;
	}

	void Vision ()
	{
		Collider[] myCols = Physics.OverlapSphere (transform.position, myCol.radius);
		for (int i = 0; i < myCols.Length; i++) {

			// If the player has entered the trigger sphere...
			if (myCols [i].tag == "Player") {
				// By default the player is not in sight.
				playerInSight = false;
				target = Vector3.zero;

				// Create a vector from the enemy to the player and store the angle between it and forward.
				Vector3 direction = myCols [i].transform.position - transform.position;
				float angle = Vector3.Angle (direction, transform.forward);

				// If the angle between forward and where the player is, is less than half the angle of view...
				if (angle < myFOV * 0.5f) {
					RaycastHit hit;

					// ... and if a raycast towards the player hits something...
					if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, myCol.radius)) {
						// ... and if the raycast hits the player...
						if (hit.collider.gameObject.tag == "Player" && !isAttacking && clipCurrCap > 0) {
							Debug.DrawRay (transform.position + transform.up, direction.normalized * 10f, Color.red); 
							playerInSight = true;
							target = hit.collider.transform.position;
							StartCoroutine (Attack (myCols [i], attakTimer, modiAmout, damDice));
						}
					}
				}
			}
		}
	}

	private void Update ()
	{
		if (clipCurrCap == 0 && !isAttacking && ammoCurrCap > 0)
			StartCoroutine (Reload (reloadimer));
		if (ammoCurrCap == 0 && !outOfAmmo && clipCurrCap == 0) {
			Debug.Log ("Out Of Ammo " + this.name);
			outOfAmmo = true;
			AudioSource.PlayClipAtPoint (testOutOfAmmo, transform.position, 1f);
		}
		Vision ();
		if (playerInSight) {
			transform.LookAt (target);
		}
		if (myRend.isVisible && Input.GetMouseButton (0)) {
			Vector3 camPos = Camera.main.WorldToScreenPoint (transform.position);
			camPos.y = Commander.invertMouseY (camPos.y);
			selected = Commander.selection.Contains (camPos);
		}
		if (selected) {
			if (firstSelected) {
				mySpriteRend.enabled = true;
				myCommander.ourGuys.Add (gameObject.GetComponent<BaseUnit> ());
				firstSelected = false;
			}
		} else {
			mySpriteRend.enabled = false;
			myCommander.ourGuys.Remove (gameObject.GetComponent<BaseUnit> ());
			firstSelected = true;
		}
	}
}
