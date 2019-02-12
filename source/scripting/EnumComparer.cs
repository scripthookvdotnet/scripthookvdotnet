using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GTA
{
	internal class EnumComparer<T> : IEqualityComparer<T> where T : Enum
	{
		#region Fields
		static readonly Func<T, T, bool> _equalsFunc;
		static readonly Func<T, int> _getHashCodeFunc;
		#endregion

		static EnumComparer()
		{
			_equalsFunc = GenerateEquals();
			_getHashCodeFunc = GenerateGetHashCode();
		}

		public bool Equals(T x, T y)
		{
			// x.Equals(y) calls the overload for another System.Object instance, so call the generated equals method
			return _equalsFunc(x, y);
		}

		public int GetHashCode(T obj)
		{
			 return _getHashCodeFunc(obj);
		}

		static Func<T, T, bool> GenerateEquals()
		{
			var xParam = Expression.Parameter(typeof(T), "x");
			var yParam = Expression.Parameter(typeof(T), "y");
			var equalExpression = Expression.Equal(xParam, yParam);
			return Expression.Lambda<Func<T, T, bool>>(equalExpression, new[] { xParam, yParam }).Compile();
		}

		static Func<T, int> GenerateGetHashCode()
		{
			var objParam = Expression.Parameter(typeof(T), "obj");

			var underlyingType = Enum.GetUnderlyingType(typeof(T));
			var convertExpression = Expression.Convert(objParam, underlyingType);

			var getHashCodeMethod = underlyingType.GetMethod("GetHashCode");
			var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod);

			return Expression.Lambda<Func<T, int>>(getHashCodeExpression, new[] { objParam }).Compile();
		}
	}
}
