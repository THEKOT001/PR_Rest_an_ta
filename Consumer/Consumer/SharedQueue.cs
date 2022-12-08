using System;
using System.Collections.Generic;
using System.Threading;

namespace Consumer
{
	public class SharedQueue<T>
	{
		private List<T> _queue;

		public SharedQueue()
		{
			_queue = new List<T>();
		}

		public void Enqueue(T data)
		{
			lock (this)
			{
				_queue.Add(data);
				
				Monitor.Pulse(this);
			}
		}

		public void Enqueue(T[] array)
		{
			lock (this)
			{
				for (var i = 0; i < array.Length; i++)
				{
					_queue.Add(array[i]);
				}
				
				Monitor.Pulse(this);
			}
		}

		public T Dequeue()
		{
			lock (this)
			{
				while (_queue.Count == 0)
				{
					Monitor.Wait(this);
				}

				var item = _queue[0];
				_queue.RemoveAt(0);

				return item;
			}
		}
	}
}