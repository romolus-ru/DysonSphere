using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;

namespace Engine.Utils
{
	/// <summary>
	/// Работа с архивным файлом (zip, поддерживается .net по умолчанию)
	/// </summary>
	internal class FileArchieve:IDisposable
	{
		/// <summary>
		/// Открываемый архив
		/// </summary>
		private FileStream _zipToOpen;

		private ZipArchive _archive;

		/// <summary>
		/// Сброшена ли информация на диск
		/// </summary>
		private Boolean _disposed;

		/// <summary>
		/// Всё равно "только для чтения", так что изменить эту переменную просто так не получится
		/// </summary>
		public ReadOnlyCollection<ZipArchiveEntry> Files;


		public FileArchieve(String fileName, Boolean createMode=true)
		{
			_zipToOpen = new FileStream(fileName, FileMode.OpenOrCreate);
			if (createMode){
				_archive = new ZipArchive(_zipToOpen, ZipArchiveMode.Create);
				Files = null;// нельзя обращаться к entities в момент создания
			}
			else{
				_archive = new ZipArchive(_zipToOpen, ZipArchiveMode.Read);
				Files = _archive.Entries;
			}
		}

		/// <summary>
		/// Добавить поток к архиву
		/// </summary>
		/// <param name="fName"></param>
		/// <param name="ms"></param>
		public void AddStream(string fName, MemoryStream ms)
		{
			ZipArchiveEntry fileEntry = _archive.CreateEntry(fName);
			using (var s = fileEntry.Open()){
				ms.WriteTo(s);
			}
		}

		/// <summary>
		/// Получить файл из архива как поток
		/// </summary>
		/// <param name="fName"></param>
		/// <returns>поток или null</returns>
		public MemoryStream GetStream(string fName)
		{
			MemoryStream ms = null;
			foreach (ZipArchiveEntry entry in _archive.Entries){
				if (entry.FullName == fName){
					ms = new MemoryStream();
					var stream = entry.Open();
					stream.CopyTo(ms);
					ms.Seek(0, SeekOrigin.Begin);
				}
			}
			return ms;
		}

		protected virtual void Dispose(Boolean disposing)
		{
			if (!_disposed){
				_archive.Dispose();
				_archive = null;
				try{
					//_zipToOpen.Flush();
					_zipToOpen.Dispose();
					_zipToOpen = null;
				}
				catch (Exception e){
					throw new Exception("Ошибка в классе FileArchieve " + e.Message);
				}
				_disposed = true;
			}
		}


		public void Dispose()
		{
			Dispose(true);
		}

		~FileArchieve()
		{
			Dispose(false);
		}

	}
}