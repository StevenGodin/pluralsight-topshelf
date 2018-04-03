using Topshelf;

namespace FileConverterService
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			HostFactory.Run(serviceConfig =>
			{
				serviceConfig.Service<ConverterService>(serviceInstance =>
					{
						serviceInstance.ConstructUsing(() => new ConverterService());
						serviceInstance.WhenStarted(execute => execute.Start());
						serviceInstance.WhenStopped(execute => execute.Stop());
					});
				serviceConfig.SetServiceName("AwesomFileConverter");
				serviceConfig.SetDisplayName("Awesom File Converter");
				serviceConfig.SetDescription("A Pluralsight demo service.");

				serviceConfig.StartAutomatically();
			});
		}
	}
}
