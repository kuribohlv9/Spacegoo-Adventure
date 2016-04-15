using UnityEngine;
using System;   // IntPtr
using System.Collections;
using System.Collections.Generic;

public class ThemeGeometry : MonoBehaviour {
	public List<GameObject> triggerCubes;
	public List<GameObject> stingerSpheres;

	private float stingerWaveAngle = 0.0f;
	private float triggerWaveAngle = 0.0f;
	
	// TODO(tobias): Expand to cover all keys for a track, e.g
	//  value = List<int>[nr_of_keys_for_track]
	private static Dictionary<int, List<int>> loopTrackDict;  // key = track_id, value = List of ints representing source_id.
	private static Dictionary<int, List<int>> stingerTrackDict;  // source_id indicate ELIAS trigger level. Consult the ELIAS manual for more information.

	public GameObject levelTriggerCubePrefab;
	public GameObject stingerTriggerPrefab;

	void OnEnable() {
		ExampleTrigger.OnThemeLoaded += ThemeLoaded;
		ExampleTrigger.OnZoneTriggered += ZoneTriggered;
	}

	void OnDisable() {
		ExampleTrigger.OnThemeLoaded -= ThemeLoaded;
		ExampleTrigger.OnZoneTriggered -= ZoneTriggered;
	}

	void Start () { }
	
	void UpdateStingerSpheres() {
		float amplitude_0 = 0.25f;
		float frequency_0 = 0.4f;
		float frequency_1 = 2.0f * frequency_0;
		
		stingerWaveAngle += Time.deltaTime * frequency_0;
		int i = 0;
		foreach (var sphere in stingerSpheres) {
			float x = (float)i * 2.5f;
			float y = 1.0f + amplitude_0 * Mathf.Cos(stingerWaveAngle + frequency_1 * (float)i);
			float z = 1.0f;
			sphere.transform.localPosition = new Vector3(x,y,z);
			i++;
		}
	}

	void UpdateTriggerCubes() {
		float amplitude_0 = 0.1f;
		float frequency_0 = 0.4f;
		float frequency_1 = 2.0f * frequency_0;
		
		triggerWaveAngle += Time.deltaTime * frequency_0;
		float d_arc = 4.0f;
		float r = d_arc * (triggerCubes.Count - 1) / (2.0f * Mathf.PI);
		float phase = 2.0f * Mathf.PI / 4.0f;  // A quarter of a revolution.
		int i = 0;
		foreach (GameObject cube in triggerCubes) {
			float angle = phase + (float)i / (float)(triggerCubes.Count + 4) * 2.0f * Mathf.PI;
			// Cubes distributed along a circle, and with a vertical wave motion.
			float xp = r * Mathf.Cos(angle);
			float yp = 1.0f + amplitude_0 * Mathf.Cos(triggerWaveAngle + frequency_1 * (float)i);
			float zp = r * Mathf.Sin(angle);
			cube.transform.localPosition = new Vector3(xp,yp,zp);
			i++;
		}
	}
			
	// Update is called once per frame
	void Update () {
		UpdateStingerSpheres();
		UpdateTriggerCubes();
	}
	
	public void ZoneTriggered(uint zone_id, ExampleTrigger.TrackType trackType) {
		// Reset the color of the other cubes.
		if (trackType == ExampleTrigger.TrackType.loop) {
			foreach (GameObject go in triggerCubes) {
				ExampleTrigger trig = go.GetComponent<ExampleTrigger>() as ExampleTrigger;
				if (trig != null) {
					uint zone_id_n = trig.zone_id;
					if (zone_id != zone_id_n) {
						go.GetComponent<Renderer>().material.color = Color.white;
					}
				}
			}
		}
	}
	
	/*
	 * Do NOT call ELIAS methods from this callback!
	 * Only static functions can be called on iOS!
	 */
	[AOT.MonoPInvokeCallback(typeof(Elias.Theme.SourceEnumerationCallback))]
	static int EnumerateSourcesCB(ref Elias.Source source, ref IntPtr source_user_data, IntPtr user_data) {
	
		string sourceString = string.Format("track {0}, File {1}, key {2}, source nr {3}", source.track_id, Elias.UTF8MarshallingHelpers.ConvertFromNativeUTF8(source.filename), source.key, source.source);
		Debug.Log (sourceString);

		int key = source.track_id;
		if (source.track_type == (int)Elias.TrackType.AudioLoop) {
			if (!loopTrackDict.ContainsKey(key)) {
				loopTrackDict[key] = new List<int>();
			}
			List<int> triggerList = loopTrackDict[source.track_id];
			triggerList.Add(source.source);  // Loop track trigger at level source.source
		} else if (source.track_type == (int)Elias.TrackType.AudioStinger) {
			if (!stingerTrackDict.ContainsKey(key)) {
				stingerTrackDict[key] = new List<int>();
			}
			List<int> stingerList = stingerTrackDict[source.track_id];
			// When playing a stinger, a track is supplied as argument and the engine choose variation based on a progression setting.
			// Therefore we save the track_id as an identifier here.
			stingerList.Add(source.track_id); 
		}

		return 1;
	}

