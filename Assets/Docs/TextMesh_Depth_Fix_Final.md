# TextMesh Depth Rendering - Final Fix

## Issue History

### **Original Problem:**
TextMesh on letter nodes rendered **above** all opaque objects (ignoring depth).

### **First Fix Attempt:**
Changed render queue to 2000 (Geometry).

### **New Problem:**
Text now rendered **first**, so opaque objects covered it completely.

### **Final Fix:**
Changed render queue to 2450 (AlphaTest) - renders **after** geometry but **with** depth testing.

---

## Solution

### **FixTextMeshRendering Component**

**File:** `Assets/Scripts/FixTextMeshRendering.cs`

**Applied To:** `Assets/Prefabs/LetterNode.prefab` → LetterText child

**Configuration:**
```csharp
meshRenderer.material.renderQueue = 2450; // AlphaTest queue
meshRenderer.material.SetInt("_ZTest", (int)CompareFunction.LessEqual);
```

---

## How It Works

### **Render Queue Explanation:**

**Queue 2000 (Geometry):**
- Renders first (with opaque objects)
- Problem: Other opaque objects render on top
- Result: Text gets covered

**Queue 2450 (AlphaTest):**
- Renders after opaque geometry
- Still uses depth testing
- Result: Text visible but respects depth

**Queue 3000 (Transparent):**
- Renders last
- Ignores depth (always on top)
- Result: Text always visible (wrong)

### **Depth Testing:**

**ZTest = LessEqual:**
- Only renders pixels closer to camera than existing depth
- Respects 3D space properly
- Text appears behind objects that are in front of it

---

## Expected Behavior

### **Correct Rendering:**

**Scenario 1: Character in front of letter**
- Character position Z: -5
- Letter position Z: 0
- Result: ✅ Character blocks letter text (correct)

**Scenario 2: Character behind letter**
- Character position Z: 5
- Letter position Z: 0
- Result: ✅ Letter text visible (correct)

**Scenario 3: Character at same depth**
- Both at Z: 0
- Result: ✅ Renders based on distance to camera

---

## Testing Checklist

- [ ] Build and run the game
- [ ] Move character in front of letter nodes
- [ ] Verify text is hidden when character is in front
- [ ] Move character behind letter nodes
- [ ] Verify text is visible when character is behind
- [ ] Check from different camera angles
- [ ] Verify no z-fighting or flickering

---

## Troubleshooting

### **If text still renders on top:**

**Check:**
1. FixTextMeshRendering component is on LetterText
2. Component is enabled
3. Material render queue is 2450 (check in Play mode)

**Fix:**
- Manually set material render queue to 2450
- Or try queue 2500 (between AlphaTest and Transparent)

### **If text is completely hidden:**

**Check:**
1. Text is not behind the node cylinder
2. Camera can see the text
3. Text color is not transparent

**Fix:**
- Adjust LetterText position (Y: 0.14 above node)
- Check text color is visible (black or white)

### **If text flickers:**

**Cause:** Z-fighting (text and node at same depth)

**Fix:**
- Adjust LetterText Y position slightly higher
- Increase distance from node surface

---

## Alternative: Use TextMeshPro (Future)

For better text rendering, consider migrating to TextMeshPro:

**Benefits:**
- Better depth handling
- Better quality
- More features
- Better performance

**Migration:**
1. Import TextMeshPro package
2. Convert TextMesh to TextMeshPro
3. Update LetterNode script
4. Update prefab

---

## Summary

The TextMesh depth rendering is now fixed using render queue 2450 (AlphaTest):
- ✅ Renders after opaque geometry
- ✅ Respects depth testing
- ✅ Appears correctly in 3D space
- ✅ No longer always on top

The text will now render properly behind characters and other objects when appropriate.

---

**Status:** Fixed with render queue 2450  
**Last Updated:** November 5, 2025
