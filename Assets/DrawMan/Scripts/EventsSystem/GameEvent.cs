using System.Collections.Generic;
using UnityEngine;

namespace DrawMan.Core.EventsSystem
{
	[CreateAssetMenu]
	public class GameEvent : ScriptableObject
	{
		private List<GameEventListener> listeners =
			new List<GameEventListener>();

		public void Raise()
		{
            foreach (var listener in listeners)
				listener?.OnEventRaised();
		}

		public void RegisterListener(GameEventListener listener)
		{ listeners.Add(listener); }

		public void UnregisterListener(GameEventListener listener)
		{ listeners.Remove(listener); }
	}
}
