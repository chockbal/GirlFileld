using System;
using System.Threading;

public static class Utility
{
	public static void Clear(this CancellationTokenSource cts)
	{
		if (cts == null)
			return;

		try
		{
			if (!cts.IsCancellationRequested)
				cts.Cancel();
		}
		catch (ObjectDisposedException)
		{
		}
		catch
		{
		}
		finally
		{
			try
			{
				cts.Dispose();
			}
			catch
			{
			}
		}
	}
}
