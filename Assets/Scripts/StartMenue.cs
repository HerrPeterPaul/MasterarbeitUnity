﻿/**
 * ReachOut 2D Experiment
 * Axel Schaffland
 * aschaffland@uos.de
 * SS2015
 * Neuroinformatics
 * Institute of Cognitive Science
 * University of Osnabrueck
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Start menue. Handles startmenue. checks for corret user input and send it to the logger instance. calls main script  to start experiment
/// </summary>
public class StartMenue : MonoBehaviour {

	public Button startExperiment;
	public InputField prename;
	public InputField surname;
	public InputField age;
	public InputField participantId;
	public Toggle male;
	public Toggle female;
	public ToggleGroup tg;
	public GameObject helper;
	Main main;
	Logger log;


	void Start () {
		main = helper.GetComponent<Main>();
		log = Logger.Instance;
		//bind listeners to the events of UI elements
		startExperiment.onClick.AddListener(() => { main.startExperimentPressed(); });
		prename.onEndEdit.AddListener(preListener);
		surname.onEndEdit.AddListener (surListener);
		age.onEndEdit.AddListener (ageListener);
		participantId.onEndEdit.AddListener (pIDListener);
		male.onValueChanged.AddListener(maleListener);
		female.onValueChanged.AddListener(femaleListener);
	
	}

	void preListener(string s)
	{
		log.prename = prename.text;
	}


	void surListener(string s)
	{
		log.surname = surname.text;
	}

	void pIDListener(string s)
	{
		log.participantID = participantId.text;
	}

	void ageListener(string s)
	{
		uint a;
		if (uint.TryParse (age.text, out a)) {
			log.age = a;
		} 
		else 
		{
			age.text = "";
			age.placeholder.GetComponent<Text>().text = "Not a number";
		}
	}

	void maleListener(bool v)
	{
		if (v)
		{
			log.gender = Logger.genderEnum.MALE;
			female.isOn = false;
		}
		else
		{
			log.gender = Logger.genderEnum.FEMALE;
			female.isOn = true;
		}
	}

	void femaleListener(bool v)
	{
		if (v)
		{
			log.gender = Logger.genderEnum.FEMALE;
			male.isOn = false;
		}
		else
		{
			log.gender = Logger.genderEnum.MALE;
			male.isOn = true;
		}
	}
}
