using UnityEngine;
using System.Collections;

public class ExampleTrigger : MonoBehaviour {
	private float da = 0;
	private bool isRotating = false;
	private float rotDeg = 60f;

	public enum TrackType {loop, stinger};
	public TrackType trackType;

	public Color activationColor;
	public uint zone_id = 0;

	public delegate void ZoneTriggered(uint zone_id, TrackType trackType);
	public delegate void ThemeLoaded(ref Elias.Theme theme);

	// All interested listeners should register to this event.
	// e.g MusicSwitcher.Switch(uint zone_id)
	public static event ZoneTriggered OnZoneTriggered;
	public static event ThemeLoaded OnThemeLoaded;

	public enum ELIASTriggerType {TriggerEnter, TriggerExit, CollisionEnter, CollisionExit};

	/* I'm not sure that action should be decided by the trigger.
	 * On the other hand, it could be of great use when designing a level.
	 * Example: Suddlenly one decides that music should stop when triggered,
	 *  or that a stinger should play instead of level change. But, what stinger?
	 * Such information has to be supplied here too in such case 
	 * (or in another well defined component on the same level and game object).
	 */
	//enum ELIASAction {SwitchLevel, PlayStinger};
	public ELIASTriggerType TriggerOn = ELIASTriggerType.TriggerEnter;
	
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if (!isRotating) {
			return;
		}
		gameObject.transform.Rotate(new Vector3(0f, da * Time.deltaTime, 0f));
		da = da - da * Time.deltaTime;
		if (da < 0.001f) {
			isRotating = false;
		}
	}

	public void StartSpin() {
		da = rotDeg;
		isRotating = true;
	}

	public static void ThemeWasLoaded(ref Elias.Theme theme) {
		if (OnThemeLoaded != null) {
			OnThemeLoaded(ref theme);
		}
	}

	void OnTriggerEnter(Collider collider) {
		gameObject.GetComponent<Renderer>().material.color = activationColor;
		StartSpin();
		// Send trigger event. Let event manager on ELIAS_Component be a registered event listener.
		if (TriggerOn == ELIASTriggerType.TriggerEnter) {
			// Call all registered methods for this event!
			if (OnZoneTriggered != null) {
				OnZoneTriggered(zone_id, trackType);
			}
		}
	}
}
