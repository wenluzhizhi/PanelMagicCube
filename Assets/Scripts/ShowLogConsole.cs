using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowLogConsole : MonoBehaviour
{

	public List<string> listLogs=new List<string>();
	public KeyCode keyShow = KeyCode.A;
	void Start () 
	{
		Application.logMessageReceived += HandleLog;
		Application.logMessageReceivedThreaded += HandleLog;

	}
	
	void HandleLog(string message,string stackTrace,LogType type){
		listLogs.Add ("Message:" + message);
	}
	void Update ()
	{
		if (listLogs.Count > 100) {
			listLogs.Clear ();
		}
		if (Input.GetKeyDown (keyShow)) {
			visible = !visible;
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			listLogs.Clear ();
		}
	}

	public static bool visible=false;
	private Rect windowRect = new Rect (10, 10,Screen.width-20, Screen.height-20);
	void OnGUI()
	{
		if (!visible)
			return;
	
		windowRect = GUILayout.Window (0, windowRect, Draw1, "ShowLog");
	}

	Vector2 ScrollPosition;
	void Draw1(int k){

		ScrollPosition = GUILayout.BeginScrollView (ScrollPosition);
		for (int i = 0; i < listLogs.Count; i++) {
			GUILayout.Label (listLogs[i]);
		}
		GUILayout.EndScrollView ();
		GUI.DragWindow(new Rect(0,0,Screen.width,Screen.height));
	}
}
