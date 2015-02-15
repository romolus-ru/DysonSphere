using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils.ExtensionMethods
{
	// http://www.techdays.ru/videos/4448.html

	/// <summary>
	/// Для последовательных операций с вложенными свойствами
	/// </summary>
	public static class MayBe
	{
		public static TResult With<TInput,TResult>(this TInput o, Func<TInput,TResult> evaluator) where TInput:class where TResult:class{
			if (o==null)return null;
			return evaluator(o);
		}

		/// <summary>
		/// Монада. если null то возвращает второе значение
		/// </summary>
		/// <typeparam name="TInput"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="o"></param>
		/// <param name="evaluator"></param>
		/// <param name="failValue"></param>
		/// <returns></returns>
		public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failValue)where TInput:class
		{
			if (o==null)return failValue;
			return evaluator(o);
		}

		public static bool ReturnSuccess<TInput>(this TInput o) where TInput : class
		{
			return o != null;
		}

		public static TInput If<TInput>(this TInput o, Predicate<TInput> evaluator) where TInput : class
		{
			if (o == null) return null;
			return evaluator(o)?o:null;
		}

		public static TInput Do<TInput>(this TInput o, Action<TInput> evaluator) where TInput:class
		{
			if (o == null) return null;
			evaluator(o);
			return o;
		}

	}

	public class Person
	{
		public Address Address { get; set; }

	}

	public class Address
	{
		public String HouseName { get; set; }
	}

	public class MyClass
	{
		public void m1(Person person)
		{
			var s = person
				.With(p => p.Address)
				.With(p => p.HouseName)
				.If(p => p.Length > 4)
				.Do(Console.WriteLine);
		}
	}

}
