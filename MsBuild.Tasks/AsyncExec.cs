using System.Diagnostics;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MsBuild.Tasks
{
	public class AsyncExec : Task // Exec
	{
		[Required]
		public string Command { get; set; }

		[Required]
		public string WorkingDirectory { get; set; }

		public override bool Execute()
		{
			var process = new Process
			{
				StartInfo = GetProcessStartInfo(WorkingDirectory, Command)
			};

			Log.LogMessage(MessageImportance.Low, "AsyncExec - Starting process..");

			if (!process.Start())
			{
				Log.LogMessage(MessageImportance.High, "AsyncExec - Process failed to start.");
				return false;
			}

			Log.LogMessage(MessageImportance.Low, "AsyncExec - Process started.");
			return true;
		}

		protected virtual ProcessStartInfo GetProcessStartInfo(string workingDirectory, string command)
		{
			var arguments = "";

			if (arguments.Length > 0x7d00)
				Log.LogWarningWithCodeFromResources("ToolTask.CommandTooLong", new object[] { GetType().Name });

			var startInfo = new ProcessStartInfo(command, arguments)
								{
									WindowStyle = ProcessWindowStyle.Hidden,
									CreateNoWindow = true,
									UseShellExecute = true
								};

			if (workingDirectory != null)
				startInfo.WorkingDirectory = workingDirectory;

			return startInfo;
		}
	}
}
