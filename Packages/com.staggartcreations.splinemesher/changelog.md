1.2.2

Added:
- Support for Bakery, lightmap UVs are now generated when Bakery starts.
- Spline Change Mode parameter, allows delaying the mesh generation until after the spline changes are done
- Parameters to control the lightmap UV packing margin and angle threshold

Changed:
- Improved readability of the Roll tool data point values in the scene view

Fixed:
- Issues with UI drawing if a Spline Mesher component is part of a prefab
- Mesh tangents not being calculated correctly, resulting in inconsistent normal mapping

1.2.1

Added:
- Demo scene, new examples:
 * Bunting
 * Rope
 * Prayer flags
- Normalized gradients are now stored in the first UV's Z and W components, to be used in shader effects.
- Option to keep the generated mesh readable from script

Fixed:
- Component and tool icons not being found on Asset Store version
- Vertex colors of source mesh not being correctly retained over the vertical length of the mesh
- Vertex color visualization not being visible on backfaces
- Mesh rebuilding when the Spline was moved, even if the "On Spline Changed" rebuild trigger was not set

Changed:
- The Z and W components of the source mesh's UV is now also retained.

1.2.0
IMPORTANT: Delete the Spline Mesher folder before importing this update! It now installs as a package
Failing to do this results in all new content importing elsewhere

Added:
- Caps feature, spawns a prefab to the start or end of each spline
- Roll deformation feature: frequency and angle parameters. Plus spline tool to add angle data points.
- Vertex color data point functionality
- UV scale & offset parameters
- Added a "Collider Only" option, which skips the visual mesh creation.
- Rebuilder Triggers: Added "On UI Change" event (not upgradable, will be disabled when updating)
- Preferences UI, options to enable smoother rebuilding and automatic lightmap UVs
- Gear menu: action to rebuild all instances using the same mesh (in case of mesh changes)
- Index unit for Scale/Roll/Color data can now be set (Distance/Normalize/Knot).

Fixed:
- Trim start/end functionality not being strictly accurate

Changed:
- Settings for collision have moved, allowing expansion, and may need to be reconfigured
- Segment count now be manually specified
- Redesigned scene toolbar icons
- Warnings in the inspector if the Output object has no Mesh- filter or renderer. No longer auto-added.
- Inspector UI, option to add a new Spline Container if none is assigned

Removed:
- Deprecated the CreateSplineFromPoints function. The native SplineUtility.FitSplineToPoints function should now be used.
- Automatic assigning of output object (when updating from 1.0.0 to 1.1.0+)

1.1.3

Fixed:
- Script compile errors in Unity 2021.3 (unsupported)

1.1.2

Added:
- Scale tool, option to use uniforming scale
- Support for lightmapping for generated spline meshes. 
  * When baking starts, Spline Mesher objects that are marked as static will have lightmap UVs generated for them if needed.
- Help window with version update notification and links to documentation & support.

Fixed:
- Mesh Filter component "Convert to Spline" context menu option unintentionally adding an empty spline to the new Spline Container.

1.1.1

Added:
- Rebuild Trigger flags, sets which sort of events cause the mesh to be regenerated.
- Public C# function to create a spline from an array of positions (CreateSplineFromPoints).
- Added a context menu option to the Spline Container component to quickly set up a default Spline Mesher.
- Option under GameObject/3D Object to create a new spline mesh.

Changed:
- QOL: Whenever the component needs to adds MeshFilter to the output object, it will also add a Mesh Renderer component if needed.
- Extra failsafes when importing into versions older than Unity 2022.3 without the Splines package installed.
- Context menu functions have been moved to a new Gear button in the inspector
- Sections in the inspector now behave as an accordion, expanding one closes all others.

Removed:
- Deprecated the "Rebuild on Start()" option (replaced by Rebuild Triggers flags).

1.1.0

Added:
- Demo scene, text description to each object explaining what is being achieved.
- Documentation now has a "Scripting" section, with example code and scripts.
- Inspector UI enhancements.
- Option to regenerate the mesh on Start(), required when prefabbing procedural meshes.
- Support for more than 65.535 vertices.
- A "SampleScale" C# function to the SplineMesher component.

Changed:
- Component no longer targets a Mesh Filter reference directly, now an output GameObject may be specified (automatically upgrades).
- Improved mesh generation performance by 120-300%.
- C#, static rebuilding events are now regular 'Action' delegates.
- Scale tool now validates data points and resets them if a scale of 0 was found.
- Scale tool now supports multi-selection editing.
- A Spline Mesher component no longer requires 2 default scale data points. If no scale data is found, a default scale is assumed.

Fixed:
- Issue with geometry not generating for multiple splines if it uses more than 1 material.
- Vertex normals not being accurately rotated if the deformation scale was negative.
- Vertices at the very start/end of the spline possibly being deformed.
- Using a Box-shape collider with +1 subdivisions no longer shifts the collider when using the "Spacing" parameter.
- Mesh rotation not taking effect for a custom collider input mesh.

1.0.0
Initial release