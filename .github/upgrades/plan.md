# Blazor.Diagrams .NET 10 Migration Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Migration Plans](#project-by-project-migration-plans)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Risk Management](#risk-management)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario
Upgrade Blazor.Diagrams solution from multi-targeted .NET versions to .NET 10, maintaining existing multi-targeting strategy where applicable.

### Scope
- **Projects**: 12 total (3 libraries, 8 applications/samples, 1 test suite)
- **Current State**: Mixed targeting (some at net9.0, others multi-targeted net10.0;net9.0;net8.0;net7.0;net6.0)
- **Target State**: All projects include net10.0 in their target frameworks
- **Code Volume**: 12,385 lines across 220 files
- **Dependencies**: 12 compatible packages; 7 require updates

### Complexity Assessment

**Solution Classification**: ?? **SIMPLE**
- Small codebase (12K LOC)
- Low to medium complexity (all projects rated ?? Low)
- Clear, linear dependency hierarchy
- No circular dependencies
- All API issues are source incompatible or behavioral (no binary breaks)

**Issues Found**:
- 13 API compatibility issues (source incompatible and behavioral changes)
- 7 NuGet package updates required
- No security vulnerabilities
- No blocking compatibility issues

### Selected Strategy

**All-At-Once Strategy** - All projects upgraded simultaneously in a single coordinated operation

**Rationale**:
- Solution size and complexity suitable for atomic upgrade
- All projects can be updated in parallel
- No intermediate states required
- Clear dependency ordering enables single-pass upgrade
- Minimal risk profile (low complexity, no vulnerabilities)
- Faster completion time
- Easier to test comprehensively

### Critical Issues
None identified. All projects are compatible with .NET 10 after package updates.

---

## Migration Strategy

### Approach: All-At-Once Atomic Upgrade

This solution will be upgraded through a single, unified operation where all projects and packages are updated simultaneously. This approach is optimal for the Blazor.Diagrams solution because:

1. **Homogeneous Codebase**: All projects follow the same patterns and dependency structure
2. **Small Solution Size**: 12 projects with clear dependencies enable safe atomic operations
3. **Low Complexity**: No architectural patterns requiring phased migration
4. **Minimal Risk**: All compatibility issues are well-understood (behavioral changes only)
5. **Single Coordination Point**: All changes happen together, reducing integration complexity

### Implementation Philosophy

**Atomic Operation**: All framework updates, package updates, and code changes occur in a single coordinated pass with no intermediate states. This ensures:
- No dependency mismatches
- Consistent testing from known state
- Single commit opportunity
- Clear success/failure criteria
- No maintenance of multiple branch versions

### Dependency-Based Ordering (Atomic Context)

While following the All-At-Once approach, the upgrade respects dependency constraints by handling three logical groups simultaneously:

**Group 1: Foundation Libraries** (no project dependencies)
- `Blazor.Diagrams.Core`
- `Blazor.Diagrams.Algorithms`

**Group 2: Core Components** (depend on Group 1)
- `Blazor.Diagrams`
- Test projects (`Blazor.Diagrams.Core.Tests`, `Blazor.Diagrams.Tests`)

**Group 3: Applications & Samples** (depend on Group 1 or Group 2)
- `SharedDemo`
- `Wasm`, `ServerSide`
- `Layouts`, `CustomNodesLinks`, `Diagram-Demo`
- `Site`

**All three groups are updated simultaneously** as one atomic operation. The ordering shown above is for understanding the dependency relationships, not for sequential execution.

### Execution Flow

**Phase 1: Atomic Upgrade** (Single operation)
1. Update all project TargetFrameworks to include net10.0
2. Update all package references to their target versions
3. Restore dependencies
4. Build solution and fix all compilation errors
5. Verify solution builds with zero errors

**Phase 2: Validation & Testing** (After atomic upgrade succeeds)
1. Run all test projects
2. Verify expected API behavior changes
3. Confirm no behavioral regressions

---

## Detailed Dependency Analysis

### Dependency Structure Summary

The solution follows a clean, layered architecture with minimal complexity:

```
Foundation Layer (0 dependencies):
  ?? Blazor.Diagrams.Core (4,578 LOC)
  ?? Blazor.Diagrams.Algorithms (65 LOC)

Component Layer (depends on Foundation):
  ?? Blazor.Diagrams (1,625 LOC)
  ?? Blazor.Diagrams.Core.Tests (2,562 LOC)
  ?? Blazor.Diagrams.Tests (340 LOC)

Application Layer (depends on Component/Foundation):
  ?? SharedDemo (1,554 LOC)
  ?? Wasm (22 LOC)
  ?? ServerSide (117 LOC)
  ?? Layouts (187 LOC)
  ?? CustomNodesLinks (230 LOC)
  ?? Diagram-Demo (184 LOC)
  ?? Site (921 LOC)
```

### Projects by Migration Phase

**Phase 1: Foundation (Can be migrated anytime)**
- `src\Blazor.Diagrams.Core\Blazor.Diagrams.Core.csproj` — Multi-target (net10.0;net9.0;net8.0;net7.0;net6.0)
- `src\Blazor.Diagrams.Algorithms\Blazor.Diagrams.Algorithms.csproj` — Multi-target (net10.0;net9.0;net8.0;net7.0;net6.0)

