namespace Producer
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var prefixes = new[]
			{
				"http://localhost:7777/",
				"http://127.0.0.1:7777/"
			};

			var receiver = new Receiver(prefixes);
			receiver.Start();

			const string consumerUri = "http://127.0.0.1:6666";
			
			var sender = new Sender(consumerUri);
			sender.Start();

			const int producersCount = 6;
			var producers = new Producer[producersCount];
			for (var i = 0; i < producersCount; i++)
			{
				producers[i] = new Producer(sender, i);
				producers[i].Start();
			}
		}
	}
}