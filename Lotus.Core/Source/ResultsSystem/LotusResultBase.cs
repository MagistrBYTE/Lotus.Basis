﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема результата операции
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusResultBase.cs
*		Определение интерфейса для представления ответа/результата операции.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		/**
         * \defgroup CoreResultsSystem Подсистема результата операции
         * \ingroup Core
         * \brief Подсистема результата операции реализует общую схему ответа/результата выполнения различных 
			операций(вызов метода, дополнительные данные в ответе и т.д) от различных подсистем и определяет 
			успешность/не успешность ответа, а также вспомогательную информацию. 
         * @{
         */
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для представления ответа/результата операции
		/// </summary>
		/// <remarks>
		/// Помимо основного назначения - характеристики успешности или неуспешности выполнения, 
		/// может также нести дополнительную информацию 
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusResult
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Статус успешности выполнения операции
			/// </summary>
			Boolean Succeeded { get; }

			/// <summary>
			/// Код ответа
			/// </summary>
			/// <remarks>
			/// В зависимости от подсистемы коды могут по-разному интерпретироваться
			/// </remarks>
			Int32 Code { get; }

			/// <summary>
			/// Сообщение о результате выполнения операции
			/// </summary>
			String Message { get; }

			/// <summary>
			/// Дополнительные данные
			/// </summary>
			System.Object Data { get; set; }
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для представления ответа/результата операции с типизированными дополнительным данными
		/// </summary>
		/// <typeparam name="TData">Тип объекта</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusResult<TData> : ILotusResult
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Дополнительные данные
			/// </summary>
			new TData Data { get; set; }
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================