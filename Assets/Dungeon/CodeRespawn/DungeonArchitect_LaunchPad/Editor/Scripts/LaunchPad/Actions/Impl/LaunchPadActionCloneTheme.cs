//$ Copyright 2015-21, Code Respawn Technologies Pvt Ltd - All Rights Reserved $//
using UnityEngine;

namespace DungeonArchitect.Editors.LaunchPad.Actions.Impl
{
    public class LaunchPadActionCloneTheme : LaunchPadActionBase
    {
        string templatePath;
        bool resourcePath;
        public LaunchPadActionCloneTheme(string templatePath, bool resourcePath)
        {
            this.templatePath = templatePath;
            this.resourcePath = resourcePath;
        }

        public override Texture2D GetIcon()
        {
            return ScreenPageLoader.LoadImageAsset("icons/font_awesome/icon_theme_new");
        }

        public override string GetText()
        {
            return "Clone Theme";
        }

        public override void Execute()
        {

            string targetPath;
            if (CloneAsset(templatePath, resourcePath, out targetPath, "Create a Theme from Template"))
            {
                PingAsset(targetPath);
                new LaunchPadActionOpenTheme(targetPath).Execute();
            }
        }
    }
}