<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmMain
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents _cmdDone_1 As System.Windows.Forms.Button
	Public WithEvents _cmdDone_0 As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents cmdDone As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me._cmdDone_1 = New System.Windows.Forms.Button
		Me._cmdDone_0 = New System.Windows.Forms.Button
		Me.Label1 = New System.Windows.Forms.Label
		Me.cmdDone = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(components)
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.cmdDone, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Text = "Psyquel - Electronic Claims"
		Me.ClientSize = New System.Drawing.Size(348, 327)
		Me.Location = New System.Drawing.Point(4, 23)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmMain"
		Me._cmdDone_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cmdDone_1.Text = "&Cancel"
		Me._cmdDone_1.Size = New System.Drawing.Size(76, 28)
		Me._cmdDone_1.Location = New System.Drawing.Point(228, 288)
		Me._cmdDone_1.TabIndex = 1
		Me._cmdDone_1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._cmdDone_1.BackColor = System.Drawing.SystemColors.Control
		Me._cmdDone_1.CausesValidation = True
		Me._cmdDone_1.Enabled = True
		Me._cmdDone_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cmdDone_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._cmdDone_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cmdDone_1.TabStop = True
		Me._cmdDone_1.Name = "_cmdDone_1"
		Me._cmdDone_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me._cmdDone_0.Text = "&OK"
		Me._cmdDone_0.Size = New System.Drawing.Size(76, 28)
		Me._cmdDone_0.Location = New System.Drawing.Point(44, 288)
		Me._cmdDone_0.TabIndex = 0
		Me._cmdDone_0.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._cmdDone_0.BackColor = System.Drawing.SystemColors.Control
		Me._cmdDone_0.CausesValidation = True
		Me._cmdDone_0.Enabled = True
		Me._cmdDone_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._cmdDone_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._cmdDone_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cmdDone_0.TabStop = True
		Me._cmdDone_0.Name = "_cmdDone_0"
		Me.Label1.Text = "X.12 837 v 4010"
		Me.Label1.Size = New System.Drawing.Size(233, 137)
		Me.Label1.Location = New System.Drawing.Point(48, 56)
		Me.Label1.TabIndex = 2
		Me.Label1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.Enabled = True
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		Me.Label1.AutoSize = False
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Name = "Label1"
		Me.Controls.Add(_cmdDone_1)
		Me.Controls.Add(_cmdDone_0)
		Me.Controls.Add(Label1)
		Me.cmdDone.SetIndex(_cmdDone_1, CType(1, Short))
		Me.cmdDone.SetIndex(_cmdDone_0, CType(0, Short))
		CType(Me.cmdDone, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class