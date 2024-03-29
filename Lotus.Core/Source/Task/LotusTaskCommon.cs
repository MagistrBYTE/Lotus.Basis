using System;

namespace Lotus.Core
{
    /**
     * \defgroup CoreTask Подсистема задач
     * \ingroup Core
     * \brief Подсистема задач (не потоки) предназначена для формирования задачи или группы задачи которая может выполняться
		последовательно или параллельно, с определенным режимом выполнения, а также с возможностью приостановки и 
		дальнейшего выполнения.
	 * \details
		## Возможности/особенности
		1. Выполнение задачи по критериям интерфейса \ref Lotus.Core.ILotusTask.
		2. Поддержка задержки начала выполнения, паузы, информирования о ходе выполнения задачи.
		3. Работа с группами задач, возможность запускать последовательно и параллельно исполнения в группе.
		4. Задержка при выполнение задач группы, информирования об окончании выполнения каждой задачи.
		
		## Описание
		Соответствующая задача формируется классом на основе реализации интерфейса.
		Под задачей понимается элементарная единица выполнения с правильно реализованным интерфейсов, выполняемая
		определённым способом с возможностью информирования об окончании задачи, паузой и принудительной остановкой
		выполнения задачи.

		Под группой задач понимается несколько задач выполняемых параллельно или последовательно определённым
		способом(в том числе с задержкой) с возможностью информирования об окончании выполнения всех задач группы,
		паузой и принудительной остановкой выполнения группы задачи.

		## Использование
		1. Правильно реализовать интерфейс \ref Lotus.Core.ILotusTask.
		2. Сформировать задачу или группу задач и запустить на исполнение методами \ref Lotus.Core.TaskExecutor или \ref Lotus.Core.TaskGroupExecutor
		3. Методы исполнителя задач нужно использовать в ручную(непосредственно вызывать его методы в нужных местах)
     * @{
     */
    /// <summary>
    /// Способ выполнения задачи.
    /// </summary>
    /// <remarks>
    /// Способ выполнения задачи определяет временные параметры выполнения задачи.
    /// </remarks>
    public enum TTaskMethod
    {
        /// <summary>
        /// Задача выполняется каждый кадр в методе Update.
        /// </summary>
        EachFrame,

        /// <summary>
        /// Задача выполняется каждый определенный кадр в методе Update. Например каждый десятый.
        /// </summary>
        /// <remarks>
        /// Применяется для задач не критичных к производительности.
        /// </remarks>
        EveryFrame
    }

    /// <summary>
    /// Режим выполнения группы задач.
    /// </summary>
    /// <remarks>
    /// Применяется когда группа задач сформирована из нескольких задач.
    /// </remarks>
    public enum TTaskExecuteMode
    {
        /// <summary>
        /// Параллельное выполнении всех задач группы.
        /// </summary>
        Parallel,

        /// <summary>
        /// Последовательно выполнении всех задач группы.
        /// </summary>
        Sequentially
    }

    /// <summary>
    /// Интерфейс для определения задачи.
    /// </summary>
    /// <remarks>
    /// Под задачей понимается элементарная единица выполнения с правильно реализованным интерфейсов, выполняемая
    /// определённым способом с возможностью информирования об окончании задачи и принудительной остановкой
    /// выполнения задачи
    /// </remarks>
    public interface ILotusTask
    {
        #region Properties
        /// <summary>
        /// Статус завершение задачи.
        /// </summary>
        /// <remarks>
        /// Свойство обязательное для реализации.
        /// Реализация должна предусматривать максимальную эффективность получения данных
        /// </remarks>
        bool IsTaskCompleted { get; }
        #endregion

        #region Methods 
        /// <summary>
        /// Запуск выполнение задачи.
        /// </summary>
        /// <remarks>
        /// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи.
        /// После запуска этого метода задача будет запущена
        /// </remarks>
        void RunTask();

