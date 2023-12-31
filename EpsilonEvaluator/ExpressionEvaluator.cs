﻿/**
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
		Expression<Action<T>> expression
	) {
		if (expression.Body is not MethodCallExpression methodExpression) {
			throw new InvalidOperationException(
				$"Can not evaluate parameters from expression of type {expression.GetType().FullName}"
			);
		}
		var parameterInfos = methodExpression.Method.GetParameters();
		if (parameterInfos.Length == 0) {
			return  new Dictionary<string, object?>();
		}
		KeyValuePair<string, object?>[] parameters = new KeyValuePair<string, object?>[parameterInfos.Length];
		for (int i = 0; i < parameterInfos.Length; i++) {
			parameters[i] = new KeyValuePair<string, object?>(
				parameterInfos[i].Name ?? "",
				Evaluate(methodExpression.Arguments[parameterInfos[i].Position])
			);
		}
		return new Dictionary<string, object?>(parameters);
	}

	public static object? Evaluate(Expression expression) {
		return expression.NodeType switch {
			// Binary expressions:
			ExpressionType.Constant => EvaluateConst((ConstantExpression)expression),

			ExpressionType.Equal => BinaryEvaluator.Equal((BinaryExpression)expression),
			ExpressionType.NotEqual => !BinaryEvaluator.Equal((BinaryExpression)expression),
			ExpressionType.GreaterThanOrEqual => BinaryEvaluator.GreaterThanOrEqual((BinaryExpression)expression),
			ExpressionType.GreaterThan => BinaryEvaluator.GreaterThan((BinaryExpression)expression),
			ExpressionType.LessThan => BinaryEvaluator.LessThan((BinaryExpression)expression),
			ExpressionType.LessThanOrEqual => BinaryEvaluator.LessThanOrEqual((BinaryExpression)expression),

			ExpressionType.Add => BinaryEvaluator.Add((BinaryExpression)expression),
			ExpressionType.Divide => BinaryEvaluator.Divide((BinaryExpression)expression),
			ExpressionType.Multiply => BinaryEvaluator.Multiply((BinaryExpression)expression),
			ExpressionType.Power => BinaryEvaluator.Power((BinaryExpression)expression),
			ExpressionType.Subtract => BinaryEvaluator.Subtract((BinaryExpression)expression),

			ExpressionType.And => BinaryEvaluator.And((BinaryExpression)expression),
			ExpressionType.Or => BinaryEvaluator.Or((BinaryExpression)expression),
			ExpressionType.ExclusiveOr => BinaryEvaluator.ExclusiveOr((BinaryExpression)expression),

			ExpressionType.Coalesce => BinaryEvaluator.Coalesce((BinaryExpression)expression),
			ExpressionType.ArrayIndex => BinaryEvaluator.ArrayIndex((BinaryExpression)expression),

			//Unary expressions:
			ExpressionType.ArrayLength => UnaryEvaluator.ArrayLength((UnaryExpression)expression),
			ExpressionType.Convert => UnaryEvaluator.Convert((UnaryExpression)expression),
			ExpressionType.Not => UnaryEvaluator.Not((UnaryExpression)expression),
			ExpressionType.Negate => UnaryEvaluator.Negate((UnaryExpression)expression),
			ExpressionType.Quote => UnaryEvaluator.Quote((UnaryExpression)expression),
			ExpressionType.TypeAs => UnaryEvaluator.TypeAs((UnaryExpression)expression),
			ExpressionType.UnaryPlus => UnaryEvaluator.UnaryPlus((UnaryExpression)expression),

			// Members:
			ExpressionType.Conditional => Conditional((ConditionalExpression)expression),
			ExpressionType.MemberAccess => EvaluateMemberExpression((MemberExpression)expression),

			// Other:
			ExpressionType.Call => EvaluateCall((MethodCallExpression)expression),
			ExpressionType.Index => EvaluateIndex((IndexExpression)expression),
			ExpressionType.Lambda => Lambda((LambdaExpression)expression),
			_ => CompileAndRun(expression),
		};
	}

	private static object? EvaluateIndex(IndexExpression expression) {
		if (expression.Indexer == null || expression.Object == null) {
			return CompileAndRun(expression);
		}
		var item = Evaluate(expression.Object);
		return expression.Indexer
			.GetGetMethod()
			?.Invoke(
				item,
				expression.Arguments.Select(y => Evaluate(y)).ToArray()
			);
	}

	private static object? Lambda(LambdaExpression expression) {
		return Evaluate(expression.Body);
	}

	private static object? Conditional(ConditionalExpression expression) {
		if ((bool?)Evaluate(expression.Test) == true) {
			return Evaluate(expression.IfTrue);
		}
		return Evaluate(expression.IfTrue);
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

	private static object? EvaluateConst(ConstantExpression expression) {
		return expression.Value;
	}

	private static object? EvaluateMemberExpression(MemberExpression memberExpression) {
		if (memberExpression.Expression?.NodeType == ExpressionType.Constant) {
			if (memberExpression.Member is FieldInfo fieldInfo) {
				return fieldInfo.GetValue(EvaluateConst((ConstantExpression)memberExpression.Expression));
			}
			if (memberExpression.Member is PropertyInfo propInfo) {
				return propInfo.GetValue(EvaluateConst((ConstantExpression)memberExpression.Expression));
			}
		}

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