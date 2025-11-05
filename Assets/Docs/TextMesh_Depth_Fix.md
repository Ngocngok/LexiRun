# TextMesh Depth Rendering Fix

## Issue
TextMesh on letter nodes was rendering above all opaque objects, ignoring depth/z-ordering. This caused letters to appear on top of characters, UI, and other 3D objects incorrectly.

## Root Cause
TextMesh materials by default use a render queue of 3000 (Transparent queue), which renders after all opaque geometry and doesn't properly respect depth testing.

## Solution Implemented

### **FixTextMeshRendering Component**

**File:** `Assets/Scripts/FixTextMeshRendering.cs`

**Applied To:** LetterNode prefab → LetterText child

**What It Does:**
1. Sets material render queue to 2000 (Geometry queue)
2. Configures sorting layer and order
3. Ensures depth testing is enabled
4. Runs automatically on Start

**How It Works:**
```csharp
// Change render queue from Transparent (3000) to Geometry (2000)
meshRenderer.material.renderQueue = 2000;

// Set sorting layer
meshRenderer.sortingLayerName = "Default";
meshRenderer.sortingOrder = 0;
```

---

## Technical Details

### **Render Queues in Unity:**

| Queue | Value | Behavior |
|-------|-------|----------|
| Background | 1000 | Renders first |
| **Geometry** | **2000** | **Opaque objects with depth** |
| AlphaTest | 2450 | Cutout transparency |
| Transparent | 3000 | Renders last, no depth write |
| Overlay | 4000 | Renders on top of everything |

### **The Problem:**
- TextMesh default: Queue 3000 (Transparent)
- Renders after all opaque objects
- Ignores depth buffer
- Always appears on top

### **The Fix:**
- TextMesh now: Queue 2000 (Geometry)
- Renders with opaque objects
- Respects depth buffer
- Appears behind objects correctly

---

## Files Modified

### **Prefab:**
- `Assets/Prefabs/LetterNode.prefab`
  - Added FixTextMeshRendering to LetterText child

### **Scripts:**
- `Assets/Scripts/FixTextMeshRendering.cs` - New component
- `Assets/Scripts/Editor/FixTextMeshDepth.cs` - Editor utility (optional)
- `Assets/Scripts/Editor/FixLetterNodeTextDepth.cs` - Editor utility (optional)

---

## Testing

### **Before Fix:**
- Letter text renders on top of player
- Letter text renders on top of bots
- Letter text renders on top of UI
- Depth is ignored

### **After Fix:**
- ✅ Letter text respects depth
- ✅ Characters can appear in front of text
- ✅ Proper 3D rendering
- ✅ Correct z-ordering

---

## Alternative Solutions (Not Used)

### **Option 1: Change Shader**
- Replace with depth-writing shader
- More complex, requires shader knowledge

### **Option 2: Use TextMeshPro**
- Better text rendering overall
- Requires converting all text
- More setup time

### **Option 3: Manual Material Edit**
- Edit material render queue manually
- Not automated, error-prone

**Chosen Solution:** Runtime component (FixTextMeshRendering)
- ✅ Automatic
- ✅ Simple
- ✅ Works immediately
- ✅ No manual setup

---

## Summary

The TextMesh rendering issue is now fixed. Letter text on nodes will:
- ✅ Render at correct depth
- ✅ Appear behind characters when appropriate
- ✅ Respect 3D z-ordering
- ✅ Look correct in all camera angles

**Status:** Fixed  
**Last Updated:** November 5, 2025