        /// <summary>
        /// Выполнение задачи.
        /// </summary>
        /// <remarks>
        /// Непосредственное выполнение задачи.
        /// Метод будет вызываться каждый раз в зависимости от способа и режима выполнения задачи
        /// </remarks>
        void ExecuteTask();

        /// <summary>
        /// Принудительная остановка выполнения задачи.
        /// </summary>
        /// <remarks>
        /// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия.
        /// </remarks>
        void StopTask();

        /// <summary>
        /// Переустановка данных задачи.
        /// </summary>
        /// <remarks>
        /// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение.
        /// </remarks>
        void ResetTask();
        #endregion
    }

    /// <summary>
    /// Интерфейс для определения задачи с возможность информирования о ходе ее выполнения.
    /// </summary>
    public interface ILotusTaskInfo : ILotusTask
    {
        #region Properties
        /// <summary>
        /// Процент выполнения задачи.
        /// </summary>
        float TaskPercentCompletion { get; }
        #endregion
    }

    /// <summary>
    /// Класс-оболочка для хранения определенной задачи.
    /// </summary>
    /// <remarks>
    /// Для реализации максимальной гибкости имеются ряд дополнительных параметров обеспечивающих нужное выполнение задачи.
    /// </remarks>
    public class CTaskHolder : ILotusPoolObject, ILotusTask
    {
        #region Fields
        // Основные параметры
        protected internal ILotusTask _task;
        protected internal int _id;
        protected internal string _name;
        protected internal bool _isCompleted;
        protected internal bool _isRunning;
        protected internal bool _isPause;
        protected internal bool _isDelayStart;
        protected internal float _delayStart;
        protected internal float _startTaskTime;
        protected internal TTaskMethod _methodMode;
        protected internal int _methodFrame;
        protected internal bool _isPoolObject;

        // События
        protected internal Action _onTaskStarted;
        protected internal Action _onTaskCompleted;
        #endregion

        #region Properties
        //
        // ОСНОВНЫЕ ПАРАМЕТРЫ
        //
        /// <summary>
        /// Задача.
        /// </summary>
        public ILotusTask Task
        {
            get { return _task; }
            set { _task = value; }
        }

        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Имя задачи.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Статус завершения выполнения задачи.
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        /// <summary>
        /// Статус выполнения задачи.
        /// </summary>
        /// <remarks>
        /// Означает что в данный момент задачи имеет статус выполнения.
        /// Непосредственное исполнение зависит еще от статуса паузы
        /// </remarks>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        /// <summary>
        /// Пауза выполнения задач.
        /// </summary>
        public bool IsPause
        {
            get { return _isPause; }
            set { _isPause = value; }
        }

        /// <summary>
        /// Задержка в секундах перед запуском задачи.
        /// </summary>
        /// <remarks>
        /// Иногда нужно запустить задачу не сразу, а например спустя какое либо время, например после визуального эффекта.
        /// </remarks>
        public float DelayStart
        {
            get { return _delayStart; }
            set { _delayStart = value; }
        }

        /// <summary>
        /// Способ выполнения задачи.
        /// </summary>
        public TTaskMethod MethodMode
        {
            get { return _methodMode; }
            set { _methodMode = value; }
        }

        /// <summary>
        /// Каждый какой кадр будет выполняться задача.
        /// </summary>
        /// <remarks>
        /// Применяется если установлен способ выполнения задачи <see cref="TTaskMethod.EveryFrame"/>.
        /// </remarks>
        public int MethodFrame
        {
            get { return _methodFrame; }
            set { _methodFrame = value; }
        }

        /// <summary>
        /// Статус объекта из пула.
        /// </summary>
        /// <remarks>
        /// В целях оптимизации некоторые оболочки задачи будут в пуле.
        /// </remarks>
        public bool IsPoolObject
        {
            get { return _isPoolObject; }
            set { _isPoolObject = value; }
        }

