using System.Linq.Expressions;
namespace EpsilonEvaluator.Tests;

public class Math_Tests {
	[Theory]
	[InlineData(5, 50, 55)]
	[InlineData(5L, 50L, 55L)]
	[InlineData(5.0, 50.0, 55.0)]
	public void Add(dynamic a, dynamic b, dynamic expectedResult) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.Add(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(result, expectedResult);
		Assert.Equal(a + b, expectedResult);
	}

	[Theory]
	[InlineData(50, 5, 45)]
	[InlineData(5, 50, -45)]
	[InlineData(50.0, 5.0, 45.0)]
	[InlineData(5.0, 50.0, -45.0)]
	public void Subtract(dynamic a, dynamic b, dynamic expectedResult) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.Subtract(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(result, expectedResult);
		Assert.Equal(a - b, expectedResult);
	}

	[Theory]
	[InlineData(3, 5, 15)]
	[InlineData(-3, 5, -15)]
	[InlineData(3L, 5L, 15L)]
	[InlineData(-3L, 5L, -15L)]
	[InlineData(1.5, 3.0, 4.5)]
	public void Multiply(dynamic a, dynamic b, dynamic expectedResult) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.Multiply(
				Expression.Constant(a),
				Expression.Constant(b)
			)
		);
		Assert.Equal(result, expectedResult);
		Assert.Equal(a * b, expectedResult);
	}
}