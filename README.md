COPY OF THE HOWTO TEXT FILE


How to use the TMPShader

1. Create new material
2. Use the TMPtxtrMul shader, located in CoolderShader > TMPtxtrMul in the shader drop down menu.
3. Copy your already existing font asset or create a new font asset.
4. Open the sub assets / Click the right arrow on the Font Asset, then drag and drop your material on the font material.

Params:

Map -> A texture used on each text sign/element. (Its applied by multiplying the pixel color of the text with the pixel color of the texture).

Tiling (X, Y) -> Tiles the input texture by X and/or Y.

Scaling -> Uniformly scale the texture in both X and Y axes.

Operation -> (Multiply, Additive, Subtractive, Replace).

Example material and font asset can be found in a folder located at Assets/TMPCompatibleShaders/EXAMPLE

<><
