using System;
using System.Runtime.CompilerServices;

namespace Lotus.Core
{
    /** \addtogroup CoreECS
	*@{*/
    /// <summary>
    /// Интерфейс фильтра компонентов для хранения списка сущностей с набором определенных компонентов.
    /// </summary>
    public interface ILotusEcsFilterComponent
    {
        /// <summary>
        /// Добавить сущность к фильтру.
        /// </summary>
        /// <param name="entityId">Индентификатор сущности.</param>
        void AddEntity(int entityId);

        /// <summary>
        /// Проверка наличия сущности в фильтре.
        /// </summary>
        /// <param name="entityId">Индентификатор сущности.</param>
        /// <returns>Статус наличия сущности.</returns>
        bool HasEntity(int entityId);

        /// <summary>
        /// Удалить сущность из фильтра.
        /// </summary>
        /// <param name="entityId">Индентификатор сущности.</param>
        void RemoveEntity(int entityId);

        /// <summary>
        /// Включить сущности с указанным типом компонента в фильтр.
        /// </summary>
        /// <param name="componentType">Тип компонента.</param>
        /// <returns>Фильтр.</returns>
        ILotusEcsFilterComponent Include(Type componentType);

        /// <summary>
        /// Проверка на наличие компонента в фильтре.
        /// </summary>
        /// <param name="componentType">Тип компонента.</param>
        /// <returns>Статус наличия.</returns>
        bool Exsist(Type componentType);

        /// <summary>
        /// Исключить сущности с указанным типом компонента из фильтра.
        /// </summary>
        /// <param name="componentType">Тип компонента.</param>
        /// <returns>Фильтр.</returns>
        ILotusEcsFilterComponent Exclude(Type componentType);

        /// <summary>
        /// Обновить сущности фильтра по текущим данным.
        /// </summary>
        void UpdateFilter();

        /// <summary>
        /// Получить список идентификатор сущностей фильтра.
        /// </summary>
        /// <returns>Список идентификатор сущностей.</returns>
        int[] GetEntities();
    }

    /// <summary>
    /// Фильтр компонентов.
    /// </summary>
    /// <remarks>
    /// Под фильтром компонентов понимается архетип содержащий список сущностей с набором определённых компонентов.
    /// </remarks>
    public class CEcsFilterComponent : ILotusEcsFilterComponent
    {
        #region Fields
        protected internal CEcsWorld _world;
        protected internal SparseSet _entities;
        protected internal ListArray<Type> _includedComponents;
        protected internal ListArray<Type> _excludedComponents;
        #endregion

        #region Properties 
        /// <summary>
        /// Мир.
        /// </summary>
        public CEcsWorld World
        {
            get
            {
                return _world;
            }
            set
            {
                _world = value;
            }
        }

        /// <summary>
        /// Текущие количество сущностей в фильтре.
        /// </summary>
        public int CountEntities
        {
            get { return _entities.Count; }
        }

        /// <summary>
        /// Список типов компонентов которые включены в фильтр.
        /// </summary>
        public ListArray<Type> IncludedComponents
        {
            get
            {
                return _includedComponents;
            }
        }

