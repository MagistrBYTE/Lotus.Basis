//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема идентификаторов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusIdentifierBase.cs
*		Определение базовых интерфейсов для идентификации объектов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreIdentifiers Подсистема идентификаторов
		//! Подсистема идентификаторов определяет базовые понятия об идентифкации и классы идентификации объектов.
		//! Под идентификацией объекта понимается наличие у сущности определённого уникального идентификатора по
		//! которому ее возможно однозначно найти или получить в определённом контексте
		//! \ingroup Core
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый интерфейс обозначающих что сущность поддерживает идентификацию
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusIdentifier
		{
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для идентификации сущности через уникальный идентификатор-ключ
		/// </summary>
		/// <typeparam name="TKey">Тип ключа</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusIdentifierId<TKey> : ILotusIdentifier where TKey : IEquatable<TKey>
		{
			/// <summary>
			/// Ключ сущности
			/// </summary>
			TKey Id { get; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для объектов реализующих идентификации через уникальный числовой идентификатор
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusIdentifierId : ILotusIdentifierId<Int64>
		{
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для объектов реализующих понятие имени
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusNameable
		{
			/// <summary>
			/// Имя объекта
			/// </summary>
			String Name { get; set; }
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================