using System;
using System.Threading;

namespace Producer
{
	public class Producer
	{
		private Sender _sender;
		
		private Random _random;
		private Thread _thread;

		public Producer(Sender sender, int seed)
		{
			_sender = sender;

			_random = new Random(seed);
		}

		public void Start()
		{
			_thread = new Thread(Update);
			_thread.Start();
		}

		private void Update()
		{
			while (true)
			{
				var number = _random.Next(10, 50);
				_sender.Add(number);
				
				Console.WriteLine($"Producer on Thread[{Thread.CurrentThread.ManagedThreadId}] send number = {number};");
				
				Thread.Sleep(number);
			}
		}
	}
}