**Phase 2: Components (Depends on Phase 1)**
- `src\Blazor.Diagrams\Blazor.Diagrams.csproj` — Multi-target (net10.0;net9.0;net8.0;net7.0;net6.0)
- `tests\Blazor.Diagrams.Core.Tests\Blazor.Diagrams.Core.Tests.csproj` — Multi-target (net10.0;net9.0;net8.0;net7.0;net6.0)
- `tests\Blazor.Diagrams.Tests\Blazor.Diagrams.Tests.csproj` — Multi-target (net10.0;net9.0;net8.0;net7.0;net6.0)

**Phase 3: Applications (Depends on Phase 1 or 2)**
- `samples\SharedDemo\SharedDemo.csproj` — Single target net9.0 ? net10.0
- `samples\Wasm\Wasm.csproj` — Single target net9.0 ? net10.0
- `samples\ServerSide\ServerSide.csproj` — Single target net9.0 ? net10.0
- `docs\Layouts\Layouts.csproj` — Single target net9.0 ? net10.0
- `docs\CustomNodesLinks\CustomNodesLinks.csproj` — Single target net9.0 ? net10.0
- `docs\Diagram-Demo\Diagram-Demo.csproj` — Single target net9.0 ? net10.0
- `site\Site\Site.csproj` — Single target net9.0 ? net10.0

### Critical Path Analysis

**Root Applications** (no dependants, define completion criteria):
- `Site` — Core production website
- `Wasm` — WebAssembly sample
- `ServerSide` — Server-side Blazor sample
- Documentation projects — Must build successfully
- Test projects — All tests must pass

**Foundation Stability**: 
- Foundation libraries (Core, Algorithms) already multi-target net10.0
- No changes required to their dependency structure
- Their update is primarily validation that nothing breaks

### Dependency Count Summary

| Project | Dependencies | Dependants | Complexity |
|---------|--------------|-----------|-----------|
| Blazor.Diagrams.Core | 1 (SvgPathProperties) | 8 | Low |
| Blazor.Diagrams.Algorithms | 1 (Blazor.Diagrams.Core) | 2 | Low |
| Blazor.Diagrams | 1 (Blazor.Diagrams.Core) | 7 | Low |
| SharedDemo | 3 | 2 | Low |
| Wasm | 2 | 0 | Low |
| ServerSide | 1 | 0 | Low |
| Site | 3 | 0 | Low |
| Documentation (3 projects) | 2 each | 0 | Low |
| Test projects (2) | 1-2 each | 0 | Low |

---

## Project-by-Project Migration Plans

### Foundation Libraries

#### src\Blazor.Diagrams.Core\Blazor.Diagrams.Core.csproj

**Current State**: 
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0`
- Package Dependencies: 1 (SvgPathProperties 1.1.2 — compatible)
- LOC: 4,578
- Complexity: ?? Low

**Target State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0` (already includes net10.0)
- Updates: No changes required to TargetFramework (already multi-targets net10.0)
- Validation: Confirm build succeeds

**Migration Notes**:
- This library already targets net10.0 as part of its multi-target strategy
- No package updates required
- No code changes needed
- Role: Validation that no breaking changes affect this foundation library

---

#### src\Blazor.Diagrams.Algorithms\Blazor.Diagrams.Algorithms.csproj

**Current State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0`
- Package Dependencies: 1 (Blazor.Diagrams.Core)
- LOC: 65
- Complexity: ?? Low

**Target State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0` (already includes net10.0)
- Updates: No changes required
- Validation: Confirm build succeeds

**Migration Notes**:
- Already multi-targets net10.0
- No package updates needed
- No code changes needed
- Validation purpose: Confirm dependency on Blazor.Diagrams.Core remains compatible

---

### Component Layer

#### src\Blazor.Diagrams\Blazor.Diagrams.csproj

**Current State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0`
- Package Dependencies: 2 updates needed
- LOC: 1,625
- Complexity: ?? Low

**Target State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0` (already includes net10.0)
- Updates: Microsoft.AspNetCore.Components: 10 ? 10.0.2; Microsoft.AspNetCore.Components.Web: 10 ? 10.0.2
- Validation: Confirm build succeeds

**Migration Steps**:
1. Update package references to latest versions
2. Build and verify no compilation errors
3. Confirm public API surface unchanged

---

#### tests\Blazor.Diagrams.Core.Tests\Blazor.Diagrams.Core.Tests.csproj

**Current State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0`
- Package Dependencies: 2 (System.Net.Http, System.Text.RegularExpressions — both included in framework, no updates needed)
- LOC: 2,562
- Complexity: ?? Low

**Target State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0` (already includes net10.0)
- Updates: Remove package references for framework-included packages (optional, build will succeed either way)
- Validation: All tests pass

**Migration Notes**:
- Already multi-targets net10.0
- System.Net.Http and System.Text.RegularExpressions are included in .NET 10 framework
- Consider removing or letting framework shadow these packages
- Test coverage: Core library functionality

---

#### tests\Blazor.Diagrams.Tests\Blazor.Diagrams.Tests.csproj

