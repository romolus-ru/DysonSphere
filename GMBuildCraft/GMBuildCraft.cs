using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;
using Engine.Views;

namespace GMBuildCraft
{

	/// <summary>
	/// Основной модуль BuildCraft
	/// </summary>
	public class GMBuildCraft:Module
	{
		private ViewControlSystem _sys;
		private ViewUniverse _vu;
		private Universe _u;

		protected override void SetUpView(View view, Controller controller)
		{
			_sys = new ViewControlSystem(Controller);
			_sys.Show();
			view.AddObject(_sys);

			_u=new Universe();
			_u.GenerateUniverse();
			_vu = new ViewUniverse(Controller, _sys);
			_vu.Universe = _u;
			_vu.Show();
		}
	}
}
