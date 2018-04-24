using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace ScriptTemplatesGenerator
{
    /// <summary>
    /// スクリプトテンプレート生成用拡張
    /// </summary>
    public class ScriptTemplatesGenerator : EndNameEditAction
    {
        // --------------------------------------------
        #region // Constants

        /// <summary>
        /// テンプレートパス
        /// </summary>
        const string TemplatesPath = "ScriptTemplatesGenerator/Templates/";

        /// <summary>
        /// 共通メニューパス
        /// </summary>
        const string CommonMenuRoot = "Assets/Create/";

        #endregion // Constants

        // ==============================================
        #region // MenuItem

        /// <summary>
        /// C# Scriptテンプレート生成処理
        /// </summary>
        [MenuItem(ScriptTemplatesGenerator.CommonMenuRoot + "C# Script(Original)", false, 64)]
        public static void CreateNewBehaviourScript()
        {
            var resFile = Path.Combine(Application.dataPath, ScriptTemplatesGenerator.TemplatesPath + "/Script-NewBehaviourScript.cs.txt");
            Texture2D icon = EditorGUIUtility.IconContent("cs Script icon").image as Texture2D;
            CreateFile(resFile, icon, "NewBehaviourScript.cs");
        }

        /// <summary>
        /// UnlitShaderテンプレート生成処理
        /// </summary>
        [MenuItem(ScriptTemplatesGenerator.CommonMenuRoot + "New Unlit Shader", false, 65)]
        public static void CreateNewSampleShader()
        {
            var resFile = Path.Combine(Application.dataPath, ScriptTemplatesGenerator.TemplatesPath + "/Shader-NewUnlitShader.shader.txt");
            Texture2D icon = EditorGUIUtility.IconContent("Shader Icon").image as Texture2D;
            CreateFile(resFile, icon, "NewUnlitShader.shader");
        }

        /// <summary>
        /// cgincテンプレート生成処理
        /// </summary>
        [MenuItem(ScriptTemplatesGenerator.CommonMenuRoot + "New cginc", false, 66)]
        public static void CreateNewCgInc()
        {
            var resFile = Path.Combine(Application.dataPath, ScriptTemplatesGenerator.TemplatesPath + "/Shader-NewCginc.cginc.txt");
            Texture2D icon = EditorGUIUtility.IconContent("Shader Icon").image as Texture2D;
            CreateFile(resFile, icon, "NewCginc.cginc");
        }

        #endregion // MenuItem

        // ==============================================
        #region // Override

        /// <summary>
        /// 生成処理
        /// </summary>
        /// <param name="instanceId">インスタンスID</param>
        /// <param name="pathName">パス名</param>
        /// <param name="resourceFile">ファイル名</param>
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);
            var className = Path.GetFileNameWithoutExtension(pathName);

            // スペースを削除
            className = className.Replace(" ", "");
            // ファイル名から拡張子を抜いた物をクラス名に設定
            text = text.Replace("#NAME#", className);

            // UTF8(BOM付き)
            var encoding = new UTF8Encoding(true, false);
            File.WriteAllText(pathName, text, encoding);
            AssetDatabase.ImportAsset(pathName);

            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathName);
            ProjectWindowUtil.ShowCreatedAsset (asset);
        }

        #endregion // Override

        // ==============================================
        #region // Private Methods

        /// <summary>
        /// ファイル生成処理
        /// </summary>
        /// <param name="resFile">リソース情報</param>
        /// <param name="iconTexture">アイコンのTexture</param>
        /// <param name="createFileName">生成するファイル名(拡張子も含めること)</param>
        static void CreateFile(string resFile, Texture2D iconTexture, string createFileName)
        {
            var endNameEditAction = ScriptableObject.CreateInstance<ScriptTemplatesGenerator>();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditAction, createFileName, iconTexture, resFile);
        }

        #endregion // Private Methods
    }
}
