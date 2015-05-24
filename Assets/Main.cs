﻿using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public GameObject sphere;
	public GameObject arrow;
	public GameObject helper;
	public GameObject goal;
	public GameObject ground;
	public states state;
	static SphereMovement sphereScript;
	static Logger logger;
	static Trials trials;


	public enum states
	{
		STARTSCREEN,
		INTRO,
		TRAINING,
		TESTING,
		PAUSE,
		END
	}

	// Use this for initialization
	void Start () {
		logger = Logger.Instance;
		sphereScript = sphere.GetComponent<SphereMovement>();
		trials = Trials.Instance;
		trials.CreateTrials();
		logger.Write("\n" + System.DateTime.Now + " New Blog of " + trials.currentTrial.type + " trials.\n");
		switchState(states.INTRO);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && state == states.END)
		{
			Application.Quit();
		}	
	}

	void switchState(states newState)
	{
		switch (newState)
		{
		case states.PAUSE:
			arrow.renderer.enabled = false;
			goal.renderer.enabled = false;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			break;
		case states.INTRO:
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.STARTSCREEN:
			arrow.renderer.enabled = false;
			goal.renderer.enabled = false;
			ground.renderer.enabled = false;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			break;
		case states.TESTING:
			arrow.renderer.enabled = false;
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.TRAINING:
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			break;
		case states.END:
			arrow.renderer.enabled = false;
			goal.renderer.enabled = false;
			ground.renderer.enabled = false;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			logger.CloseLogFile();
			break;
		}
		state = newState;
	}	

	public IEnumerator newTrial()
	{
		Trials.typeOfTrial oldType = trials.currentTrial.type;
		switchState(states.PAUSE);
		yield return new WaitForSeconds(Parameters.pauseBetweenTrials);
		trials.NextTrial();
		if (oldType != trials.currentTrial.type)
		{
			logger.Write("\n" + System.DateTime.Now + " New Blog of " + trials.currentTrial.type + " trials.\n");
		}
		logger.Write(System.DateTime.Now + " New " + trials.currentTrial.type + " trial.\n");  
		goal.transform.position = trials.currentTrial.position;
		switch (trials.currentTrial.type){
		case Trials.typeOfTrial.INTRO:
			switchState(states.INTRO);
			break;
		case Trials.typeOfTrial.TESTING:
			switchState(states.TESTING);
			break;
		case Trials.typeOfTrial.TRAINING:
			switchState(states.TRAINING);
			break;
		case Trials.typeOfTrial.END:
			logger.Write("\n" + System.DateTime.Now + " Experimend ended");
			switchState(states.END);
			break;
		}
	}
}