        //
        // СОБЫТИЯ
        //
        /// <summary>
        /// Событие для нотификации о начале выполнения задачи.
        /// </summary>
        public Action OnTaskStarted
        {
            get { return _onTaskStarted; }
            set { _onTaskStarted = value; }
        }

        /// <summary>
        /// Событие для нотификации об окончании выполнения задачи.
        /// </summary>
        public Action OnTaskCompleted
        {
            get { return _onTaskCompleted; }
            set { _onTaskCompleted = value; }
        }
        #endregion

        #region СВОЙСТВА ILotusTask 
        /// <summary>
        /// Статус завершение задачи.
        /// </summary>
        /// <remarks>
        /// Свойство обязательное для реализации.
        /// </remarks>
        public bool IsTaskCompleted
        {
            get
            {
                return _isCompleted;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию инициализирует объект класса предустановленными значениями.
        /// </summary>
        public CTaskHolder()
        {
            _id = -1;
            _methodFrame = 10;
        }

        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="isPoolObject">Статус объекта созданного в пуле.</param>
        public CTaskHolder(bool isPoolObject)
        {
            _id = -1;
            _methodFrame = 10;
            _isPoolObject = isPoolObject;
        }
        #endregion

        #region ILotusPoolObject methods
        /// <summary>
        /// Псевдо-конструктор.
        /// </summary>
        /// <remarks>
        /// Вызывается диспетчером пула в момент взятия объекта из пула.
        /// </remarks>
        public void OnPoolTake()
        {

        }

        /// <summary>
        /// Псевдо-деструктор.
        /// </summary>
        /// <remarks>
        /// Вызывается диспетчером пула в момент попадания объекта в пул.
        /// </remarks>
        public void OnPoolRelease()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _id = -1;
            _task = null;
            _delayStart = 0.0f;
            _onTaskStarted = null;
            _onTaskCompleted = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
        #endregion

        #region ILotusTask methods
        /// <summary>
        /// Запуск выполнение задачи.
        /// </summary>
        /// <remarks>
        /// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи.
        /// </remarks>
        public void RunTask()
        {
            _isRunning = true;
            _isPause = false;
            _isCompleted = false;
            _isDelayStart = _delayStart > 0;
            _startTaskTime = 0;

            if (_isDelayStart == false)
            {
                _task.RunTask();
            }
        }

        /// <summary>
        /// Выполнение задачи.
        /// </summary>
        /// <remarks>
        /// Непосредственное выполнение задачи.
        /// Метод будет вызываться каждый раз в зависимости от способа и режима выполнения задачи
        /// </remarks>
        public void ExecuteTask()
        {
            if (_isDelayStart)
            {
#if UNITY_2017_1_OR_NEWER
				_startTaskTime += UnityEngine.Time.deltaTime;
#endif
                if (_startTaskTime > _delayStart)
                {
                    _task.RunTask();
                    _isDelayStart = false;
                }
            }
            else
            {
                if (_methodMode == TTaskMethod.EachFrame)
                {
                    _task.ExecuteTask();
                    _isCompleted = _task.IsTaskCompleted;
                }
                else
                {
#if UNITY_2017_1_OR_NEWER

					if (UnityEngine.Time.frameCount % _methodFrame == 0)
					{
						_task.ExecuteTask();
						_isCompleted = _task.IsTaskCompleted;
					}
#else
#endif
                }
            }
        }

        /// <summary>
        /// Принудительная остановка выполнения задачи.
        /// </summary>
        /// <remarks>
        /// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия.
        /// </remarks>
        public void StopTask()
        {
            _isRunning = false;
            _isPause = false;
            _isCompleted = false;
            _startTaskTime = 0;
            _task.StopTask();
        }

        /// <summary>
        /// Переустановка данных задачи.
        /// </summary>
        /// <remarks>
        /// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение.
        /// </remarks>
        public void ResetTask()
        {
            _isRunning = false;
            _isPause = false;
            _startTaskTime = 0;
            _isCompleted = false;
            _task.ResetTask();
        }
        #endregion
    }
    /**@}*/
}