﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема деревьев выражений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusExpressionExtension.cs
*		Статический класс реализующий методы расширения деревьев выражений.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		/**
         * \defgroup CoreExpression Подсистема деревьев выражений
         * \ingroup Core
         * \brief Подсистема деревьев выражений определяет паттерн «Спецификация» и построитель выражений. 
         * @{
         */
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения деревьев выражений
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExpressionExtension
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Скомпилировать дерево выражений или получить закэшированный делегат
			/// </summary>
			/// <typeparam name="TIn">Тип входного параметра</typeparam>
			/// <typeparam name="TOut">Тип выходного параметра</typeparam>
			/// <param name="expression">Выражение</param>
			/// <returns>Функтор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expression)
			{
				return XCompiledExpressions<TIn, TOut>.AsFunc(expression);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Композиция деревьев выражений
			/// </summary>
			/// <typeparam name="TSource">Тип источника</typeparam>
			/// <typeparam name="TDestination">Тип цели</typeparam>
			/// <typeparam name="TReturn">Результирующий тип</typeparam>
			/// <param name="source">Источник</param>
			/// <param name="mapFrom">Дерево выражений</param>
			/// <returns>Выражение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Expression<Func<TDestination, TReturn>> From<TSource, TDestination, TReturn>(
				this Expression<Func<TSource, TReturn>> source,
				Expression<Func<TDestination, TSource>> mapFrom)
			{
				return Expression.Lambda<Func<TDestination, TReturn>>(Expression.Invoke(source, mapFrom.Body), 
					(IEnumerable<ParameterExpression>)mapFrom.Parameters);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================