# bUnit 2.x Migration Guide for Test Projects

## Overview
This document outlines the breaking changes encountered when upgrading from bUnit 1.1.5 to bUnit 2.7.2, along with required code updates.

## Key Breaking Changes in bUnit 2.x

### 1. RenderComponent Method is Obsolete
**Old Syntax (bUnit 1.x):**
```csharp
// No parameters
var rendered = _testContext.RenderComponent<MyComponent>();

// With lambda parameters
var rendered = _testContext.RenderComponent<MyComponent>(parameters => parameters
    .Add(p => p.MyProperty, value));

// With tuple parameters (HTML attributes)
var rendered = _testContext.RenderComponent<MyComponent>(
    ("id", "id1"),
    ("title", "text"));
```

**New Syntax (bUnit 2.x):**
```csharp
// No parameters  
var rendered = _testContext.Render<MyComponent>();

// With lambda parameters
var rendered = _testContext.Render<MyComponent>(parameters => parameters
    .Add(p => p.MyProperty, value));

// With HTML attributes - use .WithProperties()
var rendered = _testContext.Render<MyComponent>(parameters => parameters
    .WithProperties(("id", "id1"), ("title", "text")));

// Mixed HTML attributes and component properties
var rendered = _testContext.Render<MyComponent>(parameters => parameters
    .WithProperties(("id", "id1"))
    .Add(p => p.MyProperty, value));
```

### 2. SetParametersAndRender is Removed
**Old Syntax (bUnit 1.x):**
```csharp
rendered.SetParametersAndRender(parameters => parameters
    .Add(p => p.MyProperty, newValue));
```

**New Syntax (bUnit 2.x):**
```csharp
// For synchronous operations
await rendered.SetParametersAsync(parameters => parameters
    .Add(p => p.MyProperty, newValue));

// Or use the convenience method (if async context)
await rendered.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
{
    { nameof(MyComponent.MyProperty), newValue }
}));
```

### 3. WaitForAssertion is Removed
**Old Syntax (bUnit 1.x):**
```csharp
rendered.WaitForAssertion(() => rendered.MarkupMatches(expectedMarkup));
```

**New Syntax (bUnit 2.x):**
```csharp
// Use WaitForAssertionAsync in async tests
await rendered.WaitForAssertionAsync(() => rendered.MarkupMatches(expectedMarkup));

// Or better yet, use InvokeAsync if modifying component state
await rendered.InvokeAsync(async () => { /* state changes */ });
```

### 4. Obsolete FindAll Method
**Old:**
```csharp
var elements = rendered.FindAll(".selector");
```

**New:**
```csharp
var elements = rendered.FindAll(".selector").ToList();  // Now returns IEnumerable
```

## NuGet Package Changes

- **bunit.web is now obsolete** - functionality merged into main bunit package
- Only reference: `<PackageReference Include="bunit" Version="2.7.2" />`

## Required Using Statements

Add to your test files:
```csharp
using Bunit;
```

## .NET Framework Requirement

bUnit 2.x requires .NET 8 or higher. Ensure all test projects target:
- `.net10.0` or 
- `.net8.0` (minimum)

## Implementation Guide

### File-by-File Migration Checklist

For each test file:

1. [ ] Replace all `RenderComponent<T>()` with `Render<T>()`
2. [ ] Replace `RenderComponent<T>(parameters => ...)` with `Render<T>(parameters => ...)`
3. [ ] Convert tuple HTML attributes to `.WithProperties()`
4. [ ] Replace `SetParametersAndRender` with `SetParametersAsync` + `Render()`
5. [ ] Replace `WaitForAssertion` with `WaitForAssertionAsync` or `InvokeAsync`
6. [ ] Update async test methods as needed
7. [ ] Test and verify

### Example Migration: Modal Component Test

**Before (bUnit 1.x):**
```csharp
[TestMethod]
public void ModalDialog_should_not_rendered_anything_until_opened()
{
    var rendered = _testContext.RenderComponent<ModalDialog>(
        ("id", "id1"),
        ("title", "text"),
        (nameof(ModalDialog.OverlayOpacity), 0.5)
    );

    Assert.AreEqual(false, rendered.Instance.IsOpen);
    rendered.MarkupMatches("");
}

[TestMethod]
public async Task ModalDialog_should_rendered_correctly_when_opened()
{
    var rendered = _testContext.RenderComponent<ModalDialog>(
        ("id", "id1"),
        ("title", "text")
    );

    await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
    rendered.Render();

    rendered.WaitForAssertion(() => rendered.MarkupMatches(@"<div ..."));
}
```

**After (bUnit 2.x):**
```csharp
[TestMethod]
public void ModalDialog_should_not_rendered_anything_until_opened()
{
    var rendered = _testContext.Render<ModalDialog>(parameters => parameters
        .WithProperties(("id", "id1"), ("title", "text"))
        .Add(p => p.OverlayOpacity, 0.5)
    );

    Assert.AreEqual(false, rendered.Instance.IsOpen);
    rendered.MarkupMatches("");
}

[TestMethod]
public async Task ModalDialog_should_rendered_correctly_when_opened()
{
    var rendered = _testContext.Render<ModalDialog>(parameters => parameters
        .WithProperties(("id", "id1"), ("title", "text"))
    );

    await rendered.InvokeAsync(async () => { await rendered.Instance.Open(); });
    rendered.Render();

    await rendered.WaitForAssertionAsync(() => rendered.MarkupMatches(@"<div ..."));
}
```

## Common Patterns Summary

| Old Pattern | New Pattern |
|---|---|
| `RenderComponent<T>()` | `Render<T>()` |
| `RenderComponent<T>(params)` | `Render<T>(params)` |
| `.WithProperties(("x", "y"))` | `.WithProperties(("x", "y"))` |
| `SetParametersAndRender(params)` | `await SetParametersAsync(params); Render();` |
| `WaitForAssertion(assertion)` | `await WaitForAssertionAsync(assertion)` |

## Resources

- [bUnit Migration Guide](https://bunit.dev/docs/migrations)
- [bUnit 2.0 Release Notes](https://github.com/bUnit-dev/bUnit/releases/tag/v2.0.0)
