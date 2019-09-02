using System;
using System.Collections;
using System.Collections.Generic;

public class EventMgr
{
	static EventMgr _ins;
	public static EventMgr ins{
		get{ 
			if (_ins == null) {
				_ins = new EventMgr ();
			}
			return _ins;
		}
	}

	public static string EnterFrame = "EnterFrame";

	public class NetMsg{
		public NetMessageHead head;
		public Object para;

		public NetMsg(NetMessageHead head,System.Object para){
			this.head = head;
			this.para = para;
		}
	}

	public delegate void EventFunc(string id,System.Object data);

	Dictionary<string,EventFunc> listeners = new Dictionary<string, EventFunc>();

	public void AddEventListener(string id,EventFunc func){
		if (!listeners.ContainsKey (id)) {
			listeners [id] = func;
		} else {
			listeners [id] += func;
		}
	}

	public void RemoveEventListner(string id,EventFunc func){
		if (listeners.ContainsKey (id) && listeners[id] != null) {
			listeners [id] -= func;
		}
	}

	public void DispEvent(string id,System.Object data){
		if (listeners.ContainsKey (id) && listeners[id] != null) {
			listeners [id].Invoke (id, data);
		}
	}
}

