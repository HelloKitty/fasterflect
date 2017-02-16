using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fasterflect
{
	/// <summary>
	/// Extensions class that help make net46 and netstandard code
	/// compatible with net35 without conditional compilation everywhere.
	/// </summary>
	public static class Net35CompatibilityExtensions
	{
#if NET35
		/// <summary>
		/// A net35 extension that fills out GetTypeInfo for net35.
		/// </summary>
		/// <param name="t"></param>
		/// <returns>Just the provided type.</returns>
		public static Type GetTypeInfo(this Type t)
		{
			return t;
		}

		/// <summary>
		/// A net35 extension that fills out GetMethodInfo for net35 <see cref="Func{TResult, T1}"/>
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public static MethodInfo GetMethodInfo<TResult, T2>(this Func<TResult, T2> func)
		{
			return func.Method;
		}
#endif
	}
}
