<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmBrowse
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
	Public WithEvents _cmdBrowse_1 As System.Windows.Forms.Button
	Public WithEvents _cmdBrowse_0 As System.Windows.Forms.Button
	Public WithEvents drvList As Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
	Public WithEvents dirList As Microsoft.VisualBasic.Compatibility.VB6.DirListBox
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents cmdBrowse As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._cmdBrowse_1 = New System.Windows.Forms.Button
        Me._cmdBrowse_0 = New System.Windows.Forms.Button
        Me.drvList = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
        Me.dirList = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdBrowse = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(Me.components)
        CType(Me.cmdBrowse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_cmdBrowse_1
        '
        Me._cmdBrowse_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdBrowse_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdBrowse_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdBrowse_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowse.SetIndex(Me._cmdBrowse_1, CType(1, Short))
        Me._cmdBrowse_1.Location = New System.Drawing.Point(200, 56)
        Me._cmdBrowse_1.Name = "_cmdBrowse_1"
        Me._cmdBrowse_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdBrowse_1.Size = New System.Drawing.Size(73, 25)
        Me._cmdBrowse_1.TabIndex = 3
        Me._cmdBrowse_1.Text = "&Cancel"
        Me._cmdBrowse_1.UseVisualStyleBackColor = False
        '
        '_cmdBrowse_0
        '
        Me._cmdBrowse_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdBrowse_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdBrowse_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdBrowse_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowse.SetIndex(Me._cmdBrowse_0, CType(0, Short))
        Me._cmdBrowse_0.Location = New System.Drawing.Point(200, 24)
        Me._cmdBrowse_0.Name = "_cmdBrowse_0"
        Me._cmdBrowse_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdBrowse_0.Size = New System.Drawing.Size(73, 25)
        Me._cmdBrowse_0.TabIndex = 2
        Me._cmdBrowse_0.Text = "&OK"
        Me._cmdBrowse_0.UseVisualStyleBackColor = False
        '
        'drvList
        '
        Me.drvList.BackColor = System.Drawing.SystemColors.Window
        Me.drvList.Cursor = System.Windows.Forms.Cursors.Default
        Me.drvList.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.drvList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.drvList.FormattingEnabled = True
        Me.drvList.Location = New System.Drawing.Point(16, 24)
        Me.drvList.Name = "drvList"
        Me.drvList.Size = New System.Drawing.Size(169, 21)
        Me.drvList.TabIndex = 1
        '
        'dirList
        '
        Me.dirList.BackColor = System.Drawing.SystemColors.Window
        Me.dirList.Cursor = System.Windows.Forms.Cursors.Default
        Me.dirList.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dirList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.dirList.FormattingEnabled = True
        Me.dirList.IntegralHeight = False
        Me.dirList.Location = New System.Drawing.Point(16, 72)
        Me.dirList.Name = "dirList"
        Me.dirList.Size = New System.Drawing.Size(169, 141)
        Me.dirList.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(16, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(105, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Folder"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(41, 17)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Drive"
        '
        'cmdBrowse
        '
        '
        'frmBrowse
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(286, 217)
        Me.Controls.Add(Me._cmdBrowse_1)
        Me.Controls.Add(Me._cmdBrowse_0)
        Me.Controls.Add(Me.drvList)
        Me.Controls.Add(Me.dirList)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBrowse"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select Folder"
        CType(Me.cmdBrowse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class