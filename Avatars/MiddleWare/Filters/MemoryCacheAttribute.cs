using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Filters
{
	public class MemoryCacheAttribute : ActionFilterAttribute
	{
		static string RequestId(ActionContext context) => $"{context.HttpContext.Request.Method}-{context.HttpContext.Request.Path}";

		readonly int StorageTime = 10;
		readonly TimeSpan StorageTimeSpan = TimeSpan.FromSeconds(10);

		public MemoryCacheAttribute(int storageTime)
		{
			this.StorageTime = storageTime;
			this.StorageTimeSpan = TimeSpan.FromSeconds(this.StorageTime);
		}

		class MemCache
		{


			class Entry
			{
				public Entry(object value, DateTime expiration)
				{
					Value = value;
					Expiration = expiration;
				}
				public object Value;
				public DateTime Expiration;
			}

			ConcurrentDictionary<object, Entry> Cache = new();


			public void Set(object key, object value, int lifetime)
			{
				Cache[key] = new Entry(value, DateTime.Now.AddSeconds(lifetime));
			}

			public bool Get(object key, out object value)
			{
				value = default;

				CleanUp();

				if (Cache.TryGetValue(key, out Entry entry))
				{
					if(entry.Expiration < DateTime.Now)
					{
						Cache.TryRemove(key, out Entry removed);
					}
					else
					{
						value= entry.Value;
						return true;
					}
				}

				return false;
			}


			DateTime NextCleanUp = DateTime.Now;
			void CleanUp()
			{
				if(DateTime.Now > NextCleanUp)
				{
					NextCleanUp = DateTime.Now.AddSeconds(5);

					Cache.Where(pair => pair.Value.Expiration < DateTime.Now)
						.ToList().ForEach(pair => Cache.TryRemove(pair.Key, out Entry deleted));
				}
				if(Cache.Count > 1000)
				{
					Cache.OrderBy(pair => pair.Value.Expiration)
					.ToList().ForEach(pair =>
					{
						if (Cache.Count > 1000)
						{
							Cache.TryRemove(pair.Key, out Entry deleted);
						}
					});
				}
			}
		}

		static MemCache Cache = new MemCache();


		public override void OnActionExecuting(ActionExecutingContext context)
		{
			try
			{
				if (Cache.Get(RequestId(context), out var value) && value is IActionResult result)
				{
					context.Result = result;
				}
			}
			catch (Exception ex)
			{
				context.Result = new ObjectResult($"Exception in MemoryCacheAttribute.OnActionExecuting: {ex.Message}") { StatusCode = 500 };
			}

			base.OnActionExecuting(context);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
		}





		public override void OnResultExecuting(ResultExecutingContext context)
		{
			base.OnResultExecuting(context);
		}
		public override void OnResultExecuted(ResultExecutedContext context)
		{
			try
			{
				if (!context.Canceled && !context.ExceptionHandled)
				{
					/*
					var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();

					string id = RequestId(context);

					if (!cache.TryGetValue(id, out var value))
					{
						cache.Set(id, context.Result, new MemoryCacheEntryOptions
						{
							Size = 1,
							Priority = CacheItemPriority.Low,
							AbsoluteExpirationRelativeToNow = StorageTimeSpan,

						});
					}
					*/

					string id = RequestId(context);
					if (!Cache.Get(id, out var value))
					{
						Cache.Set(id, context.Result, StorageTime);
					}

				}
			}
			catch{}

			base.OnResultExecuted(context);
		}


	}
}
