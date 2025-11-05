# Home Scene Chicken Animation - Implementation Summary

## Overview
Added an animated chicken character to the HomeScene that wanders randomly between waypoints, making the scene more lively and playful. The chicken walks between nodes, pauses, and continues wandering indefinitely.

## Implementation Date
November 5, 2025

---

## Components Added

### 1. ChickenWanderer Script (`Assets/Scripts/ChickenWanderer.cs`)

**Purpose:** Controls the chicken's random wandering behavior between waypoints.

**Key Features:**
- Auto-finds waypoints in the scene (looks for "WaypointNodes" parent)
- Randomly selects next waypoint to visit
- Walks to waypoint with smooth movement
- Pauses at each waypoint (idle animation)
- Rotates smoothly towards movement direction
- Configurable movement speed, pause duration, and rotation speed

**Configurable Parameters:**
- `moveSpeed`: How fast the chicken moves (default: 2)
- `minPauseDuration`: Minimum pause time at waypoints (default: 1s)
- `maxPauseDuration`: Maximum pause time at waypoints (default: 3s)
- `reachedThreshold`: Distance to consider waypoint reached (default: 0.1)
- `rotationSpeed`: How fast the chicken rotates (default: 5)

**Public Properties:**
- `waypoints`: Array of Transform waypoints (auto-assigned if empty)

### 2. Scene Setup

**HomeScene Structure:**
```
HomeScene
├── Main Camera (Position: 0,8,-6, Rotation: 55°,0,0)
├── Directional Light
├── Ground (Plane, Scale: 2,1,2)
├── WaypointNodes (Parent)
│   ├── Waypoint1 (Position: -3,0,1)
│   ├── Waypoint2 (Position: 2,0,-1)
│   ├── Waypoint3 (Position: -1,0,-2)
│   ├── Waypoint4 (Position: 3,0,2)
│   └── Waypoint5 (Position: 0,0,0)
├── HomeChicken
│   └── ChickModel (Chick_LOD0.fbx, Scale: 0.5)
│       └── CharacterAnimationController
└── Canvas (UI Overlay)
    ├── Image (Background - Transparent)
    └── SafeArea
        └── MainMenu (UI Elements)
```

### 3. Camera Configuration

**Main Camera:**
- Position: (0, 8, -6)
- Rotation: (55°, 0, 0)
- Angle: 55° looking down
- Purpose: Shows chicken from above at a good viewing angle

**Why this angle?**
- 55° provides a nice 3D perspective
- Can see the chicken clearly
- Shows the ground plane
- Similar to gameplay camera but closer

### 4. Waypoint Layout

**5 Waypoints scattered randomly:**
- Waypoint1: (-3, 0, 1) - Left side
- Waypoint2: (2, 0, -1) - Right side
- Waypoint3: (-1, 0, -2) - Center-back
- Waypoint4: (3, 0, 2) - Far right
- Waypoint5: (0, 0, 0) - Center

**Layout Strategy:**
- Scattered within camera view
- No specific pattern (random feel)
- Covers the visible area
- Prevents chicken from going off-screen

### 5. UI Background Transparency

**Canvas/Image:**
- Color: (0, 0, 0, 0) - Fully transparent
- Purpose: Let the 3D scene show through as background
- UI elements remain visible on top

---

## How It Works

### Wandering Behavior:

1. **Initialization:**
   - Auto-finds waypoints under "WaypointNodes" parent
   - Starts at a random waypoint position
   - Begins wandering routine

2. **Movement Loop:**
   ```
   Pick random waypoint
   ↓
   Set Walk animation
   ↓
   Move towards waypoint (smooth)
   ↓
   Rotate towards direction
   ↓
   Reach waypoint
   ↓
   Set Idle animation
   ↓
   Pause (1-3 seconds random)
   ↓
   Repeat
   ```

3. **Animation States:**
   - **Walk**: When moving between waypoints
   - **Idle**: When paused at waypoint

