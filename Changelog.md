# Changelog

## 0.3.0 (Future)

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

## 0.2.0 (Current)

Restructured the evaluation internals to have a little less indirection.

## 0.1.0

Initial release.