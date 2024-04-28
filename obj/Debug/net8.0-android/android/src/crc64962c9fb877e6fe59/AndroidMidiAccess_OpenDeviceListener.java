package crc64962c9fb877e6fe59;


public class AndroidMidiAccess_OpenDeviceListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.media.midi.MidiManager.OnDeviceOpenedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDeviceOpened:(Landroid/media/midi/MidiDevice;)V:GetOnDeviceOpened_Landroid_media_midi_MidiDevice_Handler:Android.Media.Midi.MidiManager/IOnDeviceOpenedListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Commons.Music.Midi.AndroidExtensions.AndroidMidiAccess+OpenDeviceListener, Commons.Music.Midi", AndroidMidiAccess_OpenDeviceListener.class, __md_methods);
	}


	public AndroidMidiAccess_OpenDeviceListener ()
	{
		super ();
		if (getClass () == AndroidMidiAccess_OpenDeviceListener.class) {
			mono.android.TypeManager.Activate ("Commons.Music.Midi.AndroidExtensions.AndroidMidiAccess+OpenDeviceListener, Commons.Music.Midi", "", this, new java.lang.Object[] {  });
		}
	}


	public void onDeviceOpened (android.media.midi.MidiDevice p0)
	{
		n_onDeviceOpened (p0);
	}

	private native void n_onDeviceOpened (android.media.midi.MidiDevice p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
