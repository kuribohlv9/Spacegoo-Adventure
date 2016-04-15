using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;  // For List.
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Elias;

[RequireComponent(typeof(AudioSource))]
public class DemoPlayer : MonoBehaviour {
	private Elias.Theme _theme; 
	private AudioClip _player;

	[Tooltip("Name of the theme folder")]
	public string themeFolder = "demo";  // Configuration file takes precedence. TODO(Bjorn): What does the comment mean?!

	public Elias.Theme Theme {
		get{ return _theme; }
	}
	
	[Tooltip("Prebuffer length as a multiple of Unity's DSP buffer.")]
	public uint PrebufferLengthAsMultipleOfDSPBuffer = 16;

	[Tooltip("Processing period as a multiple of Unity's DSP buffer.")]
	public uint ProcessingPeriodAsMultipleOfDSPBuffer = 8;

	[Tooltip("Urgency value.")]
	[Range (0.0f, 100f)]
	public int startUrgency = 100;
	public bool startRendevous = false;
	public int startMinLevel = 1;
	public int startMaxLevel = 1;
	public int startStinger = 0;

	public int startKey = -1;

	public bool useActionPresetOnStart = false;
	public string startActionPreset = "";

	public bool startThemeOnStart = true;
	public DecoderFlags loadType = DecoderFlags.decoder_default;

	public bool showPendingTriggerChangeInfo = false;

	private int numChannelsInTheme = 2;
	private int numChannelsOutput = 2;

	[NonSerialized]
	public bool useLowLatencyOnAudioFilterRead = true;

	//Use this to force a shutdown from the audio thread. (As it won't allow me to disable the game object from that thread.
	private bool forceShutDown = false;
	
	// Report whether any track has a pending trigger change.
	public bool debug_isTransitioning() 
	{
		if (Theme == null)  
		{
			return false;
		}

		uint num_tracks = Theme.AudioLoopTrackCount;
		if (num_tracks == 0) 
		{
			Debug.LogWarning ("DemoPlayer::debug_isTransitioning() - Zero tracks!?");
		}

		for (uint trackid = 1; trackid <= num_tracks; ++trackid) 
		{
			int current_trigger = 0;
			int pending_trigger = 0;
			int current_trigger_key = 0;
			int pending_trigger_key = 0;
			Theme.GetActiveTriggersOnTrack((int)trackid, ref current_trigger, ref current_trigger_key, ref pending_trigger, ref pending_trigger_key);
			if (pending_trigger != 0) {
				return true;
			}
		}
		return false;
	}

	void Awake()
	{
		GetComponent<AudioSource> ().playOnAwake = false;
		GetComponent<AudioSource> ().Stop ();
	}

	/* Indicate trigger level change. Pending stingers are not indicated in this implementation. */
//#if UNITY_EDITOR
	void OnGUI(){
		if (showPendingTriggerChangeInfo) {
			if (debug_isTransitioning ()) {
				GUI.Box (new Rect (Screen.width / 6, 10, 200, 50), "Pending Trigger Change!");    
			}
		}
	}
//#endif

	void SetEliasChannelCount()
	{
		try
		{
			//TODO: Verify that they can differ and that this returns the correct version.
			//We need to get the limiting version here (if they can even differ, I don't know when it comes to unity).
			AudioSpeakerMode currentMode = AudioSettings.speakerMode < AudioSettings.driverCapabilities ? AudioSettings.speakerMode : AudioSettings.driverCapabilities;
			Debug.Log("Current speaker mode: " + currentMode);
			if (currentMode == AudioSpeakerMode.Mono) 
		    {
				Debug.Log ("Unity is in Mono");
				_theme.SetOutputChannels(1);
				numChannelsOutput = 1;
			}
			else if (currentMode == AudioSpeakerMode.Stereo)
			{
				_theme.SetOutputChannels(2);
				numChannelsOutput = 2;
			}
			else if (currentMode == AudioSpeakerMode.Quad)
			{
				_theme.SetOutputChannels(4);
				numChannelsOutput = 4;
			}
			else if (currentMode == AudioSpeakerMode.Surround)
			{
				_theme.SetOutputChannels(5);
				numChannelsOutput = 5;
			}
			else if (currentMode == AudioSpeakerMode.Mode5point1)
			{
				_theme.SetOutputChannels(6);
				numChannelsOutput = 6;
			}
			else if (currentMode == AudioSpeakerMode.Mode7point1)
			{
				_theme.SetOutputChannels(8);
				numChannelsOutput = 8;
			}
			else
			{
				//TODO: Better warning here!
				Debug.LogWarning ("Because ELIAS is currently not capable of converting from " + numChannelsInTheme + " channels to " + currentMode +
				                  ", ELIAS will be utilizing \"streamed\" AudioClip, " +
				                  "this can introduce high amounts of latency and possibly gaps in the music!\n" +
				                  "Because of this we are forced to increase the Prebuffer amount to above 400ms! " +
				                  "(If this is not enough, please contact us with information about what platform you are using!");
				//The problem here is that Unity's "streamed" AudioClips seem to require approximately 400ms of data, every 400ms.
				//  However it will read it in chunks as large as the AudioClips length.
				_theme.PrebufferLengthMS = 450;
				_theme.ProcessingPeriodMS = 225;
			    useLowLatencyOnAudioFilterRead = false;
			}
		
		}
		catch (Exception e)
		{
			Debug.LogError ("Error getting elias started: " + e.Message); 
			Debug.LogError(e);
			this.gameObject.SetActive(false);
		}
	}

