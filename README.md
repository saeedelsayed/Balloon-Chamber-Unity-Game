# Holographic Balloon Chamber
## A Stereoscopic 3D game built with Unity.
Put on your 3D glasses and control a floating spike to pop balloons in a 3D chamber.

## About The Project
It combines fundamental VR rendering techniques with spatial audio to create an immersive experience on a standard monitor.
The game renders the scene in Anaglyph 3D (Red/Cyan), allowing players with standard 3D glasses to perceive depth while playing.

## Key Features

### 1. Visual Immersion

  - Anaglyph 3D Rendering: The scene is rendered from two distinct perspectives (Left and Right eyes) and merged into a single Red/Cyan image.

  - Stereo Camera Rig: Implements an adjustable Inter-pupillary Distance (IPD) to control the depth intensity.

  - Toe-in Projection: Cameras rotate inward to focus on a specific convergence point, mimicking the natural focus of human eyes

### 2. Interaction & Physics

  - Physics-Based Gameplay: Players control a floating "Spike" using forces and rigidbodies.

  - Dynamic Spawning: Balloons follow a procedural spawning logic and float upward using negative gravity.

  - Collision Detection: Interactive destruction mechanics (balloons "pop" upon contact).

### 3. Spatial Audio

  - Binaural Audio Simulation: Implements custom spatial audio processing on the balloon sound sources.

  - ILD (Interaural Level Difference): Adjusts volume balance (Gain) based on the angle of the sound source relative to the player.

  - ITD (Interaural Time Difference): Adds micro-delays to the audio channels to simulate sound travel time to the ears.

### 4. Control

  - User can move the spive forward, backward, left and right using the characters WSAD.

## Technical Implementation

### stereo Rendering
The 3D effect is achieved by using two cameras parented to a main rig.

  1. **Separation:** The cameras are offset by the IPD (0.064m).

  2. **Rendering:** Each camera renders to a separate `RenderTexture`.

  3. **Compositing:** A custom Shader Graph merges the two textures, filtering the Left Eye to Red and the Right Eye to Cyan.

### Spatial Audio Math

Instead of using Unity's built-in spatializer, custom scripts calculate the audio properties:

  - Delay Calculation (dn​): The delay in samples is calculated using the formula dn​ = dt​ × f, where dt​ is the delay in seconds and f is the sample rate.

## Project Structure

   - `Scripts/`

      -  `StereoController.cs`: Handles Left/Right camera separation and Toe-in rotation.

      -  `AudioController.cs`: Handles ILD/ITD spatial sound calculations.

      -  `Spike.cs`: Character controller logic and collision handling.

      -  `BalloonSpawner.cs`: Manages procedural generation of balloons.

   - `Shaders/`

      -  `AnaglyphShader`: Merges Left/Right textures for the 3D glasses effect.
