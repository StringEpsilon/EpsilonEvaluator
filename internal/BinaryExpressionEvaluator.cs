/**
	This Source Code Form is subject to the terms of the Mozilla Public
	License, v. 2.0. If a copy of the MPL was not distributed with this
	file, You can obtain one at http://mozilla.org/MPL/2.0/.
**/

namespace EpsilonEvaluator;

using System;
using System.Linq.Expressions;

internal static class BinaryExpressionEvaluator {
	internal static object? Evaluate(BinaryExpression expression) {
		return expression.NodeType switch {
			ExpressionType.Equal => Equal(expression),
			ExpressionType.NotEqual => !Equal(expression),
			ExpressionType.GreaterThanOrEqual => GreaterThanOrEqual(expression),
			ExpressionType.GreaterThan => GreaterThan(expression),
			ExpressionType.LessThan => LessThan(expression),
			ExpressionType.LessThanOrEqual => LessThanOrEqual(expression),

			ExpressionType.Add => ExpressionEvaluator.CompileAndRun(expression), // todo.
			ExpressionType.Divide => ExpressionEvaluator.CompileAndRun(expression), // todo
			ExpressionType.Multiply => ExpressionEvaluator.CompileAndRun(expression), // todo
			ExpressionType.Power => ExpressionEvaluator.CompileAndRun(expression), // todo
			ExpressionType.Subtract => ExpressionEvaluator.CompileAndRun(expression), // todo

			ExpressionType.And => ExpressionEvaluator.CompileAndRun(expression), // todo
			ExpressionType.Or => ExpressionEvaluator.CompileAndRun(expression), // todo
			ExpressionType.ExclusiveOr => ExpressionEvaluator.CompileAndRun(expression), // todo.

			ExpressionType.Coalesce => ExpressionEvaluator.CompileAndRun(expression), // todo.
			_ => ExpressionEvaluator.CompileAndRun(expression)
		};
	}

	private static bool Equal(BinaryExpression expression) {
		object? left = ExpressionEvaluator.Evaluate(expression.Left);
		object? right = ExpressionEvaluator.Evaluate(expression.Right);
		if (left == null || right == null) {
			return left == null && right == null;
		}
		return left.Equals(right);
	}

	private static object? GreaterThan(BinaryExpression expression) {
		object? left = ExpressionEvaluator.Evaluate(expression.Left);
		object? right = ExpressionEvaluator.Evaluate(expression.Right);
		var leftType = left?.GetType();
		var rightType = right?.GetType();
		if (leftType?.IsValueType == true && rightType?.IsValueType == true) {
			return Type.GetTypeCode(left?.GetType()) switch {
				TypeCode.Int16 => Convert.ToInt16(left) > Convert.ToInt16(right),
				TypeCode.Int32 => Convert.ToInt32(left) > Convert.ToInt32(right),
				TypeCode.Int64 => Convert.ToInt64(left) > Convert.ToInt64(right),
				TypeCode.Double => Convert.ToDouble(left) > Convert.ToDouble(right),
				TypeCode.Single => Convert.ToSingle(left) > Convert.ToSingle(right),
				TypeCode.Char => Convert.ToChar(left) > Convert.ToChar(right),
				_ => ExpressionEvaluator.CompileAndRun(expression),
			};
		}
		return ExpressionEvaluator.CompileAndRun(expression);
	}

	private static object? LessThan(BinaryExpression expression) {
		object? left = ExpressionEvaluator.Evaluate(expression.Left);
		object? right = ExpressionEvaluator.Evaluate(expression.Right);
		var leftType = left?.GetType();
		var rightType = right?.GetType();
		if (leftType?.IsValueType == true && rightType?.IsValueType == true) {
			return Type.GetTypeCode(left?.GetType()) switch {
				TypeCode.Int16 => Convert.ToInt16(left) < Convert.ToInt16(right),
				TypeCode.Int32 => Convert.ToInt32(left) < Convert.ToInt32(right),
				TypeCode.Int64 => Convert.ToInt64(left) < Convert.ToInt64(right),
				TypeCode.Double => Convert.ToDouble(left) < Convert.ToDouble(right),
				TypeCode.Single => Convert.ToSingle(left) < Convert.ToSingle(right),
				TypeCode.Char => Convert.ToChar(left) < Convert.ToChar(right),
				_ => ExpressionEvaluator.CompileAndRun(expression),
			};
		}
		return ExpressionEvaluator.CompileAndRun(expression);
	}

	private static object? GreaterThanOrEqual(BinaryExpression expression) {
		object? left = ExpressionEvaluator.Evaluate(expression.Left);
		object? right = ExpressionEvaluator.Evaluate(expression.Right);
		var leftType = left?.GetType();
		var rightType = right?.GetType();
		if (leftType?.IsValueType == true && rightType?.IsValueType == true) {
			return Type.GetTypeCode(left?.GetType()) switch {
				TypeCode.Int16 => Convert.ToInt16(left) >= Convert.ToInt16(right),
				TypeCode.Int32 => Convert.ToInt32(left) >= Convert.ToInt32(right),
				TypeCode.Int64 => Convert.ToInt64(left) >= Convert.ToInt64(right),
				TypeCode.Double => Convert.ToDouble(left) >= Convert.ToDouble(right),
				TypeCode.Single => Convert.ToSingle(left) >= Convert.ToSingle(right),
				TypeCode.Char => Convert.ToChar(left) >= Convert.ToChar(right),
				_ => ExpressionEvaluator.CompileAndRun(expression),
			};
		}
		return ExpressionEvaluator.CompileAndRun(expression);
	}

	private static object? LessThanOrEqual(BinaryExpression expression) {
		object? left = ExpressionEvaluator.Evaluate(expression.Left);
		object? right = ExpressionEvaluator.Evaluate(expression.Right);
		var leftType = left?.GetType();
		var rightType = right?.GetType();
		if (leftType?.IsValueType == true && rightType?.IsValueType == true) {
			return Type.GetTypeCode(left?.GetType()) switch {
				TypeCode.Int16 => Convert.ToInt16(left) <= Convert.ToInt16(right),
				TypeCode.Int32 => Convert.ToInt32(left) <= Convert.ToInt32(right),
				TypeCode.Int64 => Convert.ToInt64(left) <= Convert.ToInt64(right),
				TypeCode.Double => Convert.ToDouble(left) <= Convert.ToDouble(right),
				TypeCode.Single => Convert.ToSingle(left) <= Convert.ToSingle(right),
				TypeCode.Char => Convert.ToChar(left) <= Convert.ToChar(right),
				_ => ExpressionEvaluator.CompileAndRun(expression),
			};
		}
		return ExpressionEvaluator.CompileAndRun(expression);
	}
}
