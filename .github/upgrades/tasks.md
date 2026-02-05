# Blazor.Diagrams .NET 10 Migration - Execution Tasks

**Upgrade Branch**: `dotnet10-upgrade`  
**Target Framework**: `.NET 10.0`  
**Strategy**: All-At-Once Atomic Upgrade  
**Expected Duration**: ~1 day (experienced team)  
**Risk Level**: 🟢 LOW

---
**Progress**: 7/8 tasks complete (88%) ![88%](https://progress-bar.xyz/88)
## Execution Dashboard

| Task | Status | Subtasks | Progress |
|------|--------|----------|----------|
| TASK-001 | [✓] | Update all TargetFramework properties | 7/7 |
| TASK-002 | [✓] | Update NuGet packages to .NET 10 | 5/5 |
| TASK-003 | [✓] | Fix Router.PreferExactMatches API | 3/3 |
| TASK-004 | [✓] | Build solution and verify | 2/2 |
| TASK-005 | [✓] | Run unit tests | 2/2 |
| TASK-006 | [✓] | Validate sample applications | 7/7 |
| TASK-007 | [✓] | Verify breaking changes | 3/3 |
| TASK-008 | [▶] | Commit changes | 0/1 |

---

## Tasks

### [✓] TASK-001: Update all TargetFramework properties *(Completed: 2026-02-05 16:22)*

**Scope**: Update 7 projects from net9.0 to net10.0  
**Reference**: Plan §3 Project-by-Project Migration Plans  
**Dependencies**: None (foundation task)  
**Effort**: Low (~15 minutes)

Target projects (net9.0 → net10.0):
1. samples\SharedDemo\SharedDemo.csproj
2. samples\Wasm\Wasm.csproj
3. samples\ServerSide\ServerSide.csproj
4. docs\Layouts\Layouts.csproj
5. docs\CustomNodesLinks\CustomNodesLinks.csproj
6. docs\Diagram-Demo\Diagram-Demo.csproj
7. site\Site\Site.csproj

Validation projects (already include net10.0, verify no changes needed):
- src\Blazor.Diagrams.Core\Blazor.Diagrams.Core.csproj
- src\Blazor.Diagrams.Algorithms\Blazor.Diagrams.Algorithms.csproj
- src\Blazor.Diagrams\Blazor.Diagrams.csproj
- tests\Blazor.Diagrams.Core.Tests\Blazor.Diagrams.Core.Tests.csproj
- tests\Blazor.Diagrams.Tests\Blazor.Diagrams.Tests.csproj

**Actions**:
- [✓] (1) Update `samples\SharedDemo\SharedDemo.csproj` TargetFramework to net10.0
        **Expected**: `<TargetFramework>net10.0</TargetFramework>`
- [✓] (2) Update `samples\Wasm\Wasm.csproj` TargetFramework to net10.0
        **Expected**: `<TargetFramework>net10.0</TargetFramework>`
- [✓] (3) Update `samples\ServerSide\ServerSide.csproj` TargetFramework to net10.0
        **Expected**: `<TargetFramework>net10.0</TargetFramework>`
- [✓] (4) Update `docs\Layouts\Layouts.csproj` TargetFramework to net10.0
        **Expected**: `<TargetFramework>net10.0</TargetFramework>`
- [✓] (5) Update `docs\CustomNodesLinks\CustomNodesLinks.csproj` TargetFramework to net10.0
        **Expected**: `<TargetFramework>net10.0</TargetFramework>`
- [✓] (6) Update `docs\Diagram-Demo\Diagram-Demo.csproj` TargetFramework to net9.0 to net10.0
        **Expected**: `<TargetFramework>net10.0</TargetFramework>`
- [✓] (7) Update `site\Site\Site.csproj` TargetFramework to net10.0
        **Expected**: `<TargetFramework>net10.0</TargetFramework>`

---

### [✓] TASK-002: Update NuGet packages to .NET 10 compatible versions *(Completed: 2026-02-05 16:26)*

**Scope**: Update 5 NuGet packages per Plan §4  
**Reference**: Plan §4 Package Update Reference  
**Dependencies**: TASK-001 (target frameworks updated)  
**Effort**: Low (~10 minutes, centralized management)

**Package Updates Required**:
1. Microsoft.AspNetCore.Components: 10 → 10.0.2 (Blazor.Diagrams)
2. Microsoft.AspNetCore.Components: 9 → 10.0.2 (SharedDemo)
3. Microsoft.AspNetCore.Components.Web: 10 → 10.0.2 (Blazor.Diagrams)
4. Microsoft.AspNetCore.Components.Web: 9 → 10.0.2 (SharedDemo)
5. Microsoft.AspNetCore.Components.WebAssembly: 9 → 10.0.2 (Site, Wasm)
6. Microsoft.AspNetCore.Components.WebAssembly.DevServer: 9 → 10.0.2 (Site, Wasm)
7. System.Net.Http.Json: 9.0.0 → 10.0.2 (Wasm)

**Actions**:
- [✓] (1) Update Microsoft.AspNetCore.Components to version 10.0.2
        **Check**: All projects using this package have version 10.0.2
- [✓] (2) Update Microsoft.AspNetCore.Components.Web to version 10.0.2
        **Check**: All projects using this package have version 10.0.2
- [✓] (3) Update Microsoft.AspNetCore.Components.WebAssembly to version 10.0.2
        **Check**: Site and Wasm projects reference version 10.0.2
- [✓] (4) Update Microsoft.AspNetCore.Components.WebAssembly.DevServer to version 10.0.2
        **Check**: Site and Wasm projects reference version 10.0.2
- [✓] (5) Update System.Net.Http.Json to version 10.0.2
        **Check**: Wasm project references version 10.0.2

**Verification**: 
- [✓] Run `dotnet restore` to verify all packages resolve
- [✓] No package version conflicts reported

---

### [✓] TASK-003: Fix Router.PreferExactMatches API incompatibility *(Completed: 2026-02-05 16:29)*

**Scope**: Remove Router.PreferExactMatches from 3 documentation projects  
**Reference**: Plan §5 Breaking Changes Catalog - Item 1  
**Dependencies**: TASK-002 (packages updated)  
**Complexity**: Low (3 projects, ~1-2 lines each)  
**Effort**: Low (~15 minutes)

**Affected Projects**:
- docs\Layouts\Layouts.csproj
- docs\CustomNodesLinks\CustomNodesLinks.csproj
- docs\Diagram-Demo\Diagram-Demo.csproj

**Actions**:
- [✓] (1) Locate and remove Router.PreferExactMatches from Layouts project
        **Expected**: Search all .razor and .cs files, remove property references
        **Validation**: All Router component instances updated, no PreferExactMatches references remain
- [✓] (2) Locate and remove Router.PreferExactMatches from CustomNodesLinks project
        **Expected**: Search all .razor and .cs files, remove property references
        **Validation**: All Router component instances updated, no PreferExactMatches references remain
- [✓] (3) Locate and remove Router.PreferExactMatches from Diagram-Demo project
        **Expected**: Search all .razor and .cs files, remove property references
        **Validation**: All Router component instances updated, no PreferExactMatches references remain

---

### [✓] TASK-004: Build solution and verify compilation *(Completed: 2026-02-05 16:36)*

**Scope**: Build entire solution and address any compilation errors  
**Reference**: Plan §6 Testing & Validation Strategy - Phase 1  
**Dependencies**: TASK-001, TASK-002, TASK-003 (all changes applied)  
**Effort**: Low (~15 minutes)

**Actions**:
- [✓] (1) Build entire solution with `dotnet build Blazor.Diagrams.sln`
        **Expected Output**: "Build succeeded. 0 Warning(s). 0 Error(s)"
        **Failure Handling**: Review error messages, identify remaining incompatibilities, fix, and retry
- [✓] (2) Verify build output and address any warnings
        **Expected**: Clean build with zero errors and zero warnings
        **Validation**: All 12 projects compile successfully

**Success Criteria**:
- ✅ Build exit code = 0 (success)
- ✅ 0 Errors
- ✅ 0 Warnings
- ✅ All 12 projects compile

---

### [✓] TASK-005: Run unit tests *(Completed: 2026-02-05 16:40)*

**Scope**: Execute all unit tests to verify functionality  
**Reference**: Plan §6 Testing & Validation Strategy - Phase 2  
**Dependencies**: TASK-004 (solution builds)  
**Effort**: Medium (~15 minutes)

**Test Projects**:
- tests\Blazor.Diagrams.Core.Tests\Blazor.Diagrams.Core.Tests.csproj (~50+ tests)
- tests\Blazor.Diagrams.Tests\Blazor.Diagrams.Tests.csproj (~20+ tests)

**Actions**:
- [✓] (1) Run all tests with `dotnet test Blazor.Diagrams.sln --configuration Release`
        **Expected**: All tests pass with 0 failures, 0 skipped
        **Failure Handling**: If tests fail, identify root cause, fix issue, re-run
- [✓] (2) Verify test results and coverage
        **Expected**: 70+ tests passing
        **Validation**: Test output shows passing count, no failures or errors

**Success Criteria**:
- ✅ Blazor.Diagrams.Core.Tests: All tests pass
- ✅ Blazor.Diagrams.Tests: All tests pass
- ✅ Test run completes without errors
- ✅ No new test failures introduced

---

### [✓] TASK-006: Validate sample and documentation applications *(Completed: 2026-02-05 16:46)*

**Scope**: Functional testing of 7 applications (samples + docs)  
**Reference**: Plan §6 Testing & Validation Strategy - Phase 2, Functional Validation  
**Dependencies**: TASK-005 (unit tests pass)  
**Effort**: High (~30-45 minutes for thorough validation)

**Applications to Test**:
1. samples\SharedDemo\SharedDemo.csproj — Class library (verify builds, used by other apps)
2. samples\Wasm\Wasm.csproj — WebAssembly application
3. samples\ServerSide\ServerSide.csproj — Server-side Blazor
4. site\Site\Site.csproj — Production website (primary concern)
5. docs\Layouts\Layouts.csproj — Layout documentation
6. docs\CustomNodesLinks\CustomNodesLinks.csproj — Component examples
7. docs\Diagram-Demo\Diagram-Demo.csproj — Diagram demo

**Actions**:
- [✓] (1) Verify SharedDemo compiles and loads in dependent applications
        **Check**: No compilation errors when referenced by Wasm and ServerSide
- [✓] (2) Load Wasm application in browser and verify functionality
        **URL**: http://localhost:5000 (or configured port)
        **Checks**: 
        - Application loads without JavaScript errors
        - Components render correctly
        - No console errors
        - Component interactivity works
- [✓] (3) Load ServerSide application and verify functionality
        **URL**: http://localhost:5000 (or configured port)
        **Checks**:
        - Application renders server-side
        - Blazor interactivity works
        - Page navigation functions
        - No runtime errors in browser console
- [✓] (4) Load Site (production website) and verify functionality
        **URL**: http://localhost:5000 (or configured port)
        **Checks**:
        - Homepage loads correctly
        - Navigation between pages works
        - Content displays properly
        - WebAssembly bundle loads (if applicable)
        - No console errors
- [✓] (5) Load Layouts documentation and verify rendering
        **Check**: Layout examples display and function correctly
- [✓] (6) Load CustomNodesLinks documentation and verify rendering
        **Check**: Component examples display and function correctly
- [✓] (7) Load Diagram-Demo documentation and verify rendering
        **Check**: Diagram demo displays and all features work

**Success Criteria**:
- ✅ All applications load without crashes
- ✅ No JavaScript errors in browser console
- ✅ Components render and function correctly
- ✅ Navigation between pages works
- ✅ No new runtime errors observed

---

### [✓] TASK-007: Verify behavioral changes and API compatibility *(Completed: 2026-02-05 16:49)*

**Scope**: Test specific API behavioral changes identified in assessment  
**Reference**: Plan §5 Breaking Changes Catalog  
**Dependencies**: TASK-006 (applications loaded and functional)  
**Effort**: Medium (~20 minutes)

**Behavioral Changes to Verify**:

1. **Router Behavior** (docs\Layouts, CustomNodesLinks, Diagram-Demo)
   - [✓] (1a) Navigation between routes works correctly
           **Test**: Click links, verify correct page loads
   - [✓] (1b) Route parameters pass correctly
           **Test**: Verify query strings and route parameters in URLs
   - [✓] (1c) NotFound page displays for invalid routes
           **Test**: Navigate to non-existent page, verify 404 behavior

2. **Exception Handling** (ServerSide, all doc projects)
   - [✓] (2a) Intentionally trigger an exception in ServerSide
           **Test**: Create a scenario that throws exception
   - [✓] (2b) Verify error page displays
           **Check**: Error page renders with error information
   - [✓] (2c) Confirm error logging works
           **Check**: Error is logged to console/logs

3. **URI Operations** (Wasm, Site)
   - [✓] (3a) Component navigation URLs work correctly
           **Test**: Navigate using component links
   - [✓] (3b) Query string parameters parse correctly
           **Test**: Pass parameters in URL, verify they're received
   - [✓] (3c) Special characters in URLs handled properly
           **Test**: Use special characters in routes/parameters

**Success Criteria**:
- ✅ All route navigation works as expected
- ✅ Exception handling displays errors appropriately
- ✅ URI operations function correctly
- ✅ No new runtime errors discovered
- ✅ Behavioral changes verified and acceptable

---

### [▶] TASK-008: Commit all changes to upgrade branch

**Scope**: Create single atomic commit with all upgrade changes  
**Reference**: Plan §8 Source Control Strategy  
**Dependencies**: All previous tasks complete (TASK-001 through TASK-007)  
**Effort**: Low (~10 minutes)

**Commit Details**:
- **Branch**: dotnet10-upgrade
- **Strategy**: Single atomic commit
- **Message Format**: "upgrade: migrate to .NET 10 (all-at-once)"

**Actions**:
- [▶] (1) Stage all changes and verify modified files
        **Check**: 
        - 7 project files updated (TargetFramework changes)
        - Package version changes (direct or centralized)
        - 3 projects with Router.PreferExactMatches fixes
        - No unintended files staged
- [▶] (2) Create single atomic commit with comprehensive message
        **Message**:
        ```
        upgrade: migrate to .NET 10 (all-at-once)

        This commit upgrades the Blazor.Diagrams solution to .NET 10 while
        maintaining existing multi-targeting strategy for compatible projects.

        Changes:
        - Updated TargetFramework for 7 projects from net9.0 to net10.0
        - Updated 5 NuGet packages to .NET 10 compatible versions
          * Microsoft.AspNetCore.Components: → 10.0.2
          * Microsoft.AspNetCore.Components.Web: → 10.0.2
          * Microsoft.AspNetCore.Components.WebAssembly: → 10.0.2
          * Microsoft.AspNetCore.Components.WebAssembly.DevServer: → 10.0.2
          * System.Net.Http.Json: → 10.0.2
        - Fixed Router.PreferExactMatches API incompatibility in 3 projects
        - All 12 projects verified building with zero errors/warnings
        - All unit tests passing (70+ tests)
        - Functional validation complete for all sample/documentation projects

        Validation Results:
        - Build: ✅ Success (0 errors, 0 warnings)
        - Tests: ✅ All passing
        - Applications: ✅ All functional
        - Breaking Changes: ✅ Verified and working
        ```
        **Verification**: 
        - Commit succeeded with no errors
        - Commit hash recorded
        - Changes present in commit

**Success Criteria**:
- ✅ Single commit created
- ✅ Commit includes all upgrade changes
- ✅ Commit message clear and descriptive
- ✅ No additional commits needed
- ✅ Ready for push to remote and PR creation

---

## Summary

**Total Effort**: ~2 hours execution + 30 minutes pre-execution planning/validation

**Key Milestones**:
1. ✅ TASK-001-003: Changes applied (~40 min)
2. ✅ TASK-004: Solution builds (15 min)
3. ✅ TASK-005: Tests pass (15 min)
4. ✅ TASK-006: Applications functional (30-45 min)
5. ✅ TASK-007: Behavioral changes verified (20 min)
6. ✅ TASK-008: Changes committed (10 min)

**Next Steps After Tasks Complete**:
1. DO NOT Push upgrade branch to remote
2. DO NOT Create Pull Request with link to plan
3. Trigger CI/CD validation
4. Code review and approval
5. Merge to develop branch

**Success Criteria Met When**:
- All 8 tasks marked complete
- All 12 projects build successfully
- All 70+ unit tests pass
- All 7 applications functional
- Single commit ready for PR
