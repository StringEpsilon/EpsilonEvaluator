/**
	This Source Code Form is subject to the terms of the Mozilla Public
	License, v. 2.0. If a copy of the MPL was not distributed with this
	file, You can obtain one at http://mozilla.org/MPL/2.0/.
**/

namespace EpsilonEvaluator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public static class ExpressionEvaluator {
	public static Dictionary<string, object?>? EvaluateParameters<T>(
		Expression<Action<T>> expression,
		bool alwaysCompile = false) {
		if (expression.Body is not MethodCallExpression methodExpression) {
			throw new InvalidOperationException(
				$"Can not evaluate parameters from expression of type {expression.GetType().FullName}"
			);
		}

		var parameters = methodExpression.Method.GetParameters();
		return new Dictionary<string, object?>(
			parameters.Select(argument => new KeyValuePair<string, object?>(
				argument.Name ?? "",
				alwaysCompile
					? CompileAndRun(methodExpression.Arguments[argument.Position])
					: Evaluate(methodExpression.Arguments[argument.Position])
				)
			)
		);
	}

	public static object? Evaluate(Expression expression) {
		return expression switch {
			ConstantExpression => EvaluateConst(expression),
			BinaryExpression binaryExpression => BinaryExpressionEvaluator.Evaluate(binaryExpression),
			UnaryExpression unaryExpression => UnaryExpressionEvaluator.Evaluate(unaryExpression),
			MemberExpression memberExpression => EvaluateMemberExpression(memberExpression),
			MethodCallExpression callExpression => EvaluateCall(callExpression),
			_ => CompileAndRun(expression),
		};
	}

	private static object? EvaluateCall(MethodCallExpression callExpression) {
		try {
			object?[] parameters = new object?[callExpression.Arguments.Count];
			for (int i = 0; i < callExpression.Arguments.Count; i++) {
				Expression? argument = callExpression.Arguments[i];
				parameters[i] = Evaluate(argument);
			}
			return callExpression.Method.Invoke(
				callExpression.Object != null
					? Evaluate(callExpression.Object)
					: null,
				parameters
			);
		} catch (Exception) {
			return CompileAndRun(callExpression);
		}
	}

	private static object? EvaluateConst(Expression expression) {
		if (expression is ConstantExpression constExpression) {
			return constExpression.Value;
		}
		return null;
	}

	private static object? EvaluateMemberExpression(MemberExpression memberExpression) {
		var memberStack = new List<MemberExpression>();
		Expression? currentExpression = memberExpression;
		while (currentExpression is MemberExpression currentMember) {
			memberStack.Add(currentMember);
			currentExpression = currentMember.Expression;
		}

		object? resolvedValue;
		if (currentExpression is ConstantExpression constantExpression) {
			resolvedValue = constantExpression.Value;
		} else if (currentExpression != null) {
			resolvedValue = CompileAndRun(currentExpression);
		} else {
			// Bail evaluation:
			return CompileAndRun(memberExpression);
		}
		for (var i = memberStack.Count; i > 0; i--) {
			currentExpression = memberStack[i - 1];
			resolvedValue = ((MemberExpression)currentExpression).Member switch {
				FieldInfo fieldInfo => fieldInfo.GetValue(resolvedValue),
				PropertyInfo propertyInfo => propertyInfo.GetValue(resolvedValue),
				_ => CompileAndRun(currentExpression),
			};
		}

		return resolvedValue;
	}

	public static object? CompileAndRun(Expression expression) {
		return Expression.Lambda(expression).Compile().DynamicInvoke();
	}
}