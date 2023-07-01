﻿//=====================================================================================================================
// Проект: Модуль репозитория
// Раздел: Подсистема постраничного разделения
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusRepositoryPageInfoQueryable.cs
*		Статический класс реализующий методы расширения для работы с IQueryable для постраничного запроса.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//=====================================================================================================================
namespace Lotus
{
	namespace Repository
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup RepositoryPageInfo
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для работы с IQueryable
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XQueryableExtensionsPageInfo
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Максимальный размер страницы
			/// </summary>
			private const int MaxPageSize = 9999;
			#endregion

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение постраничного запроса данных
			/// </summary>
			/// <typeparam name="TEntity">Тип сущности</typeparam>
			/// <param name="query">Запрос</param>
			/// <param name="pageNumber">Номер страницы</param>
			/// <param name="pageSize">Размер страницы</param>
			/// <returns>Запрос</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> query, Int32 pageNumber, Int32 pageSize)
			{
				query = query ?? throw new ArgumentNullException(nameof(query));

				if (pageNumber < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(pageNumber), "Value should be equal or more 0");
				}

				if (pageSize < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(pageSize), "Value should be equal or more 0");
				}

				if (pageSize == 0 || pageSize > MaxPageSize)
				{
					pageSize = MaxPageSize;
				}

				return query.Skip(pageNumber * pageSize).Take(pageSize);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение постраничных данных
			/// </summary>
			/// <typeparam name="TEntity">Тип сущности</typeparam>
			/// <typeparam name="TResponse">Тип данных ответа</typeparam>
			/// <param name="query">Запрос</param>
			/// <param name="request">Запрос</param>
			/// <param name="token">Токен отмены</param>
			/// <returns>Ответ</returns>
			//---------------------------------------------------------------------------------------------------------
			public static async Task<ResponsePage<TResponse>> ToResponsePageAsync<TEntity, TResponse>(
				this IQueryable<TEntity> query, Request request, CancellationToken token = default)
			{
				return await query.ToResponsePageAsync<TEntity, TResponse>(request.PageInfo.PageNumber, 
					request.PageInfo.PageSize, token);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение постраничничных данных
			/// </summary>
			/// <typeparam name="TEntity">Тип сущности</typeparam>
			/// <typeparam name="TResponse">Тип данных ответа</typeparam>
			/// <param name="query">Запрос</param>
			/// <param name="page">Номер страницы</param>
			/// <param name="pageSize">Размер страницы</param>
			/// <param name="token">Токен отмены</param>
			/// <returns>Ответ</returns>
			//---------------------------------------------------------------------------------------------------------
			public static async Task<ResponsePage<TResponse>> ToResponsePageAsync<TEntity, TResponse>(
				this IQueryable<TEntity> query, Int32 page, Int32 pageSize, CancellationToken token = default)
			{
				var totalCount = await query.CountAsync(token);
				var data = await query.Paging(page, pageSize)
					.ProjectToType<TResponse>()
					.ToArrayAsync(token);

				CPageInfoResponse pageInfo = new CPageInfoResponse()
				{
					PageNumber = page,
					PageSize = pageSize,
					CurrentPageSize = data.Length,
					TotalCount = totalCount,
				};

				var paging = new ResponsePage<TResponse>
				{
					PageInfo = pageInfo,
					Payload = data
				};

				return paging;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================