using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils.GraphView;
using Engine.Utils.Settings;
using Engine.Views;
using Engine.Utils.Editor;
using Engine.Views.Templates;
using GMTubes.Model;
using Button = Engine.Views.Templates.Button;

namespace GMTubes
{
	/// <summary>
	/// GameModule Tubes / Игровой модуль Трубы
	/// </summary>
	/// <remarks>
	/// На поле расположены трубы. надо их повернуть так, что бы образовался замкнутый контур
	/// </remarks>
	public class GMTubes:Module
	{
		private ViewControlSystem _sys;
		private ViewField _vf;
		private Field _f;
		private ViewMenu _menu;
		private ViewModalInputName _input;
		private ViewModalCongratulations _congratulations;
		private Background background;
		private ViewGraph _graph;


		public static UserInfo UserInfo=new UserInfo();

		protected override void SetUpView(Engine.Views.View view, Controller controller)
		{
			InitUserInfo();

			_sys = new ViewControlSystem(Controller);
			_sys.Show();
			view.AddObject(_sys);

			_menu = new ViewMenu(controller, _sys);
			
			//var b1 = Button.CreateButton(controller, 95, 0, 74, 20, "systemExit", "Выход", "Esc", Keys.Escape, "");
			//_sys.AddComponent(b1);

			_f = new Field(UserInfo.CurrentLevel);
			_vf = new ViewField(controller, _sys);
			_vf.Hide();
			_vf.SetF(_f);

			background = new Background(controller,null,@"..\Resources\gmTubes\wp1.jpg","GMTubesWP1");
			_sys.AddComponent(background);

			// сначала создаётся и запускается меню. из него выбираются параметры запуска
			// при нажатии "старт" в модуле прячется меню и запускается игра
			// если нажать esc то откроется меню - там можно будет выбрать продолжить, начать снова, параметры или выход

			Controller.AddEventHandler("GMTubesStart", GMTubesStart);
			Controller.AddEventHandler("GMTubesPause", GMTubesPause);
			Controller.AddEventHandler("GMTubesContinue", GMTubesContinue);
			Controller.AddEventHandler("GMTubesNewPlayer", GMTubesNewPlayer);
			Controller.AddEventHandler("GMTubesNewPlayerGetName", GMTubesNewPlayerGetName);
			Controller.AddEventHandler("GMTubesNewPlayerCloseModal", GMTubesNewPlayerCloseModal);
			Controller.AddEventHandler("GMTubesVictory", GMTubesVictory);
			Controller.AddEventHandler("GMTubesVictoryOk", GMTubesVictoryOk);
			Controller.AddEventHandler("GMTubesVictoryCloseModal", GMTubesVictoryCloseModal);
			Controller.AddEventHandler("GMTubesSet1", GMTubesSet1);
			Controller.AddEventHandler("GMTubesExit", GMTubesExit);
			Controller.AddEventHandler("GMTubesGraph", GMTubesGraph);
			Controller.AddEventHandler("GMTubesGraphOut", GMTubesGraphOut);
			Controller.AddEventHandler("GMTubesGraphCloseModal", GMTubesGraphCloseModal);

			//Controller.AddToStore(this, StoredEventEventArgs.StoredMilliseconds(100, "GMTubesStart", this, EventArgs.Empty));
		}

		private void InitUserInfo()
		{
			UserInfo.UserName = "none";
			UserInfo.CurrentLevel = 1;
			UserInfo.Exp = 0;
			UserInfo.ExpNext = 100;
			UserInfo.UserName = Settings.EngineSettings.GetValue("GMTubes", "UserName");
			String s;
			s = Settings.EngineSettings.GetValue("GMTubes", "CurrentLevel");
			if (s != "") { int.TryParse(s, out UserInfo.CurrentLevel); }
			s = Settings.EngineSettings.GetValue("GMTubes", "Exp");
			if (s != "") { int.TryParse(s, out UserInfo.Exp); }
			s = Settings.EngineSettings.GetValue("GMTubes", "ExpNext");
			if (s != "") { int.TryParse(s, out UserInfo.ExpNext); }
			if (UserInfo.UserName==""){
				Controller.AddToOperativeStore(this, StoredEventEventArgs.Stored("GMTubesNewPlayer", this, EventArgs.Empty));
			}
		}

		private void GMTubesExit(object sender, EventArgs e)
		{
			Settings.EngineSettings.AddValue("GMTubes", "UserName", UserInfo.UserName, "");
			Settings.EngineSettings.AddValue("GMTubes", "CurrentLevel", UserInfo.CurrentLevel.ToString(), "");
			Settings.EngineSettings.AddValue("GMTubes", "Exp", UserInfo.Exp.ToString(), "");
			Settings.EngineSettings.AddValue("GMTubes", "ExpNext", UserInfo.ExpNext.ToString(), "");
			Controller.StartEvent("systemExit", this, EventArgs.Empty);
		}

		private void GMTubesPause(object sender, EventArgs e)
		{
			_f.Time.Stop();
			_menu.btnContinue.Show();
			_menu.Show();
			_vf.Hide();
		}

		private void GMTubesSet1(object sender, EventArgs e)
		{
			var s = sender as ViewButtonCoords;
			StartNew(s.Selected);
		}

		private void GMTubesStart(object sender, EventArgs e)
		{
			//StartNew(UserInfo.CurrentLevel);
			_menu.Hide();
			_f = new Field(UserInfo.CurrentLevel);
			_f.CalculateLevelCost();
			_vf.SetF(_f);
			_vf.Alpha = 0;
			_vf.Show();
			_f.Time.Restart();
		}

