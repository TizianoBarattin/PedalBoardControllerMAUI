package crc64962c9fb877e6fe59;


public class MidiInput_Receiver
	extends android.media.midi.MidiReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSend:([BIIJ)V:GetOnSend_arrayBIIJHandler\n" +
			"";
		mono.android.Runtime.register ("Commons.Music.Midi.AndroidExtensions.MidiInput+Receiver, Commons.Music.Midi", MidiInput_Receiver.class, __md_methods);
	}


	public MidiInput_Receiver ()
	{
		super ();
		if (getClass () == MidiInput_Receiver.class) {
			mono.android.TypeManager.Activate ("Commons.Music.Midi.AndroidExtensions.MidiInput+Receiver, Commons.Music.Midi", "", this, new java.lang.Object[] {  });
		}
	}


	public MidiInput_Receiver (int p0)
	{
		super (p0);
		if (getClass () == MidiInput_Receiver.class) {
			mono.android.TypeManager.Activate ("Commons.Music.Midi.AndroidExtensions.MidiInput+Receiver, Commons.Music.Midi", "System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0 });
		}
	}


	public void onSend (byte[] p0, int p1, int p2, long p3)
	{
		n_onSend (p0, p1, p2, p3);
	}

	private native void n_onSend (byte[] p0, int p1, int p2, long p3);

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
