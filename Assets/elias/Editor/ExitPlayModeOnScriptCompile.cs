// Copyright Cape Guy Ltd. 2015. http://capeguy.co.uk.
// Provided under the terms of the MIT license -
// http://opensource.org/licenses/MIT. Cape Guy accepts
// no responsibility for any damages, financial or otherwise,
// incurred as a result of using this code.

using UnityEngine;
using UnityEditor;

/// <summary>
/// This script exits play mode whenever script compilation is detected during an editor update.
/// </summary>
[InitializeOnLoad] // Make static initialiser be called as soon as the scripts are initialised in the editor (rather than just in play mode).
public class ExitPlayModeOnScriptCompile {
	
	// Static initialiser called by Unity Editor whenever scripts are loaded (editor or play mode)
	static ExitPlayModeOnScriptCompile () {
		Unused (_instance);
		_instance = new ExitPlayModeOnScriptCompile ();
	}
	
	private ExitPlayModeOnScriptCompile () {
		EditorApplication.update += OnEditorUpdate;
	}  
	 
	~ExitPlayModeOnScriptCompile () {
		EditorApplication.update -= OnEditorUpdate; 
		// Silence the unused variable warning with an if.
		_instance = null;
	} 
	
	// Called each time the editor updates.
	private static void OnEditorUpdate () {
		if (EditorApplication.isPlaying && EditorApplication.isCompiling) {
			DemoPlayer[] existingDemoPlayers = MonoBehaviour.FindObjectsOfType<DemoPlayer>();
			for (int i = 0; i < existingDemoPlayers.Length; i++)
			{
				if (existingDemoPlayers[i].useLowLatencyOnAudioFilterRead == false) {
					Debug.Log ("Exiting play mode due to script compilation. Otherwise the editor may crash! (Only applies when useLowLatencyOnAudioFilterRead is not used on a Elias Demo Player)");
					EditorApplication.isPlaying = false; 
					return;
				}
			}
		}
	} 
	 
	// Used to silence the 'is assigned by its value is never used' warning for _instance.
	private static void Unused<T> (T unusedVariable) {}
	
	private static ExitPlayModeOnScriptCompile _instance = null;
}