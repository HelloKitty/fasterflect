using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fasterflect
{
	/// <summary>
	/// Extensions classes used internally for <see cref="Enum"/> reflection.
	/// </summary>
	internal static class EnumReflectionExtensions
	{
		public static bool isEnumType(this Type type)
		{
#if NETSTANDARD1_6
			return type.GetTypeInfo().IsEnum;
#else
			return type.IsEnum;
#endif
		}
	}
}
