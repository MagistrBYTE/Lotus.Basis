//=====================================================================================================================
// Проект: Модуль трехмерного объекта
// Раздел: Подсистема сцены
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusObject3DScene.cs
*		Сцена для представления всех сущностей 3D контента.
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
using System.Linq;
//---------------------------------------------------------------------------------------------------------------------
#if USE_WINDOWS
using System.Windows;
using System.Windows.Media;
using Media3D = System.Windows.Media.Media3D;
#endif
//---------------------------------------------------------------------------------------------------------------------
#if USE_HELIX
using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Model.Scene;
#endif
//---------------------------------------------------------------------------------------------------------------------
#if (UNITY_2017_1_OR_NEWER)
using UnityEngine;
using UnityEditor;
#endif
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Maths;
//=====================================================================================================================
namespace Lotus
{
	namespace Object3D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Object3DBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Сцена для представления всех сущностей 3D контента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CScene3D : CEntity3D, IEnumerator, IEnumerable
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
#if USE_ASSIMP
			/// <summary>
			/// Представляет контекст импорта/экспорта Assimp, который загружает или сохраняет модели с помощью неуправляемой библиотеки. 
			/// Кроме того, предлагается функция преобразования для обхода загрузки данных модели в управляемую память
			/// </summary>
			protected readonly static Assimp.AssimpContext AssimpContextDefault = new Assimp.AssimpContext();
#endif
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
#if USE_ASSIMP
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка поддерживаемых экспортируемых файлов
			/// </summary>
			/// <returns>Список обозначений экспортируемых файлов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String[] GetSupportedExportFormats()
			{
				var items = AssimpContextDefault.GetSupportedExportFormats();
				String[] formats = new String[items.Length];
				for (Int32 i = 0; i < items.Length; i++)
				{
					formats[i] = items[i].FormatId;
				}

				return (formats);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка поддерживаемых импортируемых файлов
			/// </summary>
			/// <returns>Список расширений импортируемых файлов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String[] GetSupportedImportFormats()
			{
				return (AssimpContextDefault.GetSupportedImportFormats());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка 3D контента по полному пути
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Объект <see cref="CScene3D"/></returns>
			//---------------------------------------------------------------------------------------------------------
			public static CScene3D LoadFromFile(String file_name)
			{
				try
				{
					Assimp.PostProcessSteps step =
						Assimp.PostProcessSteps.FindInstances |
						Assimp.PostProcessSteps.OptimizeGraph |
						Assimp.PostProcessSteps.ValidateDataStructure |
						Assimp.PostProcessSteps.SortByPrimitiveType |
						Assimp.PostProcessSteps.Triangulate |
						Assimp.PostProcessSteps.FlipWindingOrder |
						Assimp.PostProcessSteps.FlipUVs;

					//Assimp.PostProcessPreset

					Assimp.Scene assimp_scene = AssimpContextDefault.ImportFile(file_name, step);
					if(assimp_scene != null)
					{
						CScene3D scene = new CScene3D(Path.GetFileNameWithoutExtension(file_name), assimp_scene);
						scene.FileName = file_name;
						return (scene);
					}
				}
				catch (Exception exc)
				{
					XLogger.LogExceptionModule(nameof(CScene3D), exc);
				}

				return (null);
			}
#endif
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Идентификация
			internal String mFileName;

			// Основные параметры
			internal CNode3D mRootNode;
			internal CMeshSet mMeshSet;
			internal CMaterialSet mMaterialSet;
			internal CTextureSet mTextureSet;
			internal ListArray<CEntity3D> mAllEntities;

			// Размеры и позиция
			internal Vector3Df mMinPosition;
			internal Vector3Df mMaxPosition;
			internal Vector3Df mCenterPosition;

			// Поддержка перечисления
			internal Int32 mEnumeratorIndex;

			// Платформенно-зависимая часть
#if USE_ASSIMP
			internal Assimp.Scene mAssimpScene;
#endif
#if USE_HELIX
			internal SceneNode mHelixScene;
#endif
#if (UNITY_2017_1_OR_NEWER)
			internal GameObject mUnityScene;
#endif
#if UNITY_EDITOR
			internal Autodesk.Fbx.FbxScene mFbxScene;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ИДЕНТИФИКАЦИЯ
			//
			/// <summary>
			/// Имя файла
			/// </summary>
			[Browsable(false)]
			public String FileName
			{
				get { return (mFileName); }
				set
				{
					mFileName = value;
				}
			}

			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Корневой узел сцены
			/// </summary>
			[Browsable(false)]
			public CNode3D RootNode
			{
				get { return (mRootNode); }
			}

			/// <summary>
			/// Набор всех трехмерных сеток(мешей) в сцене
			/// </summary>
			[Browsable(false)]
			public CMeshSet MeshSet
			{
				get { return (mMeshSet); }
			}

			/// <summary>
			/// Набор всех материалов в сцене
			/// </summary>
			[Browsable(false)]
			public CMaterialSet MaterialSet
			{
				get { return (mMaterialSet); }
			}

			/// <summary>
			/// Все элементы сцены
			/// </summary>
			[Browsable(false)]
			public ListArray<CEntity3D> AllEntities
			{
				get
				{
					if (mAllEntities == null)
					{
						mAllEntities = new ListArray<CEntity3D>();
						mAllEntities.IsNotify = false;
						mAllEntities.Add(mMeshSet);
						mAllEntities.Add(mMaterialSet);
						mAllEntities.Add(mTextureSet);
						mAllEntities.Add(mRootNode);
					}

					return (mAllEntities);
				}
			}

			//
			// РАЗМЕРЫ И ПОЗИЦИЯ
			//
			/// <summary>
			/// Геометрический центр сцены
			/// </summary>
			[DisplayName("Центр")]
			[Description("Геометрический центр сцены")]
			[Category(XInspectorGroupDesc.Size)]
			public Vector3Df CenterPosition
			{
				get { return (mCenterPosition); }
			}

			/// <summary>
			/// Размер сцены по X с учетом всех элементов
			/// </summary>
			[DisplayName("SizeX")]
			[Description("Размер сцены по X с учетом всех элементов")]
			[Category(XInspectorGroupDesc.Size)]
			public Single SizeX
			{
				get { return (mMaxPosition.X - mMinPosition.X); }
			}

			/// <summary>
			/// Размер сцены по Y с учетом всех элементов
			/// </summary>
			[DisplayName("SizeY")]
			[Description("Размер сцены по Y с учетом всех элементов")]
			[Category(XInspectorGroupDesc.Size)]
			public Single SizeY
			{
				get { return (mMaxPosition.Y - mMinPosition.Y); }
			}

			/// <summary>
			/// Размер сцены по Z с учетом всех элементов
			/// </summary>
			[DisplayName("SizeZ")]
			[Description("Размер сцены по Z с учетом всех элементов")]
			[Category(XInspectorGroupDesc.Size)]
			public Single SizeZ
			{
				get { return (mMaxPosition.Z - mMinPosition.Z); }
			}

			//
			// ПЛАТФОРМЕННО-ЗАВИСИМАЯ ЧАСТЬ
			//
#if USE_HELIX
			/// <summary>
			/// Сцена Helix
			/// </summary>
			[Browsable(false)]
			public SceneNode HelixScene
			{
				get { return (mHelixScene); }
			}
#endif
#if (UNITY_2017_1_OR_NEWER)
			/// <summary>
			/// Сцена Unity
			/// </summary>
			[Browsable(false)]
			public UnityEngine.GameObject UnityScene
			{
				get { return (mUnityScene);}
				set
				{
					if (mUnityScene != value)
					{
						mUnityScene = value;
						CreateSceneFromUnityGameObject();
					}
				}
			}
#endif
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CScene3D()
			{
				mMeshSet = new CMeshSet(this);
				mMaterialSet = new CMaterialSet(this);
				mTextureSet = new CTextureSet(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя сцены</param>
			//---------------------------------------------------------------------------------------------------------
			public CScene3D(String name)
				:this()
			{
				mName = name;
			}

#if USE_HELIX
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя сцены</param>
			/// <param name="helix_scene">Сцена</param>
			//---------------------------------------------------------------------------------------------------------
			public CScene3D(String name, SceneNode helix_scene)
			{
				mName = name;
				mHelixScene = helix_scene;
				//CreateSceneFromHelixScene();
				//mMeshSet = new CMeshSet();
				//mMaterialSet = new CMaterialSet();
				//mRootNode = new CNode3D(this, mAssimpScene.RootNode);
				//ComputeBoundingBox();
			}
#endif

#if USE_ASSIMP
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя сцены</param>
			/// <param name="assimp_scene">Сцена</param>
			//---------------------------------------------------------------------------------------------------------
			public CScene3D(String name, Assimp.Scene assimp_scene)
			{
				mName = name;
				mAssimpScene = assimp_scene;
				mMeshSet = new CMeshSet(mAssimpScene);
				mMaterialSet = new CMaterialSet(this, mAssimpScene);
				mRootNode = new CNode3D(this, mAssimpScene.RootNode);
				ComputeBoundingBox();
			}
#endif
#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="unity_scene">Сцена</param>
			//---------------------------------------------------------------------------------------------------------
			public CScene3D(GameObject unity_scene)
				:this()
			{
				mUnityScene = unity_scene;
				CreateSceneFromUnityGameObject();
			}
#endif

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="fbx_scene">Сцена</param>
			//---------------------------------------------------------------------------------------------------------
			public CScene3D(Autodesk.Fbx.FbxScene fbx_scene)
				: this()
			{
				mFbxScene = fbx_scene;
				CreateSceneFromFbxScene();
			}
#endif
			#endregion

			#region ======================================= МЕТОДЫ IEnumerator ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Текущий объект
			/// </summary>
			/// <returns>Текущий объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object Current
			{
				get
				{
					return(this);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возможность продвинуться вперед
			/// </summary>
			/// <returns>Статус возможности продвинуться вперед</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean MoveNext()
			{
				mEnumeratorIndex++;
				return (mEnumeratorIndex == 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				mEnumeratorIndex = 0;
			}
			#endregion

			#region ======================================= МЕТОДЫ IEnumerable ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение перечислителя
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			IEnumerator IEnumerable.GetEnumerator()
			{
				return (this);
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusViewItemBuilder ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества дочерних узлов
			/// </summary>
			/// <returns>Количество дочерних узлов</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetCountChildrenNode()
			{
				return (AllEntities.Count);
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
				return (AllEntities[index]);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение меша по индексу
			/// </summary>
			/// <param name="index">Индекс меша</param>
			/// <returns>Меш</returns>
			//---------------------------------------------------------------------------------------------------------
			public CMesh3Df GetMeshFromIndex(Int32 index)
			{
				return (mMeshSet.Meshes[index]);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение материала по индексу
			/// </summary>
			/// <param name="index">Индекс материала</param>
			/// <returns>Материал</returns>
			//---------------------------------------------------------------------------------------------------------
			public CMaterial GetMaterialFromIndex(Int32 index)
			{
				return (mMaterialSet.Materials[index]);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление ограничивающего объема сцены
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeBoundingBox()
			{
				//Assimp.Vector3D min_position = new Assimp.Vector3D(1e10f, 1e10f, 1e10f);
				//Assimp.Vector3D max_position = new Assimp.Vector3D(-1e10f, -1e10f, -1e10f);
				//Assimp.Matrix4x4 identity = Assimp.Matrix4x4.Identity;

				//ComputeBoundingBox(mAssimpScene.RootNode, ref min_position, ref max_position, ref identity);

				//mMaxPosition = max_position.ToVector3D();
				//mMinPosition = min_position.ToVector3D();

				//mCenterPosition.X = (mMinPosition.X + mMaxPosition.X) / 2.0f;
				//mCenterPosition.Y = (mMinPosition.Y + mMaxPosition.Y) / 2.0f;
				//mCenterPosition.Z = (mMinPosition.Z + mMaxPosition.Z) / 2.0f;
			}
			#endregion

			#region ======================================= МЕТОДЫ ПЛАТФОРМЫ UNITY ====================================
#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание сцены из игрового объекта Unity
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void CreateSceneFromUnityGameObject()
			{
				if (mUnityScene == null) return;

				mRootNode = null;
				mMeshSet.Meshes.Clear();
				mMaterialSet.Materials.Clear();

				mName = mUnityScene.name;
				mRootNode = CreateSceneRecursiveFromUnity(mUnityScene.transform, null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивно создаем объекты
			/// </summary>
			/// <param name="transform">Компонент трансформации Unity</param>
			/// <param name="parent_node">Родительский узел</param>
			/// <returns>Созданный узел</returns>
			//---------------------------------------------------------------------------------------------------------
			private CNode3D CreateSceneRecursiveFromUnity(Transform transform, CNode3D parent_node)
			{
				// Если на присутствует компонент меш фильтр то сразу  создаем корневой узел как модель
				MeshFilter mesh_filter = transform.GetComponent<MeshFilter>();
				CNode3D node = null;
				if (mesh_filter != null)
				{
					// Добавляем меш
					if(mesh_filter.sharedMesh != null)
					{
						CMesh3Df mesh = new CMesh3Df(mesh_filter.sharedMesh);
						mMeshSet.Meshes.Add(mesh);
					}

					// Добавляем материал
					MeshRenderer mesh_renderer = transform.GetComponent<MeshRenderer>();
					if(mesh_renderer != null)
					{
						Material[] unity_materials = mesh_renderer.sharedMaterials;
						for (Int32 i = 0; i < unity_materials.Length; i++)
						{
							Material unity_mat = unity_materials[i];
							if(unity_mat != null)
							{
								CMaterial material = new CMaterial(this, unity_mat);
								mMaterialSet.Materials.Add(material);

								// Получаем все текстуры
								String[] textures_name = unity_mat.GetTexturePropertyNames();
								for (Int32 t = 0; t < textures_name.Length; t++)
								{
									String texture_prop_name = textures_name[t];
									Texture unity_texture = unity_mat.GetTexture(texture_prop_name);
									if(unity_texture != null)
									{
										CTexture texture = new CTexture(material, unity_texture as Texture2D);
										mTextureSet.Textures.Add(texture);
									}
								}
							}
						}
					}

					node = new CModel3D(this, parent_node, mesh_filter);
				}
				else
				{
					node = new CNode3D(this, parent_node);
				}

				if (parent_node != null)
				{
					parent_node.Children.Add(node);
				}

				for (Int32 i = 0; i < transform.childCount; i++)
				{
					Transform child_transform = transform.GetChild(i);
					CreateSceneRecursiveFromUnity(child_transform, node);
				}

				return (node);
			}
#endif
			#endregion

			#region ======================================= МЕТОДЫ ПЛАТФОРМЫ UNITY_EDITOR =============================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание сцены из сцены FbxScene
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void CreateSceneFromFbxScene()
			{
				if (mFbxScene == null) return;

				mRootNode = null;
				mMeshSet.Meshes.Clear();
				mMaterialSet.Materials.Clear();

				mName = mFbxScene.GetName();
				mRootNode = CreateSceneRecursiveFromFbx(mFbxScene.GetRootNode(), null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание объектов Fbx
			/// </summary>
			/// <param name="fbx_node">Узел сцены Fbx</param>
			/// <param name="parent_node">Родительский узел</param>
			/// <returns>Созданный узел</returns>
			//---------------------------------------------------------------------------------------------------------
			private CNode3D CreateSceneRecursiveFromFbx(Autodesk.Fbx.FbxNode fbx_node, CNode3D parent_node)
			{
				// Если на узле присутсвет меш то сразу создаем корневой узел как модель
				Autodesk.Fbx.FbxNodeAttribute node_attribute = fbx_node.GetNodeAttribute();
				CNode3D node = null;
				if (node_attribute != null && node_attribute.GetAttributeType() == Autodesk.Fbx.FbxNodeAttribute.EType.eMesh)
				{
					// Добавляем меш
					Autodesk.Fbx.FbxMesh fbx_mesh = fbx_node.GetMesh();
					if (fbx_mesh != null)
					{
						CMesh3Df mesh = new CMesh3Df(fbx_mesh);
						mMeshSet.Meshes.Add(mesh);
					}

					// Добавляем материал
					Int32 count_material = 1;
					for (Int32 i = 0; i < count_material; i++)
					{
						Autodesk.Fbx.FbxSurfaceMaterial fbx_material = fbx_node.GetMaterial(i);
						if (fbx_material != null)
						{
							CMaterial material = new CMaterial(this, fbx_material);
							mMaterialSet.Materials.Add(material);
						}
					}

					node = new CModel3D(this, parent_node, fbx_node);
				}
				else
				{
					node = new CNode3D(this, parent_node, fbx_node);
				}

				if (parent_node != null)
				{
					parent_node.Children.Add(node);
				}

				for (Int32 i = 0; i < fbx_node.GetChildCount(); i++)
				{
					Autodesk.Fbx.FbxNode child_node = fbx_node.GetChild(i);
					CreateSceneRecursiveFromFbx(child_node, node);
				}

				return (node);
			}
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================