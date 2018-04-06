using Topshelf;

namespace FileConverterService
{
	internal static class Program
	{
		private static void Main()
		{
			HostFactory.Run(serviceConfig =>
			{
				serviceConfig.UseNLog();
				serviceConfig.SetServiceName("AwesomeFileConverter");
				serviceConfig.SetDisplayName("Awesome File Converter");
				serviceConfig.SetDescription("A Pluralsight demo service.");
				serviceConfig.Service<ConverterService>(serviceInstance =>
				{
					serviceInstance.ConstructUsing(() => new ConverterService());
					serviceInstance.WhenStarted(execute => execute.Start());
					serviceInstance.WhenStopped(execute => execute.Stop());
					serviceInstance.WhenPaused(execute => execute.Pause());
					serviceInstance.WhenContinued(execute => execute.Continue());
					serviceInstance.WhenCustomCommandReceived(
						(execute, hostControl, commandNumber) => execute.CustomCommand(commandNumber));
				});
				serviceConfig.EnablePauseAndContinue();

				serviceConfig.EnableServiceRecovery(recoveryConfig =>
				{
					recoveryConfig.RestartService(1);
					recoveryConfig.RestartService(1);
					recoveryConfig.RestartService(1);
				});

				serviceConfig.StartAutomatically();
				serviceConfig.RunAsLocalService();
			});
		}
	}
}
