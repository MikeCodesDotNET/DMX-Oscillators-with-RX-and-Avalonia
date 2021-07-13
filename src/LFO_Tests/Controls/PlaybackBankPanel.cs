using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using LFO_Tests.Controls;
using LFO_Tests.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFO_Tests
{
    public class PlaybackBankPanel : SelectingItemsControl
    {

        public static readonly StyledProperty<int> IdProperty = AvaloniaProperty.Register<PlaybackBankPanel, int>(nameof(Id));


        /// <summary>
        /// The default value for the <see cref="ItemsControl.ItemsPanel"/> property.
        /// </summary>
        private static readonly FuncTemplate<IPanel> DefaultPanel = new FuncTemplate<IPanel>(() => new WrapPanel());

        /// <summary>
        /// Defines the <see cref="SelectedItems"/> property.
        /// </summary>
        public static readonly new DirectProperty<SelectingItemsControl, IList> SelectedItemsProperty = SelectingItemsControl.SelectedItemsProperty;


        /// <summary>
        /// Defines the <see cref="Selection"/> property.
        /// </summary>
        public static readonly new DirectProperty<SelectingItemsControl, ISelectionModel> SelectionProperty = SelectingItemsControl.SelectionProperty;


        /// <summary>
        /// Defines the <see cref="SelectionMode"/> property.
        /// </summary>
        public static readonly new StyledProperty<SelectionMode> SelectionModeProperty = SelectingItemsControl.SelectionModeProperty;

        static PlaybackBankPanel()
        {
            ItemsPanelProperty.OverrideDefaultValue<PlaybackBankPanel>(DefaultPanel);
        }

        public new IList SelectedItems
        {
            get => base.SelectedItems;
            set => base.SelectedItems = value;
        }

        public new ISelectionModel Selection
        {
            get => base.Selection;
            set => base.Selection = value;
        }

        public new SelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }

        public int Id
        {
            get { return GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public void SelectAll() => Selection.SelectAll();

        public void DeselectAll() => Selection.Clear();

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator<PlaybackFader>(this, DataContextProperty, ItemTemplateProperty);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.Source is IVisual source)
            {
                var point = e.GetCurrentPoint(source);

                if (point.Properties.IsLeftButtonPressed || point.Properties.IsRightButtonPressed)
                {
                    e.Handled = UpdateSelectionFromEventSource(
                        e.Source,
                        true,
                        e.KeyModifiers.HasFlagCustom(KeyModifiers.Shift),
                        e.KeyModifiers.HasFlagCustom(KeyModifiers.Control),
                        point.Properties.IsRightButtonPressed);
                }
            }
        }


    }
}
