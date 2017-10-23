using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class PanelManager : MonoBehaviour, IManager {

	public ManagerState currentState { get; private set; }


	private PanelConfig rightPanel;
	private PanelConfig leftPanel;
	private NarrativeEvent currentEvent;
	private bool leftCharacterActive = true;
	private int stepIndex = 0;

	public void BootSequence() {
		Debug.Log (string.Format ("{0} is booting up", GetType ().Name));

		rightPanel = GameObject.Find ("RightCharacterPanel").GetComponent<PanelConfig> ();
		leftPanel = GameObject.Find ("LeftCharacterPanel").GetComponent<PanelConfig> ();
		currentEvent = JSONAssembly.RunJSONFactoryForScene (1);
		InitializePanels ();

		Debug.Log (string.Format ("{0} status = 1", GetType().Name, currentState));
	}

	private void InitializePanels (){
		leftPanel.CharacterIsTalking = true;
		rightPanel.CharacterIsTalking = false;
		leftCharacterActive = !leftCharacterActive;

		leftPanel.Configure (currentEvent.dialogues [stepIndex]);
		rightPanel.Configure (currentEvent.dialogues [stepIndex + 1]);

		MasterManager.animationManager.IntroAnimation ();

		stepIndex++;
	}


	private void ConfigurePanels () {
		if (leftCharacterActive) {
			leftPanel.CharacterIsTalking = true;
			rightPanel.CharacterIsTalking = false;

			leftPanel.Configure (currentEvent.dialogues [stepIndex]);
			rightPanel.ToggleCharacterMask ();
		} else {
			leftPanel.CharacterIsTalking = false;
			rightPanel.CharacterIsTalking = true;

			leftPanel.ToggleCharacterMask ();
			rightPanel.Configure (currentEvent.dialogues [stepIndex]);

		}

	}
}
