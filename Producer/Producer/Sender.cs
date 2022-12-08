using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;

namespace Producer
{
	public class Sender
	{
		private HttpClient _httpClient;
		private string _uri;
		
		private readonly List<int> _queue;

		private readonly Thread _thread;

		public Sender(string uri)
		{
			_httpClient = new HttpClient();
			_uri = uri;

			_queue = new List<int>();
			_thread = new Thread(Update);
		}

		public void Add(int number)
		{
			lock (_queue)
			{
				_queue.Add(number);
			}
		}

		public void Start()
		{
			_thread.Start();
		}

		private void Update()
		{
			while (true)
			{
				lock (_queue)
				{
					if (_queue.Count != 0)
					{
						Send();
					}
				}

				Thread.Sleep(100);
			}
		}

		private async void Send()
		{
			var array = new int[_queue.Count];
			
			for (var i = 0; i < _queue.Count; i++)
			{
				array[i] = _queue[i];
			}
			
			_queue.Clear();

			var json = JsonSerializer.Serialize(array);
			
			Console.WriteLine($"Sender on Thread[{Thread.CurrentThread.ManagedThreadId}] send json = {json};");

			var httpRequest = new HttpRequestMessage(HttpMethod.Post, _uri);
			httpRequest.Content = new StringContent(json);
			
			await _httpClient.SendAsync(httpRequest);
		}
	}
}