	/*
	 * While the theme.EnumerateSources utilizes a callback, it's a blocking thread safe operation
	 * 	that calls back on the same thread it was called from. So no problems with thread safety here
	 * 	as long as you call it on the main thread!
	 */
	void EnumerateSources(ref Elias.Theme theme) {
		uint number_of_loop_tracks = theme.AudioLoopTrackCount;
		uint number_of_stinger_tracks = theme.AudioStingerTrackCount;
		
		loopTrackDict = new Dictionary<int, List<int>>();
		stingerTrackDict = new Dictionary<int, List<int>>();

		Debug.Log("Number of loop tracks: " + number_of_loop_tracks.ToString());
		Debug.Log("Number of stinger tracks: " + number_of_stinger_tracks.ToString());
		
		theme.EnumerateSources(this.EnumerateSourcesCB);
	}

	/*
	 * By doing as below, the geometry will be spatially distributed depending on 
	 * track ID's. This is not optimal since the ID's can be arbitrary as long as 
	 *  they are unique (and of course with values within the data types range. )
	 */
	private void CreateStingerGeometry() {
		stingerSpheres = new List<GameObject>();
		
		Transform stingerTriggers = GameObject.Find("StingerTriggers").transform;
		if (transform == null) {
			Debug.LogError("CreateStingerGeometry - Did not find a GameObject with name StingerTriggers so could not complete geometry creation.");
			return;
		}

		/* We only need to loop over tracks since DemoPlayer.PlayStinger takes track as argument.
		 * ELIAS will then play a variation of the stinger based on the current state, its progression settings
		 * and the available variations on the track.
		 */
		foreach (KeyValuePair<int, List<int>> entry in stingerTrackDict) {
			int track_id = entry.Key;
//			List<int> stingers = entry.Value;
			Vector3 origo = stingerTriggers.position;
			float xp = origo.x -18.0f + track_id * 2.0f;
			float yp = origo.y + 0.5f;
			float zp = origo.z + 1.0f;
			GameObject inst = Instantiate(stingerTriggerPrefab, new Vector3(xp, yp, zp), Quaternion.identity) as GameObject;
			ExampleTrigger trig = inst.GetComponent<ExampleTrigger>();
			trig.zone_id = (uint)track_id;
			trig.trackType = ExampleTrigger.TrackType.stinger;
			trig.transform.parent = stingerTriggers;

			// Should probably check for null here.
			inst.GetComponent<Collider>().isTrigger = true;
			stingerSpheres.Add (inst.gameObject);
		}
	}

	void CreateLoopTrackGeometry(uint greatestTrigger) {
		triggerCubes = new List<GameObject>();
		float d_arc = 4.0f;
		float r = d_arc * (greatestTrigger - 1) / (2.0f * Mathf.PI);
		Transform triggerObjects = GameObject.Find("LevelTriggers").transform;
		if (triggerObjects == null) {
			Debug.LogError("The scene does not contain a GameObject with name LevelTriggers, will not create level trigger geometry.");
			return;
		}

		float phase = 2.0f * Mathf.PI / 4.0f;  // A quarter of a revolution.
		for (uint x = 0; x < greatestTrigger; x++) {
			float angle = phase + (float)x / (float)(greatestTrigger + 4) * 2.0f * Mathf.PI;
			float xp = r * Mathf.Cos(angle);
			float yp = 1.0f;
			float zp = r * Mathf.Sin(angle);

			GameObject inst = Instantiate(levelTriggerCubePrefab, new Vector3(xp, yp, zp), Quaternion.identity) as GameObject;
			float s = 0.8f;
			inst.transform.localScale = new Vector3(s, s, s);
			inst.transform.localRotation = Quaternion.AngleAxis( -180.0f * angle / Mathf.PI, Vector3.up);
			ExampleTrigger tg = inst.GetComponent<ExampleTrigger>();
			tg.zone_id = x + 1;  // Trigger index start at 1.
			triggerCubes.Add(inst.gameObject);
			inst.transform.parent = triggerObjects;
		}
	}

	// Called when a theme has finished loading.
	public void ThemeLoaded(ref Elias.Theme theme) {
		uint triggerLevels = (uint)theme.GreatestTrigger;

		// So, a theme is loaded. Now get some information about it ...
		//	All this currently does is output the info into the debug log, but if we wanted to we could use the
		//	data to show what actual sources are playing, and not only what track nr is.
		EnumerateSources(ref theme);
		
		// ... and create some representative geometry.
		CreateStingerGeometry();
		CreateLoopTrackGeometry(triggerLevels);
	}
}
