using System;
using System.ComponentModel;

namespace Lotus.Core
{
    /**
     * \defgroup CoreServiceOS Подсистема сервисов OS
     * \ingroup Core
     * \brief Подсистема вспомогательных сервисов обеспечивает дополнительную рабочую функциональность, связанную с 
		платформо-зависимыми реализациями и особенностями отдельных системных элементов.
	 * \details Сюда входит работа с диалоговыми окнами открытия/закрытия файла, объекта реализующего выполнения задачи 
		в отельном потоке и работа с реестром Windows.
     * @{
     */
    /// <summary>
    /// Диспетчер для выполнения задачи/метода в отдельном потоке.
    /// </summary>
    /// <remarks>
    /// Реализация диспетчера который обеспечивает удобное выполнения задачи/метода в отдельном потоке на основе 
    /// системного объекта <see cref="BackgroundWorker"/> 
    /// </remarks>
    public static class XBackgroundManager
    {
        #region Fields
        internal static BackgroundWorker _default;
        internal static Action _onCompute;
        internal static Action<int, object?>? _onProgress;
        internal static Action<object?> _onCompleted;
        #endregion

        #region Properties
        /// <summary>
        /// Диспетчер для выполнения задачи в отдельном потоке.
        /// </summary>
        public static BackgroundWorker Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new BackgroundWorker();
                    _default.WorkerReportsProgress = true;
                    _default.DoWork += OnBackgroundWorkerDoWork;
                    _default.ProgressChanged += OnBackgroundWorkerProgressWork;
                    _default.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
                }
                return _default;
            }
        }

        /// <summary>
        /// Основной делегат для выполнения задачи.
        /// </summary>
        public static Action OnCompute
        {
            get { return _onCompute; }
            set { _onCompute = value; }
        }

        /// <summary>
        /// Делегат для информирования ходе выполнения задачи. Аргумент – процент выполнения и объект состояния.
        /// </summary>
        public static Action<int, object?>? OnProgress
        {
            get { return _onProgress; }
            set { _onProgress = value; }
        }

        /// <summary>
        /// Делегат для информирования окончания задачи. Аргумент – результата выполнения задачи.
        /// </summary>
        public static Action<object?> OnCompleted
        {
            get { return _onCompleted; }
            set { _onCompleted = value; }
        }
        #endregion

        #region Main methods
        /// <summary>
        /// Запуск выполнения задачи.
        /// </summary>
        /// <param name="onCompute">Основной делегат для выполнения задачи.</param>
        /// <param name="onCompleted">Делегат для информирования окончания задачи.</param>
        public static void Run(Action onCompute, Action<object?> onCompleted)
        {
            _onCompute = onCompute;
            _onCompleted = onCompleted;
            _onProgress = null;
            Default.RunWorkerAsync();
        }

        /// <summary>
        /// Запуск выполнения задачи.
        /// </summary>
        /// <param name="onCompute">Основной делегат для выполнения задачи.</param>
        /// <param name="onProgress">Делегат для информирования ходе выполнения задачи.</param>
        /// <param name="onCompleted">Делегат для информирования окончания задачи.</param>
        public static void Run(Action onCompute, Action<int, object?> onProgress, Action<object?> onCompleted)
        {
            _onCompute = onCompute;
            _onCompleted = onCompleted;
            _onProgress = onProgress;
            Default.RunWorkerAsync();
        }

        /// <summary>
        /// Информирование о ходе выполнения задачи.
        /// </summary>
        /// <param name="percent">Процент выполнения.</param>
        /// <param name="userState">Объект состояния.</param>
        public static void ReportProgress(int percent, object userState)
        {
            if (Default.WorkerReportsProgress)
            {
                Default.ReportProgress(percent, userState);
            }
        }
        #endregion

        #region Event handler methods 
        /// <summary>
        /// Основной метод для выполнения задачи в фоновом потоке.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="args">Аргументы события.</param>
        private static void OnBackgroundWorkerDoWork(object? sender, DoWorkEventArgs args)
        {
            _onCompute();
        }

        /// <summary>
        /// Информирование о процессе выполнения задачи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="args">Аргументы события.</param>
        private static void OnBackgroundWorkerProgressWork(object? sender, ProgressChangedEventArgs args)
        {
            if (_onProgress != null)
            {
                _onProgress(args.ProgressPercentage, args.UserState);
            }
            else
            {
                if (args.UserState is LogMessage message)
                {
                    XLogger.Log(message);
                }
            }
        }

        /// <summary>
        /// Окончание выполнения задачи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="args">Аргументы события.</param>
        private static void OnBackgroundWorkerRunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                XLogger.LogException(args.Error);
            }
            else
            {
                _onCompleted(args.Result);
            }
        }
        #endregion
    }

    /// <summary>
    /// Статический класс реализующий методы расширения <see cref="BackgroundWorker"/>.
    /// </summary>
    public static class XBackgroundWorkerExtension
    {
        #region Main methods
        /// <summary>
        /// Оповещение о простой информации из отдельного потока.
        /// </summary>
        /// <param name="backgroundWorker">Диспетчер отдельного потока.</param>
        /// <param name="percent">Процент выполнения .</param>
        /// <param name="info">Объект информации.</param>
        public static void ReportProgressLogInfo(this BackgroundWorker backgroundWorker, int percent, object? info)
        {
            if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
            {
                var text = info != null ? info.ToString()! : $"Persent: {percent}";
                backgroundWorker.ReportProgress(percent, new LogMessage(text, TLogType.Info));
            }
        }

        /// <summary>
        /// Оповещение о простой информации из отдельного потока.
        /// </summary>
        /// <param name="backgroundWorker">Диспетчер отдельного потока.</param>
        /// <param name="percent">Процент выполнения .</param>
        /// <param name="moduleName">Имя модуля/подсистемы.</param>
        /// <param name="info">Объект информации.</param>
        public static void ReportProgressLogInfo(this BackgroundWorker backgroundWorker, int percent,
                string moduleName, object? info)
        {
            if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
            {
                var text = info != null ? info.ToString()! : $"Persent: {percent}";
                backgroundWorker.ReportProgress(percent, new LogMessage(moduleName, text, TLogType.Info));
            }
        }

        /// <summary>
        /// Оповещение об ошибке из отдельного потока.
        /// </summary>
        /// <param name="backgroundWorker">Диспетчер отдельного потока.</param>
        /// <param name="percent">Процент выполнения .</param>
        /// <param name="error">Объект ошибки.</param>
        public static void ReportProgressLogError(this BackgroundWorker backgroundWorker, int percent, object? error)
        {
            if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
            {
                var text = error != null ? error.ToString()! : $"Persent: {percent}";
                backgroundWorker.ReportProgress(percent, new LogMessage(text, TLogType.Error));
            }
        }

        /// <summary>
        /// Оповещение об ошибке из отдельного потока.
        /// </summary>
        /// <param name="backgroundWorker">Диспетчер отдельного потока.</param>
        /// <param name="percent">Процент выполнения .</param>
        /// <param name="moduleName">Имя модуля/подсистемы.</param>
        /// <param name="error">Объект ошибки.</param>
        public static void ReportProgressLogError(this BackgroundWorker backgroundWorker, int percent,
                string moduleName, object? error)
        {
            if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
            {
                var text = error != null ? error.ToString()! : $"Persent: {percent}";
                backgroundWorker.ReportProgress(percent, new LogMessage(moduleName, text, TLogType.Error));
            }
        }
        #endregion
    }
    /**@}*/
}