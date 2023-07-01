﻿//=====================================================================================================================
// Проект: Модуль трехмерного объекта
// Раздел: Подсистема мешей
// Подраздел: Плоскостные трехмерные примитивы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusMesh3DPlanarTriangle.cs
*		Базовый плоскостной трехмерный примитив - треугольник.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Maths;
//=====================================================================================================================
namespace Lotus
{
	namespace Object3D
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup Object3DMeshPlanar
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый плоскостной трехмерный примитив - треугольник
		/// </summary>
		/// <remarks>
		/// <para>
		/// Топология вершин:
		/// 1)-------2)
		/// |      /
		/// |    /
		/// |  /
		/// |/
		/// 0)
		/// </para>
		/// <para>
		/// Треугольник: 0, 1, 2
		/// </para>
		/// <para>
		/// Опорная точка треугольника – первая вершина (индекс у которой вершины - 0)
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CMeshPlanarTriangle3Df : CMeshPlanar3Df
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание треугольника по трем вершинам
			/// </summary>
			/// <remarks>
			/// Задавать вершины нужно по часовой стрелки
			/// </remarks>
			/// <param name="p1">Первая вершина треугольника</param>
			/// <param name="p2">Вторая вершина треугольника</param>
			/// <param name="p3">Третья вершина треугольника</param>
			/// <returns>Треугольник</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CMeshPlanarTriangle3Df CreateOfPoint(Vector3Df p1, Vector3Df p2, Vector3Df p3)
			{
				CMeshPlanarTriangle3Df mesh = new CMeshPlanarTriangle3Df(p1, p2, p3);
				return (mesh);
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CMeshPlanarTriangle3Df()
				:base()
			{
				mName = "Triangle3D";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <remarks>
			/// Задавать вершины нужно по часовой стрелки
			/// </remarks>
			/// <param name="p1">Первая вершина треугольника</param>
			/// <param name="p2">Вторая вершина треугольника</param>
			/// <param name="p3">Третья вершина треугольника</param>
			//---------------------------------------------------------------------------------------------------------
			public CMeshPlanarTriangle3Df(Vector3Df p1, Vector3Df p2, Vector3Df p3)
				: base()
			{
				CreateTriangleOfPoint(p1, p2, p3);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание треугольника по трем вершинам
			/// </summary>
			/// <remarks>
			/// Задавать вершины нужно по часовой стрелки
			/// </remarks>
			/// <param name="p1">Первая вершина треугольника</param>
			/// <param name="p2">Вторая вершина треугольника</param>
			/// <param name="p3">Третья вершина треугольника</param>
			//---------------------------------------------------------------------------------------------------------
			public void CreateTriangleOfPoint(Vector3Df p1, Vector3Df p2, Vector3Df p3)
			{
				mVertices.Clear();
				mVertices.AddVertex(p1);
				mVertices.AddVertex(p2);
				mVertices.AddVertex(p3);

				mTriangles.Clear();
				mTriangles.AddTriangle(0, 1, 2);

				this.ComputeNormals();
				this.ComputeUVMap();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных меша
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdateData()
			{
				ComputeNormals();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление нормалей для треугольника
			/// </summary>
			/// <remarks>
			/// Нормаль вычисления путем векторного произведения по часовой стрелки
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeNormals()
			{
				Int32 iv0 = mVertices.Count - 3;
				Int32 iv1 = mVertices.Count - 2;
				Int32 iv2 = mVertices.Count - 1;

				Vector3Df down = mVertices.Vertices[iv1].Position - mVertices.Vertices[iv0].Position;
				Vector3Df right = mVertices.Vertices[iv2].Position - mVertices.Vertices[iv0].Position;

				Vector3Df normal = Vector3Df.Cross(in down, in right).Normalized;

				mVertices.Vertices[iv0].Normal = normal;
				mVertices.Vertices[iv1].Normal = normal;
				mVertices.Vertices[iv2].Normal = normal;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление текстурных координат (развертки) для треугольника
			/// </summary>
			/// <param name="channel">Канал текстурных координат</param>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeUVMap(Int32 channel = 0)
			{
				mVertices.Vertices[0].UV = XGeometry2D.MapUV_BottomLeft;
				mVertices.Vertices[1].UV = XGeometry2D.MapUV_TopLeft;
				mVertices.Vertices[2].UV = XGeometry2D.MapUV_TopRight;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================