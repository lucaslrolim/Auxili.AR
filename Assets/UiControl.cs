using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public float scalingspeed = 0.01f;
	bool scaleUp = false;
	bool scaleDown = false;
	public int step = 1;

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

	public void previousStep()
	{
        if(step>1)
        {
            step -= 1;
            updateStepCounter();
        }
	}
	public void nextStep()
	{
		step += 1;
		updateStepCounter();
	}

	public void updateStepCounter()
	{
		Text textObject = GameObject.FindWithTag("step_counter").GetComponent<Text>();
		textObject.text = "ETAPA " + step.ToString();
	}
}
