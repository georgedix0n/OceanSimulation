# Ocean Simulation Using Phillips Spectrum and Fourier Transform

This project is a GPU-based ocean surface simulation that leverages Jerry Tessendorf's research on ocean waves and the Phillips Spectrum. It provides realistic ocean wave generation and dynamics through a combination of statistical distributions and GPU-computed Fourier transforms.

## Project Overview

  -  The entry point to the application is an `OceanGenerator.cs` class.
  -  It generates a plane mesh with `MeshGenerator.cs` and assigns a material with an associated surface shader.
  -  Using the GPU with `Resources/Shaders/ButterflyShader.compute` on play the twiddle factors for the fft are stored in a 2D texture.
  -  Also on the GPU on play, using the phillips spectrum and a box-muller random texture generator, fourier amplitudes, derivatives and a Jacobian texture are generated with `FourierAmplitudesShader.compute`.
  -  Each texture is then fft'd/permuted on the gpu with `fft.compute`/`permute.compute`, to give the associated height map and normals etc. for the surface shader.

## Setup & Installation
1. Clone this repository:
   ```bash
   git clone https://github.com/georgedix0n/SurfaceTest.git   
2. Open project with Unity Editor
3. Alter values on `OceanGenerator.cs`
4. Hit Play

## Improvements
- The surface shader is incomplete, so the ocean surface colouring does not look realistic, though the wave shapes/ generated textures etc. are realistic.
- Intending to add foam.
- The compute shaders are certainly faster than a CPU implementation and are relatively efficient. Though more work can be done to make them optimal, taking into account specific architectures, warp sizes, memory coalescing and latency hiding.
- Slight problem with the fft algo, requires further debugging but doesn't affect visuals greatly.