**Current State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0`
- Package Dependencies: 2 (System.Net.Http, System.Text.RegularExpressions — both included in framework, no updates needed)
- LOC: 340
- Complexity: ?? Low

**Target State**:
- Target Frameworks: `net10.0;net9.0;net8.0;net7.0;net6.0` (already includes net10.0)
- Updates: Remove package references for framework-included packages (optional)
- Validation: All tests pass

---

### Application & Sample Layer

#### samples\SharedDemo\SharedDemo.csproj

**Current State**:
- Target Framework: `net9.0`
- Package Dependencies: 2 updates needed (Microsoft.AspNetCore.Components: 9 ? 10.0.2; Microsoft.AspNetCore.Components.Web: 9 ? 10.0.2)
- LOC: 1,554
- Dependencies: 3 (Blazor.Diagrams.Core, Blazor.Diagrams, Blazor.Diagrams.Algorithms)
- Complexity: ?? Low

**Target State**:
- Target Framework: `net10.0`
- Updates: Microsoft.AspNetCore.Components: 10.0.2; Microsoft.AspNetCore.Components.Web: 10.0.2
- Validation: Confirm build succeeds

**Migration Steps**:
1. Update TargetFramework to net10.0
2. Update package references
3. Build and verify
4. Confirm downstream consumers (Wasm, ServerSide) remain compatible

---

#### samples\Wasm\Wasm.csproj

**Current State**:
- Target Framework: `net9.0`
- Package Dependencies: 3 updates needed
- LOC: 22
- Dependencies: 2 (SharedDemo, Blazor.Diagrams)
- Complexity: ?? Low (minimal code, mostly configuration)

**Target State**:
- Target Framework: `net10.0`
- Updates: Microsoft.AspNetCore.Components.WebAssembly: 9 ? 10.0.2; Microsoft.AspNetCore.Components.WebAssembly.DevServer: 9 ? 10.0.2; System.Net.Http.Json: 9.0.0 ? 10.0.2
- Validation: Build succeeds, WebAssembly bundle generates correctly

**Migration Notes**:
- Depends on SharedDemo (must be updated first in same atomic operation)
- System.Net.Http.Json update is critical for compatibility
- WebAssembly-specific packages must align with host framework

---

#### samples\ServerSide\ServerSide.csproj

**Current State**:
- Target Framework: `net9.0`
- Package Dependencies: None requiring updates
- LOC: 117
- Dependencies: 1 (SharedDemo)
- Complexity: ?? Low

**Target State**:
- Target Framework: `net10.0`
- Updates: None (depends on SharedDemo which will be updated)
- Behavioral Changes: Verify UseExceptionHandler behavior (see Breaking Changes Catalog)
- Validation: Build succeeds

**Migration Notes**:
- Depends on SharedDemo
- One behavioral change in exception handling (see Breaking Changes Catalog)

---

#### docs\Layouts\Layouts.csproj

**Current State**:
- Target Framework: `net9.0`
- Package Dependencies: None requiring updates
- LOC: 187
- Dependencies: 2 (Blazor.Diagrams.Core, Blazor.Diagrams)
- Complexity: ?? Low

**Target State**:
- Target Framework: `net10.0`
- Updates: None
- Behavioral Changes: 1 router change, 1 exception handler change (see Breaking Changes Catalog)
- Validation: Build succeeds

---

#### docs\CustomNodesLinks\CustomNodesLinks.csproj

**Current State**:
- Target Framework: `net9.0`
- Package Dependencies: None requiring updates
- LOC: 230
- Dependencies: 2 (Blazor.Diagrams.Core, Blazor.Diagrams)
- Complexity: ?? Low

**Target State**:
- Target Framework: `net10.0`
- Updates: None
- Behavioral Changes: 1 router change, 1 exception handler change (see Breaking Changes Catalog)
- Validation: Build succeeds

---

#### docs\Diagram-Demo\Diagram-Demo.csproj

**Current State**:
- Target Framework: `net9.0`
- Package Dependencies: None requiring updates
- LOC: 184
- Dependencies: 2 (Blazor.Diagrams.Core, Blazor.Diagrams)
- Complexity: ?? Low

**Target State**:
- Target Framework: `net10.0`
- Updates: None
- Behavioral Changes: 1 router change, 1 exception handler change (see Breaking Changes Catalog)
- Validation: Build succeeds

---

#### site\Site\Site.csproj

**Current State**:
- Target Framework: `net9.0`
- Package Dependencies: 2 updates needed
- LOC: 921
- Dependencies: 3 (Blazor.Diagrams.Core, Blazor.Diagrams, Blazor.Diagrams.Algorithms)
- Complexity: ?? Low

**Target State**:
- Target Framework: `net10.0`
- Updates: Microsoft.AspNetCore.Components.WebAssembly: 9 ? 10.0.2; Microsoft.AspNetCore.Components.WebAssembly.DevServer: 9 ? 10.0.2
- Behavioral Changes: 3 changes (routing, exception handling, URI changes) — see Breaking Changes Catalog
- Validation: Build succeeds, website content loads correctly

**Migration Notes**:
- Primary production site
- Multiple API behavioral changes require verification
- WebAssembly hosting setup must be validated

---

## Package Update Reference

### Summary

| Status | Count |
|--------|-------|
| ? Compatible (no update needed) | 12 |
| ?? Upgrade Required | 7 |
| **Total** | **19** |

### Package Updates Required

#### ASP.NET Core Component Packages

These packages are core to Blazor component rendering and must be updated to .NET 10 compatible versions:

| Package | Current | Target | Projects Affected | Reason |
|---------|---------|--------|-------------------|--------|
| Microsoft.AspNetCore.Components | 9-10 | 10.0.2 | Blazor.Diagrams (1.0?10.0.2), SharedDemo (9?10.0.2) | Framework compatibility, API updates |
| Microsoft.AspNetCore.Components.Web | 9-10 | 10.0.2 | Blazor.Diagrams (1.0?10.0.2), SharedDemo (9?10.0.2) | Framework compatibility, web component support |
| Microsoft.AspNetCore.Components.WebAssembly | 9 | 10.0.2 | Site (9?10.0.2), Wasm (9?10.0.2) | WebAssembly runtime compatibility |
| Microsoft.AspNetCore.Components.WebAssembly.DevServer | 9 | 10.0.2 | Site (9?10.0.2), Wasm (9?10.0.2) | Development server compatibility |

#### Utility Packages

| Package | Current | Target | Projects Affected | Reason |
|---------|---------|--------|-------------------|--------|
| System.Net.Http.Json | 9.0.0 | 10.0.2 | Wasm | JSON serialization for HTTP operations |

#### Framework-Included Packages (Optional Removal)

The following packages are included in .NET 10 framework. They may remain as package references (no harm) or be removed for cleaner project files:

| Package | Current | Status | Projects Affected | Note |
|---------|---------|--------|-------------------|------|
| System.Net.Http | 4.3.4 | Included in .NET 10 | Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests | Safe to remove |
| System.Text.RegularExpressions | 4.3.1 | Included in .NET 10 | Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests | Safe to remove |

#### Compatible Packages (No Updates)

The following packages are compatible with .NET 10 and require no updates:

| Package | Current | Projects | Status |
|---------|---------|----------|--------|
| bunit | 1.36.0 | Blazor.Diagrams.Tests | ? Compatible |
| coverlet.collector | 6.0.0 | Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests | ? Compatible |
| FluentAssertions | 6.12.0 | Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests | ? Compatible |
| GraphShape | 1.2.1 | Layouts | ? Compatible |
| MatBlazor | 2.10.0 | Layouts | ? Compatible |
| Microsoft.NET.Test.Sdk | 17.8.0 | Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests | ? Compatible |
| Moq | 4.18.4 | Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests | ? Compatible |
| SvgPathProperties | 1.1.2 | Blazor.Diagrams.Core | ? Compatible |
| xunit | 2.6.3 | Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests | ? Compatible |
| xunit.runner.visualstudio | 2.5.5 | Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests | ? Compatible |

### Update Locations

All package references are defined in `src\Directory.Build.props` using centralized package management (`ManagePackageVersionsCentrally = true`). This means:

**Single Update Point**: Update package versions in `Directory.Packages.props` (if it exists) or in the central version specification, and all projects automatically use the updated versions.

**Affected Projects** (indirectly through centralized versions):
- All projects using updated packages will automatically reference new versions after central update

---

## Breaking Changes Catalog

### Expected Breaking Changes

The following breaking changes have been identified from the assessment. Most are behavioral changes requiring testing validation rather than code changes.

#### 1. Router.PreferExactMatches Property — Source Incompatible

**Category**: ?? Source Incompatible  
**Affected Projects**: 3
- `docs\Layouts\Layouts.csproj`
- `docs\CustomNodesLinks\CustomNodesLinks.csproj`
- `docs\Diagram-Demo\Diagram-Demo.csproj`

**Description**: The `Router.PreferExactMatches` property has been removed or changed in .NET 10.

**Impact**: Code accessing this property will fail to compile.

**Resolution**: 
- Locate all usages of `Router.PreferExactMatches` in these three projects
- Remove or replace with alternative routing configuration
- Test that routing still works as expected
- .NET 10 routing behavior should handle this automatically

**Expected Code Changes**: 2-3 lines per project

---

#### 2. UseExceptionHandler Behavior Change — Behavioral Change

**Category**: ?? Behavioral Change  
**Affected Projects**: 4
- `samples\ServerSide\ServerSide.csproj` (1 instance)
- `docs\Layouts\Layouts.csproj` (1 instance)
- `docs\CustomNodesLinks\CustomNodesLinks.csproj` (1 instance)
- `docs\Diagram-Demo\Diagram-Demo.csproj` (1 instance)

**API**: `Microsoft.AspNetCore.Builder.ExceptionHandlerExtensions.UseExceptionHandler(IApplicationBuilder, String)`

**Description**: The exception handling middleware behavior has changed in .NET 10.

**Impact**: Exception handling paths may behave differently at runtime. Error pages might be called with different state or timing.

**Resolution**:
1. Review each usage of `app.UseExceptionHandler(path)` in Program.cs or startup code
2. Test error scenarios to confirm expected behavior
3. Verify error pages render correctly
4. No code change likely required, but runtime behavior testing is essential

**Testing**: Create test cases that trigger exceptions and verify error pages display correctly

---

#### 3. System.Uri Constructor Behavior — Behavioral Change

**Category**: ?? Behavioral Change  
**Affected Projects**: Multiple (Wasm, Site projects and others that use URI handling)

**API**: `System.Uri` class and constructor `System.Uri.#ctor(String)`