		/// <summary>
		/// Запустить уровень со сложностью level
		/// </summary>
		/// <param name="level"></param>
		private void StartNew(int level)
		{
			_menu.Hide();
			_f = new Field(level);
			_f.CalculateLevelCost();
			_vf.SetF(_f);
			_vf.Alpha = 0;
			_vf.Show();
			_f.Time.Restart();
		}

		private void GMTubesContinue(object sender, EventArgs e)
		{
			_menu.Hide();
			_vf.Show();
			_vf.Alpha = 0;
			_f.Time.Start();
		}

		private void GMTubesNewPlayer(object sender, EventArgs e)
		{
			_input = new ViewModalInputName(Controller, null, "GMTubesNewPlayerGetName", "GMTubesNewPlayerCloseModal", "Неизвестный");
			_sys.AddComponent(_input);
			_sys.BringToFront(_input);
		}

		private void GMTubesNewPlayerGetName(object sender, EventArgs e)
		{
			if (_input != null){
				UserInfo.UserName = _input.GetResult();
				UserInfo.CurrentLevel = 1;
				UserInfo.Exp = 0;
				UserInfo.ExpNext = 100;
			}
		}

		private void GMTubesNewPlayerCloseModal(object sender, EventArgs e)
		{
			_sys.Remove(_input);
			_input.Dispose();
			_input = null;
		}

		private void GMTubesVictory(object sender, EventArgs e)
		{
			var dt = _f.Time.Elapsed;
			var dts = dt.Minutes.ToString().PadLeft(2, '0') + ":" + dt.Seconds.ToString().PadLeft(2, '0');
			_congratulations = new ViewModalCongratulations(Controller, null, "GMTubesVictoryOk", "GMTubesVictoryCloseModal");
			_congratulations.c1 = Color.White;
			_congratulations.c2 = Color.Yellow;
			_congratulations.str1a = "За прохождение уровня вы заработали ";
			_congratulations.str1b = ""+_f.LevelCost;
			_congratulations.str1c = " экспы";
			_congratulations.str2a = "вы прошли уровень за ";
			_congratulations.str2b = ""+dts;
			_congratulations.str2c = "";
			var t = _f.LevelTimeInterval();
			if (t > 0){
				int d = (int) (_f.LevelCost*t*0.2);
				_congratulations.str3a = "За быстрое прохождение вам положена добавка ";
				_congratulations.str3b = "" +d;
				_congratulations.str3c = " экспы";
			}
			_sys.AddComponent(_congratulations);
			_sys.BringToFront(_congratulations);
		}

		private void GMTubesVictoryOk(object sender, EventArgs e){}// тут ничего не нужно делать

		private void GMTubesVictoryCloseModal(object sender, EventArgs e)
		{
			_sys.Remove(_congratulations);
			_congratulations.Dispose();
			_congratulations = null;
			var oldl = UserInfo.CurrentLevel;
			UserInfo.AddExp(_f.LevelCost);
			UserInfo.AddExp((int)(_f.LevelCost * _f.LevelTimeInterval() * 0.2));// добавка
			if (oldl != UserInfo.CurrentLevel)
			{// тут можно запустить ещё одно поздравление со следущим уровнем
			}
			GMTubesStart(sender, e);
		}

		private void GMTubesGraph(object sender, EventArgs e)
		{
			_graph=new ViewGraph(Controller, null, "GMTubesGraphOut","GMTubesGraphCloseModal");
			//_graph.AddGraphLine("Высота", Color.GreenYellow);
			var maxh = new List<PointF>();
			var maxw = new List<PointF>();
			var tGold = new List<PointF>();
			var tSilver = new List<PointF>();
			var max3x = new List<PointF>();
			var max4x = new List<PointF>();
			var fsize = new List<PointF>();
			var fsize2 = new List<PointF>();
			for (int i = 1; i < 11; i++)
			{
				_f.InitCreationParams(i);
				maxh.Add(new PointF(_f.MaxH, i));
				maxw.Add(new PointF(_f.MaxW, i));
				tGold.Add(new PointF(_f.TimeGold, i));
				tSilver.Add(new PointF(_f.TimeSilver, i));
				max3x.Add(new PointF(_f.Max3x, i));
				max4x.Add(new PointF(_f.Max4x, i));
				fsize.Add(new PointF(i, _f.MaxH * _f.MaxW/100));
				fsize2.Add(new PointF(_f.MaxH * _f.MaxW/100,i));
				//_graph.AddPoint("Высота",new PointF(i,_f.MaxH));
			}
			_graph.AddGraphLine("Высота", Color.MidnightBlue, maxh);
			_graph.AddGraphLine("Ширина", Color.MediumVioletRed, maxw);
			_graph.AddGraphLine("Золото", Color.DarkGoldenrod, tGold);
			//_graph.AddGraphLine("Серебро", Color.Silver, tSilver);
			//_graph.AddGraphLine("3х элементы ", Color.SeaGreen, max3x);
			//_graph.AddGraphLine("4х элементы", Color.SteelBlue, max4x);
			//_graph.AddGraphLine("Размер поля", Color.SeaGreen, fsize);
			//_graph.AddGraphLine("Размер поля2", Color.SeaGreen, fsize2);
			var fsine = new List<PointF>();
			for (int i = 1; i < 90; i++){
				fsine.Add(new PointF(i, (float)(10 * Math.Sin(Math.PI * i * 5 / 180.0))));
			}
			_graph.AddGraphLine("Синус", Color.Black, fsine);
			_sys.AddComponent(_graph);
			_sys.BringToFront(_graph);
		}
		
		private void GMTubesGraphOut(object sender, EventArgs e) { }// тут ничего не нужно делать

		private void GMTubesGraphCloseModal(object sender, EventArgs e)
		{
			_sys.Remove(_graph);
			_graph.Dispose();
			_graph = null;
		}

	}
}
