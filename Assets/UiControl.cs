using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Vuforia;

public class UiControl : MonoBehaviour {

    public Dictionary<string, List<GameObject>> myLegoDict = new Dictionary<string, List<GameObject>>();
	public float scalingspeed = 0.01f;
	public bool  scaleUp      = false;
	public bool  scaleDown    = false;
	public int   step         = 0;

	// Use this for initialization
	void Start ()
    {
        step = 0;
        UpdateStepCounter();
    }

	// Update is called once per frame
	void Update () {
		if(scaleUp == true)
		{
			ScaleUpButton();
		}

        if (scaleDown == true)
        {
			ScaleDownButton();
		}
	}

	public void ScaleUpButton()
    {
		GameObject.FindWithTag("step_0").transform.localScale += new Vector3(scalingspeed,scalingspeed,scalingspeed);
	}

	public void ScaleDownButton()
    {
		GameObject.FindWithTag("step_0").transform.localScale -= new Vector3(scalingspeed,scalingspeed,scalingspeed);
	}

	public void PreviousStep()
	{
        if(step > 0)
        {
            step -= 1;
            UpdateStepCounter();

            // Select all GameObjects from next step and set active to false
            string tag = string.Format("step_{0}", step + 1);
            print(tag);
            myLegoDict[tag].ForEach(go => go.SetActive(false));

            // Set view again
            if (step == 0)
            {
               // myLegoDict["step_0"][0].SetActive(true);
            }
        }
	}

	public void NextStep()
	{
        string tag;
        if (myLegoDict.Count == 0)
        {
            // Initialize all GameObjects as inactive
            for (int i = 0; i <= 11; i++)
            {
                tag = string.Format("step_{0}", i);
                print("Init: " + tag);
                foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag).ToList())
                {
                    if (!myLegoDict.ContainsKey(tag))
                    {
                        myLegoDict.Add(tag, new List<GameObject>());
                    }

                    myLegoDict[tag].Add(go);

                    go.SetActive(false);
                }
            }

            // Enable Full Rocket View
            //myLegoDict["step_0"][0].SetActive(true);
        }

        step += 1;
        UpdateStepCounter();

        // Disable full rocket view
        if (step > 0)
        {
            //myLegoDict["step_0"][0].SetActive(false);
        }

        // Select all GameObjects from current step and set active to true
        tag = string.Format("step_{0}", step);
        print(tag);
        myLegoDict[tag].ForEach(go => go.SetActive(true));

	}

	public void UpdateStepCounter()
	{
		Text textObject = GameObject.FindWithTag("step_counter").GetComponent<Text>();
		textObject.text = "ETAPA " + step.ToString();
	}
}