	void OnAudioConfigurationChanged(bool deviceWasChanged)
	{
		Debug.Log(deviceWasChanged ? "Device was changed" : "Reset was called");
		
		_theme.SetOutputSampleRate (AudioSettings.outputSampleRate);

		SetEliasChannelCount();

		GetComponent<AudioSource>().Play ();
	}

	void Update()
	{
		if (forceShutDown) {
			this.gameObject.SetActive(false);
		}
	}

	void Start () {	
		forceShutDown = false;

		//This is so we can handle changes to SampleRate, and number of output channels.
		AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;

		//Put your themes in the ELIAS_Themes subfolder to StreamingAssets.
		// Name the theme directory the same as the ELIAS project file but without the .epro suffix, e.g
		// Assets/StreamingAssets/ELIAS_Themes/theme/{theme.epro, audio}
		string MusicFile = Path.Combine(Application.streamingAssetsPath, "ELIAS_Themes");
		MusicFile = Path.Combine(MusicFile, themeFolder);
		MusicFile = Path.Combine(MusicFile, themeFolder + ".epro");
		string themeFile = MusicFile;

		try {
#if UNITY_ANDROID && !UNITY_EDITOR
            WWW xmlWWW = new WWW(themeFile);  

			// TODO: CAREFUL here, for safety reasons you shouldn't let this while loop unattended, 
			//		place a timer and error check or find a different better way of waiting for the file to be done reading!
			while(!xmlWWW.isDone) {}
			string xml = xmlWWW.text;
#else
			System.IO.StreamReader myFile = new System.IO.StreamReader (themeFile);
			string xml = myFile.ReadToEnd ();
			myFile.Close ();
#endif
			
            if (xml != null && xml != "")
            {
				_theme = Theme.CreateFromXML (xml);
				string musicDir = Path.GetDirectoryName (themeFile) + "/";
#if UNITY_ANDROID && !UNITY_EDITOR
				_theme.LoadSourcesFromApk (musicDir, loadType); 
#else
				//INFO: Decoder_Flags.default is the "normal" elias behavior, it will use elias_decoder_stream | elias_decoder_verify
				//		  verifying that all files exist when started, and then stream only those that are needed.
				//		Decoder_Flags.decoder_preload causes all sources to be loaded into memory on start.
				//		  preloading will allow a lower prebuffer amount, as there is no risk of HDD stalls.
				//		  In exhcange it will consume more memory as the music files will reside in memory.
				_theme.LoadSources (musicDir, loadType);
#endif
				
				
				int dspBufferSize;
				int numDspBuffers;
				AudioSettings.GetDSPBufferSize(out dspBufferSize, out numDspBuffers);
				Debug.Log ("Unity DSP Buffer settings, \n dspBufferSize: " + dspBufferSize + " amount of buffers: " + numDspBuffers);
				//TODO: I may need to multiply this by the nr of dsp buffers!?
				//TODO: We may want to set these to a multiple of the DSPBuffer, as that is the size of the chunks unity asks for!
				_theme.PrebufferLengthSamples = PrebufferLengthAsMultipleOfDSPBuffer * (uint)dspBufferSize;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
				//On the devices we use double the same processing period as we do prebuffer, this to allow 
				_theme.ProcessingPeriodSamples = ProcessingPeriodAsMultipleOfDSPBuffer * (uint)dspBufferSize * 2;
#else
				_theme.ProcessingPeriodSamples = ProcessingPeriodAsMultipleOfDSPBuffer * (uint)dspBufferSize;
#endif
				_theme.SuggestedThreadCount = 1;

				int bpm = 0;
				int numerator = 0;
				int denominator = 0;
				int bars = 0;
				int sampleRate = 0;
				
				_theme.GetBasicInfo (ref bpm, ref numerator, ref denominator, ref bars, ref sampleRate, ref numChannelsInTheme);

				SetEliasChannelCount();

				if (useLowLatencyOnAudioFilterRead)
				{
					//Because unity is unlikely to be using the sample rate that we have our files in, we need to resample ourselves when using OnAudioFilterRead instead of a AudioClip.
					_theme.SetOutputSampleRate(AudioSettings.outputSampleRate);
				}

				if (useLowLatencyOnAudioFilterRead == false)
				{
//TODO: Make sure this macro applies to all Unity 5 versions, (alternatively make the macro check the inverse?)
#if UNITY_5
					//TODO: Why half the processing period!?
					_player = AudioClip.Create("eliasClip", (int)_theme.ProcessingPeriodSamples, numChannelsInTheme, sampleRate, true, OnAudioRead);
#else
					_player = AudioClip.Create("eliasClip", (int)_theme.ProcessingPeriodSamples, numChannelsInTheme, sampleRate, false, true, OnAudioRead);
#endif
					GetComponent<AudioSource> ().clip = _player;
					GetComponent<AudioSource> ().loop = true;
				}

				if (startThemeOnStart)
				{
					StartTheme ();
				}
			} 
		} catch (Exception e) {
			Debug.LogError ("Error getting elias started: " + e.Message); 
			Debug.LogError(e);
			this.gameObject.SetActive(false);
		}

//		Call the static method which will trigger the event
//		 ThemeLoaded (to which ELIASMusicSwitcher listens).
//		 
//		 Any object which is interested in knowing that a theme was
//		 loaded can of course register to that event.
		ExampleTrigger.ThemeWasLoaded(ref _theme);
	}