        /// <summary>
        /// Список типов компонентов которые исключены из фильтра.
        /// </summary>
        public ListArray<Type> ExcludedComponents
        {
            get
            {
                return _excludedComponents;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию инициализирует объект класса предустановленными значениями.
        /// </summary>
        public CEcsFilterComponent()
        {
            _entities = new SparseSet(16);
            _includedComponents = new ListArray<Type>(8);
            _excludedComponents = new ListArray<Type>(4);
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Добавить сущность к фильтру.
        /// </summary>
        /// <param name="entityId">Индентификатор сущности.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEntity(int entityId)
        {
            _entities.Add(entityId);
        }

        /// <summary>
        /// Проверка наличия сущности в фильтре.
        /// </summary>
        /// <param name="entityId">Индентификатор сущности.</param>
        /// <returns>Статус наличия сущности.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasEntity(int entityId)
        {
            return _entities.Contains(entityId);
        }

        /// <summary>
        /// Удалить сущность из фильтра.
        /// </summary>
        /// <param name="entityId">Индентификатор сущности.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveEntity(int entityId)
        {
            _entities.Remove(entityId);
        }

        /// <summary>
        /// Включить сущности с указанным типом компонента в фильтр.
        /// </summary>
        /// <typeparam name="TComponent">Тип компонента.</typeparam>
        /// <returns>Фильтр.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CEcsFilterComponent Include<TComponent>() where TComponent : struct
        {
            Include(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Включить сущности с указанным типом компонента в фильтр.
        /// </summary>
        /// <param name="componentType">Тип компонента.</param>
        /// <returns>Фильтр.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILotusEcsFilterComponent Include(Type componentType)
        {
            _includedComponents.AddIfNotContains(componentType);
            UpdateFilter();
            return this;
        }

        /// <summary>
        /// Проверка на наличие компонента в фильтре.
        /// </summary>
        /// <typeparam name="TComponent">Тип компонента.</typeparam>
        /// <returns>Статус наличия.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Exsist<TComponent>() where TComponent : struct
        {
            var component_type = typeof(TComponent);
            return _includedComponents.Contains(component_type) || _excludedComponents.Contains(component_type);
        }

        /// <summary>
        /// Проверка на наличие компонента в фильтре.
        /// </summary>
        /// <param name="componentType">Тип компонента.</param>
        /// <returns>Статус наличия.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Exsist(Type componentType)
        {
            return _includedComponents.Contains(componentType) || _excludedComponents.Contains(componentType);
        }

        /// <summary>
        /// Исключить сущности с указанным типом компонента из фильтра.
        /// </summary>
        /// <typeparam name="TComponent">Тип компонента.</typeparam>
        /// <returns>Фильтр.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CEcsFilterComponent Exclude<TComponent>() where TComponent : struct
        {
            Exclude(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Исключить сущности с указанным типом компонента из фильтра.
        /// </summary>
        /// <param name="componentType">Тип компонента.</param>
        /// <returns>Фильтр.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILotusEcsFilterComponent Exclude(Type componentType)
        {
            _excludedComponents.AddIfNotContains(componentType);
            if (_world._componentsData.TryGetValue(componentType, out var component_data))
            {
                var exclude_entities = component_data.GetEntities();
                _entities.RemoveValues(exclude_entities);
            }

            return this;
        }

        /// <summary>
        /// Обновить сущности фильтра по текущим данным.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateFilter()
        {
            _entities.Clear();
            if (_includedComponents.Count > 1)
            {
                var first_type_filter = _includedComponents[0];
                ILotusEcsComponentData? component_data;
                if (_world._componentsData.TryGetValue(first_type_filter, out component_data))
                {
                    var entities = component_data.GetEntities();
                    for (var i = 0; i < component_data.Count; i++)
                    {
                        var id = entities[i];
                        var find_count = 0;
                        for (var f = 1; f < _includedComponents.Count; f++)
                        {
                            var type_filter = _includedComponents[f];
                            ILotusEcsComponentData? filter_data;
                            if (_world._componentsData.TryGetValue(type_filter, out filter_data))
                            {
                                if (filter_data.HasEntity(id))
                                {
                                    find_count++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (find_count == _includedComponents.Count - 1)
                        {
                            AddEntity(id);
                        }
                    }
                }
            }

            for (var i = 0; i < _excludedComponents.Count; i++)
            {
                ILotusEcsComponentData? component_data;
                if (_world._componentsData.TryGetValue(_excludedComponents[i], out component_data))
                {
                    var include_entities = component_data.GetEntities();
                    _entities.RemoveValues(include_entities);
                }
            }
        }

        /// <summary>
        /// Получить список идентификатор сущностей фильтра.
        /// </summary>
        /// <returns>Список идентификатор сущностей.</returns>
        public int[] GetEntities()
        {
            return _entities._denseItems;
        }
        #endregion
    }
    /**@}*/
}