/**
	This Source Code Form is subject to the terms of the Mozilla Public
	License, v. 2.0. If a copy of the MPL was not distributed with this
	file, You can obtain one at http://mozilla.org/MPL/2.0/.
**/

namespace EpsilonEvaluator;

using System;
using System.Linq;
using System.Linq.Expressions;

internal static class UnaryExpressionEvaluator {
	// TODO: Properly evaluate Expression.IsLifted and IsLiftedToNull.

	internal static object? Evaluate(UnaryExpression expression) {
		return expression.NodeType switch {
			ExpressionType.ArrayLength => EvaluateArrayLength(expression),
			ExpressionType.Convert => EvaluateConvert(expression),
			ExpressionType.Not => EvaluateNot(expression),
			ExpressionType.Negate => ExpressionEvaluator.CompileAndRun(expression), // todo
			ExpressionType.Quote => ExpressionEvaluator.CompileAndRun(expression), // todo
			ExpressionType.TypeAs => ExpressionEvaluator.CompileAndRun(expression), // todo
			ExpressionType.UnaryPlus => ExpressionEvaluator.CompileAndRun(expression), // todo
			_ => ExpressionEvaluator.CompileAndRun(expression)
		};
	}

	private static object? EvaluateArrayLength(UnaryExpression expression) {
		var operandValue = ExpressionEvaluator.Evaluate(expression.Operand);
		if (operandValue is Array array) {
			return array.Length;
		}
		return ExpressionEvaluator.CompileAndRun(expression);
	}

	private static object? EvaluateConvert(UnaryExpression expression) {
		var operandValue = ExpressionEvaluator.Evaluate(expression.Operand);
		if (expression.IsLifted) {
			if (expression.IsLiftedToNull) {
				if (operandValue == null) {
					return null;
				}
			}
			var targetType = expression.Type.GenericTypeArguments.FirstOrDefault();
			if (targetType != null) {
				return Convert.ChangeType(operandValue, targetType);
			} else {
				// Bail because IDK what to do if we don't have a generic argument on what should be Nullable<T>:
				return ExpressionEvaluator.CompileAndRun(expression);
			}
		} else {
			return Convert.ChangeType(operandValue, expression.Type);
		}
	}

	private static object? EvaluateNot(UnaryExpression expression) {
		var operandValue = ExpressionEvaluator.Evaluate(expression.Operand);
		if (expression.IsLifted) {
			if (expression.IsLiftedToNull) {
				if (operandValue == null) {
					return null;
				}
				return !((bool?)operandValue).Value;
			}
			// Bail because nullable error here:
			return ExpressionEvaluator.CompileAndRun(expression);
		} else {
			if (operandValue != null) {
				return !(bool)operandValue;
			} else {
				// Bail because nullable error here:
				return ExpressionEvaluator.CompileAndRun(expression);
			}
		}
	}
}
