using System.Collections.Generic;
using UnityEngine.Events;

namespace MidiPlayerTK
{

    [System.Serializable]
    public class EventMidiClass : UnityEvent<MPTKEvent>
    {
    }

    [System.Serializable]
    public class EventNotesMidiClass : UnityEvent<List<MPTKEvent>>
    {
    }

    [System.Serializable]
    public class EventSynthClass : UnityEvent<string>
    {
    }

    [System.Serializable]
    public enum EventEndMidiEnum
    {
        MidiEnd,
        ApiStop,
        Replay,
        Next,
        Previous,
        MidiErr,
        Loop
    }

    /// <summary>@brief
    /// Status of the last midi file loaded
    /// @li      -1: midi file is loading
    /// @li       0: succes, midi file loaded 
    /// @li       1: error, no Midi found
    /// @li       2: error, not a midi file, too short size
    /// @li       3: error, not a midi file, signature MThd not found.
    /// @li       4: error, network error or site not found.
    /// </summary>
    [System.Serializable]
    public enum LoadingStatusMidiEnum
    {
        /// <summary>@brief
        /// -1: midi file is loading.
        /// </summary>
        NotYetDefined = -1,

        /// <summary>@brief
        /// 0: succes, midi file loaded.
        /// </summary>
        Success = 0,

        /// <summary>@brief
        /// 1: error, no Midi file found.
        /// </summary>
        NotFound = 1,

        /// <summary>@brief
        /// 2: error, not a midi file, too short size.
        /// </summary>
        TooShortSize = 2,

        /// <summary>@brief
        /// 3: error, not a midi file, signature MThd not found.
        /// </summary>
        NoMThdSignature = 3,

        /// <summary>@brief
        /// 4: error, network error or site not found.
        /// </summary>
        NetworkError = 4,

        /// <summary>@brief
        /// 5: error, midi file corrupted, error detected when loading the midi events.
        /// </summary>
        MidiFileInvalid = 5,

        /// <summary>@brief
        /// 6: SoundFont not loaded.
        /// </summary>
        SoundFontNotLoaded = 6,

        /// <summary>@brief
        /// 7: error, Already playing.
        /// </summary>
        AlreadyPlaying = 7,

        /// <summary>@brief
        /// 8: error, MPTK_MidiName must start with file:// or http:// or https:// (only for MidiExternalPlayer).
        /// </summary>
        MidiNameInvalid = 8,

        /// <summary>@brief
        /// 9: error,  Set MPTK_MidiName by script or in the inspector with Midi Url/path before playing.
        /// </summary>
        MidiNameNotDefined = 9,

        /// <summary>@brief
        /// 10: error, Read 0 byte from the MIDI file.
        /// </summary>
        MidiFileEmpty = 10,
    }

    [System.Serializable]
    public class EventStartMidiClass : UnityEvent<string>
    {
    }

    [System.Serializable]
    public class EventEndMidiClass : UnityEvent<string, EventEndMidiEnum>
    {
    }

    [System.Serializable]
    static public class ToolsUnityEvent
    {

        static public bool HasPersistantEvent(this EventMidiClass evt)
        {
            if (evt != null && evt.GetPersistentEventCount() > 0 && !string.IsNullOrEmpty(evt.GetPersistentMethodName(0)))
                return true;
            else
                return false;
        }

        static public bool HasPersistantEvent(this UnityEvent evt)
        {
            if (evt != null && evt.GetPersistentEventCount() > 0 && !string.IsNullOrEmpty(evt.GetPersistentMethodName(0)))
                return true;
            else
                return false;
        }
        static public bool HasPersistantEvent(this EventNotesMidiClass evt)
        {
            if (evt != null && evt.GetPersistentEventCount() > 0 && !string.IsNullOrEmpty(evt.GetPersistentMethodName(0)))
                return true;
            else
                return false;
        }

        static public bool HasPersistantEvent(this EventStartMidiClass evt)
        {
            if (evt != null && evt.GetPersistentEventCount() > 0 && !string.IsNullOrEmpty(evt.GetPersistentMethodName(0)))
                return true;
            else
                return false;
        }

        static public bool HasPersistantEvent(this EventEndMidiClass evt)
        {
            if (evt != null && evt.GetPersistentEventCount() > 0 && !string.IsNullOrEmpty(evt.GetPersistentMethodName(0)))
                return true;
            else
                return false;
        }

        static public bool HasPersistantEvent(this EventSynthClass evt)
        {
            if (evt != null && evt.GetPersistentEventCount() > 0 && !string.IsNullOrEmpty(evt.GetPersistentMethodName(0)))
                return true;
            else
                return false;
        }

    }
}
