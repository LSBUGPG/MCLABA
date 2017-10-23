using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerState{
	Offline, Initialising, Completed
}


public interface IManager{

	ManagerState currentState { get; }

	void BootSequence ();
}
