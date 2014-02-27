using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.CodeRush.StructuralParser;
using DevExpress.Refactor;

namespace CR_IntroduceLocalAllArguments
{
    public partial class PlugIn1 : StandardPlugIn
    {
        // DXCore-generated code...
        #region InitializePlugIn
        public override void InitializePlugIn()
        {
            base.InitializePlugIn();
            registerIntroduceLocalAllArguments();
        }
        #endregion
        #region FinalizePlugIn
        public override void FinalizePlugIn()
        {
            //
            // TODO: Add your finalization code here.
            //

            base.FinalizePlugIn();
        }
        #endregion

        public void registerIntroduceLocalAllArguments()
        {
            DevExpress.Refactor.Core.RefactoringProvider IntroduceLocalAllArguments = new DevExpress.Refactor.Core.RefactoringProvider(components);
            ((System.ComponentModel.ISupportInitialize)(IntroduceLocalAllArguments)).BeginInit();
            IntroduceLocalAllArguments.ProviderName = "IntroduceLocalAllArguments"; // Should be Unique
            IntroduceLocalAllArguments.DisplayName = "Introduce Local (All Arguments)";
            IntroduceLocalAllArguments.CheckAvailability += IntroduceLocalAllArguments_CheckAvailability;
            IntroduceLocalAllArguments.Apply += IntroduceLocalAllArguments_Execute;
            ((System.ComponentModel.ISupportInitialize)(IntroduceLocalAllArguments)).EndInit();
        }
        private void IntroduceLocalAllArguments_CheckAvailability(Object sender, CheckContentAvailabilityEventArgs ea)
        {
            // Are we on a MethodReferenceExpression?
            LanguageElement element = CodeRush.Source.Active;
            MethodReferenceExpression MRE = element as MethodReferenceExpression;
            if (MRE == null)
                return;
            // Does Method have arguments?
            LanguageElement parent = MRE.Parent;
            ExpressionCollection arguments = getArguments(parent);
            if (arguments == null)
                return;
            ea.Available = true;
        }
        private void IntroduceLocalAllArguments_Execute(Object sender, ApplyContentEventArgs ea)
        {
            TextDocument ActiveDoc = CodeRush.Documents.ActiveTextDocument;
            using (ActiveDoc.NewCompoundAction("Introduce Local (All Arguments)"))
            {
                LanguageElement element = CodeRush.Source.Active;
                MethodReferenceExpression MRE = element as MethodReferenceExpression;
                LanguageElement parent = MRE.Parent;
                ExpressionCollection arguments = getArguments(parent);

                for (int i = 0; i < arguments.Count; i++)
                {
                    // Select expression
                    CodeRush.Selection.SelectRange(arguments[i].Range);
                    // Invoke refactoring to Introduce Local.
                    ExecuteRefactoring("Introduce Local");
                    ActiveDoc.SetText(GetSelectionRange(), "Param" + (i + 1));
                }
            }

        }
        private static SourceRange GetSelectionRange()
        {
            SourceRange SelectRange = SourceRange.Empty;
            CodeRush.Documents.ActiveTextDocument.GetSelection(out SelectRange);
            return SelectRange;
        }

        private static ExpressionCollection getArguments(LanguageElement parent)
        {
            ExpressionCollection arguments = null;
            if (parent.ElementType == LanguageElementType.MethodCall)
            {
                // do something
                arguments = (parent as MethodCall).Arguments;
            }
            else if (parent.ElementType == LanguageElementType.MethodCallExpression)
            {
                //do something else
                arguments = (parent as MethodCallExpression).Arguments;
            }
            return arguments;
        }
        private static void ExecuteRefactoring(string RefactoringName)
        {
            var Refactoring = CodeRush.Refactoring.Get(RefactoringName);
            CodeRush.SmartTags.UpdateContext();
            if (Refactoring.IsAvailable)
            {
                Refactoring.IsNestedProvider = true;
                Refactoring.Execute();
            }
        }
        //private string MyMethod(int Fred)
        //{
        //    MyMethod(42);
        //    string result = MyMethod(42);
        //    result = MyMethod(42);
        //}

    }
}