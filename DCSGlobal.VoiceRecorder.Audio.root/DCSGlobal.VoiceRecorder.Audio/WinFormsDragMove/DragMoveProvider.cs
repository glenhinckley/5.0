using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace WinFormsDragMove
{
    /// <summary>
    /// Provides an extra property for controls to enable or disable the DragMove behavior.
    /// </summary>
    [ProvideProperty("EnableDragMove", typeof(Control))]
    public partial class DragMoveProvider : Component, IExtenderProvider
    {
        /// <summary>
        /// Initializes a new instance of the DragMoveProvider class without a specified container.
        /// </summary>
        public DragMoveProvider()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the DragMoveProvider class with a specified container.
        /// <param name="container">An IContainer that represents the container of the DragMoveProvider.</param>
        /// </summary>
        public DragMoveProvider(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region IExtenderProvider Members

        /// <summary>
        /// Renvoie true si le DragMoveProvider peut fournir une propriété d'extension à l'objet cible spécifié.
        /// </summary>
        /// <param name="extendee">L'objet cible auquel ajouter une propriété d'extension.</param>
        /// <returns>true si le DragMoveProvider peut fournir une ou plusieurs propriété d'extensions ; sinon, false.</returns>
        public bool CanExtend(object extendee)
        {
            return (extendee is Control);
        }

        #endregion

        /// <summary>
        /// Retrieves a value indicating whether the DragMove behavior is enabled on the specified control.
        /// </summary>
        /// <param name="control">The Control for which to retrieve DragMove status</param>
        /// <returns>true if the DragMove behavior is enabled for this control ; otherwise, false.</returns>
        [DefaultValue(false)]
        public bool GetEnableDragMove(Control control)
        {
#if FX_20
            return DragMoveExtensions.IsDragMoveEnabled(control);
#else
            return control.IsDragMoveEnabled();
#endif
        }

        /// <summary>
        /// Enable or disables the DragMove behavior for the specified control.
        /// </summary>
        /// <param name="control">The control for which to enable or disable the DragMove behavior.</param>
        /// <param name="value">A value indicating if the DragMove behavior must be enabled or disabled for this control.</param>
        public void SetEnableDragMove(Control control, bool value)
        {
#if FX_20
            DragMoveExtensions.EnableDragMove(control, value);
#else
            control.EnableDragMove(value);
#endif
        }
    }
}
