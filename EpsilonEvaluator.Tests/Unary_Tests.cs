using System.Linq.Expressions;
namespace EpsilonEvaluator.Tests;

public class Unary_Tests {

	[Theory]
	[InlineData(true, false)]
	[InlineData(false, true)]
	public void Not(dynamic a, dynamic expectedResult) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.Not(
				Expression.Constant(a)
			)
		);
		Assert.Equal(expectedResult, result);
		Assert.Equal(!a, result);
	}

	[Fact]
	public void Not_Nullable() {
		bool? a = true;
		var result = ExpressionEvaluator.Evaluate(
			Expression.Not(
				Expression.Constant(a)
			)
		);
		Assert.Equal(false, result);
		Assert.Equal(!a, result);

		a = null;
		result = ExpressionEvaluator.Evaluate(
			() => !a
		);
		Assert.Null(result);
		Assert.Equal(!a, result);
	}

	[Theory]
	[InlineData(1, typeof(double), 1.0)]
	[InlineData(1.0, typeof(int), 1)]
	public void Convert(dynamic a, Type targetType, dynamic expectedResult) {
		var result = ExpressionEvaluator.Evaluate(
			Expression.Convert(
				Expression.Constant(a),
				targetType
			)
		);
		Assert.Equal(expectedResult, result);
	}
}