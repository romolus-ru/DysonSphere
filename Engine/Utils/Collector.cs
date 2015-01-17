using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Engine.Attributes;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Utils
{
	/// <summary>
	/// Важный класс, собирает и хранит всю информацию
	/// </summary>
	public class Collector
	{

		/// <summary>
		/// Событие ошибки. Теоретически не фатальной ошибки
		/// </summary>
		private ControllerEvent _errorEvent = null;

		/// <summary>
		/// хранит все объекты, по типам
		/// </summary>
		private readonly Dictionary<string, Dictionary<string, Type>> _objects =
			new Dictionary<string, Dictionary<string, Type>>();

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		public Collector(Controller controller)
		{
			// подписываемся на событие отправки ошибки
			_errorEvent = controller.GetControllerEvent("SendError");
			controller.AddEventHandler(@"GetCollectorObject", (o, args) => getThisObject(o, args as GetCollectorEventArgs));
			//controller.SendError("Создание коллектора");
		}

		/// <summary>
		/// Получить объект - коллектор
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void getThisObject(object sender, GetCollectorEventArgs e)
		{
			if (e != null){
				e.Collector = this;
			}
		}

		/// <summary>
		/// собирает из переданного файла всю информацию об объектах
		/// </summary>
		/// <param name="assemblyFile">Файл сборки</param>
		/// <param name="types">список типов, которые надо найти</param>
		public Boolean FindObjectsInAssembly(string assemblyFile, IEnumerable<Type> types)
		{
			Assembly assembly; // объявляем сборку
			// ищем имя сборки чтоб её загрузить
			if (!File.Exists(assemblyFile)){return false;}
			// пробуем загрузить
			try{
				AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception e){// если не загрузилось то показываем что к чему
				var ea = MessageEventArgs.Msg("Ошибка при загрузке сборки " +
					assemblyFile + Environment.NewLine + e.Message + Environment.NewLine + e.GetType());
				_errorEvent.StartEvent(null, ea);
				return false; // и выходим
			}
			var ret = false;
			// ищем нужные типы в объектах и сохраняем их для последующего использования
			foreach (Type type in types){
				if (type.IsInterface){
					var r=SearchObjectsWithInterface(assembly, type);
					if (r) ret = true;
				}else{
					var r=SearchObjects(assembly, type); 
					if (r) ret = true;
				}
			}
			return ret;
		}

		/// <summary>
		/// Поиск в переданной сборке объектов с интерфейсом
		/// </summary>
		/// <param name="assembly">Сборка</param>
		/// <param name="interfaceType">интерфейс</param>
		/// <returns>Есть ли хоть один интерфейс в сборке</returns>
		private Boolean SearchObjectsWithInterface(Assembly assembly, Type interfaceType)
		{
			var ret = false;
			Type[] types = assembly.GetTypes();
			foreach (var type in types){
				if (type == interfaceType) continue;
				// проходим по всем типам в сборке и проверяем есть ли у типа заданный интерфейс
				if (ExistInterface(type, interfaceType)){
					// интерфейс есть, надо добавлять
					AddTypeToObjects(interfaceType, type);
					ret = true;
				}
			}
			return ret;
		}

		/// <summary>
		/// Поиск в переданной сборке объектов нужного типа
		/// </summary>
		/// <param name="assembly">Сборка</param>
		/// <param name="objectType">Родительский тип</param>
		private Boolean SearchObjects(Assembly assembly, Type objectType)
		{
			var ret = false;
			Type[] types = assembly.GetTypes();
			var pE = from pe in types where pe != objectType select pe;// исключаем только родительский тип, чтоб ошибок небыло, да и они почти абстрактные
			//var pE = from pe in types where !pe.IsEnum select pe;
			foreach (Type type in pE){
				var searchType = type;
				var b = false; // флаг что нашли тип с нужным предком
				while (searchType != typeof(Object)){
					if (searchType == objectType){
						b = true; // нашли, выходим
						break;
					}
					// идём выше по иерархии
					if (searchType.BaseType != null) searchType = searchType.BaseType;
					// или прерываем поиск
					if (searchType.BaseType == null) break;
				}
				if (b){
					// проверяем доступность атрибута для данного типа
					var typeObjName = typeof(HideThisClassAttribute);
					var addType = true;// флаг можно ли добавить объект
					if (Attribute.IsDefined(type, typeObjName)){
						// проверяем флаг с помощью атрибута HideThisClassAttribute, если он есть. 
						// если нету - то можно создавать
						HideThisClassAttribute att = (HideThisClassAttribute)Attribute.GetCustomAttribute(type, typeObjName);
						if (att.HideThisObject) addType = false;
					}
					if (addType) {AddTypeToObjects(objectType, type);ret = true;}
				}
			}
			return ret;
		}

		/// <summary>
		/// проверяет, содержит ли класс указанный интерфейс
		/// </summary>
		/// <param name="type">Тип у которого проверяется интерфейс</param>
		/// <param name="iType">Интерфейс, наличие которого проверяется у типа</param>
		/// <returns></returns>
		private bool ExistInterface(Type type, Type iType)
		{
			var t = type.GetInterface(iType.ToString());
			return t != null;// если поддерживает то вернёт тру
		}

		/// <summary>
		/// Получить имя типа
		/// </summary>
		/// <param name="type">Тип у которого надо определить имя</param>
		/// <returns>Имя типа</returns>
		private String GetTypeName(Type type)
		{
			string s = ""; // узнаём имя типа в том числе и из атрибута
			var typeObjName = typeof(ClassNameAttribute);
			if (Attribute.IsDefined(type, typeObjName)){// используем имя определённое в атрибуте
				var att = (ClassNameAttribute)Attribute.GetCustomAttribute(type, typeObjName);
				s = att.ClassName;
			}// если интерфейса нету или там пустое имя, то присваиваем имя объекта
			if (s == ""){
				s = type.ToString(); // используем имя типа объекта
			}
			return s;
		}

		/// <summary>
		/// Добавить найденный тип в коллектор
		/// </summary>
		/// <param name="objectType"></param>
		/// <param name="type"></param>
		private void AddTypeToObjects(Type objectType, Type type)
		{
			string s = GetTypeName(type);// узнаём имя объекта в том числе и из атрибута
			// если объект не существует, добавляем словарь
			if (!_objects.ContainsKey(objectType.ToString())){
				// если нету ещё такого списка - создаём
				var dictionary = new Dictionary<string, Type>();
				_objects[objectType.ToString()] = dictionary;
			}
			// формируем имя объекта
			// получаем внутренний словарь, соответствующий типу objectType
			var objectsType = _objects[objectType.ToString()];
			// смотрим, есть ли с таким ключом уже значение
			if (objectsType.ContainsKey(s)){
				// содержит - значит проверяем интерфейс
				var typeObjName = typeof(ReplaceObjectAttribute);
				var replace = false;
				if (Attribute.IsDefined(type, typeObjName)){
					// используем имя определённое в атрибуте
					var att = (ReplaceObjectAttribute)Attribute.GetCustomAttribute(type, typeObjName);
					if (att.ReplaceObject) replace = true;
				}
				// флаг не перезаписывать - не добавляем
				if (replace){
					objectsType[s] = type;
				}else{
					// в наличии повторение индексов и отсутстви флага на разрешение перезаписи - исключение
					var ea = MessageEventArgs.Msg("Ошибка при добавлении объекта в коллектор " +
												  Environment.NewLine + objectType.ToString() + "/" + s);
					_errorEvent.StartEvent(null, ea);
				}
			}else{
				// нету такого значения - добавляем без вопросов
				objectsType.Add(s, type);
			}
		}

		/// <summary>
		/// создаёт объект типа keyType по имени objName из общего списка объектов
		/// </summary>
		/// <param name="keyType">Базовый тип, предка которого создаём</param>
		/// <param name="objName">Имя объекта, который надо создать</param>
		/// <returns></returns>
		public Object Create(Type keyType, string objName)
		{
			object o = null;// создаём объект активатором или возвращаем нул
			if (_objects.ContainsKey(keyType.ToString())){
				var types = _objects[keyType.ToString()];
				if (types.ContainsKey(objName)){
					o = Activator.CreateInstance(types[objName]);
				}
			}
			return o;
		}

		/// <summary>
		/// позволяет создать только 1 экземпляр объекта. после создания удаляет его описание
		/// </summary>
		/// <param name="keyType"></param>
		/// <param name="objName"></param>
		/// <returns>Но может вызвать ряд других ошибок при недостаточной проверке данных</returns>
		public Object CreateOne(Type keyType, string objName)
		{
			var o = Create(keyType, objName);
			// а вот тут надо удалить этот объект чтоб он больше не появлялся
			if (_objects.ContainsKey(keyType.ToString()))
				_objects[keyType.ToString()].Remove(objName);
			return o;
		}

		/// <summary>
		/// получить значения всех Key и ихних Value, хранящихся в Хранилище
		/// </summary>
		/// <returns></returns>
		/// <remarks>Диагностическая функция</remarks>
		public string GetKeyValuesString()
		{
			string rs = ""; // проходим по внешнему словарю
			foreach (var o in _objects){
				rs += "" + o.Key /* + " - " + o.Value*/+ Environment.NewLine;
				// проходим по внутреннему словарю
				foreach (var valuePair in o.Value)
					rs += " " + valuePair.Key + " - " + valuePair.Value + Environment.NewLine;
			}
			return rs;
		}

		/// <summary>
		/// ищем объекты нужного типа
		/// </summary>
		/// <param name="typeOfBaseObject"></param>
		/// <returns></returns>
		public Dictionary<string, Type> GetObjects(Type typeOfBaseObject)
		{
			if (_objects.ContainsKey(typeOfBaseObject.ToString()))
				return _objects[typeOfBaseObject.ToString()];
			return new Dictionary<string, Type>();
		}
	}
}
