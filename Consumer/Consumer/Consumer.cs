using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;

namespace Consumer
{
	public class Consumer
	{
		private SharedQueue<int> _queue;

		private HttpClient _httpClient;
		private Sender _sender;
		
		private Thread _thread;

		public Consumer(Sender sender, SharedQueue<int> queue)
		{
			_httpClient = new HttpClient();
			_sender = sender;
			
			_queue = queue;
			
			_thread = new Thread(Update);
		}
		
		public void Start()
		{
			_thread.Start();
		}

		private void Update()
		{
			while (true)
			{
				var item = _queue.Dequeue();

				var number = item * 2;
				
				Console.WriteLine($"Consumer on Thread[{Thread.CurrentThread.ManagedThreadId}] processed data = {number};");
				
				_sender.Add(number);
			}
		}
	}
}