using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Engine.Utils.Editor;
using Engine.Views.Templates;
using Button = Engine.Views.Templates.Button;

namespace SimpleMapEditor
{
	class SimpleMapEditor : Module
	{
		private editor _editor;
		private viewObjectFullView _voFullView;
		private Boolean _newObjClick;
		private Dictionary<int, SimpleEditableObject> data = new Dictionary<int, SimpleEditableObject>();
		private LayerSimpleEditableObject l1;
		private LayerSimpleEditableObjectView l2;
		private LayerSimpleEditableObjectMove l3;
		private LayerSimpleEditableObjectTeleport l4;
		private LayerSimpleEditableObjectMap l9;
		private int currLayer = 0;//переключение между слоями редактора
		private ViewModalSelectFile selectFile;
		private ViewModalInputName InputString;
		private ViewWindow window2;
		private ViewModalInput input;
		private ShowMsg ShowMsg;

		private ViewControlSystem sys;

		protected override void SetUpView(Engine.Views.View view, Controller controller)
		{
			sys=new ViewControlSystem(Controller);
			sys.Show();
			view.AddObject(sys);

			//sys = view._viewMainObjects;

			var b1=Button.CreateButton(controller, 950, 0, 74, 20, "systemExit", "Выход", "Esc", Keys.Escape,"");
			sys.AddComponent(b1);

			sys.AddComponent(Button.CreateButton(controller, 900, 720, 100, 20, "SimpleSave", "Сохранить", "S", Keys.S,""));
			sys.AddComponent(Button.CreateButton(controller, 800, 720, 100, 20, "SimpleLoad", "Загрузить", "L", Keys.L,""));
			sys.AddComponent(Button.CreateButton(controller, 900, 25, 130, 20, "MapView", "Смена слоя", "Активировать другой слой редактора", Keys.M,""));
			sys.AddComponent(Button.CreateButton(controller, 950, 55, 74, 20, "ModalStart", "Modal", "Модальный запрос", Keys.H,""));
			sys.AddComponent(Button.CreateButton(controller, 900, 85, 130, 20, "ModalInput", "ModalInput", "Ввод текста", Keys.I, "modalInput"));
			
			CreateEditor();
			var s = new SimpleEditableObject();
			s.X = 32;
			s.Y = 64;
			_editor.AddNewObject(s);
			
			Controller.AddEventHandler("SimpleSave", Save);
			Controller.AddEventHandler("SimpleLoad", Load);
			Controller.AddEventHandler("MapView", MapView);
			Controller.AddEventHandler("ExitFullView", MapView);
			Controller.AddEventHandler("ModalStart", ModalStart);
			Controller.AddEventHandler("ModalResult", ModalResult);
			Controller.AddEventHandler("ModalDestroy", ModalDestroy);
			Controller.AddEventHandler("ModalInput", ModalInput);
			Controller.AddEventHandler("ModalInputResult", ModalInputResult);
			Controller.AddEventHandler("ModalInputDestroy", ModalInputDestroy);

			//Controller.AddEventHandler("ModalInputStart", ModalInputStart);
			//Controller.AddEventHandler("ModalInputClosed", ModalInputResult);
			
			window2 = new SimpleWindow(Controller, sys);
			window2.SetCoordinates(200, 650, 0);
			window2.SetSize(100, 50);
			//window2.Name = "w2";
			//sys.AddComponent(window2);
			//var background = new Background(controller);
			//view.AddObject(background);
			ShowMsg=new ShowMsg(controller,sys);
		}

		/*private void ModalInputResult(object sender, EventArgs e)
		{
			var s = input.GetResult();
			sys.Remove(input);
			input.Dispose();
			input = null;
		}

		private void ModalInputStart(object sender, EventArgs e)
		{
			input = new ViewModalInput(Controller, "ModalInputClosed","");
			//Controller.StartEvent("ViewAddObject", this, ViewObjectEventArgs.vObj(input));
			//Controller.ViewAddObjectCommand(this, input);
			sys.AddComponent(input);
		}*/
		
		private void ModalResult(object sender, EventArgs e)
		{
			var m = sender as ViewModal;
			if (m != null){
				var i = m.ModalResult;
			}
		}

