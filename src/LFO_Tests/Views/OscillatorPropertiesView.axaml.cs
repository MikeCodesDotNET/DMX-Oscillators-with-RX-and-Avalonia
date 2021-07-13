using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LFO_Tests.Views
{
    public class OscillatorPropertiesView : UserControl
    {
        public OscillatorPropertiesView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
