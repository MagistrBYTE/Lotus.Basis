﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема результата операции
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusResult.cs
*		Определение класса для представления ответа/результата выполнения операции.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup CoreResultsSystem
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для формирования ответа/результата выполнения операции
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XResult
		{
			#region ======================================= НЕУСПЕШЕЫЙ РЕЗУЛЬТАТ ======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование результата/ответа о неуспешности выполнения операции
			/// </summary>
			/// <typeparam name="TData">Тип объекта</typeparam>
			/// <param name="result">Результат/ответ операции</param>
			/// <returns>Результат/ответ операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Result<TData> Failed<TData>(Result result)
			{
				return new Result<TData>(result.Code, result.Message, default, false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование результата/ответа о неуспешности выполнения операции
			/// </summary>
			/// <typeparam name="TData">Тип объекта</typeparam>
			/// <param name="code">Код</param>
			/// <param name="message">Сообщение о результате выполнения операции</param>
			/// <returns>Результат/ответ операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Result<TData> Failed<TData>(Int32 code, String message)
			{
				return new Result<TData>(code, message, default, false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование результата/ответа о неуспешности выполнения операции
			/// </summary>
			/// <typeparam name="TData">Тип объекта</typeparam>
			/// <param name="code">Код</param>
			/// <param name="message">Сообщение о результате выполнения операции</param>
			/// <param name="data">Данные</param>
			/// <returns>Результат/ответ операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Result<TData> Failed<TData>(Int32 code, String message, TData data)
			{
				return new Result<TData>(code, message, data, false);
			}
			#endregion

			#region ======================================= УСПЕШЕЫЙ РЕЗУЛЬТАТ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование результата о успешности выполнения операции
			/// </summary>
			/// <typeparam name="TData">Тип объекта</typeparam>
			/// <param name="data">Объект</param>
			/// <returns>Результат/ответ операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Result<TData> Succeed<TData>(TData data)
			{
				return new Result<TData>(data, true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование результата о успешности выполнения операции
			/// </summary>
			/// <typeparam name="TData">Тип объекта</typeparam>
			/// <param name="code">Код</param>
			/// <param name="data">Объект</param>
			/// <returns>Результат/ответ операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Result<TData> Succeed<TData>(Int32 code, TData data)
			{
				return new Result<TData>(code, data, true);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс определяющий некий результат/ответ операции
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class Result : ILotusResult, ICloneable
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Результат успешного выполнения операции
			/// </summary>
			public static readonly Result Ok = new Result(0, true);

			/// <summary>
			/// Результат неуспешного выполнения операции
			/// </summary>
			public static readonly Result Failed = new Result(0, "Error", null, false);
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Статус успешности выполнения операции
			/// </summary>
			public Boolean Succeeded { get; set; }

			/// <summary>
			/// Код
			/// </summary>
			public Int32 Code { get; set; }

			/// <summary>
			/// Сообщение о результате выполнения операции
			/// </summary>
			public String Message { get; set; }

			/// <summary>
			/// Дополнительные данные
			/// </summary>
			public System.Object? Data { get; set; }
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные поверхности предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public Result()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="code">Код</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(Int32 code, Boolean status)
			{
				Code = code;
				Succeeded = status;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="message">Сообщение о результате выполнения операции</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(String message, Boolean status)
			{
				Message = message;
				Succeeded = status;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="code">Код</param>
			/// <param name="data">Дополнительные данные</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(Int32 code, System.Object data, Boolean status)
			{
				Code = code;
				Data = data;
				Succeeded = status;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="message">Сообщение о результате выполнения операции</param>
			/// <param name="data">Дополнительные данные</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(String message, System.Object? data, Boolean status)
			{
				Message = message;
				Data = data;
				Succeeded = status;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="code">Код</param>
			/// <param name="message">Сообщение о результате выполнения операции</param>
			/// <param name="data">Дополнительные данные</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(Int32 code, String message, System.Object data, Boolean status)
			{
				Code = code;
				Message = message;
				Data = data;
				Succeeded = status;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование объекта
			/// </summary>
			/// <returns>Копия объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object Clone()
			{
				return MemberwiseClone();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return $"OK: {Succeeded} | Message: {Message}";
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс определяющий некий результат/ответ операции с типизированными дополнительным данными
		/// </summary>
		/// <typeparam name="TData">Тип данных</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class Result<TData> : Result, ILotusResult<TData>
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Результат успешного выполнения операции
			/// </summary>
			new public static readonly Result<TData> Ok = new Result<TData>(0, true);
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Данные
			/// </summary>
			new public TData Data { get; set; }
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные поверхности предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public Result()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="code">Код</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(Int32 code, Boolean status)
				: base(code, status)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="code">Код</param>
			/// <param name="data">Дополнительные данные</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(Int32 code, TData? data, Boolean status)
				: base(code, data, status)
			{
				Data = data;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="data">Дополнительные данные</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(TData data, Boolean status)
			{
				base.Data = data;
				Data = data;
				Succeeded = status;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="message">Сообщение о результате выполнения операции</param>
			/// <param name="data">Дополнительные данные</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(String message, TData? data, Boolean status)
				: base(message, data, status)
			{
				Data = data;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="code">Код</param>
			/// <param name="message">Сообщение о результате выполнения операции</param>
			/// <param name="data">Дополнительные данные</param>
			/// <param name="status">Статус выполнения операции</param>
			//---------------------------------------------------------------------------------------------------------
			public Result(Int32 code, String message, TData? data, Boolean status)
				:base(code, message, data, status)
			{
				Data = data;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================