using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
	static class Program
	{
		/// <summary>
		/// Запуск серверной части движка
		/// </summary>
		static void Main()
		{
			var application = new Application();
			application.Run();
		}
	}
}
