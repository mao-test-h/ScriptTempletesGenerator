using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
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
        #region // 各種定義・宣言

        /// <summary>
        /// テンプレートパス
        /// </summary>
        const string TemplatesPath = "ScriptTemplatesGenerator/Templates/";

        /// <summary>
        /// 共通メニューパス
        /// </summary>
        const string CommonMenuRoot = "Assets/Create/";

        #endregion // 各種定義・宣言

        // ==============================================
        #region // MenuItem

        /// <summary>
        /// C# Script生成処理
        /// </summary>
        [MenuItem(ScriptTemplatesGenerator.CommonMenuRoot + "C# Script(Original)", false, 64)]
        public static void CreateNewBehaviourScript()
        {
            var resFile = Path.Combine(Application.dataPath, ScriptTemplatesGenerator.TemplatesPath + "/Script-NewBehaviourScript.cs.txt");
            CreateScript(resFile);
        }

        #endregion // MenuItem

        // ==============================================
        #region // override

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
            text = text.Replace("#SCRIPTNAME#", className);

            // UTF8(BOM付き)
            var encoding = new UTF8Encoding(true, false);
            File.WriteAllText(pathName, text, encoding);
            AssetDatabase.ImportAsset(pathName);
            var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }

        #endregion // override

        // ==============================================
        #region // private method

        /// <summary>
        /// ファイル生成処理
        /// </summary>
        /// <param name="resFile">リソース情報</param>
        static void CreateScript(string resFile)
        {
            Texture2D csicon = EditorGUIUtility.IconContent("cs Script icon").image as Texture2D;
            var endNameEditAction = ScriptableObject.CreateInstance<ScriptTemplatesGenerator>();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditAction, "NewCSharpScript.cs", csicon, resFile);
        }

        #endregion // private method
    }
}
