﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusCoreExtensionTesting.cs
*		Тестирование методов расширений модуля базового ядра.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif
using NUnit.Framework;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для тестирования методов расширений модуля базового ядра
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XCoreExtensionTesting
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Тестирование методов <see cref="XStringExtension"/>
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[Test]
			public static void TestExtensionString()
			{
				var test_beetwen = "Use the Assert[2222] class[00000] to";
				Assert.AreEqual(test_beetwen.RemoveAllBetweenSymbol('[', ']'),
					"Use the Assert[] class[] to");

				test_beetwen = "Use the Assert[2222] class";
				Assert.AreEqual(test_beetwen.RemoveAllBetweenSymbol('[', ']'),
					"Use the Assert[] class");

				var test_beetwen_all = "Use the Assert[2222] class";
				Assert.AreEqual(test_beetwen_all.RemoveAllBetweenSymbolWithSymbols('[', ']'),
					"Use the Assert class");

				test_beetwen_all = "Use the Assert[2222] class[00000] to";
				Assert.AreEqual(test_beetwen_all.RemoveAllBetweenSymbolWithSymbols('[', ']'),
					"Use the Assert class to");



				var eqal11 = "привет";
				var eqal12 = "Привет";

				var eqal21 = "привет";
				var eqal22 = "Привет";

				Assert.AreEqual(eqal21.Equal(eqal11), true);
				Assert.AreEqual(eqal21.Equal(eqal12), false);

				Assert.AreEqual(eqal22.EqualIgnoreCase(eqal11), true);
				Assert.AreEqual(eqal22.EqualIgnoreCase(eqal12), true);


				var test = "Use the Assert class to test conditions class.";

				test = test.RemoveFirstOccurrence("566");

				test = test.RemoveFirstOccurrence("class");
				Assert.AreEqual(test, "Use the Assert  to test conditions class.");


				test = "Use the Assert class to test conditions class.";
				test = test.RemoveLastOccurrence("class");
				Assert.AreEqual(test, "Use the Assert class to test conditions .");

				test = "Use the Assert class to test conditions class.xtx";
				test = test.RemoveExtension();
				Assert.AreEqual(test, "Use the Assert class to test conditions class");

				test = "Проверяемая строка хороша";
				test = test.RemoveFromSearchOption("хороша", TStringSearchOption.End);
				Assert.AreEqual(test, "Проверяемая строка ");

				test = "dfsfsd[778]sdfsd[090]";
				var nf = test.ExtractNumber();
				Assert.AreEqual(nf, 778);

				test = "dfsfsd[778]sdfsd[090]";
				var nl = test.ExtractNumberLast();
				Assert.AreEqual(nl, 90);

				test = "/// <param name=\"begin\">String begin</param>";
				var token = test.ExtractString(">", "<");
				Assert.AreEqual(token, "String begin");

				test = "222.3333";
				var before = test.SubstringTo(".", false);
				Assert.AreEqual(before, "222");

				var after = test.SubstringFrom(".", false);
				Assert.AreEqual(after, "3333");

				before = test.SubstringTo(".", true);
				Assert.AreEqual(before, "222.");

				after = test.SubstringFrom(".", true);
				Assert.AreEqual(after, ".3333");


				//
				// МЕТОДЫ РАБОТЫ СО СЛОВАМИ
				//
				test = "yield null to skip";
				test = test.ToWordUpper();
				Assert.AreEqual(test, "Yield null to skip");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный класс для тестирования
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public interface IA
			{
				void TestA();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный класс для тестирования
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public interface IB
			{
				void TestB();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный класс для тестирования
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public class A : IA
			{
				public void TestA()
				{
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный класс для тестирования
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public class B : A, IB
			{
				public void TestB()
				{
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный класс для тестирования
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public class C : B
			{
				public void TestC()
				{
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Тестирование методов <see cref="XReflectionTypeExtension"/>
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[Test]
			public static void TestExtensionReflection()
			{
				// Проверка типа на поддержку интерфейса
				Assert.AreEqual(typeof(A).IsSupportInterface<IA>(), true);
				Assert.AreEqual(typeof(A).IsSupportInterface<IB>(), false);

				Assert.AreEqual(typeof(B).IsSupportInterface<IA>(), true);
				Assert.AreEqual(typeof(B).IsSupportInterface<IB>(), true);

				// Проверка типа на базовый класс
				Assert.AreEqual(typeof(B).IsSubclassOf(typeof(System.Object)), true);
				Assert.AreEqual(typeof(B).IsSubclassOf(typeof(A)), true);
				Assert.AreEqual(typeof(B).IsSubclassOf(typeof(B)), false);

				// Проверка на равенство
				Assert.AreEqual(typeof(B).IsAssignableFrom(typeof(A)), false);
				Assert.AreEqual(typeof(B).IsAssignableFrom(typeof(B)), true);
				Assert.AreEqual(typeof(B).IsAssignableFrom(typeof(C)), true);
			}
		}
	}
}
//=====================================================================================================================