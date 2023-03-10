//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема логирования
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusLoggerView.cs
*		Определение интерфейса регистратора(логгера) для визуального отображения оповещений.
*		Сама подсистем лишь хранит все оповещения приложения, но не отображает их, так как отображение зависит от конечной
*	платформы. Определение интерфейса регистратора для отображения оповещений позволяет определить конкретный механизм 
*	отображения в зависимости от платформы и иных целей.
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
		//! \addtogroup CoreLogger
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс регистратора(логгера) для визуального отображения оповещений
		/// </summary>
		/// <remarks>
		/// Основная задача интерфейса представить механизм для визуального отображения оповещений
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILoggerView
		{
			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Общее оповещение
			/// </summary>
			/// <param name="text">Имя сообщения</param>
			/// <param name="type">Тип сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			void Log(String text, TLogType type);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Общее оповещение
			/// </summary>
			/// <param name="message">Сообщение</param>
			//---------------------------------------------------------------------------------------------------------
			void Log(TLogMessage message);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Оповещение определённого модуля/подсистемы
			/// </summary>
			/// <param name="module_name">Имя модуля/подсистемы</param>
			/// <param name="text">Имя сообщения</param>
			/// <param name="type">Тип сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			void LogModule(String module_name, String text, TLogType type);
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================