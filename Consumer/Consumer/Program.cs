using System.Threading;

namespace Consumer
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			var prefixes = new[]
			{
				"http://localhost:6666/",
				"http://127.0.0.1:6666/"
			};

			const string producerUri = "http://127.0.0.1:7777/";

			var sender = new Sender(producerUri);
			sender.Start();

			var queue = new SharedQueue<int>();

			var receiver = new Receiver(prefixes, queue);
			receiver.Start();
			
			const int consumersCount = 6;

			var consumers = new Consumer[consumersCount];
			for (var i = 0; i < consumersCount; i++)
			{
				consumers[i] = new Consumer(sender, queue);
				consumers[i].Start();
			}
			
			Thread.Sleep(-1);
		}
	}
}