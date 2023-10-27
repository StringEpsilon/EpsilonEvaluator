/**
	This Source Code Form is subject to the terms of the Mozilla Public
	License, v. 2.0. If a copy of the MPL was not distributed with this
	file, You can obtain one at http://mozilla.org/MPL/2.0/.
**/

namespace EpsilonEvaluator;

using System;
using System.Linq.Expressions;

internal static class BinaryEvaluator {
	internal static object? Add(BinaryExpression expression) {

		if (expression.IsLifted && expression.IsLiftedToNull) {
			return null;
		}
		object? left = ExpressionEvaluator.Evaluate(expression.Left);
		object? right = ExpressionEvaluator.Evaluate(expression.Right);
		if (left is string || right is string) {
			return left?.ToString() + right?.ToString();
		}
		if (left != null && right != null) {
			return left switch {
				double => (double)left + (double)right,
				long => (long)left + (long)right,
				ulong => (ulong)left + (ulong)right,
				int => (int)left + (int)right,
				uint => (uint)left + (uint)right,
				short => (short)left + (short)right,
				ushort => (ushort)left + (ushort)right,
				sbyte => (sbyte)left + (sbyte)right,
				byte => (byte)left + (byte)right,
				nint => (nint)left + (nint)right,
				nuint => (nuint)left + (nuint)right,
				_ => ExpressionEvaluator.CompileAndRun(expression),
			};
		}
		return ExpressionEvaluator.CompileAndRun(expression);
	}

	internal static object? Divide(BinaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);
	internal static object? Multiply(BinaryExpression expression) {
		if (expression.IsLifted && expression.IsLiftedToNull) {
			return null;
		}
		object? left = ExpressionEvaluator.Evaluate(expression.Left);
		object? right = ExpressionEvaluator.Evaluate(expression.Right);
		if (left is string || right is string) {
			return ExpressionEvaluator.CompileAndRun(expression);
		}
		if (left != null && right != null) {
			return left switch {
				double => (double)left * (double)right,
				long => (long)left * (long)right,
				ulong => (ulong)left * (ulong)right,
				int => (int)left * (int)right,
				uint => (uint)left * (uint)right,
				short => (short)left * (short)right,
				ushort => (ushort)left * (ushort)right,
				sbyte => (sbyte)left * (sbyte)right,
				byte => (byte)left * (byte)right,
				nint => (nint)left * (nint)right,
				nuint => (nuint)left * (nuint)right,
				_ => ExpressionEvaluator.CompileAndRun(expression),
			};
		}
		return ExpressionEvaluator.CompileAndRun(expression);
	}

	internal static object? Power(BinaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);
	internal static object? Subtract(BinaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);

	internal static object? And(BinaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);
	internal static object? Or(BinaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);
	internal static object? ExclusiveOr(BinaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);

	internal static object? Coalesce(BinaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);

	internal static bool Equal(BinaryExpression expression) {
		object? left = ExpressionEvaluator.Evaluate(expression.Left);
		object? right = ExpressionEvaluator.Evaluate(expression.Right);
		if (left == null || right == null) {
			return left == null && right == null;
		}
		return left.Equals(right);
	}

	internal static object? GreaterThan(BinaryExpression expression) {
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

	internal static object? LessThan(BinaryExpression expression) {
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

	internal static object? GreaterThanOrEqual(BinaryExpression expression) {
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

	internal static object? LessThanOrEqual(BinaryExpression expression) {
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