4. **Movement:**
   - Smooth linear interpolation
   - Constant speed
   - Smooth rotation towards target
   - Stops when within threshold distance

---

## Visual Design

### Scene Composition:
- **Foreground**: UI elements (buttons, text)
- **Background**: 3D scene with chicken
- **Ground**: Simple plane for context
- **Lighting**: Directional light for depth

### Color Scheme:
- Ground: Default material (can be customized)
- Chicken: Quirky Series Farm chick model
- UI: Existing design (on top)

### Camera Framing:
- Shows entire movement area
- Chicken always visible
- Good perspective for character
- Matches game's visual style

---

## Configuration

### Adjusting Movement Speed:

**In Unity Editor:**
1. Open `HomeScene`
2. Select `HomeChicken` GameObject
3. Find `ChickenWanderer` component
4. Adjust parameters:
   - **Move Speed**: How fast chicken walks
   - **Min Pause Duration**: Shortest pause time
   - **Max Pause Duration**: Longest pause time
   - **Rotation Speed**: How fast chicken turns

**Recommended Settings:**

**Slow and Relaxed:**
- Move Speed: 1.5
- Min Pause: 2s
- Max Pause: 4s

**Default (Current):**
- Move Speed: 2.0
- Min Pause: 1s
- Max Pause: 3s

**Fast and Energetic:**
- Move Speed: 3.0
- Min Pause: 0.5s
- Max Pause: 2s

### Adding More Waypoints:

1. Create new empty GameObject
2. Position it within camera view
3. Parent it to `WaypointNodes`
4. Chicken will automatically include it

### Changing Camera Angle:

1. Select `Main Camera`
2. Adjust Position Y (height)
3. Adjust Position Z (distance)
4. Adjust Rotation X (angle)

**Angle Guidelines:**
- 45° - More top-down view
- 55° - Current (balanced)
- 65° - More perspective view

---

## Technical Details

### Auto-Waypoint Finding:
```csharp
GameObject waypointParent = GameObject.Find("WaypointNodes");
if (waypointParent != null)
{
    waypoints = new Transform[waypointParent.transform.childCount];
    for (int i = 0; i < waypointParent.transform.childCount; i++)
    {
        waypoints[i] = waypointParent.transform.GetChild(i);
    }
}
```

### Movement Algorithm:
```csharp
Vector3 direction = (currentTarget.position - transform.position).normalized;
transform.position += direction * moveSpeed * Time.deltaTime;

Quaternion targetRotation = Quaternion.LookRotation(direction);
transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
```

### Random Pause Duration:
```csharp
float pauseDuration = Random.Range(minPauseDuration, maxPauseDuration);
yield return new WaitForSeconds(pauseDuration);
```

---

## Performance

### Optimization:
- ✅ Single coroutine (no Update loop spam)
- ✅ Simple movement calculation
- ✅ Low-poly chicken model (LOD0)
- ✅ Minimal draw calls
- ✅ No physics (Transform-based movement)

### Memory:
- Minimal memory footprint
- No dynamic allocations during runtime
- Waypoint array cached at start

---

## Integration with Existing Systems

### Works With:
- ✅ **UI System** - Overlay canvas stays on top
- ✅ **Audio System** - Can add footstep sounds if desired
- ✅ **Scene Transition** - Chicken stops when scene changes
- ✅ **Settings** - No conflicts

### UI Transparency:
- Background image set to transparent
- UI elements remain fully visible
- 3D scene shows through as background

---

## Future Enhancements

### Potential Improvements:
1. **Sound Effects**: Add footstep sounds when walking
2. **Multiple Characters**: Add more animals wandering
3. **Particle Effects**: Add dust clouds when walking
4. **Interactive**: Chicken reacts when UI buttons are pressed
5. **Day/Night Cycle**: Change lighting over time
6. **Weather Effects**: Add rain, snow, etc.
7. **Pecking Animation**: Chicken pecks ground during idle
8. **Chirping Sounds**: Random chirp sounds during idle

