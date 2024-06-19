//$ Copyright 2015-21, Code Respawn Technologies Pvt Ltd - All Rights Reserved $//
using DungeonArchitect.Editors.Flow.DomainEditors.Layout3D;
using DungeonArchitect.Flow.Domains;
using DungeonArchitect.Flow.Exec;
using DungeonArchitect.Flow.Impl.SnapGridFlow;
using DungeonArchitect.UI.Widgets;
using UnityEditor;

namespace DungeonArchitect.Editors.Flow.Impl
{
    public class SnapGridFlowEditorWindow : FlowEditorWindow
    {
        private Layout3DGraphDomainEditor layoutDomainEditor;

        protected override string WindowTitle { get; } = "Snap Grid Flow Editor";
        protected override IWidget DomainLayoutWidget { get; set; }
        protected override void InitDomains()
        {
            layoutDomainEditor = CreateInstance<Layout3DGraphDomainEditor>();
            layoutDomainEditor.Init(new SnapGridFlowLayoutGraph3DDomain(), editorConfig, uiSystem);
            RegisterDomainEditor(layoutDomainEditor);

            DomainLayoutWidget = layoutDomainEditor.Content;
        }
        
        protected override FlowEditorConfig CreateEditorConfig()
        {
            return CreateInstance<SnapGridFlowEditorConfig>();
        }

        protected override FlowExecNodeOutputRegistry GetLinkedDungeonNodeOutputRegistry()
        {
            return null;
        }

        protected override bool IsDomainStateInvalid()
        {
            return layoutDomainEditor == null || !layoutDomainEditor.IsInitialized();
        }
        
        protected override void AddDomainExtenders(FlowDomainExtensions domainExtensions)
        {
            var config = editorConfig as SnapGridFlowEditorConfig;
            
            var extension = domainExtensions.GetExtension<SnapGridFlowDomainExtension>();
            extension.ModuleDatabase = (config != null) ? config.moduleDatabase : null;
        }

        public void SetModuleDatabase(SnapGridFlowModuleDatabase moduleDatabase)
        {
            var config = editorConfig as SnapGridFlowEditorConfig;
            if (config != null)
            {
                config.moduleDatabase = moduleDatabase;
            }
        }
        
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<SnapGridFlowEditorWindow>();
            window.Init(null);
        }
        
    }
}