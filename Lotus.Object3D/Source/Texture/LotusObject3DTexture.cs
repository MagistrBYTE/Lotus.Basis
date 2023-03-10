//=====================================================================================================================
// Проект: Модуль трехмерного объекта
// Раздел: Подсистема текстур
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusObject3DTexture.cs
*		Определение типов и параметров изображения представляющего собой текстуру.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
//---------------------------------------------------------------------------------------------------------------------
#if USE_WINDOWS
using System.Windows;
using System.Windows.Media;
using Media3D = System.Windows.Media.Media3D;
#endif
//---------------------------------------------------------------------------------------------------------------------
#if USE_HELIX
using HelixToolkit.Wpf;
using Helix3D = HelixToolkit.Wpf.SharpDX;
#endif
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Object3D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Object3DTexture Подсистема текстур
		//! Подсистема текстур определяет данные текстуры и праметров ее наложения
		//! \ingroup Object3D
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Назначение текстуры
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TTextureDestination
		{
			/// <summary>
			/// Нет особого(конкретного) предназначения текстуры
			/// </summary>
			None,

			/// <summary>
			/// Основная текстура
			/// </summary>
			Diffuse,

			/// <summary>
			/// Тексутра для формирования бликов
			/// </summary>
			Specular,

			/// <summary>
			/// Текстура для представления подсветки
			/// </summary>
			Ambient,

			/// <summary>
			/// Текстура для определения свечения объекта
			/// </summary>
			/// <remarks>
			/// Излучающая текстура, добавляемая к результату расчета освещения. 
			/// На нее не влияет падающий свет, вместо этого она представляет свет, который объект излучает естественным образом
			/// </remarks>
			Emissive,

			/// <summary>
			/// Текстура представляющая собой карту нормалей касательного пространства
			/// </summary>
			Normals,

			/// <summary>
			/// Текстура представляющее собой некое смещение.
			/// </summary>
			/// <remarks>
			/// Точное назначение и формат зависят от приложения. Более высокие значения цвета означают более высокие смещения вершин.
			/// </remarks>
			Displacement,

			/// <summary>
			/// Текстура карты высот
			/// </summary>
			/// <remarks>
			/// По соглашению, более высокие значения оттенков серого означают более высокие отметки от некоторой базовой высоты
			/// </remarks>
			Height,

			/// <summary>
			/// Текстура, определяющая глянцевитость материала
			/// </summary>
			/// <remarks>
			/// Это показатель уравнения зеркального (фонгового) освещения. 
			/// Обычно существует функция преобразования, определенная для сопоставления линейных значений цвета 
			/// в текстуре с подходящей экспонентой.
			/// </remarks>
			Shininess
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Формат цвета изображения предстающего собой текстуру
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TTextureFormatColor
		{
			/// <summary>
			/// Alpha-only texture format, 8 bit integer
			/// </summary>
			Alpha8,

			/// <summary>
			/// Color texture format, 8-bits per channel
			/// </summary>
			RGB24,

			/// <summary>
			/// Color with alpha texture format, 8-bits per channel
			/// </summary>
			RGBA32,

			/// <summary>
			/// Color with alpha texture format, 8-bits per channel
			/// </summary>
			ARGB32,

			/// <summary>
			/// Color with alpha texture format, 8-bits per channel
			/// </summary>
			RGB565,
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс определяющий  параметры изображения представляющего текстуру и отдельные параметры наложения текстуры
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CTexture : CEntity3D
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			protected static readonly PropertyChangedEventArgs PropertyArgsFileName = new PropertyChangedEventArgs(nameof(FileName));
			protected static readonly PropertyChangedEventArgs PropertyArgsDestination = new PropertyChangedEventArgs(nameof(Destination));
			protected static readonly PropertyChangedEventArgs PropertyArgsWidth = new PropertyChangedEventArgs(nameof(Width));
			protected static readonly PropertyChangedEventArgs PropertyArgsHeight = new PropertyChangedEventArgs(nameof(Height));
			protected static readonly PropertyChangedEventArgs PropertyArgsAlphaIsTransparency = new PropertyChangedEventArgs(nameof(AlphaIsTransparency));
			protected static readonly PropertyChangedEventArgs PropertyArgsFormatColor = new PropertyChangedEventArgs(nameof(FormatColor));
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка тексутры в память по полному пути
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Объект <see cref="MemoryStream"/></returns>
			//---------------------------------------------------------------------------------------------------------
			public static MemoryStream LoadTextureToMemory(String file_name)
			{
				using (var file = new FileStream(file_name, FileMode.Open))
				{
					var memory = new MemoryStream();
					file.CopyTo(memory);
					return memory;
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			protected internal String mFileName;
			protected internal TTextureDestination mDestination;
			protected internal Int32 mWidth;
			protected internal Int32 mHeight;
			protected internal Boolean mAlphaIsTransparency;
			protected internal TTextureFormatColor mFormatColor;
			protected internal CMaterial mOwnerMaterial;

#if (UNITY_2017_1_OR_NEWER)
			internal UnityEngine.Texture2D mUnityTexture;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя файла текстуры
			/// </summary>
			[DisplayName("Имя текстуры")]
			[Description("Имя файла текстуры")]
			[Category(XInspectorGroupDesc.Params)]
			public String FileName
			{
				get
				{
					return (mFileName);
				}
				set
				{
					mFileName = value;
					RaiseFileNameChanged();
					NotifyPropertyChanged(PropertyArgsFileName);
				}
			}

			/// <summary>
			/// Назначение текстуры
			/// </summary>
			[DisplayName("Назначение текстуры")]
			[Description("Назначение текстуры")]
			[Category(XInspectorGroupDesc.Params)]
			public TTextureDestination Destination
			{
				get
				{
					return (mDestination);
				}
				set
				{
					mDestination = value;
					RaiseDestinationChanged();
					NotifyPropertyChanged(PropertyArgsDestination);
				}
			}

			/// <summary>
			/// Ширина текстуры
			/// </summary>
			[DisplayName("Ширина текстуры")]
			[Description("Ширина текстуры")]
			[Category(XInspectorGroupDesc.Params)]
			public Int32 Width
			{
				get
				{
					return (mWidth);
				}
				set
				{
					mWidth = value;
					RaiseWidthChanged();
					NotifyPropertyChanged(PropertyArgsWidth);
				}
			}

			/// <summary>
			/// Высота текстуры
			/// </summary>
			[DisplayName("Высота текстуры")]
			[Description("Высота текстуры")]
			[Category(XInspectorGroupDesc.Params)]
			public Int32 Height
			{
				get
				{
					return (mHeight);
				}
				set
				{
					mHeight = value;
					RaiseHeightChanged();
					NotifyPropertyChanged(PropertyArgsHeight);
				}
			}

			/// <summary>
			/// Альфа канал текстуру определяет ее прозрачность
			/// </summary>
			[DisplayName("Высота текстуры")]
			[Description("Высота текстуры")]
			[Category(XInspectorGroupDesc.Params)]
			public Boolean AlphaIsTransparency
			{
				get
				{
					return (mAlphaIsTransparency);
				}
				set
				{
					mAlphaIsTransparency = value;
					RaiseAlphaIsTransparencyChanged();
					NotifyPropertyChanged(PropertyArgsAlphaIsTransparency);
				}
			}

			/// <summary>
			/// Формат цвета изображения предстающего собой текстуру
			/// </summary>
			[DisplayName("Формат цвета")]
			[Description("Формат цвета изображения предстающего собой текстуру")]
			[Category(XInspectorGroupDesc.Params)]
			public TTextureFormatColor FormatColor
			{
				get
				{
					return (mFormatColor);
				}
				set
				{
					mFormatColor = value;
					RaiseFormatColorChanged();
					NotifyPropertyChanged(PropertyArgsFormatColor);
				}
			}

			/// <summary>
			/// Владелец материал
			/// </summary>
			[Browsable(false)]
			public CMaterial OwnerMaterial
			{
				get { return (mOwnerMaterial); }
				set
				{
					mOwnerMaterial = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTexture()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="owner_material">Материал</param>
			//---------------------------------------------------------------------------------------------------------
			public CTexture(CMaterial owner_material)
			{
				mOwnerMaterial = owner_material;
			}

#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="owner_material">Материал</param>
			/// <param name="unity_texture">Двухмерная текстура Unity</param>
			//---------------------------------------------------------------------------------------------------------
			public CTexture(CMaterial owner_material, UnityEngine.Texture2D unity_texture)
				:this(owner_material)
			{
				if(unity_texture != null)
				{
					mName = unity_texture.name;
					mUnityTexture = unity_texture;
				}
			}
#endif
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение имени текстуры.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseFileNameChanged()
			{
#if USE_WINDOWS

#endif
#if (UNITY_2017_1_OR_NEWER)

#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение назначение текстуры.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseDestinationChanged()
			{
#if USE_WINDOWS

#endif
#if (UNITY_2017_1_OR_NEWER)

#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение ширины текстуры.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseWidthChanged()
			{
#if USE_WINDOWS

#endif
#if (UNITY_2017_1_OR_NEWER)

#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение высоты текстуры.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseHeightChanged()
			{
#if USE_WINDOWS

#endif
#if (UNITY_2017_1_OR_NEWER)

#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение статуса использования aльфа каналы текстуры как её прозрачности.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseAlphaIsTransparencyChanged()
			{
#if USE_WINDOWS

#endif
#if (UNITY_2017_1_OR_NEWER)

#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение формата цвета изображения.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseFormatColorChanged()
			{
#if USE_WINDOWS

#endif
#if (UNITY_2017_1_OR_NEWER)

#endif
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Набор всех текстур в сцене
		/// </summary>
		/// <remarks>
		/// Предназначен для логического группирования всех текстур в обозревателе сцены
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CTextureSet : CEntity3D
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			internal ListArray<CTexture> mTextures;
			internal CScene3D mOwnerScene;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Наблюдаемая коллекция материал
			/// </summary>
			[Browsable(false)]
			public ListArray<CTexture> Textures
			{
				get { return (mTextures); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="owner_scene">Сцена</param>
			//---------------------------------------------------------------------------------------------------------
			public CTextureSet(CScene3D owner_scene)
			{
				mOwnerScene = owner_scene;
				mName = "Тексутры";
				mTextures = new ListArray<CTexture>
				{
					IsNotify = true
				};
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusTreeNodeViewBuilder ==========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества дочерних узлов
			/// </summary>
			/// <returns>Количество дочерних узлов</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetCountChildrenNode()
			{
				return (mTextures.Count);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дочернего узла по индексу
			/// </summary>
			/// <param name="index">Индекс дочернего узла</param>
			/// <returns>Дочерней узел</returns>
			//---------------------------------------------------------------------------------------------------------
			public override System.Object GetChildrenNode(Int32 index)
			{
				return (mTextures[index]);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПЛАТФОРМЫ HELIX ====================================
#if USE_HELIX
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================