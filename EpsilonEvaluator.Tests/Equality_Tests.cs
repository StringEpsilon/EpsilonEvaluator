using System.Linq.Expressions;
namespace EpsilonEvaluator.Tests;

public class Equality_Tests {
	[Theory]
	[InlineData(5, 50, false)]
	[InlineData(50, 50, true)]
	[InlineData(100, 50, false)]
	[InlineData("Hello World", "Hello World", true)]
	[InlineData("Hello World", "Hallo Welt", false)]
	[InlineData(false, false, true)]
	[InlineData(false, true, false)]
	public void Equal(dynamic a, dynamic b, bool isTrue) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.Equal(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(isTrue, result);
	}


	[Theory]
	[InlineData(5, 50, true)]
	[InlineData(50, 50, false)]
	[InlineData(100, 50, true)]
	[InlineData("Hello World", "Hello World", false)]
	[InlineData("Hello World", "Hallo Welt", true)]
	[InlineData(false, false, false)]
	[InlineData(false, true, true)]
	public void NotEqual(dynamic a, dynamic b, bool isTrue) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.NotEqual(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(isTrue, result);
		Assert.Equal(a != b, result);
	}

	[Theory]
	[InlineData(5, 50, false)]
	[InlineData(50, 50, false)]
	[InlineData(51, 50, true)]
	[InlineData(100, 50, true)]
	public void GreaterThan(int a, int b, bool isTrue) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.GreaterThan(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(isTrue, result);
		Assert.Equal(a > b, result);
	}

	[Theory]
	[InlineData(5, 50, false)]
	[InlineData(50, 50, true)]
	[InlineData(51, 50, true)]
	[InlineData(100, 50, true)]
	public void GreaterThanOrEqual(int a, int b, bool isTrue) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.GreaterThanOrEqual(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(isTrue, result);
		Assert.Equal(a >= b, result);
	}

	[Theory]
	[InlineData(5, 50, true)]
	[InlineData(50, 50, false)]
	[InlineData(51, 50, false)]
	public void LessThan(int a, int b, bool isTrue) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.LessThan(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(isTrue, result);
		Assert.Equal(a < b, result);
	}

	[Theory]
	[InlineData(5, 50, true)]
	[InlineData(50, 50, true)]
	[InlineData(51, 50, false)]
	[InlineData(5.5, 50.0, true)]
	[InlineData(50.0, 50.0, true)]
	[InlineData(50.0, 50.1, true)]
	[InlineData(50.00001, 50.0, false)]
	[InlineData(51.0, 50.5, false)]
	public void LessThanOrEqual(dynamic a, dynamic b, bool isTrue) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.LessThanOrEqual(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(isTrue, result);
		Assert.Equal(a <= b, result);
	}
}