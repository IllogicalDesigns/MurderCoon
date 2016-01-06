using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {
	public float amourClass = 13f;
	[SerializeField] private float myHealth = 100f;
	[SerializeField] private float myMaxHealth = 100f;
	[SerializeField] private Slider hpSlider;
	// Use this for initialization
	void Start () {
		myHealth = myMaxHealth;
		hpSlider.maxValue = myHealth;
	}

	public void ApplyDamage (float dmg) {
		myHealth -= dmg;
		CheckHealth ();
	}

	void CheckHealth () {
		hpSlider.value = myHealth;
		if (myHealth <= 0) {
			myHealth = 0;
			Debug.Log (gameObject.name + " has died");
			Destroy (gameObject);
		}
		if(myHealth >= myMaxHealth) {
			myHealth = myMaxHealth;
		}
	}
}
