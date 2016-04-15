using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;  // List of strings.
using System.IO;  // For getting a list of files.
 
[CustomEditor (typeof(DemoPlayer))]
public class DemoPlayerEditor : Editor {
	private const string ELIASFileSuffix = ".epro";

	// Popup menu member variables.
	// This list will be built by theme finder code that browse the StreamingAssets/ELIAS_Themes-folder.
	public string[] popup_options;
	private int popup_index = 0;

	private string[] prebufferOptions = {"Minimal Latency", "Low Latency", "Medium Latency", "High Latency"};
	private int[] prebufferOptionsValues = {4, 8, 16, 32};
	private int prebufferOptionsIndex = 3; //TODO: What is a good default setting here?
	
	SerializedProperty PrebufferLengthAsMultipleOfDSPBufferProp;
	SerializedProperty ProcessingPeriodAsMultipleOfDSPBufferProp;
	SerializedProperty startUrgencyProp;
	SerializedProperty themeFolderProp;
	SerializedProperty startingActionPresetProp;
	SerializedProperty startRendevousProp;
	SerializedProperty startMinLevelProp;
	SerializedProperty startMaxLevelProp;
	SerializedProperty startStingerProp;
	SerializedProperty startKeyProp;
	SerializedProperty useActionPresetOnStartProp;
	SerializedProperty startThemeOnStartProp;
	SerializedProperty showPendingTriggerChangeInfoProp;

	SerializedProperty loadTypeProp;

	private int FindStringIndex(string str, string[] strings) {
		int result = 0;
		for (int i = 0; i < strings.Length; ++i) {
			if (str == strings[i]) {
				result = i;
				break;
			}
		}
		return result;
	}
	
	private int FindIntIndex(string str, string[] strings) {
		int result = 0;
		for (int i = 0; i < strings.Length; ++i) {
			if (str == strings[i]) {
				result = i;
				break;
			}
		}
		return result;
	}

	void OnDisable()
	{

	}

	void OnEnable() {
		PrebufferLengthAsMultipleOfDSPBufferProp = serializedObject.FindProperty("PrebufferLengthAsMultipleOfDSPBuffer");
		ProcessingPeriodAsMultipleOfDSPBufferProp = serializedObject.FindProperty("ProcessingPeriodAsMultipleOfDSPBuffer");
		startUrgencyProp = serializedObject.FindProperty("startUrgency");
		themeFolderProp = serializedObject.FindProperty("themeFolder");
		startingActionPresetProp = serializedObject.FindProperty ("startActionPreset");
		startRendevousProp = serializedObject.FindProperty ("startRendevous");
		startMinLevelProp = serializedObject.FindProperty ("startMinLevel");
		startMaxLevelProp = serializedObject.FindProperty ("startMaxLevel");
		startStingerProp = serializedObject.FindProperty ("startStinger");
		startKeyProp = serializedObject.FindProperty ("startKey");
		useActionPresetOnStartProp = serializedObject.FindProperty ("useActionPresetOnStart");
		startThemeOnStartProp = serializedObject.FindProperty ("startThemeOnStart");
		loadTypeProp = serializedObject.FindProperty ("loadType");
		showPendingTriggerChangeInfoProp = serializedObject.FindProperty ("showPendingTriggerChangeInfo");
		
		string basePath = Path.Combine(Application.streamingAssetsPath, "ELIAS_Themes");
		popup_options = FindSubDirectories(basePath);
		
		// Each time this editor is enabled (i.e turns up in the inspector), find
		// the currently chosen element.
		popup_index = FindStringIndex(themeFolderProp.stringValue, popup_options);

		prebufferOptionsIndex = ArrayUtility.IndexOf<int> (prebufferOptionsValues, PrebufferLengthAsMultipleOfDSPBufferProp.intValue);
	}

/*
 * This will check and find all epro files that are placed according to the specifications set.
 *	Note that the placment specification is for the DemoPlayer implementation, and to keep it simple for us.
 *	The folder name = .epro name is not something that is required by the engine!
 */
	private string[] FindSubDirectories(string basePath) {
		DirectoryInfo dir = new DirectoryInfo(basePath);
		
		DirectoryInfo[] dirinfo = dir.GetDirectories();
		string[] returnValue = new string[dirinfo.Length];
		
		// Go through each of the found subdirectories ...
		for (int i = 0; i < dirinfo.Length; ++i) {
			DirectoryInfo subdir = new DirectoryInfo(dirinfo[i].FullName);
			FileInfo[] fileInfo = subdir.GetFiles();
			
			// ... and add its name to the return array if 
			// the directory contains an .epro file with the same name.
			foreach (FileInfo f in fileInfo) {
				if (subdir.Name + ELIASFileSuffix == f.Name) {
					returnValue[i] = dirinfo[i].Name;
					break;
				} else if (f.Extension == ELIASFileSuffix) {
					//NOTE: This is only an example, in your own implementation you are NOT restricted to placing the files in this manner!
					Debug.LogWarning(
						"DemoPlayerEditor - Ignoring an " + 
						ELIASFileSuffix + 
						" file with different name than its containing folder:\n" + f.FullName
					);
				}
			}
		}
		
		return returnValue;
	}