**Description**: URI parsing behavior has changed in .NET 10, particularly around validation and encoding.

**Impact**: Code creating or parsing URIs may behave differently. Validation might be stricter or more lenient.

**Resolution**:
1. Identify code constructing `Uri` objects
2. Test with various URI formats used in the application
3. Verify URI parsing still works for Blazor routing and navigation
4. No code changes expected unless URI validation was relying on old behavior

**Locations to Check**:
- Component navigation code
- API endpoint URLs
- Routing parameters

---

### API Compatibility Analysis

**Summary**:
- ?? Source Incompatible: 3 instances (Router.PreferExactMatches)
- ?? Behavioral Changes: 10 instances (UseExceptionHandler: 4, Uri: 4, others: 2)
- ? Compatible APIs: 20,790+ (vast majority unaffected)

### Verification Checklist

- [ ] Router.PreferExactMatches references removed/replaced in documentation projects
- [ ] Exception handling tested in all affected projects
- [ ] Error pages render and function correctly
- [ ] URI-based operations (navigation, routing) work as expected
- [ ] All tests pass
- [ ] No new runtime errors observed

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

Testing occurs in two phases: immediately after atomic upgrade (build validation) and after successful compilation (functional validation).

### Phase 1: Build Validation

**Immediate after all updates applied**:

1. **Compile Solution**
   - Command: `dotnet build Blazor.Diagrams.sln`
   - Expected: All projects compile with zero errors
   - Success Criteria: Build exit code 0

2. **Address Compilation Errors**
   - Errors expected: Router.PreferExactMatches references
   - Action: Remove or replace incompatible API calls (3 projects)
   - Verify: No remaining compilation errors

3. **Warnings Review**
   - Ensure no new warnings introduced
   - Address any obsolete API warnings
   - Success: Clean compilation

### Phase 2: Functional Validation

**After successful compilation**:

#### Unit Tests

| Test Project | Tests | Framework | Expected Outcome |
|--------------|-------|-----------|------------------|
| Blazor.Diagrams.Core.Tests | ~50+ | xUnit | ? All pass |
| Blazor.Diagrams.Tests | ~20+ | xUnit + bUnit | ? All pass |

**Execution**:
```
dotnet test Blazor.Diagrams.sln --configuration Release
```

**Success Criteria**:
- All tests pass
- No timeout issues
- No new failures

#### Component & Application Validation

**Projects to Verify**:

| Project | Validation Type | Approach |
|---------|-----------------|----------|
| SharedDemo | Component functionality | Verify in host applications |
| Wasm | WebAssembly runtime | Load in browser, test component interaction |
| ServerSide | Server-side Blazor | Render pages, test interactivity |
| Site | Production website | Full navigation, feature testing |
| Layouts | Layout documentation | Render and test layout components |
| CustomNodesLinks | Component examples | Verify diagram rendering |
| Diagram-Demo | Diagram functionality | Verify all demo features work |

#### Specific Behavioral Change Tests

**Router Changes** (Layouts, CustomNodesLinks, Diagram-Demo):
- [ ] Navigation between routes works
- [ ] Route parameters pass correctly
- [ ] NotFound page displays for invalid routes
- [ ] Link generation is correct

**Exception Handling** (ServerSide, all ASP.NET Core projects):
- [ ] Intentionally trigger an exception
- [ ] Verify error page displays
- [ ] Confirm error details are logged
- [ ] Check error page styling is intact

**URI Operations** (Wasm, Site):
- [ ] Component navigation URLs work
- [ ] Query string parameters parse correctly
- [ ] API endpoint URLs are valid
- [ ] Special characters in URLs handled properly

### Testing Checklist

**Pre-Testing**:
- [ ] All projects upgraded
- [ ] Solution builds without errors
- [ ] Solution builds without warnings

**Unit Testing**:
- [ ] Blazor.Diagrams.Core.Tests: All tests pass
- [ ] Blazor.Diagrams.Tests: All tests pass
- [ ] Test execution time reasonable (< 30s for full suite)

**Functional Validation**:
- [ ] Wasm application loads in browser
- [ ] ServerSide application renders correctly
- [ ] Site homepage loads and is responsive
- [ ] Documentation sites render without errors
- [ ] All navigation paths work
- [ ] Error handling behaves correctly

**Regression Testing**:
- [ ] Diagram creation and manipulation works
- [ ] Component rendering is correct
- [ ] No JavaScript errors in console
- [ ] No performance degradation observed

### Validation Success Criteria

The upgrade is considered successful when:
1. ? All 12 projects build without errors
2. ? All 12 projects build without warnings
3. ? All unit tests pass (Blazor.Diagrams.Core.Tests, Blazor.Diagrams.Tests)
4. ? All sample applications (Wasm, ServerSide, Site) load and function
5. ? All documentation projects render correctly
6. ? Router behavior works as expected
7. ? Exception handling works as expected
8. ? No security vulnerabilities introduced

---

## Complexity & Effort Assessment

### Per-Project Complexity Rating

| Project | Type | LOC | Complexity | Risk | Dependencies | Estimated Effort |
|---------|------|-----|-----------|------|--------------|------------------|
| Blazor.Diagrams.Core | Library | 4,578 | ?? Low | Low | 1 | Validation only |
| Blazor.Diagrams.Algorithms | Library | 65 | ?? Low | Low | 1 | Validation only |
| Blazor.Diagrams | Library | 1,625 | ?? Low | Low | 1 | Package updates |
| SharedDemo | Library | 1,554 | ?? Low | Low | 3 | Target + Packages |
| Blazor.Diagrams.Core.Tests | Test | 2,562 | ?? Low | Low | 1 | Framework update |
| Blazor.Diagrams.Tests | Test | 340 | ?? Low | Low | 1 | Framework update |
| Wasm | App | 22 | ?? Low | Low | 2 | Target + Packages |
| ServerSide | App | 117 | ?? Low | Low | 1 | Target + Testing |
| Site | App | 921 | ?? Low | Low | 3 | Target + Testing |
| Layouts | App | 187 | ?? Low | Low | 2 | Target + API fix |
| CustomNodesLinks | App | 230 | ?? Low | Low | 2 | Target + API fix |
| Diagram-Demo | App | 184 | ?? Low | Low | 2 | Target + API fix |

### Solution-Level Complexity Factors

