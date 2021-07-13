using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFO_Tests.Controls
{
    [PseudoClasses(":pressed", ":selected")]
    public class PlaybackFader : TemplatedControl, ISelectable
    {
        public static readonly StyledProperty<bool> IsSelectedProperty =
                    AvaloniaProperty.Register<PlaybackFader, bool>(nameof(IsSelected));


        static PlaybackFader()
        {
            SelectableMixin.Attach<PlaybackFader>(IsSelectedProperty);
            PressedMixin.Attach<PlaybackFader>();
            FocusableProperty.OverrideDefaultValue<PlaybackFader>(true);
        }

        public bool IsSelected
        {
            get { return GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
    }
}