### Additional Features:
- **Camera Zoom**: Slowly zoom in/out for dynamic feel
- **Camera Pan**: Slight camera movement following chicken
- **Background Music**: Ambient farm sounds
- **Seasonal Themes**: Different decorations per season
- **Unlockable Characters**: Different animals based on progress

---

## Troubleshooting

### Chicken Not Moving:

**Check:**
1. Waypoints are assigned (auto-finds "WaypointNodes")
2. ChickenWanderer component is enabled
3. CharacterAnimationController is on ChickModel
4. Animator has Idle_A and Walk animations

**Fix:**
- Ensure WaypointNodes parent exists
- Check console for warnings
- Verify animations are set up

### Chicken Goes Off-Screen:

**Fix:**
1. Adjust waypoint positions
2. Keep waypoints within camera view
3. Test in Game view, not Scene view

### Animations Not Playing:

**Check:**
1. CharacterAnimationController is on ChickModel child
2. Animator component exists
3. Animations are assigned in Animator Controller

**Fix:**
- Re-add CharacterAnimationController
- Check Chick_Animations.fbx is imported

### UI Not Visible:

**Check:**
1. Canvas render mode is "Screen Space - Overlay"
2. Canvas is above 3D objects in hierarchy
3. UI elements have proper sorting order

**Fix:**
- Ensure Canvas render mode is Overlay
- Check Canvas Scaler settings

---

## Testing Checklist

- [x] Chicken spawns at random waypoint
- [x] Chicken walks to waypoints
- [x] Walk animation plays when moving
- [x] Idle animation plays when paused
- [x] Chicken rotates towards movement direction
- [x] Pauses at each waypoint (1-3s random)
- [x] Picks random next waypoint
- [x] Stays within camera view
- [x] UI remains visible on top
- [x] Background is transparent
- [x] No compilation errors

---

## Code Quality

### Best Practices:
- ✅ Coroutine-based movement (clean)
- ✅ Auto-finds waypoints (no manual setup)
- ✅ Configurable via Inspector
- ✅ Null checks for safety
- ✅ XML documentation comments
- ✅ Gizmos for editor visualization

### Maintainability:
- Clear variable names
- Well-commented code
- Easy to extend
- Follows Unity conventions

---

## Related Files

### Scripts:
- `Assets/Scripts/ChickenWanderer.cs` - Main wandering behavior
- `Assets/Scripts/CharacterAnimationController.cs` - Animation control
- `Assets/Scripts/Editor/SetupHomeChicken.cs` - Editor utility (optional)

### Assets:
- `Assets/Quirky Series Ultimate/Quirky Series Vol.1/Farm Vol.1/Models/Chick_LOD0.fbx` - Chicken model
- `Assets/Quirky Series Ultimate/Quirky Series Vol.1/Farm Vol.1/Animations/Chick_Animations.fbx` - Animations

### Scenes:
- `Assets/Scenes/HomeScene.unity` - Contains chicken setup

### Documentation:
- `Assets/Docs/LexiRun_Implementation_Summary.md` - Overall project
- `Assets/Docs/Home_Scene_Chicken_Animation.md` - This document

---

## Summary

The HomeScene now features a lively animated chicken that wanders randomly between waypoints, making the scene feel more dynamic and playful. The implementation is:

- ✅ **Clean and simple** - Easy to understand and modify
- ✅ **Configurable** - Adjust speed, pause time, etc.
- ✅ **Performance-friendly** - Minimal overhead
- ✅ **Well-integrated** - Works with existing UI
- ✅ **Visually appealing** - Good camera angle and movement
- ✅ **Production-ready** - Tested and documented

The chicken adds personality to the home screen without distracting from the UI, creating a more engaging and less empty experience for players.

---

**Implementation Status:** Complete and tested  
**Last Updated:** November 5, 2025
