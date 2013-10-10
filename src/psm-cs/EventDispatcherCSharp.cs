using System;
using System.Collections.Generic;
using psm.events;
using System.Reflection;

namespace psm.events {
	
	public class EventDispatcherCSharp {
		
		private Dictionary<string, System.Collections.Generic.List<EventTuple>> listeners;
		
		public EventDispatcherCSharp() {
			listeners = new Dictionary<string, System.Collections.Generic.List<EventTuple>>();
		}
		
		public void addEventListener(string type, object inListener) {
			haxe.lang.Closure closure = (haxe.lang.Closure) inListener;
			MethodInfo callback = closure.obj.GetType().GetMethod(closure.field);
			
			System.Collections.Generic.List<EventTuple> listener = null;
			
			if (type == "") Lib.trace("Warning, added listener for unnamed event type");
			
			if (!listeners.TryGetValue(type, out listener)) {
				listener = new System.Collections.Generic.List<EventTuple>();
				listeners.Add(type, listener);
			}
			
			listener.Add(new EventTuple(callback, closure.obj));
		}

		public bool dispatchEvent(Event e){
			if (!hasEventListener(e.type)) return false;
			
			System.Collections.Generic.List<EventTuple> listener = null;
			listeners.TryGetValue(e.type, out listener);
			var parameters = new object[] {e};
			
			for (int i = 0; i < listener.Count; i++) {
				listener[i].callback.Invoke(listener[i].obj, parameters);
			}
			
			return true;
		}
	
		public bool hasEventListener(string type){
			return listeners.ContainsKey(type);
		}
	
		public void removeEventListener(string type, object inListener){
			if(!hasEventListener(type)) return;
			System.Collections.Generic.List<EventTuple> listener = null;
			listeners.TryGetValue(type, out listener);
			
			haxe.lang.Closure closure = (haxe.lang.Closure) inListener;
			MethodInfo callback = closure.obj.GetType().GetMethod(closure.field);
			
			foreach (var target in listener) {
				if (target.obj == closure.obj && closure.field == target.callback.Name) {
					listener.Remove(target);
					break;
				}
			}
		}
	}
}

struct EventTuple {
	public MethodInfo callback;
	public object obj;
	
	public EventTuple(MethodInfo callback, object obj){
		this.callback = callback;
		this.obj = obj;
	}
}