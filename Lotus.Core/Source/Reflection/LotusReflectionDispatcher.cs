﻿//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема рефлексии
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusReflectionDispatcher.cs
*		Центральный диспетчер для работы с рефлексией данных, проверки и анализа типов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup CoreReflection
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер реализующий работу с рефлексией данных, проверку и анализ типов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XReflection
		{
			#region ======================================= ДАННЫЕ ====================================================
			private static Dictionary<String, CReflectedType> _cached;
			private static MethodInfo _stringContainsMethod;
			private static MethodInfo _stringStartsWithMethod;
			private static MethodInfo _stringEndsWithMethod;
			private static Dictionary<Type, MethodInfo> _enumerableCountMethods;
			private static Dictionary<Type, MethodInfo> _enumerableContainsMethods;
			private static Dictionary<Type, MethodInfo> _enumerableAnyMethods;
			private static Dictionary<Type, MethodInfo> _enumerableAllMethods;
			private static Dictionary<Type, MethodInfo> _enumerableUnionMethods;
			private static Dictionary<Type, MethodInfo> _enumerableExceptMethods;
			private static Dictionary<KeyValuePair<Type, Type>, MethodInfo> _enumerableSelectMethods;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Тип <see cref="System.Object"/>
			/// </summary>
			public static readonly Type ObjectType = typeof(System.Object);

			/// <summary>
			/// Тип <see cref="String"/>
			/// </summary>
			public static readonly Type StringType = typeof(String);

			/// <summary>
			/// Словарь кэшированных данных рефлексии по полному имени типа
			/// </summary>
			public static Dictionary<String, CReflectedType> Cached
			{
				get
				{
					if (_cached == null)
					{
						_cached = new Dictionary<String, CReflectedType>(400);
					}
					return _cached;
				}
			}

			/// <summary>
			/// Метаданные метода <see cref="String.Contains(String)"/>
			/// </summary>
			public static MethodInfo StringContainsMethod
			{
				get
				{
					if (_stringContainsMethod == null)
					{
						Init();
					}
					return _stringContainsMethod!;
				}
			}

			/// <summary>
			/// Метаданные метода <see cref="String.StartsWith(String)"/>
			/// </summary>
			public static MethodInfo StringStartsWithMethod
			{
				get
				{
					if (_stringStartsWithMethod == null)
					{
						Init();
					}
					return _stringStartsWithMethod!;
				}
			}

			/// <summary>
			/// Метаданные метода <see cref="String.EndsWith(String)"/>
			/// </summary>
			public static MethodInfo StringEndsWithMethod
			{
				get
				{
					if (_stringEndsWithMethod == null)
					{
						Init();
					}
					return _stringEndsWithMethod!;
				}
			}

			/// <summary>
			/// Словарь метаданных типизированных методов <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/>
			/// </summary>
			public static Dictionary<Type, MethodInfo> EnumerableCountMethods
			{
				get
				{
					if (_enumerableCountMethods == null)
					{
						_enumerableCountMethods = new Dictionary<Type, MethodInfo>(4);
					}
					return _enumerableCountMethods;
				}
			}

			/// <summary>
			/// Словарь метаданных типизированных методов <see cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource)"/>
			/// </summary>
			public static Dictionary<Type, MethodInfo> EnumerableContainsMethods
			{
				get
				{
					if (_enumerableContainsMethods == null)
					{
						_enumerableContainsMethods = new Dictionary<Type, MethodInfo>(4);
					}
					return _enumerableContainsMethods;
				}
			}

			/// <summary>
			/// Словарь метаданных типизированных методов <see cref="Enumerable.Any{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
			/// </summary>
			public static Dictionary<Type, MethodInfo> EnumerableAnyMethods
			{
				get
				{
					if (_enumerableAnyMethods == null)
					{
						_enumerableAnyMethods = new Dictionary<Type, MethodInfo>(4);
					}
					return _enumerableAnyMethods;
				}
			}

			/// <summary>
			/// Словарь метаданных типизированных методов <see cref="Enumerable.All{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
			/// </summary>
			public static Dictionary<Type, MethodInfo> EnumerableAllMethods
			{
				get
				{
					if (_enumerableAllMethods == null)
					{
						_enumerableAllMethods = new Dictionary<Type, MethodInfo>(4);
					}
					return _enumerableAllMethods;
				}
			}

			/// <summary>
			/// Словарь метаданных типизированных методов <see cref="Enumerable.Union{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
			/// </summary>
			public static Dictionary<Type, MethodInfo> EnumerableUnionMethods
			{
				get
				{
					if (_enumerableUnionMethods == null)
					{
						_enumerableUnionMethods = new Dictionary<Type, MethodInfo>(4);
					}
					return _enumerableUnionMethods;
				}
			}

			/// <summary>
			/// Словарь метаданных типизированных методов <see cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
			/// </summary>
			public static Dictionary<Type, MethodInfo> EnumerableExceptMethods
			{
				get
				{
					if (_enumerableExceptMethods == null)
					{
						_enumerableExceptMethods = new Dictionary<Type, MethodInfo>(4);
					}
					return _enumerableExceptMethods;
				}
			}

			/// <summary>
			/// Словарь метаданных типизированных методов <see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/>
			/// </summary>
			public static Dictionary<KeyValuePair<Type, Type>, MethodInfo> EnumerableSelectMethods
			{
				get
				{
					if (_enumerableSelectMethods == null)
					{
						_enumerableSelectMethods = new Dictionary<KeyValuePair<Type, Type>, MethodInfo>(16);
					}
					return _enumerableSelectMethods;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void Init()
			{
				var methodsContains = StringType.GetMethods()
					.Where(method => method.Name == nameof(String.Contains))
					.Where(method => method.GetParameters().Length == 1)
					.ToList();
				foreach (var method in methodsContains)
				{
					var arg = method.GetParameters()[0];
					if (arg.ParameterType == StringType)
					{
						_stringContainsMethod = method;
						break;
					}
				}

				var methodsStartsWith = StringType.GetMethods()
					.Where(method => method.Name == nameof(String.StartsWith))
					.Where(method => method.GetParameters().Length == 1)
					.ToList();
				foreach (var method in methodsStartsWith)
				{
					var arg = method.GetParameters()[0];
					if (arg.ParameterType == StringType)
					{
						_stringStartsWithMethod = method;
						break;
					}
				}

				var methodsEndsWith = StringType.GetMethods()
					.Where(method => method.Name == nameof(String.EndsWith))
					.Where(method => method.GetParameters().Length == 1)
					.ToList();
				foreach (var method in methodsEndsWith)
				{
					var arg = method.GetParameters()[0];
					if (arg.ParameterType == StringType)
					{
						_stringEndsWithMethod = method;
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление типа в кэш
			/// </summary>
			/// <param name="cachedType">Тип данные которого будут сохранены</param>
			/// <returns>Статус успешности добавления</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean AddCachedType(Type cachedType)
			{
				if (Cached.ContainsKey(cachedType.FullName!))
				{
					return false;
				}
				else
				{
					var reflected_type = new CReflectedType(cachedType);
					Cached.Add(cachedType.FullName!, reflected_type);
					return true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление типа в кэш
			/// </summary>
			/// <param name="cachedType">Тип данные которого будут сохранены</param>
			/// <param name="extractMembers">Объем извлекаемых данных для кэширования</param>
			/// <returns>Статус успешности добавления</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean AddCachedType(Type cachedType, TExtractMembers extractMembers)
			{
				if (Cached.ContainsKey(cachedType.FullName!))
				{
					return false;
				}
				else
				{
					var reflected_type = new CReflectedType(cachedType, extractMembers);
					Cached.Add(cachedType.FullName!, reflected_type);
					return true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения статических данных от поля или свойства по строки данных 
			/// </summary>
			/// <remarks>
			/// Строка данных представляет собой строку в формате: полное имя типа.статический член данных
			/// </remarks>
			/// <param name="fullTypeNameMemberName">Строка данных</param>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetStaticDataFromType(String fullTypeNameMemberName)
			{
				var last_dot = fullTypeNameMemberName.LastIndexOf(XChar.Dot);
				if (last_dot > -1)
				{
					var full_type_name = fullTypeNameMemberName.Substring(0, last_dot);
					var member_name = fullTypeNameMemberName.Substring(last_dot + 1);

					// Проверяем наличие типа
					if (Cached.ContainsKey(full_type_name))
					{
						// Проверяем наличие статического поля
						if (Cached[full_type_name].ContainsField(member_name))
						{
							return Cached[full_type_name].GetFieldValue(member_name, null);
						}
						else
						{
							// Проверяем наличие статического свойства
							if (Cached[full_type_name].ContainsProperty(member_name))
							{
								return Cached[full_type_name].GetPropertyValue(member_name, null);
							}
							else
							{
								return null;
							}
						}
					}
					else
					{
						return null;
					}
				}
				else
				{
					return null;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Фильтрация сборок для анализа
			/// </summary>
			/// <remarks>
			/// Для многих методов требуется получить все загруженные в домен сборки и их проанализировать, 
			/// для того чтобы избавиться от точно ненужных сборок их нужно отфильтровать
			/// </remarks>
			/// <param name="assembly">Сборка</param>
			/// <returns>Статус фильтрации</returns>
			//---------------------------------------------------------------------------------------------------------
			private static Boolean FilterAssembly(Assembly assembly)
			{
				if (assembly.FullName!.Contains("Mono")) return false;
				if (assembly.FullName!.Contains("mscorlib")) return false;
				if (assembly.FullName!.Contains("System")) return false;
				if (assembly.FullName!.Contains("Editor")) return false;
				if (assembly.FullName!.Contains("Test")) return false;
				return true;
			}
			#endregion

			#region ======================================= МЕТОДЫ СОЗДАНИЯ ОБЪЕКТА ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создания экземпляра объекта указанного типа
			/// </summary>
			/// <typeparam name="TType">Тип объекта</typeparam>
			/// <returns>Созданный объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType? CreateInstance<TType>()
			{
				return (TType?)CreateInstance(typeof(TType));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создания экземпляра объекта указанного типа
			/// </summary>
			/// <param name="type">Тип объекта</param>
			/// <returns>Созданный объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? CreateInstance(Type type)
			{
				return CreateInstance(type, null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создания экземпляра объекта указанного типа
			/// </summary>
			/// <typeparam name="TType">Тип объекта</typeparam>
			/// <param name="args">Аргументы для конструктора</param>
			/// <returns>Созданный объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType? CreateInstance<TType>(params System.Object[]? args)
            {
				return (TType?)CreateInstance(typeof(TType), args);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создания экземпляра объекта указанного типа
			/// </summary>
			/// <remarks>
			/// Этот метод является оболочкой для вызова методов типа <see cref="System.Activator"/> только с дополнительными
			/// проверками и частными случаями
			/// </remarks>
			/// <param name="type">Тип объекта</param>
			/// <param name="args">Аргументы для конструктора</param>
			/// <returns>Созданный объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? CreateInstance(Type type, params System.Object[]? args)
			{
				// Проверяем тип на принадлежность к системе Unity
#if UNITY_2017_1_OR_NEWER
				if(type == typeof(UnityEngine.GameObject))
				{
					// Создаем игровой объект
					var game_object = new UnityEngine.GameObject("create_from_instance");
					return game_object;
				}
				else
				{
					// Создать экземпляр компонента мы не можем
					if (type.IsSubclassOf(typeof(UnityEngine.Component)))
					{
						UnityEngine.Debug.LogErrorFormat("You cannot create components: <{0}>", type.Name);
						return null;
					}
					else
					{
						// Получается это у нас ресурс, как правило ресурсы не создаются в игре, они должны только загружаться
						// Сделаем исключение только для материала и меша
						if(type == typeof(UnityEngine.Material))
						{
							// Возвращаем новый стандартный материал
							return new UnityEngine.Material(UnityEngine.Shader.Find("Standard"));
						}
						else
						{
							if (type == typeof(UnityEngine.Mesh))
							{
								// Возвращаем новый пустой меш
								// Используется при процедурной генерации
								return new UnityEngine.Mesh();
							}
						}
					}
				}
#endif
				// Системные типы у которых нет конструкторов по умолчанию, но они будут использоваться и поэтому их надо создавать
				if (type == typeof(String))
				{
					return "";
				}
				if (type == typeof(Uri))
				{
					return "http://www.contoso.com/";
				}
				else
				{
					// Мы не рассматриваем случай когда у пользовательских типов нет конструктора по умолчанию
					if (args == null)
					{
						return Activator.CreateInstance(type);
					}
					else
					{
						return Activator.CreateInstance(type, args);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание объектов типов производных от указанного типа
			/// </summary>
			/// <remarks>
			/// Метод проходит по всем сборка находит производные типы и создает объекты конструктором по умолчанию
			/// </remarks>
			/// <param name="baseType">Базовый тип</param>
			/// <returns>Список объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<Object> CreateObjectsFromBaseType(Type baseType)
			{
				var list = new List<Object>();

				// Получаем все загруженные сборки в домене
				var assemblies = AppDomain.CurrentDomain.GetAssemblies();

				// Проходим по всем сборкам
				for (var ia = 0; ia < assemblies.Length; ia++)
				{
					// Сборка
					var assemble = assemblies[ia];

					if (FilterAssembly(assemble))
					{
						// Получаем все типы в сборке
						var types = assemble.GetTypes();

						// Проходим по всем типам
						for (var it = 0; it < types.Length; it++)
						{
							// Получаем тип
							var type = types[it];

							// Если он производный и не абстрактный
							if (type.IsSubclassOf(baseType) && !type.IsAbstract)
							{
								try
								{
									var instance = Activator.CreateInstance(type, true);
									if (instance != null)
									{
										list.Add(instance);
										continue;
									}
								}
								catch (Exception)
								{
								}
							}
						}
					}
				}

				return list;
			}
			#endregion

			#region ======================================= МЕТОДЫ ДЛЯ РАБОТЫ С UNITY =================================
#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Определение типа объекта в Unity
			/// </summary>
			/// <param name="obj">Проверяемый объект</param>
			/// <returns>Тип объекта в системе Unity</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TUnityObjectType GetUnityObjectType(System.Object obj)
			{
				// Получаем тип
				Type type = obj.GetType();
				return GetUnityObjectType(type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Определение типа объекта в Unity на основе информации типа
			/// </summary>
			/// <param name="type">Информация о типе</param>
			/// <returns>Тип объекта в системе Unity</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TUnityObjectType GetUnityObjectType(Type type)
			{
				// Игровой объект
				if (type == typeof(UnityEngine.GameObject))
				{
					return TUnityObjectType.GameObject;
				}

				// Компоненты
				if (type.IsSubclassOf(typeof(UnityEngine.Component)))
				{
					if(type.IsUnityModule())
					{
						return TUnityObjectType.Component;
					}
					else
					{
						return TUnityObjectType.UserComponent;
					}
				}

				// Пользовательские ресурсы
				if (type.IsSubclassOf(typeof(UnityEngine.ScriptableObject)))
				{
					if (type.IsUnityModule())
					{
						return TUnityObjectType.Resource;
					}
					else
					{
						return TUnityObjectType.UserResource;
					}
				}

				// Остаются или ресурсы Unity (простые структурные типы Unity не рассматриваются)
				if (type.IsSubclassOf(typeof(UnityEngine.Object)))
				{
					return TUnityObjectType.Resource;
				}

				return TUnityObjectType.Resource;
			}
#endif
			#endregion

			#region ======================================= РАБОТА С ПОЛЯМИ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование поля с указанным именем
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ContainsField(System.Object instance, String fieldName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].ContainsField(fieldName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.ContainsField(fieldName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение метаданных поля по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <returns>Метаданные поля или null если поля с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static FieldInfo? GetField(System.Object instance, String fieldName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetField(fieldName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetField(fieldName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение типа поля по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <returns>Тип поля или null если поля с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Type? GetFieldType(System.Object instance, String fieldName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetFieldType(fieldName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetFieldType(fieldName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени типа поля по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <returns>Имя типа поля или пустая строка если поля с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetFieldTypeName(System.Object instance, String fieldName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetFieldTypeName(fieldName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetFieldTypeName(fieldName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка метаданных полей имеющих указанный тип
			/// </summary>
			/// <typeparam name="TType">Тип поля</typeparam>
			/// <param name="instance">Экземпляр объекта</param>
			/// <returns>Список метаданных полей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<FieldInfo> GetFieldsFromType<TType>(System.Object instance)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetFieldsFromType<TType>();
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetFieldsFromType<TType>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка метаданных полей имеющих указанный атрибут
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <param name="instance">Экземпляр объекта</param>
			/// <returns>Список метаданных полей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<FieldInfo> GetFieldsHasAttribute<TAttribute>(System.Object instance) where TAttribute : Attribute
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetFieldsHasAttribute<TAttribute>();
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetFieldsHasAttribute<TAttribute>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение атрибута указанного поля
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <returns>Атрибут поля или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TAttribute? GetAttributeFromField<TAttribute>(System.Object instance, String fieldName) where TAttribute : System.Attribute
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetAttributeFromField<TAttribute>(fieldName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetAttributeFromField<TAttribute>(fieldName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного поля
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <returns>Значение поля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetFieldValue(System.Object instance, String fieldName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetFieldValue(fieldName, instance);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetFieldValue(fieldName, instance);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного поля
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <param name="fieldInfoResult">Метаданные поля</param>
			/// <returns>Значение поля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetFieldValue(System.Object instance, String fieldName, out FieldInfo fieldInfoResult)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetFieldValue(fieldName, instance, out fieldInfoResult);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetFieldValue(fieldName, instance, out fieldInfoResult);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного поля которое представляет собой коллекцию
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <param name="index">Индекс элемента</param>
			/// <returns>Значение поля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetFieldValue(System.Object instance, String fieldName, Int32 index)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetFieldValue(fieldName, instance, index);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetFieldValue(fieldName, instance, index);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного поля которое представляет собой коллекцию
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <param name="index">Индекс элемента</param>
			/// <param name="fieldInfoResult">Метаданные поля</param>
			/// <returns>Значение поля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetFieldValue(System.Object instance, String fieldName, Int32 index, out FieldInfo fieldInfoResult)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetFieldValue(fieldName, instance, index, out fieldInfoResult);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetFieldValue(fieldName, instance, index, out fieldInfoResult);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка значения указанного поля
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <param name="value">Значение поля</param>
			/// <returns> Статус успешности установки поля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean SetFieldValue(System.Object instance, String fieldName, System.Object value)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].SetFieldValue(fieldName, instance, value);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.SetFieldValue(fieldName, instance, value);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка значения указанного поля которое представляет собой коллекцию
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="fieldName">Имя поля</param>
			/// <param name="value">Значение поля</param>
			/// <param name="index">Индекс элемента</param>
			/// <returns> Статус успешности установки поля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean SetFieldValue(System.Object instance, String fieldName, System.Object value, Int32 index)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].SetFieldValue(fieldName, instance, value, index);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Fields);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.SetFieldValue(fieldName, instance, value, index);
				}
			}
			#endregion

			#region ======================================= РАБОТА СО СВОЙСТВАМИ ======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование свойства с указанным именем
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ContainsProperty(System.Object instance, String propertyName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].ContainsProperty(propertyName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.ContainsProperty(propertyName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение метаданных свойства по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <returns>Метаданные свойства или null если свойства с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static PropertyInfo? GetProperty(System.Object instance, String propertyName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetProperty(propertyName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetProperty(propertyName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение типа свойства по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <returns>Тип свойства или null если свойства с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Type? GetPropertyType(System.Object instance, String propertyName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetPropertyType(propertyName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetPropertyType(propertyName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени типа свойства по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <returns>Имя типа свойства или пустая строка если свойство с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetPropertyTypeName(System.Object instance, String propertyName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetPropertyTypeName(propertyName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetPropertyTypeName(propertyName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка метаданных свойств имеющих указанный тип
			/// </summary>
			/// <typeparam name="TType">Тип свойства</typeparam>
			/// <param name="instance">Экземпляр объекта</param>
			/// <returns>Список метаданных свойств</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<PropertyInfo> GetPropertiesFromType<TType>(System.Object instance)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetPropertiesFromType<TType>();
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetPropertiesFromType<TType>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка метаданных свойств имеющих указанный атрибут
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <param name="instance">Экземпляр объекта</param>
			/// <returns>Список метаданных свойств</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<PropertyInfo> GetPropertiesHasAttribute<TAttribute>(System.Object instance) where TAttribute : Attribute
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetPropertiesHasAttribute<TAttribute>();
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetPropertiesHasAttribute<TAttribute>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение атрибута указанного свойства
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <returns>Атрибут свойства или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TAttribute? GetAttributeFromProperty<TAttribute>(System.Object instance, String propertyName) where TAttribute : System.Attribute
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetAttributeFromProperty<TAttribute>(propertyName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetAttributeFromProperty<TAttribute>(propertyName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного свойства
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <returns>Значение свойства</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetPropertyValue(System.Object instance, String propertyName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetPropertyValue(propertyName, instance);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetPropertyValue(propertyName, instance);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного свойства которое представляет собой коллекцию
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <param name="index">Индекс элемента</param>
			/// <returns>Значение свойства</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetPropertyValue(System.Object instance, String propertyName, Int32 index)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetPropertyValue(propertyName, instance, index);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetPropertyValue(propertyName, instance, index);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка значения указанного свойства
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <param name="value">Значение свойства</param>
			/// <returns> Статус успешности установки свойства</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean SetPropertyValue(System.Object instance, String propertyName, System.Object value)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].SetPropertyValue(propertyName, instance, value);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.SetPropertyValue(propertyName, instance, value);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка значения указанного свойства которое представляет собой коллекцию
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <param name="value">Значение свойства</param>
			/// <param name="index">Индекс элемента</param>
			/// <returns> Статус успешности установки свойства</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean SetPropertyValue(System.Object instance, String propertyName, System.Object value, Int32 index)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].SetPropertyValue(propertyName, instance, value, index);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Properties);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.SetPropertyValue(propertyName, instance, value, index);
				}
			}
			#endregion

			#region ======================================= РАБОТА С МЕТОДАМИ =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование метода с указанным именем
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ContainsMethod(System.Object instance, String methodName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].ContainsMethod(methodName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.ContainsMethod(methodName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение метаданных метода по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <returns>Метаданные метода или null если метода с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static MethodInfo? GetMethod(System.Object instance, String methodName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetMethod(methodName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetMethod(methodName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение типа возвращаемого значения метода по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <returns>Тип возвращаемого значения метода или null если метода с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Type? GetMethodReturnType(System.Object instance, String methodName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetMethodReturnType(methodName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetMethodReturnType(methodName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени типа возвращаемого значения метода по имени
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <returns>Имя типа возвращаемого значения метода или пустая строка если метода с таким именем не оказалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetMethodReturnTypeName(System.Object instance, String methodName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetMethodReturnTypeName(methodName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetMethodReturnTypeName(methodName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка метаданных методов имеющих указанный атрибут
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <param name="instance">Экземпляр объекта</param>
			/// <returns>Список метаданных методов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<MethodInfo> GetMethodsHasAttribute<TAttribute>(System.Object instance) where TAttribute : Attribute
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetMethodsHasAttribute<TAttribute>();
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetMethodsHasAttribute<TAttribute>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение атрибута указанного метода
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <returns>Атрибут метода или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TAttribute? GetAttributeFromMethod<TAttribute>(System.Object instance, String methodName) where TAttribute : System.Attribute
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].GetAttributeFromMethod<TAttribute>(methodName);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.GetAttributeFromMethod<TAttribute>(methodName);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вызов указанного метода
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <returns>Значение метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? InvokeMethod(System.Object instance, String methodName)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].InvokeMethod(methodName, instance);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.InvokeMethod(methodName, instance);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вызов указанного метода с одним аргументом
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <param name="arg">Аргумент метода</param>
			/// <returns>Значение метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? InvokeMethod(System.Object instance, String methodName, System.Object arg)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].InvokeMethod(methodName, instance, arg);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.InvokeMethod(methodName, instance, arg);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вызов указанного метода с двумя аргументами
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <param name="arg1">Первый аргумент метода</param>
			/// <param name="arg2">Второй аргумент метода</param>
			/// <returns>Значение метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? InvokeMethod(System.Object instance, String methodName, System.Object arg1,
				System.Object arg2)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].InvokeMethod(methodName, instance, arg1, arg2);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.InvokeMethod(methodName, instance, arg1, arg2);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вызов указанного метода с тремя аргументами
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="methodName">Имя метода</param>
			/// <param name="arg1">Первый аргумент метода</param>
			/// <param name="arg2">Второй аргумент метода</param>
			/// <param name="arg3">Третий аргумент метода</param>
			/// <returns>Значение метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? InvokeMethod(System.Object instance, String methodName, System.Object arg1,
				System.Object arg2, System.Object arg3)
			{
				Type type = instance.GetType();
				if (Cached.ContainsKey(type.FullName!))
				{
					return Cached[type.FullName!].InvokeMethod(methodName, instance, arg1, arg2, arg3);
				}
				else
				{
					var reflected_type = new CReflectedType(type, TExtractMembers.Methods);
					Cached.Add(type.FullName!, reflected_type);
					return reflected_type.InvokeMethod(methodName, instance, arg1, arg2, arg3);
				}
			}
			#endregion

			#region ======================================= РАБОТА СО СТАТИЧЕСКИМИ ПОЛЯМИ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование статического поля с указанным именем
			/// </summary>
			/// <param name="fullTypeName">Полное имя типа</param>
			/// <param name="fieldName">Имя поля</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ContainsStaticField(String fullTypeName, String fieldName)
			{
				if (Cached.ContainsKey(fullTypeName))
				{
					return Cached[fullTypeName].ContainsField(fieldName);
				}
				else
				{
					return false;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного статического  поля
			/// </summary>
			/// <param name="fullTypeName">Полное имя типа</param>
			/// <param name="fieldName">Имя поля</param>
			/// <returns>Значение поля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetStaticFieldValue(String fullTypeName, String fieldName)
			{
				if (Cached.ContainsKey(fullTypeName))
				{
					return Cached[fullTypeName].GetFieldValue(fieldName, null);
				}
				else
				{
					return null;
				}
			}
			#endregion

			#region ======================================= РАБОТА СО СТАТИЧЕСКИМИ СВОЙСТВАМИ =========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование статического свойства с указанным именем
			/// </summary>
			/// <param name="fullTypeName">Полное имя типа</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ContainsStaticProperty(String fullTypeName, String propertyName)
			{
				if (Cached.ContainsKey(fullTypeName))
				{
					return Cached[fullTypeName].ContainsProperty(propertyName);
				}
				else
				{
					return false;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного свойства
			/// </summary>
			/// <param name="fullTypeName">Полное имя типа</param>
			/// <param name="propertyName">Имя свойства</param>
			/// <returns>Значение свойства</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object? GetStaticPropertyValue(String fullTypeName, String propertyName)
			{
				if (Cached.ContainsKey(fullTypeName))
				{
					return Cached[fullTypeName].GetPropertyValue(propertyName, null);
				}
				else
				{
					return null;
				}
			}
			#endregion

			#region ======================================= РАБОТА С МЕТОДАМИ Enumerable ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить типизированную версию метода <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/>
			/// </summary>
			/// <param name="type">Тип данных</param>
			/// <returns>Типизированная версия метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static MethodInfo GetEnumerableCountMethod(Type type)
			{
				var result = EnumerableCountMethods.GetValueOrDefault(type, null);
				if (result != null)
				{
					return result;
				}

				var genericMethod = EnumerableCountMethods.GetValueOrDefault(ObjectType, null);

				if (genericMethod != null)
				{
					result = genericMethod.MakeGenericMethod(type);

					EnumerableCountMethods.Add(type, result);

					return result;
				}
				else
				{
					var methodsEnumerableCount = typeof(Enumerable)
					.GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Where(x => x.Name == nameof(Enumerable.Count))
					.Where(x => x.GetParameters().Length == 1);

					genericMethod = methodsEnumerableCount.Single();

					EnumerableCountMethods.Add(ObjectType, genericMethod);

					result = genericMethod.MakeGenericMethod(type);

					EnumerableCountMethods.Add(type, result);

					return result;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить типизированную версию метода <see cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource)"/>
			/// </summary>
			/// <param name="type">Тип данных</param>
			/// <returns>Типизированная версия метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static MethodInfo GetEnumerableContainsMethod(Type type)
			{
				var result = EnumerableContainsMethods.GetValueOrDefault(type, null);
				if (result != null)
				{
					return result;
				}

				var genericMethod = EnumerableContainsMethods.GetValueOrDefault(ObjectType, null);

				if (genericMethod != null)
				{
					result = genericMethod.MakeGenericMethod(type);

					EnumerableContainsMethods.Add(type, result);

					return result;
				}
				else
				{
					var methodsEnumerableContains = typeof(Enumerable)
					.GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Where(x => x.Name == nameof(Enumerable.Contains))
					.Where(x => x.GetParameters().Length == 2);

					genericMethod = methodsEnumerableContains.Single();

					EnumerableContainsMethods.Add(ObjectType, genericMethod);

					result = genericMethod.MakeGenericMethod(type);

					EnumerableContainsMethods.Add(type, result);

					return result;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить типизированную версию метода <see cref="Enumerable.Any{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
			/// </summary>
			/// <param name="type">Тип данных</param>
			/// <returns>Типизированная версия метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static MethodInfo GetEnumerableAnyMethod(Type type)
			{
				var result = EnumerableAnyMethods.GetValueOrDefault(type, null);
				if (result != null)
				{
					return result;
				}

				var genericMethod = EnumerableAnyMethods.GetValueOrDefault(ObjectType, null);

				if (genericMethod != null)
				{
					result = genericMethod.MakeGenericMethod(type);

					EnumerableAnyMethods.Add(type, result);

					return result;
				}
				else
				{
					var methodsEnumerableAny = typeof(Enumerable)
					.GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Where(x => x.Name == nameof(Enumerable.Any))
					.Where(x => x.GetParameters().Length == 2);

					genericMethod = methodsEnumerableAny.Single();

					EnumerableAnyMethods.Add(ObjectType, genericMethod);

					result = genericMethod.MakeGenericMethod(type);

					EnumerableAnyMethods.Add(type, result);

					return result;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить типизированную версию метода <see cref="Enumerable.All{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
			/// </summary>
			/// <param name="type">Тип данных</param>
			/// <returns>Типизированная версия метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static MethodInfo GetEnumerableAllMethod(Type type)
			{
				var result = EnumerableAllMethods.GetValueOrDefault(type, null);
				if (result != null)
				{
					return result;
				}

				var genericMethod = EnumerableAllMethods.GetValueOrDefault(ObjectType, null);

				if (genericMethod != null)
				{
					result = genericMethod.MakeGenericMethod(type);

					EnumerableAllMethods.Add(type, result);

					return result;
				}
				else
				{
					var methodsEnumerableAll = typeof(Enumerable)
					.GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Where(x => x.Name == nameof(Enumerable.All))
					.Where(x => x.GetParameters().Length == 2);

					genericMethod = methodsEnumerableAll.Single();

					EnumerableAllMethods.Add(ObjectType, genericMethod);

					result = genericMethod.MakeGenericMethod(type);

					EnumerableAllMethods.Add(type, result);

					return result;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить типизированную версию метода <see cref="Enumerable.Union{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
			/// </summary>
			/// <param name="type">Тип данных</param>
			/// <returns>Типизированная версия метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static MethodInfo GetEnumerableUnionMethod(Type type)
			{
				var result = EnumerableUnionMethods.GetValueOrDefault(type, null);
				if (result != null)
				{
					return result;
				}

				var genericMethod = EnumerableUnionMethods.GetValueOrDefault(ObjectType, null);

				if (genericMethod != null)
				{
					result = genericMethod.MakeGenericMethod(type);

					EnumerableUnionMethods.Add(type, result);

					return result;
				}
				else
				{
					var methodsEnumerableUnion = typeof(Enumerable)
					.GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Where(x => x.Name == nameof(Enumerable.Union))
					.Where(x => x.GetParameters().Length == 2);

					genericMethod = methodsEnumerableUnion.Single();

					EnumerableUnionMethods.Add(ObjectType, genericMethod);

					result = genericMethod.MakeGenericMethod(type);

					EnumerableUnionMethods.Add(type, result);

					return result;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить типизированную версию метода <see cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
			/// </summary>
			/// <param name="type">Тип данных</param>
			/// <returns>Типизированная версия метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static MethodInfo GetEnumerableExceptMethod(Type type)
			{
				var result = EnumerableExceptMethods.GetValueOrDefault(type, null);
				if (result != null)
				{
					return result;
				}

				var genericMethod = EnumerableExceptMethods.GetValueOrDefault(ObjectType, null);

				if (genericMethod != null)
				{
					result = genericMethod.MakeGenericMethod(type);

					EnumerableExceptMethods.Add(type, result);

					return result;
				}
				else
				{
					var methodsEnumerableExcept = typeof(Enumerable)
					.GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Where(x => x.Name == nameof(Enumerable.Except))
					.Where(x => x.GetParameters().Length == 2);

					genericMethod = methodsEnumerableExcept.Single();

					EnumerableExceptMethods.Add(ObjectType, genericMethod);

					result = genericMethod.MakeGenericMethod(type);

					EnumerableExceptMethods.Add(type, result);

					return result;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить типизированную версию метода <see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/>
			/// </summary>
			/// <param name="typeEntity">Тип сущности</param>
			/// <param name="typeResult">Тип результата</param>
			/// <returns>Типизированная версия метода</returns>
			//---------------------------------------------------------------------------------------------------------
			public static MethodInfo GetEnumerableSelectMethod(Type typeEntity, Type typeResult)
			{
				var check = new KeyValuePair<Type, Type>(typeEntity, typeResult);
				var result = EnumerableSelectMethods.GetValueOrDefault(check, null);
				if (result != null)
				{
					return result;
				}

				var checkObject = new KeyValuePair<Type, Type>(ObjectType, ObjectType);
				var genericMethod = EnumerableSelectMethods.GetValueOrDefault(checkObject, null);

				if (genericMethod != null)
				{
					result = genericMethod.MakeGenericMethod(typeEntity, typeResult);

					EnumerableSelectMethods.Add(check, result);

					return result;
				}
				else
				{
					var methodsEnumerableSelect = typeof(Enumerable)
					.GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Where(x => x.Name == nameof(Enumerable.Select))
					.Where(x => x.GetParameters().Length == 2);

					genericMethod = methodsEnumerableSelect.Single();

					EnumerableSelectMethods.Add(checkObject, genericMethod);

					result = genericMethod.MakeGenericMethod(typeEntity, typeResult);

					EnumerableSelectMethods.Add(check, result);

					return result;
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