    public void StartTheme(string actionPreset, int key)
    {
        if ((actionPreset != "") && (_theme.ActionPresetExists(actionPreset)))
        {
            _theme.StartFromActionPreset(key, actionPreset);
        }
        else
        {
            Debug.LogError("The action preset " + startActionPreset + " does not exist!");
        }
        GetComponent<AudioSource>().Play();
    }

    public void StartTheme(int key, int minLevel, int maxLevel, int urgency, int stinger, bool rendevous)
    {
        _theme.Start(key, minLevel, maxLevel, urgency, stinger, rendevous ? 1 : 0);
        GetComponent<AudioSource>().Play();
    }

	public void StartTheme ()
	{
		if (useActionPresetOnStart) {
			if ((startActionPreset != "") && (_theme.ActionPresetExists(startActionPreset))) 
			{
				_theme.StartFromActionPreset (startKey, startActionPreset);
			}
			else 
			{
				Debug.LogError ("The action preset " + startActionPreset + " does not exist, starting without an action preset!");
				_theme.Start (startKey, startMinLevel, startMaxLevel, startUrgency, startStinger, startRendevous ? 1 : 0);
			}
		}
		else {
			_theme.Start (startKey, startMinLevel, startMaxLevel, startUrgency, startStinger, startRendevous ? 1 : 0);
		}

		GetComponent<AudioSource> ().Play();
	}

    public void StopTheme()
    {
        GetComponent<AudioSource>().Stop();
        _theme.Stop();
    }

	public void PlayStinger(int track_id) {
		Debug.Log("PlayStinger " + track_id.ToString());
		_theme.PlayStinger(track_id);
	}

	public void SetLevel(int minimum_level, int maximum_level, int urgency, int stinger, int immediately, int rendezvous)
	{
		_theme.SetLevel(minimum_level, maximum_level, urgency, stinger, immediately, rendezvous);
	}

	public void Silence(int urgency, int stinger, int immediately, int rendezvous)
	{
		_theme.Silence(urgency, stinger, immediately, rendezvous);
	}

	public void ActivateActionPreset(string actionPreset, bool immediately)
	{
		_theme.TriggerActionPreset (actionPreset, immediately);
	}
	
	void OnAudioFilterRead(float[] data, int channels)
	{
		if (useLowLatencyOnAudioFilterRead) {
			if (numChannelsOutput != channels) {
				Debug.LogError ("ELIAS is currently set up to use " + numChannelsOutput + " channels, unity is requesting: " + channels + " channels. Due to this the demo player is shutting down!");
				forceShutDown = true;
			}
			_theme.ReadFloatSamplesBlocking (data);
			//Alternative approach where ELIAS will return silence instead of waiting for enough samples to be ready.
//			uint numSamplesRead = _theme.ReadFloatSamples (data);
//			if (numSamplesRead != data.Length)
//			{
//				Debug.LogError("Missing: " + (numSamplesRead - data.Length) + " samples, requested: " + data.Length + " got: " + numSamplesRead);
//			}
		}
	}

	void OnAudioRead(float[] data)
	{
		_theme.ReadFloatSamplesBlocking (data);
	}
	
	void OnDestroy() 
	{
		GetComponent<AudioSource> ().Stop ();
        

        _theme.Stop(true);
        _theme.Destroy();
	}

}