using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public float scalingspeed = 0.01f;
	bool scaleUp = false;
	bool scaleDown = false;

	// Update is called once per frame
	void Update () {
		if(scaleUp == true)
		{
			scaleUpButton();
		}
		if(scaleDown == true){
			scaleDownButton();
		}
	}

	public void scaleUpButton(){
		GameObject.FindWithTag("Lego").transform.localScale += new Vector3(scalingspeed,scalingspeed,scalingspeed);
	}

	public void scaleDownButton(){
		GameObject.FindWithTag("Lego").transform.localScale -= new Vector3(scalingspeed,scalingspeed,scalingspeed);
	}

	public void up()
	{
		scaleUp = true;
		scaleDown = false;
	}

	public void down()
	{
		scaleDown = true;
		scaleUp = false;
	}

	public void stop()
	{
		scaleUp = false;
		scaleDown = false;
	}
}
