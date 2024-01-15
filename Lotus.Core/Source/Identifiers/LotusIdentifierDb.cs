﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема идентификаторов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusIdentifierDb.cs
*		Определение шаблонного класса для сущностей баз данных поддерживающих идентификатор.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.ComponentModel;
#if NET6_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup CoreIdentifiers
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблонный класс для сущностей баз данных поддерживающих идентификатор через первичный ключ
		/// </summary>
		/// <typeparam name="TKey">Тип ключа</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class EntityDb<TKey> : ILotusIdentifierIdTemplate<TKey> where TKey : IEquatable<TKey>
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Ключ сущности
			/// </summary>
#if NET6_0_OR_GREATER
			[Key]
			[Required]
#endif
			public virtual TKey Id { get; set; } = default!;
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public EntityDb()
#pragma warning disable CS8604 // Possible null reference argument.
				: this(default)
#pragma warning restore CS8604 // Possible null reference argument.
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="id">Ключ сущности</param>
			//---------------------------------------------------------------------------------------------------------
			public EntityDb(TKey id)
			{
				Id = id;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Наименование объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String? ToString()
			{
				return Id.ToString();
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблонный класс для сущностей баз данных поддерживающих идентификатор через первичный ключ
		/// </summary>
		/// <typeparam name="TKey">Тип ключа</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class EntityDbNotifyProperty<TKey> : EntityDb<TKey>, INotifyPropertyChanged where TKey : IEquatable<TKey>
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public EntityDbNotifyProperty()
#pragma warning disable CS8604 // Possible null reference argument.
                : this(default)
#pragma warning restore CS8604 // Possible null reference argument.
            {

            }

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="id">Ключ сущности</param>
			//---------------------------------------------------------------------------------------------------------
			public EntityDbNotifyProperty(TKey id)
			{
				Id = id;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Наименование объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String? ToString()
			{
				return Id.ToString();
			}
			#endregion

			#region ======================================= ДАННЫЕ INotifyPropertyChanged =============================
			/// <summary>
			/// Событие срабатывает ПОСЛЕ изменения свойства
			/// </summary>
			public event PropertyChangedEventHandler? PropertyChanged;

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный метод для нотификации изменений свойства
			/// </summary>
			/// <param name="propertyName">Имя свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void NotifyPropertyChanged(String propertyName = "")
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный метод для нотификации изменений свойства
			/// </summary>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			public void NotifyPropertyChanged(PropertyChangedEventArgs args)
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, args);
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблонный класс для сущностей баз данных поддерживающих идентификатор через первичный ключ c поддержкой имени
		/// </summary>
		/// <typeparam name="TKey">Тип ключа</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class EntityNameDb<TKey> : EntityDb<TKey>, ILotusNameable where TKey : IEquatable<TKey>
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя сущности
			/// </summary>
#if NET6_0_OR_GREATER
			[Required]
#endif
			public virtual String Name { get; set; } = default!;
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public EntityNameDb()
#pragma warning disable CS8604 // Possible null reference argument.
                : this(default)
#pragma warning restore CS8604 // Possible null reference argument.
            {

            }

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="id">Ключ сущности</param>
			//---------------------------------------------------------------------------------------------------------
			public EntityNameDb(TKey id)
			{
				Id = id;
				Name = String.Empty;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Наименование объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String? ToString()
			{
				return $"Name: {Name} | Id: {Id}";
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================