| Factor | Assessment | Impact |
|--------|-----------|--------|
| **Codebase Size** | 12,385 LOC total (small) | ? Low impact on upgrade duration |
| **Project Count** | 12 projects (manageable) | ? All can be updated simultaneously |
| **Dependency Depth** | 3 layers maximum | ? Clear ordering, no cycles |
| **Package Changes** | 7 updates required | ? Mostly framework version alignment |
| **API Breaking Changes** | 3 source incompatible (3 projects) | ?? Minimal code changes expected |
| **Behavioral Changes** | 10 identified (requires testing) | ? No code changes needed |
| **Test Coverage** | 2 test projects, ~70+ tests | ? Good coverage for validation |
| **Architecture Complexity** | Clean, layered design | ? No architectural refactoring needed |

### Effort Summary

**Total Estimated Effort**: Low (~1 day for experienced team)

- **Framework Updates**: 30 minutes (apply to all projects)
- **Package Updates**: 15 minutes (centralized management)
- **API Fixes**: 30 minutes (3 projects, ~1-2 lines each)
- **Build & Compile**: 15 minutes
- **Testing**: 1-2 hours (running tests, verifying functionality)
- **Documentation Review**: 30 minutes

**Effort is Low Because**:
1. Small, homogeneous codebase
2. Simple dependency structure
3. All projects at same target framework (net9.0 or net10.0-ready)
4. Minimal API surface changes required
5. Good test coverage for validation
6. No architectural changes needed
7. Centralized package management

### Risk Assessment

**Overall Risk Level**: ?? **LOW**

#### Risk Factors

| Risk | Probability | Impact | Mitigation |
|------|------------|--------|-----------|
| Package version conflicts | Low | Medium | Use centralized package management |
| API incompatibilities beyond what's identified | Low | Medium | Comprehensive testing after build |
| Build failures in CI/CD | Low | Low | Test locally before commit |
| Runtime behavior changes | Low | Low | Functional testing phase |
| WebAssembly runtime issues | Low | Medium | Test Wasm project thoroughly |
| Routing problems | Medium | Low | Router.PreferExactMatches fix needed |
| Exception handling timing | Low | Low | Test error scenarios |

#### High-Risk Items

**None identified**. All complexity factors are low-risk.

#### Mitigation Strategies

1. **Testing First**: Run full test suite before considering any deployment
2. **Phased Validation**: Atomic upgrade, then immediate build validation, then functional tests
3. **Backup**: Current code remains on develop branch; can revert if needed
4. **Documentation**: Keep notes of any behavioral differences discovered during testing
5. **Communication**: Inform team of expected breaking changes before testing

---

## Risk Management

### Risk Register

#### Risk 1: Router.PreferExactMatches API Incompatibility

**Severity**: ?? Medium  
**Probability**: High  
**Status**: Identified, Expected

**Description**: The `Router.PreferExactMatches` property is no longer available or has changed in .NET 10. This will cause compilation errors in three documentation projects.

**Affected Projects**:
- docs\Layouts\Layouts.csproj
- docs\CustomNodesLinks\CustomNodesLinks.csproj
- docs\Diagram-Demo\Diagram-Demo.csproj

**Impact**: Build will fail; these three projects won't compile until resolved.

**Mitigation**:
1. Identify exact location of Router.PreferExactMatches usage before upgrade
2. Document current routing configuration
3. During upgrade, remove or comment out the property
4. Verify routing still works without it
5. Test routing behavior comprehensively

**Contingency**: If routing breaks after fix, roll back to previous approach or investigate alternative routing configuration in .NET 10 docs.

---

#### Risk 2: Exception Handling Behavior Change

**Severity**: ?? Low  
**Probability**: Medium  
**Status**: Identified, Expected

**Description**: Exception handling middleware behavior may differ in .NET 10, potentially affecting when/how error pages display.

**Affected Projects**:
- samples\ServerSide\ServerSide.csproj
- docs\Layouts\Layouts.csproj
- docs\CustomNodesLinks\CustomNodesLinks.csproj
- docs\Diagram-Demo\Diagram-Demo.csproj

**Impact**: Error handling may behave differently; error pages might display with different timing or state.

**Mitigation**:
1. Test each application with intentional exceptions
2. Verify error pages display correctly
3. Confirm error logging still works
4. Check error page styling and content
5. Document any behavior differences

**Contingency**: If error handling is broken, investigate .NET 10 exception handling documentation and update middleware configuration accordingly.

---

#### Risk 3: WebAssembly Runtime Compatibility

**Severity**: ?? Medium  
**Probability**: Low  
**Status**: Identified

**Description**: WebAssembly runtime or interop might have changes between .NET 9 and 10.

**Affected Projects**:
- samples\Wasm\Wasm.csproj
- site\Site\Site.csproj

**Impact**: WebAssembly application might fail to load or initialize; JavaScript interop might break.

**Mitigation**:
1. Load Wasm projects in browser
2. Open browser developer tools console
3. Check for JavaScript errors or warnings
4. Test component interactivity
5. Verify WebAssembly module loads
6. Test any JavaScript interop code

**Contingency**: If Wasm fails to load, check DevServer logs and .NET 10 WebAssembly release notes for breaking changes.

---

#### Risk 4: Package Version Lock Issues

**Severity**: ?? Low  
**Probability**: Low  
**Status**: Mitigated by centralized package management

**Description**: Centralized package management might lock package versions preventing proper resolution.

**Affected**: All projects using centralized versions

**Mitigation**:
1. Update all package versions to target versions in one operation
2. Use `dotnet restore` to verify resolution
3. Check for any unresolved version conflicts
4. Verify all projects can see updated versions