		private void ModalDestroy(object sender, EventArgs e)
		{
			sys.Remove(selectFile);
			selectFile.Dispose();
			selectFile = null;
		}

		private void ModalStart(object sender, EventArgs e)
		{
			selectFile = new ViewModalSelectFile(Controller,null, "ModalClosed","ModalDestroy");
			sys.AddComponent(selectFile);
		}

		private void ModalInputResult(object sender, EventArgs e)
		{
			var m = sender as ViewModalInput;
			if (m != null)
			{
				var s = m.GetResult();
			}
		}

		private void ModalInputDestroy(object sender, EventArgs e)
		{
			sys.Remove(InputString);
			InputString.Dispose();
			InputString = null;
		}
		
		private void ModalInput(object sender, EventArgs e)
		{
			InputString = new ViewModalInputName(Controller, null, "ModalInputClosed", "ModalInputDestroy","строка");
			InputString.SetSize(300, 50);
			InputString.SetCoordinates(100, 100, 0);
			sys.AddComponent(InputString);
		}

		/// <summary>
		/// Переключение режима просмотра карты
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MapView(object sender, EventArgs e)
		{
			currLayer++;
			if (currLayer > 4) currLayer = 0;
			if (currLayer == 0) { _editor.SetActiveLayer("objects"); l1.SynhronizeMapCoords(l4.MapX, l4.MapY);Msg("Режим : Основной");}
			if (currLayer == 1) { _editor.SetActiveLayer("objectsView"); l2.SynhronizeMapCoords(l1.MapX, l1.MapY);Msg("Режим : установка дополнительных текстур"); }
			if (currLayer == 2) { _editor.SetActiveLayer("objectsMove"); l3.SynhronizeMapCoords(l2.MapX, l2.MapY);Msg("Режим : редактирование перемещения"); }
			if (currLayer == 3) { _editor.SetActiveLayer("objectsTeleport"); l4.SynhronizeMapCoords(l3.MapX, l3.MapY);Msg("Режим : редактирование телепортов"); }
			if (currLayer == 4) { _editor.SetActiveLayer("Map");Msg("Режим : карта"); }
		}

		private void Msg(String s)
		{
			Controller.StartEvent("ShowMsg", this, MessageEventArgs.Msg(s));
		}

		private void Load(object sender, EventArgs e)
		{
			// TODO и при сохранении и при загрузке выбирать из списка файлов
			// так же сделать и ввод имени файла сохранения
			CreateEditor();
			_editor.Load("_a1.arch");
		}

		/// <summary>
		/// Создаём редактор и иницализируем что ещё нужно
		/// </summary>
		private void CreateEditor()
		{
			// потом всё равно надо будет продумать систему для удаления обработчиков и слоёв. так что решение временное
			// и лучше к нему вернуться побыстрее
			// TODO переделать что бы была возможность удалить объект
			if (_editor == null){// если редактор ещё не инициирован, то создаём его и всё такое
				_editor = new editor(Controller, sys);
				_editor.Show();
				l1 = new LayerSimpleEditableObject(Controller, "objects", data,_editor);
				l2 = new LayerSimpleEditableObjectView(Controller, "objectsView", data, _editor);
				l3 = new LayerSimpleEditableObjectMove(Controller, "objectsMove", data, _editor);
				l4 = new LayerSimpleEditableObjectTeleport(Controller, "objectsTeleport", data, _editor);
				l9 = new LayerSimpleEditableObjectMap(Controller, "Map", data, _editor);
				_editor.AddNewLayer(l1);
				_editor.AddNewLayer(l2);
				_editor.AddNewLayer(l3);
				_editor.AddNewLayer(l4);
				_editor.AddNewLayer(l9);
			}
			l9.CanStore = false;
			l9.Hide();
			l2.CanStore = false;
			l2.Hide();
			l3.CanStore = false;
			l3.Hide();
			l4.CanStore = false;
			l4.Hide();
			_editor.SetActiveLayer("objects");
			data.Clear();
		}
		
		private void Save(object sender, EventArgs e)
		{
			_editor.Save("_a1.arch");
		}

	}
}
