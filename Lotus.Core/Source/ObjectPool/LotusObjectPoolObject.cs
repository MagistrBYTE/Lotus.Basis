//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема объектного пула
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusObjectPoolObject.cs
*		Определение интерфейса для объекта поддерживающего пул.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreObjectPool
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения объекта поддерживающего пул
		/// </summary>
		/// <remarks>
		/// Максимально общая реализация
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusPoolObject
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Статус объекта из пула
			/// </summary>
			/// <remarks>
			/// Позволяет определять был ли объект взят из пула и значит его надо вернуть или создан обычным образом
			/// </remarks>
			Boolean IsPoolObject { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдо-конструктор
			/// </summary>
			/// <remarks>
			/// Вызывается диспетчером пула в момент взятия объекта из пула
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			void OnPoolTake();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдо-деструктор
			/// </summary>
			/// <remarks>
			/// Вызывается диспетчером пула в момент попадания объекта в пул
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			void OnPoolRelease();
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================