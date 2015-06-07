using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils.ExtensionMethods;
using Engine.Utils.GraphView;
using Engine.Utils.Settings;
using Engine.Views;
using Engine.Utils.Editor;
using Engine.Views.Templates;
using GMTubes.Controllers;
using GMTubes.Model;
using GMTubes.View;
using Button = Engine.Views.Templates.Button;
using UserInfo = GMTubes.View.UserInfo;

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
		private Field _mf;
		private ViewMenu _menu;
		private ViewModalInputName _input;
		private ViewModalCongratulations _congratulations;
		private Background background;
		private ViewGraph _graph;

		public View.UserInfo VUserInfo=new UserInfo();
		public Model.UserInfo MUserInfo;

		protected override void SetUpModel(Engine.Models.Model model, Controller controller)
		{
			base.SetUpModel(model, controller);
			MUserInfo=new Model.UserInfo();
			_mf = new Field(controller);
			Controller.AddEventHandler("GMTubesUserInfoRecreate", GMTubesUserInfoRecreateEH);
			Controller.AddEventHandler("GMTubesFieldResolved", GMTubesFieldResolvedEH);// внутреннее событие - уровень решён игроком, нужно собрать нужную информацию и отослать виду
			Controller.AddEventHandler("GMTubesUserInfoGet", GMTubesUserInfoGetEH);
		}

		private void GMTubesUserInfoGetEH(object sender, EventArgs e)
		{
			// отправляем информацию о пользователе по требованию
			Controller.SendToViewCommand("GMTubesUserInfoFill",
				UserInfoEventArgs.Send(MUserInfo.UserName, MUserInfo.Exp, MUserInfo.ExpNext, MUserInfo.CurrentLevel));
		}

		private void GMTubesFieldResolvedEH(object sender, EventArgs e)
		{
			// формируем событие которое отправится виду. получаем параметры уровня и прибавляем результат пользователю
			_mf.CalculateLevelCost();
			var t = _mf.LevelTimeInterval();
			int d = (int)(_mf.LevelCost * t * 0.2);
			
			var old1 = MUserInfo.CurrentLevel;
			MUserInfo.AddExp(_mf.LevelCost);
			MUserInfo.AddExp(d);// добавка
			var levelUpdated = MUserInfo.CurrentLevel - old1;
			GMTubesUserInfoGetEH(sender, e);// обновляем информацию о пользователе

			var v = new VictoryInfoEventArgs();
			v.IsVictory = true;
			v.MainExp = _mf.LevelCost;
			v.extraExp = d;
			v.SecondsElapsed = (int) _mf.Time.Elapsed.TotalSeconds;
			v.LevelUpdate = levelUpdated;

			Controller.SendToModelCommand("GMTubesVictory", v);
		}

		private void GMTubesUserInfoRecreateEH(object sender, EventArgs e)
		{
			var ec = e as MessageEventArgs;
			GMTubesUserInfoRecreate(sender, ec.Deserialize<UserInfoEventArgs>(e));
		}
		private void GMTubesUserInfoRecreate(object sender, UserInfoEventArgs e)
		{
			MUserInfo.UserName = e.UserName;
			MUserInfo.CurrentLevel = 1;
			MUserInfo.Exp = 0;
			MUserInfo.ExpNext = 100;
			GMTubesUserInfoGetEH(sender, e);
		}

		protected override void SetUpView(Engine.Views.View view, Controller controller)
		{
			_sys = new ViewControlSystem(Controller);
			_sys.Show();
			view.AddObject(_sys);

			_menu = new ViewMenu(controller, _sys, VUserInfo);
			
			_vf = null;//new ViewField(controller, _sys);
			//_vf.Hide();

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
			Controller.AddEventHandler("GMTubesSet1", GMTubesStartSelected);
			Controller.AddEventHandler("GMTubesExit", GMTubesExit);
			Controller.AddEventHandler("GMTubesGraph", GMTubesGraph);
			Controller.AddEventHandler("GMTubesGraphOut", GMTubesGraphOut);
			Controller.AddEventHandler("GMTubesGraphCloseModal", GMTubesGraphCloseModal);
			Controller.AddEventHandler("GMTubesUserInfoFill", GMTubesUserInfoFillEH);

			// требуем у модели передать информацию о пользователе (в ответ получаем GMTubesUserInfoFill)
			Controller.SendToModelCommand("GMTubesUserInfoGet", new EngineEventArgs());
			//Controller.AddToStore(this, StoredEventEventArgs.StoredMilliseconds(100, "GMTubesStart", this, EventArgs.Empty));
		}

		private void GMTubesUserInfoFillEH(object sender, EventArgs e)
		{
			var ec = e as MessageEventArgs;
			GMTubesUserInfoFill(sender, ec.Deserialize<UserInfoEventArgs>(e));
		}
		private void GMTubesUserInfoFill(object sender, UserInfoEventArgs e)
		{
			VUserInfo.UserName = e.UserName;
			VUserInfo.Exp = e.Exp;
			VUserInfo.ExpNext = e.ExpNext;
			VUserInfo.CurrentLevel = e.CurrentLevel;
			if (VUserInfo.UserName == ""){// рекурсивно вызываем функцию.
				Controller.AddToOperativeStore(this, StoredEventEventArgs.Stored("GMTubesNewPlayer", this, EventArgs.Empty));
			}
		}

		private void GMTubesExit(object sender, EventArgs e)
		{
			MUserInfo.SaveUserInfo();
			Controller.StartEvent("systemExit", this, EventArgs.Empty);
		}

		private void GMTubesPause(object sender, EventArgs e)
		{
			_vf.TimePause();
			//_menu.btnContinue.Show();
			_menu.Show();
			_vf.Hide();
		}

		private void GMTubesStartSelected(object sender, EventArgs e)
		{
			var s = sender as ViewButtonCoords;
			StartNew(s.Selected);
		}

		private void GMTubesStart(object sender, EventArgs e)
		{
			_menu.Hide();
			StartNew(VUserInfo.CurrentLevel);
		}

		/// <summary>
		/// Запустить уровень со сложностью level
		/// </summary>
		/// <param name="level"></param>
		private void StartNew(int level)
		{
			_menu.Hide();
			if (_vf != null) {_sys.Remove(_vf); _vf.Dispose();}
			_vf = new ViewField(level, Controller,_sys);
			_vf.Alpha = 0;
			_vf.Show();
			_vf.TimeContinue();
			_sys.BringToFront(_vf);
		}

		private void GMTubesContinue(object sender, EventArgs e)
		{
			_menu.Hide();
			_vf.Show();
			_vf.Alpha = 0;
			_vf.TimeContinue();
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
				var u = UserInfoEventArgs.Send(_input.GetResult(), 0, 100, 1);
				Controller.SendToModelCommand("GMTubesUserInfoRecreate", u);// сохраняем информацию о пользователе, оттуда придёт обновление информации
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
			var ec = e as MessageEventArgs;
			var ev = ec.Deserialize<VictoryInfoEventArgs>(e);
			var dt = TimeSpan.FromSeconds(ev.SecondsElapsed);
			var dts = dt.Minutes.ToString().PadLeft(2, '0') + ":" + dt.Seconds.ToString().PadLeft(2, '0');
			_congratulations = new ViewModalCongratulations(Controller, null, "GMTubesVictoryOk", "GMTubesVictoryCloseModal");
			_congratulations.c1 = Color.White;
			_congratulations.c2 = Color.Yellow;
			_congratulations.str1a = "За прохождение уровня вы заработали ";
			_congratulations.str1b = ""+ev.MainExp;
			_congratulations.str1c = " экспы";
			_congratulations.str2a = "вы прошли уровень за ";
			_congratulations.str2b = ""+dts;
			if (ev.LevelUpdate > 0){
				_congratulations.str2c = " и получили новый уровень!";
			}
			if (ev.extraExp > 0){
				_congratulations.str3a = "За быстрое прохождение вам положена добавка ";
				_congratulations.str3b = "" +ev.extraExp;
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
			//if (oldl != UserInfo.CurrentLevel){// тут можно запустить ещё одно поздравление со следущим уровнем}
			//GMTubesStart(sender, e); заменить на что то другое!
			_menu.Show();// выводим меню
			_menu.btnContinue.Hide();// прячем кнопку "продолжить"
			_vf.Hide();
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
				_mf.InitCreationParams(i);
				maxh.Add(new PointF(_mf.MaxH, i));
				maxw.Add(new PointF(_mf.MaxW, i));
				tGold.Add(new PointF(_mf.TimeGold, i));
				tSilver.Add(new PointF(_mf.TimeSilver, i));
				max3x.Add(new PointF(_mf.Max3x, i));
				max4x.Add(new PointF(_mf.Max4x, i));
				fsize.Add(new PointF(i, _mf.MaxH * _mf.MaxW/100));
				fsize2.Add(new PointF(_mf.MaxH * _mf.MaxW/100,i));
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
