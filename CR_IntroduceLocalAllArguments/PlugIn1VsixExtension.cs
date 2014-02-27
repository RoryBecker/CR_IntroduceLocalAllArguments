using System.ComponentModel.Composition;
using DevExpress.CodeRush.Common;

namespace CR_IntroduceLocalAllArguments
{
    [Export(typeof(IVsixPluginExtension))]
    public class CR_IntroduceLocalAllArgumentsExtension : IVsixPluginExtension { }
}