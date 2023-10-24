/**
	This Source Code Form is subject to the terms of the Mozilla Public
	License, v. 2.0. If a copy of the MPL was not distributed with this
	file, You can obtain one at http://mozilla.org/MPL/2.0/.
**/

namespace EpsilonEvaluator;

using System;
using System.Linq;
using System.Linq.Expressions;

internal static class UnaryEvaluator {
	internal static object? Negate(UnaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);
	internal static object? Quote(UnaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);
	internal static object? TypeAs(UnaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);
	internal static object? UnaryPlus(UnaryExpression expression) => ExpressionEvaluator.CompileAndRun(expression);

	internal static object? ArrayLength(UnaryExpression expression) {
		var operandValue = ExpressionEvaluator.Evaluate(expression.Operand);
		if (operandValue is Array array) {
			return array.Length;
		}
		return ExpressionEvaluator.CompileAndRun(expression);
	}

	internal static object? Convert(UnaryExpression expression) {
		var operandValue = ExpressionEvaluator.Evaluate(expression.Operand);
		if (expression.IsLifted) {
			if (expression.IsLiftedToNull) {
				if (operandValue == null) {
					return null;
				}
			}
			var targetType = expression.Type.GenericTypeArguments.FirstOrDefault();
			if (targetType != null) {
				return System.Convert.ChangeType(operandValue, targetType);
			} else {
				// Bail because IDK what to do if we don't have a generic argument on what should be Nullable<T>:
				return ExpressionEvaluator.CompileAndRun(expression);
			}
		} else {
			return System.Convert.ChangeType(operandValue, expression.Type);
		}
	}

	internal static object? Not(UnaryExpression expression) {
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
