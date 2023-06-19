using Spawners;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateSpawnPositionsEditor : EditorWindow
{
    private const string Path = "Assets/Settings/Spawn Positions/";
    private const string FileName = "SpawnPositions-";
    private const string FileExtension = ".asset";

    [MenuItem("Window/Custom/Generate Spawn Positions Editor")]
    public static void ShowEditor()
    {
        GenerateSpawnPositionsEditor wnd = GetWindow<GenerateSpawnPositionsEditor>();
        wnd.titleContent = new GUIContent("Generate Spawn Positions");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement label = new Label("Configure body properties");
        root.Add(label);

        var numberOfRawsField = new TextField("Number of raws:");
        root.Add(numberOfRawsField);
        
        var bodySizeField = new TextField("Body size:");
        root.Add(bodySizeField);
        
        var generateButton = new Button(() => 
            GenerateSpawnPositions(int.Parse(numberOfRawsField.value), float.Parse(bodySizeField.value)));
        generateButton.text = "Generate positions";
        root.Add(generateButton);
    }

    private void GenerateSpawnPositions(int numberOfRaws, float cubeSize)
    {
        var center = Vector3.zero;
        for (var j = 1; j <= numberOfRaws; j++)
        {
            var spawnPositions = CreateInstance<SpawnPositions>();
            var radius = cubeSize * j;
            var angle = Mathf.Atan2(cubeSize / 2, radius) * 2 * Mathf.Rad2Deg;
            var count = Mathf.RoundToInt(360 / angle);
            var positionsInRaw = new Vector3[count];
            for (var i = 0; i < count; i++)
            {
                angle = 360f / count * i;
                var circlePosition = GetPositionOnCircle(center, radius, angle);
                positionsInRaw[i] = circlePosition;
            }
            spawnPositions.Positions = positionsInRaw;
            AssetDatabase.CreateAsset(spawnPositions, $"{Path}{FileName}{j}{FileExtension}");
            AssetDatabase.SaveAssets();
        }
    }

    private Vector3 GetPositionOnCircle(Vector3 center, float radius, float ang)
    {
        Vector3 circlePosition;
        circlePosition.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        circlePosition.y = center.y;
        circlePosition.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return circlePosition;
    }
}