# Changelog

## 0.4.0 (Current):

### Changed (BREAKING)
- Target .NET 8.0

### Added

Direct evaluation for:

- ArrayIndex
- Index
- Subtract

## 0.3.0

### Added

Direct evaluation for:

- Lambda
- Conditional
- Add
- Multiply

### Changed

- Added fast path evaluatoon for direct field and property access.
	example: `let i; ExpressionEvaluator.Evaluate(() => i);`
- Removed duplicate type check in constant expression evaluation.

## 0.2.0

Restructured the evaluation internals to have a little less indirection.

## 0.1.0

Initial release.