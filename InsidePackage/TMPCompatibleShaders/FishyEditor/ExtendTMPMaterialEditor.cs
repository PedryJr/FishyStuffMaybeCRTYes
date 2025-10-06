#if UNITY_EDITOR

using System.Text;
using TMPro.EditorUtilities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

//  /-------------------------------------------------------------------------------------------------------\
//  |                                                                                                       |
//  |  ><>       ><(((º>       ><(((º>       ><(((º>       <º)))><       <º)))><       <º)))><         <><  |
//  |  ><> Note from Pedry:                                                                            <><  |
//  |  ><> Yes. I Extracted the living bonkers out of the core logic into it's own functions.. Lol.    <><  |
//  |  ><> Idk, Inline the jigemajig yourself if ya need to read stuff :)                              <><  |
//  |  ><> Tho, IMO, The function names should label the action described well enough.                 <><  |
//  |  ><> Blub... .. .                                                                                <><  |
//  |  ><>       ><(((º>       ><(((º>       ><(((º>       <º)))><       <º)))><       <º)))><         <><  |
//  |                                                                                                       |
//  \-------------------------------------------------------------------------------------------------------/

public class ExtendTMPMaterialEditor : TMP_SDFShaderGUI // <><
{
    MaterialEditor feeesh; // <><
    MaterialProperty mulMap, tiling, scale, operation; // <><
    StringBuilder outputLog = new StringBuilder(); // <><

    public override void OnGUI(MaterialEditor matFeeesh, MaterialProperty[] props) // <><
    {

        GetEditor(matFeeesh); // <><
        GetProperties(props); // <><

        GenerateFishyLabel(); // <><
        if (!IsCompatibleEditor()) LogPossibleReasonForIncompatibiliy(); // <><
        else DisplayFishyProperties(); // <><

        base.OnGUI(matFeeesh, props); // <><
    }

    void DisplayFishyProperties() // <><
    {
        AssignMap(); // <><
        AssignMapTiling(); // <><
        AssignMapScale(); // <><
        AssignMapOp(); // <><

        EditorGUILayout.Space(30); // <><
    }

    void AssignMap() => feeesh.TexturePropertySingleLine(new GUIContent(mulMap.displayName), mulMap); // <><
    void AssignMapTiling() => tiling.vectorValue = GetVector2FieldAsFloat4(((float4)tiling.vectorValue).xy); // <><
    void AssignMapScale() => scale.floatValue = EditorGUILayout.FloatField("Scaling", scale.floatValue); // <><
    void AssignMapOp() => operation.floatValue = (float)(MapOperation)EditorGUILayout.EnumPopup("Color Operation", GetOperationField()); // <><

    MapOperation GetOperationField() => (MapOperation)Mathf.RoundToInt(operation.floatValue); // <><
    float4 GetVector2FieldAsFloat4(in float2 oldVal) => new float4(EditorGUILayout.Vector2Field("Tiling (X, Y)", oldVal), 0, 0); // <><
    void GetEditor(in MaterialEditor materialEditor) => feeesh = materialEditor; // <><
    void GetProperties(in MaterialProperty[] props) =>  // <><
        (mulMap, // <><
        tiling, // <><
        scale, // <><
        operation) =  // <><
        (GetMulMapProperty(props),  // <><
        GetTilingMulMapProperty(props), // <><
        GetScalingMulMapProperty(props), // <><
        GetOperationProperty(props)); // <><
    MaterialProperty GetMulMapProperty(in MaterialProperty[] props) => FindProperty("_Map", props, false); // <><
    MaterialProperty GetTilingMulMapProperty(in MaterialProperty[] props) => FindProperty("_MapTiling", props, false); // <><
    MaterialProperty GetScalingMulMapProperty(in MaterialProperty[] props) => FindProperty("_MapScale", props, false); // <><
    MaterialProperty GetOperationProperty(in MaterialProperty[] props) => FindProperty("_MapOperation", props, false); // <><
    void GenerateFishyLabel() => EditorGUILayout.LabelField("Fishy stuff..", EditorStyles.boldLabel); // <><
    bool IsCompatibleEditor() => mulMap != null && tiling != null && scale != null; // <><
    void LogPossibleReasonForIncompatibiliy() // <><
    {

        outputLog.Clear(); // <><
        IncompatibilityReason incompatibilityReason = 0; // <><

        if (tiling == null) incompatibilityReason = incompatibilityReason | IncompatibilityReason.NoMulMapTilingInShader; // <><
        if (mulMap == null) incompatibilityReason = incompatibilityReason | IncompatibilityReason.NoMulMapInShader; // <><
        if (scale == null) incompatibilityReason = incompatibilityReason | IncompatibilityReason.NoMulMapScalingInShader; // <><
        if (operation == null) incompatibilityReason = incompatibilityReason | IncompatibilityReason.NoMulMapOperationInShader; // <><

        if ((incompatibilityReason & IncompatibilityReason.NoMulMapInShader) != 0) outputLog.AppendLine(NoMulMapInShaderLog); // <><
        if ((incompatibilityReason & IncompatibilityReason.NoMulMapTilingInShader) != 0) outputLog.AppendLine(NoMulMapTilingInShaderLog); // <><
        if ((incompatibilityReason & IncompatibilityReason.NoMulMapScalingInShader) != 0) outputLog.AppendLine(NoMulMapScalingInShaderLog); // <><
        if ((incompatibilityReason & IncompatibilityReason.NoMulMapOperationInShader) != 0) outputLog.AppendLine(NoMulMapOperationInShaderLog); // <><

        Debug.Log(outputLog.ToString()); // <><
    }

    const string LogPropertyMissingPrefix = "The attatched shader is trying to use a custom editor, but does not use a "; // <><
    const string LogPropertyMissingSuffix = " property.\nThe custom editor is expecting this property!"; // <><

    const string NoMulMapInShaderLog = LogPropertyMissingPrefix + "Map" + LogPropertyMissingSuffix; // <><
    const string NoMulMapTilingInShaderLog = LogPropertyMissingPrefix + "MapTiling" + LogPropertyMissingSuffix; // <><
    const string NoMulMapScalingInShaderLog = LogPropertyMissingPrefix + "MapScale" + LogPropertyMissingSuffix; // <><
    const string NoMulMapOperationInShaderLog = LogPropertyMissingPrefix + "MapOperation" + LogPropertyMissingSuffix; // <><

    enum MapOperation : byte // <><
    { // <><
        Multiply = 1, // <><
        Additive = 2, // <><
        Subtractive = 3, // <><
        Replace = 4 // <><
    }

    enum IncompatibilityReason : byte // <><
    {
        None = 0, // <><
        NoMulMapInShader = 1 << 0, // <><
        NoMulMapTilingInShader = 1 << 1, // <><
        NoMulMapScalingInShader = 1 << 2, // <><
        NoMulMapOperationInShader = 1 << 3, // <><
    }
}
#endif