	private void DrawSliderProperty(SerializedProperty prop, float minValue, float maxValue, string description) {
		EditorGUILayout.IntSlider (prop, (int)minValue, (int)maxValue, new GUIContent(description));
	}

	private int Math_GetHighestFactor(int a, int b) {
		// Make sure that 'a' is the highest number by swapping them if necessary.
		if (a < b) {
			int c = b;
			b = a;
			a = c;
		}
		return a / b;
	}

	private void ViewThemePopup() {
		EditorGUILayout.PrefixLabel("ELIAS Theme:");
		if (popup_options.Length != 0) {
		popup_index = EditorGUILayout.Popup(popup_index, popup_options);
		themeFolderProp.stringValue = popup_options[popup_index];
		} else {
			EditorGUILayout.HelpBox(
			@"No theme to select! Please copy an ELIAS theme directory into the 'Assets/StreamingAssets/ELIAS_Themes'-folder.
You might have to deselect and select the GameObject for this component for changes to take effect.",
			MessageType.Error);
		}
	}
	
	private void DrawEngineSettings() {
		EditorGUILayout.PrefixLabel("ELIAS Engine Settings");
		
		EditorGUILayout.Separator();
		EditorGUILayout.HelpBox(
			@"The higher latency the more resistant ELIAS will be to disk read and write operations. A to low latency / small DSP buffer (Audio settings) can result in gaps in the audio.",
			MessageType.Info);

		prebufferOptionsIndex = EditorGUILayout.Popup(prebufferOptionsIndex, prebufferOptions);
		PrebufferLengthAsMultipleOfDSPBufferProp.intValue = prebufferOptionsValues[prebufferOptionsIndex];

		//Unity actually asks for 2x it's DSP buffer size every time it wants samples!
		ProcessingPeriodAsMultipleOfDSPBufferProp.intValue = Mathf.Max (2, prebufferOptionsValues[prebufferOptionsIndex] / 2);
		EditorGUILayout.PropertyField (loadTypeProp);
	}
	
	private void DrawThemeSettings() {
		EditorGUILayout.LabelField("ELIAS Theme Start settings");
		startThemeOnStartProp.boolValue = EditorGUILayout.Toggle ("Start theme on Start", startThemeOnStartProp.boolValue);
		startKeyProp.intValue = EditorGUILayout.IntField ("Starting key (Default: -1)", startKeyProp.intValue);
		
		useActionPresetOnStartProp.boolValue = EditorGUILayout.BeginToggleGroup ("Use action preset at start", useActionPresetOnStartProp.boolValue);
		startingActionPresetProp.stringValue = EditorGUILayout.TextField ("Staring action preset", startingActionPresetProp.stringValue);
		EditorGUILayout.EndToggleGroup ();
		
		//Negating both the result and the value shown will make this act as a radio button!
		useActionPresetOnStartProp.boolValue = !EditorGUILayout.BeginToggleGroup ("Manual starting parameters", !useActionPresetOnStartProp.boolValue);
		DrawSliderProperty(startUrgencyProp, 0, 100, "Starting Urgency");
		startMinLevelProp.intValue = EditorGUILayout.IntField ("Starting Min Level", startMinLevelProp.intValue);
		startMaxLevelProp.intValue = EditorGUILayout.IntField ("Starting Max Level", startMaxLevelProp.intValue);
		startStingerProp.intValue = EditorGUILayout.IntField ("Starting Stinger (0 : None)", startStingerProp.intValue);
		startRendevousProp.boolValue = EditorGUILayout.Toggle ("Start using Rendevous", startRendevousProp.boolValue);
		EditorGUILayout.EndToggleGroup ();
	}
	
	/* OnInspectorGUI is used when making custom Editors or custom Inspectors.
	 * 
	 * OnGUI on the other hand is used when making an Editor window, 
	 * 		material property drawer, decorator drawer or property drawers.
	 */
	public override void OnInspectorGUI() {
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update ();
		
		showPendingTriggerChangeInfoProp.boolValue = EditorGUILayout.Toggle ("Show pending box", showPendingTriggerChangeInfoProp.boolValue);


		DrawEngineSettings();
		ViewThemePopup();
		DrawThemeSettings();

		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties ();
	}

	void ProgressBar(float value, string label) {
		Rect rect = GUILayoutUtility.GetRect(18, 8, "TextField");
		EditorGUI.ProgressBar(rect, value, "");
		EditorGUILayout.Space();
	}
}
