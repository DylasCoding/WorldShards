using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    private const string LAST_ID_KEY = "LastCharacterID";

    [MenuItem("Assets/GameConfiguration/CharConfig/Create/Character/Create New Character", priority = 1)]
    private static void CreateCharacterData()
    {
        // Tạo instance mới của CharacterData
        CharacterData characterData = ScriptableObject.CreateInstance<CharacterData>();

        // Lấy ID cuối cùng từ EditorPrefs, nếu không có thì bắt đầu từ 1
        int lastID = EditorPrefs.GetInt(LAST_ID_KEY, 6);
        int newID = lastID + 1;

        // Gán ID mới cho characterData
        characterData.characterID = newID;

        // Lưu ID mới vào EditorPrefs
        EditorPrefs.SetInt(LAST_ID_KEY, newID);

        // Lưu ScriptableObject vào thư mục Assets
        string path = "Assets/CharConfig_" + newID + ".asset";
        AssetDatabase.CreateAsset(characterData, path);
        AssetDatabase.SaveAssets();

        // Focus vào asset vừa tạo
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = characterData;
    }

    public override void OnInspectorGUI()
    {
        // Hiển thị các field mặc định trong Inspector
        base.OnInspectorGUI();

        CharacterData character = (CharacterData)target;

        // Optional: Có thể thêm logic kiểm tra hoặc chỉnh sửa ID trong Inspector
        EditorGUILayout.LabelField("Character ID (Auto-Generated)", character.characterID.ToString());
    }
}