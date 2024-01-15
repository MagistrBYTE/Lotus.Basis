﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема хранения состояний
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusMementoCaretaker.cs
*		Определение концепции смотрителя за объектом.
*		Смотритель может сохранить состояние объекта и восстановить его. Также реализована концепция смотрителя с 
*	поддержкой отмены/повторения действий.
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
		/** \addtogroup CoreMemento
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс смотрителя за объектом
		/// </summary>
		/// <remarks>
		/// Смотритель может сохранить состояние объекта и восстановить его
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusMementoCaretaker
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Объект состояние которого сохраняется и восстанавливается
			/// </summary>
			ILotusMementoOriginator MementoOriginator { get; set; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранить состояние объекта
			/// </summary>
			/// <param name="stateName">Наименование состояния объекта</param>
			//---------------------------------------------------------------------------------------------------------
			void SaveState(String stateName);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановить состояние объекта
			/// </summary>
			/// <param name="stateName">Наименование состояния объекта</param>
			//---------------------------------------------------------------------------------------------------------
			void RestoreState(String stateName);
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс реализующий смотрителя за объектом
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CMementoCaretaker : ILotusMementoCaretaker
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Общие данные
			internal ILotusMementoOriginator mOriginator;
			internal System.Object mState;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Объект состояние которого сохраняется и восстанавливается
			/// </summary>
			public ILotusMementoOriginator MementoOriginator
			{
				get { return mOriginator; }
				set { mOriginator = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CMementoCaretaker()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="originator">Объект</param>
			/// <param name="nameState">Наименование состояния объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CMementoCaretaker(ILotusMementoOriginator originator, String nameState)
			{
				mOriginator = originator;
				mState = originator.GetMemento(nameState);
			}
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранить состояние объекта
			/// </summary>
			/// <param name="stateName">Наименование состояния объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SaveState(String stateName)
			{
				mState = mOriginator.GetMemento(stateName);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановить состояние объекта
			/// </summary>
			/// <param name="stateName">Наименование состояния объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void RestoreState(String stateName)
			{
				mOriginator.SetMemento(mState, stateName);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс реализующий смотрителя за объектом с поддержкой отмены/повторения действий
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CMementoCaretakerChanged : ILotusMementoCaretaker, ILotusMementoState
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Общие данные
			internal ILotusMementoOriginator mOriginator;
			internal System.Object mBeforeState;
			internal System.Object mAfterState;
			internal String mNameState;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Объект состояние которого сохраняется и восстанавливается
			/// </summary>
			public ILotusMementoOriginator MementoOriginator
			{
				get { return mOriginator; }
				set { mOriginator = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			/// <remarks>
			/// Конструктор без параметров запрещен
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			private CMementoCaretakerChanged()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="originator">Объект</param>
			/// <param name="nameState">Наименование состояния объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CMementoCaretakerChanged(ILotusMementoOriginator originator, String nameState)
			{
				mOriginator = originator;
				mBeforeState = originator.GetMemento(nameState);
				mNameState = nameState;
			}
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранить состояние объекта
			/// </summary>
			/// <param name="stateName">Наименование состояния объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SaveState(String stateName)
			{
				mBeforeState = mOriginator.GetMemento(stateName);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановить состояние объекта
			/// </summary>
			/// <param name="stateName">Наименование состояния объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void RestoreState(String stateName)
			{
				mOriginator.SetMemento(mBeforeState, stateName);
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusMementoState =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена последнего действия
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Undo()
			{
				if (mOriginator != null)
				{
					// Сначала сохраняем актуальное значение
					mAfterState = mOriginator.GetMemento(mNameState);

					// Теперь ставим предыдущие
					mOriginator.SetMemento(mBeforeState, mNameState);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Повторение последнего действия
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Redo()
			{
				if (mOriginator != null)
				{
					// Сначала сохраняем актуальное значение
					mBeforeState = mOriginator.GetMemento(mNameState);

					// Теперь ставим предыдущие
					mOriginator.SetMemento(mAfterState, mNameState);
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================