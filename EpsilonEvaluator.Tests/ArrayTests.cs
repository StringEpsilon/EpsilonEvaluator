using System.Linq.Expressions;
using System.Reflection;
namespace EpsilonEvaluator.Tests;

public class Array_Tests {
	[Fact]
	public void ArrayAccess() {
		int[] array = [1, 2, 3, 4];
		var result = ExpressionEvaluator.Evaluate(
			Expression.ArrayAccess(
				Expression.Constant(array),
				Expression.Constant(3)
			)
		);
		Assert.Equal(4, result);
		Assert.Equal(array[3], result);
	}
	[Fact]
	public void ArrayLength() {
		int[] array = [1, 2, 3, 4];
		var result = ExpressionEvaluator.Evaluate(
			Expression.ArrayLength(
				Expression.Constant(array)
			)
		);
		Assert.Equal(4, result);
	}

	[Fact]
	public void ListAccess() {
		List<int> list = [1, 2, 3, 4];
		var result = ExpressionEvaluator.Evaluate(
			Expression.MakeIndex(
				Expression.Constant(list),
				typeof(List<int>).GetProperty("Item"),
				[Expression.Constant(3)]
			)
		);
		Assert.Equal(4, result);
	}

	[Fact]
	public void DictionaryAccess() {
		Dictionary<int, string> dictionary = new Dictionary<int, string>{ { 1, "Eins" }, { 2, "Zwei" }, { 3, "Drei" } };
		var result = ExpressionEvaluator.Evaluate(
			Expression.MakeIndex(
				Expression.Constant(dictionary),
				typeof(Dictionary<int, string>).GetProperty("Item"),
				[Expression.Constant(3)]
			)
		);
		Assert.Equal("Drei", result);
	}

	[Fact]
	public void ArrayIndex() {
		int[] array = [0, 1, 2, 3, 4];
		var result = ExpressionEvaluator.Evaluate(
			Expression.ArrayIndex(
				Expression.Constant(array),
				Expression.Constant(2)
			)
		);
		Assert.Equal(2, result);

		array = [0, 10, 20, 30, 40];
		result = ExpressionEvaluator.Evaluate(
			Expression.ArrayIndex(
				Expression.Constant(array),
				Expression.Constant(2)
			)
		);
		Assert.Equal(20, result);
		Assert.Equal(array[2], result);
	}
	[Fact]
	public void ArrayIndexMultiDimensional() {
		int[,] array = { { 0, 1, 2, 3 }, { 4, 5, 6, 7 }, { 8, 9, 10, 11 } };
		var result = ExpressionEvaluator.Evaluate(
			Expression.ArrayIndex(
				Expression.Constant(array),
				Expression.Constant(2),
				Expression.Constant(2)
			)
		);
		Assert.Equal(10, result);
		Assert.Equal(array[2, 2], result);
	}
}