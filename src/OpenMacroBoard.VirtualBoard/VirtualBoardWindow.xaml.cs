using System;
using System.Windows;

namespace OpenMacroBoard.VirtualBoard
{
    /// <summary>
    /// Interaction logic for VirtualBoardWindow.xaml
    /// </summary>
    public partial class VirtualBoardWindow : Window
    {
        /// <summary>
        /// Creates an instance of <see cref="VirtualBoardWindow"/>.
        /// </summary>
        /// <param name="viewModel"></param>
        internal VirtualBoardWindow(VirtualBoardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        /// <summary>
        /// Is used to hide title bar icon
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>
        /// Thanks to Sheridan (https://stackoverflow.com/questions/18580430/hide-the-icon-from-a-wpf-window)
        /// </remarks>
        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
    }
}
