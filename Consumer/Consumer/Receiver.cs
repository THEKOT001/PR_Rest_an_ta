using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;

namespace Consumer
{
	public class Receiver
	{
		private HttpListener _listener;

		private SharedQueue<int> _queue;

		private Thread _thread;
		
		public Receiver(string[] prefixes, SharedQueue<int> queue)
		{
			_listener = new HttpListener();
			for (var i = 0; i < prefixes.Length; i++)
			{
				_listener.Prefixes.Add(prefixes[i]);
			}

			_queue = queue;

			_thread = new Thread(Update);
		}

		public void Start()
		{
			_listener.Start();
			
			_thread.Start();
		}
		
		private void Update()
		{
			while (true)
			{
				var context = _listener.GetContext();

				var request = context.Request;
				var response = context.Response;

				var reader = new StreamReader(request.InputStream, request.ContentEncoding);
				var json = reader.ReadToEnd();
				
				Console.WriteLine($"Receiver on Thread[{Thread.CurrentThread.ManagedThreadId}] got data = {json};");

				var array = JsonSerializer.Deserialize<int[]>(json);
				_queue.Enqueue(array);
				
				response.Close();
			}
		}
	}
}