**Contingency**: Manually remove and re-add package references if issues occur.

---

### No Security Vulnerabilities Identified

The assessment found no security vulnerabilities in the current packages. Package updates are for framework compatibility, not security remediation.

### Rollback Plan

If critical issues are discovered that cannot be quickly resolved:

1. **Immediate Rollback**:
   ```
   git checkout develop
   ```

2. **Root Cause Analysis**:
   - Identify what broke
   - Research solution in .NET 10 documentation
   - Update plan based on findings

3. **Retry**:
   - Implement fix in plan
   - Create new upgrade branch
   - Re-execute with fix in place

4. **Documentation**:
   - Update plan with discovered issues
   - Share learnings with team

### What Mitigates All Risks

1. **Comprehensive Testing**: Full test suite validation before calling it done
2. **Atomic Approach**: All changes together, clear success/failure criteria
3. **Git Branching**: Can always revert to develop branch
4. **Low Complexity**: No complex interactions to break
5. **Good Test Coverage**: Can quickly identify regressions
6. **Small Codebase**: Easy to review changes and trace issues

---

## Source Control Strategy

### All-At-Once Single Commit Approach

This upgrade uses a **single-commit strategy** reflecting the atomic nature of the migration.

### Branching Strategy

**Source Branch**: `develop`
- Current production development branch
- Contains latest code before upgrade
- Protected branch (requires PR for changes)

**Upgrade Branch**: `dotnet10-upgrade`
- Created from `develop`
- Contains all upgrade changes
- Will be merged back to `develop` via PR after validation

**Workflow**:
```
develop (starting point)
  ?
dotnet10-upgrade (all changes here)
  ?
Pull Request for review & CI/CD validation
  ?
Merge to develop
```

### Commit Strategy

**Single Comprehensive Commit**

All upgrade changes are committed in one atomic commit:

```
Commit Message: "upgrade: migrate to .NET 10 (all-at-once)"

Changes included:
- Update all TargetFramework properties to include net10.0
- Update NuGet packages to .NET 10 compatible versions
- Fix Router.PreferExactMarches API incompatibility (3 projects)
- Update centralized package versions
```

**Why Single Commit**:
1. **Atomic**: Everything is tested together, released together
2. **Reversible**: One commit to revert if critical issue found
3. **Trackable**: Clear single point where upgrade occurred
4. **Testable**: Either all changes work or none do

### Changes to Be Committed

#### File Changes

1. **Project Files** (12 total):
   - All `.csproj` files with TargetFramework updates

2. **Central Package Management**:
   - `src\Directory.Build.props` (if package versions defined there)
   - `Directory.Packages.props` (if it exists)

3. **Source Code** (3 projects):
   - Remove/comment Router.PreferExactMatches references in:
     - docs\Layouts\Layouts.csproj (files to be determined)
     - docs\CustomNodesLinks\CustomNodesLinks.csproj (files to be determined)
     - docs\Diagram-Demo\Diagram-Demo.csproj (files to be determined)

#### Files NOT Changed

- No configuration file changes expected
- No appsettings changes required
- No Program.cs/Startup.cs updates needed (for most projects)
- No global.json changes (not mentioned in assessment)

### Pre-Commit Validation

Before committing:

1. **Local Build Success**
   - All projects build with `dotnet build`
   - Zero errors, zero warnings

2. **Unit Tests Pass**
   - Run `dotnet test`
   - All tests pass locally

3. **Functional Validation Complete**
   - Manual testing of sample apps
   - Verification of behavioral changes
   - Documentation of any issues found

### Push & Pull Request

**After local validation**:

1. Push upgrade branch to remote
   ```
   git push origin dotnet10-upgrade
   ```

2. Create Pull Request with:
   - Description of upgrade scope
   - Link to this plan
   - Testing validation checklist
   - Known behavioral changes documented

3. CI/CD Pipeline:
   - Full build on CI server
   - All tests re-run
   - Code analysis (if enabled)
   - Results feed into PR review

4. Code Review:
   - Review project file changes
   - Review Router.PreferExactMatches fixes
   - Review package version updates
   - Approval before merge

5. Merge to `develop`:
   - After CI passes and review approved
   - Merge with squash or regular merge (team preference)
   - Delete upgrade branch after merge

### Commit Hygiene

**Clear, descriptive commit message**:
```
upgrade: migrate solution to .NET 10 (all-at-once strategy)

This commit upgrades the Blazor.Diagrams solution to .NET 10 while 
maintaining existing multi-targeting strategy for compatible projects.

Changes:
- Updated TargetFramework for 7 projects from net9.0 to net10.0
- Updated 5 NuGet packages to .NET 10 compatible versions
  - Microsoft.AspNetCore.Components: 9/10 ? 10.0.2 (2 projects)
  - Microsoft.AspNetCore.Components.Web: 9/10 ? 10.0.2 (2 projects)
  - Microsoft.AspNetCore.Components.WebAssembly: 9 ? 10.0.2 (2 projects)
  - Microsoft.AspNetCore.Components.WebAssembly.DevServer: 9 ? 10.0.2 (2 projects)
  - System.Net.Http.Json: 9.0.0 ? 10.0.2 (1 project)
- Fixed Router.PreferExactMatches API incompatibility (3 projects)
- All 12 projects verified building with zero errors/warnings
- All unit tests passing (70+ tests)
- Functional validation complete for all sample/documentation projects

Addresses:
- All NuGet packages updated per assessment recommendations
- All API compatibility issues resolved
- All behavioral changes documented and tested
```

