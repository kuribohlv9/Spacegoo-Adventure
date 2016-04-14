using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Elias {

	public enum Modes : int{
		Objective = 1,
		Exploration
	}

	public enum TrackType : int
	{
		AudioLoop = 1,
		AudioStinger
	}

	public enum StingerProgressions : int
	{
		Sequential,
		Random,
		Shuffle
	}

	public enum DataCallbackActions : int
	{
		Read = 1,
		Seek = 2,
		Open = 3,
		Close = 4
	}

	public enum ErrorCode : int
	{
		Success, /* All is well. */
		UnknownError, /* An unknown error condition. */
		BadHandle, /* An invalid handle was given. */
		InvalidInput, /* One or more invalid parameters. */
		OutOfMemory, /* Out of memory. */
		EngineRunning, /* Cannot perform this operation while the engine is running. */
		NotRunning, /* Cannot perform this operation while the engine isn't running. */
		NoTracks, /* No tracks have been added. */
		NoContent, /* No content has been added to the given track. */
		InvalidForTrackType, /* The given operation cannot be performed for this type of track. */
		InvalidInMode, /* The given operation cannot be performed in the current mode. */
		InvalidAgility, /* An invalid agility setting was specified for the given track. */
		InvalidPickup, /* An invalid pickup beats setting was specified for the given track. */
		ThreadingError, /* Could not start one or more of the internal thread/synchronization subsystems. */
		DuplicateEntry, /* Duplicate entries are not allowed. */
		UserCancelled /* The operation was cancelled by you. */
	};

	public enum DecoderFlags : int
	{
		decoder_default=0x0/* Defaults to elias_decoder_stream | elias_decoder_verify. */
		,decoder_stream=0x1 /* Stream the audio sources. */
		,decoder_preload=0x2 /* Preload the audio sources into memory before playback starts. */
		,decoder_verify=0x4/* Verify that all audio sources can be opened successfully before returning from elias_theme_prepare_playback_decoders. */
	};

	//TODO: Remove this if we are not going to use it!
	public enum DecoderErrors : int
	{
		Success = 0,
		FileNotFound,
		UnknownFormat,
		InvalidFormat,
		Reading,
		UnsupportedBitDepth,
		InternalError,
		OutOfMemory,
		UnknownError,
		InvalidInput
	};

	//TODO: Verify that the marshalling of the string works properly !
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct Source
	{
		public IntPtr theme; /* The theme to which this source belongs. */
		public int track_type; /* The type of the track to which this source belongs (see the elias_track_types enum above). */
		public int track_id; /* The ID of the track to which this source belongs. */
		public int key; /* The key on the given track to which the source belongs, specified as the number of semitones above C. */
		public int source; /* The ID of the source on the given track in the specified key. */
		public IntPtr filename; /* A NULL terminated, UTF-8 encoded string. This pointer is NULL if no filename was specified for the source. Because C#'s marshalling does not guarantee
		                         that the encoding is correctly changed, it will have to be decoded explicitly. */
		public ulong length_in_samples; /* The length of the source data, in samples. */
		//TODO: Perhaps this should be an IntPtr !?
		public uint source_user_data; /* This pointer contains application data that is specific to the given source. It is set to 0 by default. */
		public int dummy; /* This is non-zero if this is a dummy source, which is to say a source that is silent. */
	};
	
}
