namespace Engine
{
	public interface ISound
	{
		/// <summary>
		/// Загрузка файла и присвоение ему имени
		/// </summary>
		/// <param name="soundName"></param>
		/// <param name="fileName"></param>
		void Load(string soundName, string fileName);

		/// <summary>
		/// Выгрузка файла из памяти
		/// </summary>
		/// <param name="soundName"></param>
		void Unload(string soundName);

		/// <summary>
		/// Остановить воспроизведение звука
		/// </summary>
		/// <param name="soundName"></param>
		void Stop(string soundName);

		/// <summary>
		/// Запустить воспроизведение файла
		/// </summary>
		/// <param name="soundName"></param>
		void Start(string soundName);
	}
}
