using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;
using static Unity.VectorGraphics.VectorUtils;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VectorShapeController : MonoBehaviour
{
    private readonly Dictionary<int, BezierContour> contours = new Dictionary<int, BezierContour>();
    public IFill Fill
    {
        set
        {
            shape.Fill = value;
            SetGeometryDirty();
        }
        get => shape.Fill;
    }

    private readonly Shape shape = new Shape
    {
        Contours = new BezierContour[1],
        Fill = new SolidFill
        {
            Mode = FillMode.NonZero,
            Color = Color.magenta
        }
    };

    private Scene scene;
    private Material solidFillMaterial;
    protected TessellationOptions options = new TessellationOptions()
    {
        StepDistance = 1000.0f,
        MaxCordDeviation = 0.05f,
        MaxTanAngleDeviation = 0.05f,
        SamplingStepSize = 0.01f
    };

    private List<Geometry> geoms;

    private bool isFillDrity = false;
    private bool isGeometryDirty = false;

    void Awake()
    {
        scene = new Scene()
        {
            Root = new SceneNode() { Shapes = new List<Shape> { shape } }
        };
        solidFillMaterial = new Material(UnityEditor.AssetDatabase.LoadAssetAtPath("Packages/com.unity.vectorgraphics/Runtime/Shaders/VectorGradient.shader", typeof(Shader)) as Shader);
        GetComponent<MeshRenderer>().material = solidFillMaterial;
    }

    void LateUpdate()
    {
        if (isFillDrity || isGeometryDirty)
        {
            BuildGeoms();
        }
        if (isFillDrity)
        {
            FillMeshMaterial();
        }
        if (isGeometryDirty)
        {
            FillMeshGeometry();
        }
        isFillDrity = false;
        isGeometryDirty = false;
    }

    private void BuildGeoms()
    {
        geoms = TessellateScene(scene, options);
    }

    private void FillMeshMaterial()
    {
        if (shape.Fill is GradientFill)
        {
            
                GetComponent<MeshRenderer>().material.mainTexture = GenerateAtlasAndFillUVs(geoms, 128).Texture;

            
        }
        if (shape.Fill is SolidFill)
        {
            GetComponent<MeshRenderer>().material = solidFillMaterial;
        }
    }

    private void FillMeshGeometry()
    {
        FillMesh(GetComponent<MeshFilter>().mesh, geoms, 50);
    }

    private void UpdateContoursInShape()
    {
        shape.Contours = (new List<BezierContour>(contours.Values)).ToArray();
        SetGeometryDirty();
    }

    public void SetContour(int id, BezierContour contour)
    {
        contours[id] = contour;
        UpdateContoursInShape();
    }

    public void UnsetContour(int id)
    {
        contours.Remove(id);
        UpdateContoursInShape();
    }

    public void SetFillDirty()
    {
        isFillDrity = true;
    }

    public void SetGeometryDirty()
    {
        isGeometryDirty = true;
        SetFillDirty();
    }
}
