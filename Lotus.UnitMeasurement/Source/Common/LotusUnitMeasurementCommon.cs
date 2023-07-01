﻿//=====================================================================================================================
// Проект: Модуль единиц измерения
// Раздел: Общие данные
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUnitMeasurementCommon.cs
*		Общие типы и структуры данных единиц измерения.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace Lotus
{
	namespace UnitMeasurement
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup UnitMeasurement
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения величины поддерживающую определённую единицу измерения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusUnitValue
		{
			/// <summary>
			/// Значение величины
			/// </summary>
			Double Value { get; set; }

			/// <summary>
			/// Единица измерения
			/// </summary>
			Enum UnitType { get; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения величины поддерживающую определённую единицу измерения
		/// </summary>
		/// <typeparam name="TUnit">Тип единицы измерения</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusUnitValue<TUnit> : ILotusUnitValue where TUnit : Enum
		{
			/// <summary>
			/// Единица измерения
			/// </summary>
			new TUnit UnitType { get; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс определяющий величину и соответствующую определённую единицу измерения
		/// </summary>
		/// <typeparam name="TUnit">Тип единицы измерения</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public struct TUnitValue<TUnit> : ILotusUnitValue<TUnit>, IEquatable<TUnitValue<TUnit>>, 
			IComparable<TUnitValue<TUnit>>, ICloneable where TUnit : Enum
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация объекта из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TUnitValue<TUnit> DeserializeFromString(String data)
			{
				if(data.Contains(','))
				{
					data = data.Replace(',', '.');
				}

				Double result = 0;
				if(Double.TryParse(data, out result))
				{
				}

				TUnitValue<TUnit> value = new TUnitValue<TUnit>(result);
				return value;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal Double mValue;
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal TUnit mUnitType;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Значение
			/// </summary>
			public Double Value
			{
				get { return (mValue); }
				set
				{
					mValue = value;
				}
			}

			/// <summary>
			/// Единица измерения
			/// </summary>
			public TUnit UnitType
			{
				get { return mUnitType; }
			}

			/// <summary>
			/// Единица измерения
			/// </summary>
			Enum ILotusUnitValue.UnitType
			{
				get
				{
					return (mUnitType);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="value">Значение</param>
			//---------------------------------------------------------------------------------------------------------
			public TUnitValue(Double value)
			{
				mValue = value;
				mUnitType = (TUnit)(System.Object)1;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="unit_type">Единица измерения</param>
			//---------------------------------------------------------------------------------------------------------
			public TUnitValue(Double value, TUnit unit_type)
			{
				mValue = value;
				mUnitType = unit_type;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверяет равен ли текущий объект другому объекту того же типа
			/// </summary>
			/// <param name="obj">Сравниваемый объект</param>
			/// <returns>Статус равенства объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean Equals(System.Object obj)
			{
				if (obj != null)
				{
					if (typeof(TUnitValue<TUnit>) == obj.GetType())
					{
						TUnitValue<TUnit> value = (TUnitValue<TUnit>)obj;
						return Equals(value);
					}
				}
				return base.Equals(obj);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка равенства объектов по значению
			/// </summary>
			/// <param name="other">Сравниваемый объект</param>
			/// <returns>Статус равенства объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Equals(TUnitValue<TUnit> other)
			{
				return mValue == other.mValue;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов для упорядочивания
			/// </summary>
			/// <param name="other">Сравниваемый объект</param>
			/// <returns>Статус сравнения объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(TUnitValue<TUnit> other)
			{
				return mValue.CompareTo(other.mValue);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода объекта
			/// </summary>
			/// <returns>Хеш-код объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return mValue.GetHashCode() ^ base.GetHashCode();
			}

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
				return mValue.ToString();
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ =================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов на равенство
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус равенства</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator ==(TUnitValue<TUnit> left, TUnitValue<TUnit> right)
			{
				return left.Equals(right);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов на неравенство
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус неравенство</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator !=(TUnitValue<TUnit> left, TUnitValue<TUnit> right)
			{
				return !(left == right);
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ ПРЕОБРАЗОВАНИЯ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в объект типа <see cref="System.Double"/>
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Объект <see cref="System.Double"/></returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator Double(TUnitValue<TUnit> value)
			{
				return value.mValue;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в объект типа <see cref="TUnitValue{TUnit}"/>
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Объект <see cref="TUnitValue{TUnit}"/></returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator TUnitValue<TUnit>(Double value)
			{
				return new TUnitValue<TUnit>(value);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация объекта в строку
			/// </summary>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public String SerializeToString()
			{
				return (mValue.ToString());
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================