### Rollback Contingency

If critical issues discovered after merge:

```
git revert <commit-hash>
```

This creates a reverse commit without losing history, then:
1. Analyze root cause
2. Fix in new upgrade attempt
3. Create new PR with fixes

---

## Success Criteria

### Technical Success Criteria

**? Build Success**
- [ ] All 12 projects build with `dotnet build` 
- [ ] Zero compilation errors across entire solution
- [ ] Zero compiler warnings (clean build)
- [ ] Build takes reasonable time (< 2 minutes)

**? Framework Target Verification**
- [ ] 7 projects updated from net9.0 to net10.0:
  - [ ] SharedDemo
  - [ ] Wasm
  - [ ] ServerSide
  - [ ] Layouts
  - [ ] CustomNodesLinks
  - [ ] Diagram-Demo
  - [ ] Site
- [ ] 5 multi-targeted projects verified to include net10.0:
  - [ ] Blazor.Diagrams.Core
  - [ ] Blazor.Diagrams.Algorithms
  - [ ] Blazor.Diagrams
  - [ ] Blazor.Diagrams.Core.Tests
  - [ ] Blazor.Diagrams.Tests

**? Package Update Verification**
- [ ] Microsoft.AspNetCore.Components updated to 10.0.2 in all projects using it
- [ ] Microsoft.AspNetCore.Components.Web updated to 10.0.2 in all projects using it
- [ ] Microsoft.AspNetCore.Components.WebAssembly updated to 10.0.2 in Site and Wasm
- [ ] Microsoft.AspNetCore.Components.WebAssembly.DevServer updated to 10.0.2
- [ ] System.Net.Http.Json updated to 10.0.2 in Wasm
- [ ] Compatible packages verified (no unexpected version changes)

**? API Compatibility Resolution**
- [ ] Router.PreferExactMatches references removed/replaced in:
  - [ ] Layouts project
  - [ ] CustomNodesLinks project
  - [ ] Diagram-Demo project
- [ ] All other API incompatibilities verified as behavioral only (no code changes needed)

**? Unit Test Success**
- [ ] `dotnet test` runs successfully
- [ ] Blazor.Diagrams.Core.Tests: All tests pass
- [ ] Blazor.Diagrams.Tests: All tests pass
- [ ] No new test failures introduced
- [ ] Test execution completes in reasonable time

### Functional Success Criteria

**? Application Functionality**
- [ ] **Wasm Sample**: Loads in browser, components render, no JavaScript errors
- [ ] **ServerSide Sample**: Renders correctly, interactivity works
- [ ] **Site (Production)**: Homepage loads, all navigation works, content displays
- [ ] **Documentation Projects**: All examples render without errors

**? Feature Validation**
- [ ] Diagram creation and manipulation works as expected
- [ ] Component rendering is correct
- [ ] Routing between pages works correctly
- [ ] Exception handling displays error pages appropriately
- [ ] No performance degradation observed

**? Breaking Change Validation**
- [ ] Router behavior works without PreferExactMatches
- [ ] Exception handler middleware executes and displays errors
- [ ] URI operations work for routing and navigation
- [ ] No new runtime errors observed

### Quality Success Criteria

**? Code Quality**
- [ ] No new compiler warnings
- [ ] No new code analysis issues
- [ ] API surface unchanged for public libraries
- [ ] Code patterns consistent with existing codebase

**? Test Coverage**
- [ ] Existing test coverage maintained
- [ ] No test skips introduced
- [ ] All tests run successfully
- [ ] Coverage reports generated (if applicable)

**? Documentation**
- [ ] CHANGELOG or release notes updated
- [ ] Breaking changes documented for consumers
- [ ] API changes documented (if any public API affected)
- [ ] Migration guide created (if needed)

### Process Success Criteria

**? Source Control**
- [ ] Single atomic commit on dotnet10-upgrade branch
- [ ] Commit message clearly describes upgrade
- [ ] All changes tracked in Git
- [ ] No untracked files left behind

**? Code Review & CI/CD**
- [ ] Pull request created with clear description
- [ ] CI/CD pipeline passes successfully
- [ ] Code review approved
- [ ] No merge conflicts

**? Deployment Readiness**
- [ ] Plan followed as documented
- [ ] All validation steps completed
- [ ] No unexpected issues discovered
- [ ] Team notified of changes
- [ ] Ready for merge to develop

### Final Success Checklist

Before declaring upgrade complete, verify:

- [ ] **All 12 projects build successfully**
- [ ] **All 70+ unit tests pass**
- [ ] **All sample/documentation apps load and function**
- [ ] **API compatibility issues resolved**
- [ ] **Package updates applied and verified**
- [ ] **Breaking changes tested and validated**
- [ ] **No new compilation errors or warnings**
- [ ] **Single commit created on upgrade branch**
- [ ] **Pull request ready for review and merge**
- [ ] **Team notified and ready for deployment**

### Definition of "Done"

The upgrade is **DONE** when:

1. **Technical Requirements Met**: All above technical criteria verified ?
2. **Testing Complete**: Full test suite passes locally and on CI/CD ?
3. **Code Review Approved**: At least one reviewer approved changes ?
4. **Ready to Merge**: PR approved and all CI checks pass ?
5. **Documentation Updated**: Any needed docs/changelogs updated ?

At this point, the upgrade branch is ready to be merged to `develop`, and the Blazor.Diagrams project is running on .NET 10.
