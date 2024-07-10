<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEDIWizard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdDone = New System.Windows.Forms.Button
        Me.chkTest = New System.Windows.Forms.CheckBox
        Me.barStatus = New System.Windows.Forms.ProgressBar
        Me.txtStatus = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(174, 92)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(248, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "ANSI X.12 837 v 5010 Electronic Claim Submission"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(174, 139)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(137, 13)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = "starus something or another"
        '
        'cmdOk
        '
        Me.cmdOk.Location = New System.Drawing.Point(305, 285)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 4
        Me.cmdOk.Text = "Ok"
        Me.cmdOk.UseVisualStyleBackColor = True
        '
        'cmdDone
        '
        Me.cmdDone.Location = New System.Drawing.Point(563, 296)
        Me.cmdDone.Name = "cmdDone"
        Me.cmdDone.Size = New System.Drawing.Size(75, 23)
        Me.cmdDone.TabIndex = 5
        Me.cmdDone.Text = "Cancel"
        Me.cmdDone.UseVisualStyleBackColor = True
        '
        'chkTest
        '
        Me.chkTest.AutoSize = True
        Me.chkTest.Location = New System.Drawing.Point(522, 25)
        Me.chkTest.Name = "chkTest"
        Me.chkTest.Size = New System.Drawing.Size(135, 17)
        Me.chkTest.TabIndex = 6
        Me.chkTest.Text = "Submit claims as TEST"
        Me.chkTest.UseVisualStyleBackColor = True
        '
        'barStatus
        '
        Me.barStatus.Location = New System.Drawing.Point(44, 25)
        Me.barStatus.Name = "barStatus"
        Me.barStatus.Size = New System.Drawing.Size(411, 23)
        Me.barStatus.TabIndex = 7
        '
        'txtStatus
        '
        Me.txtStatus.Location = New System.Drawing.Point(256, 202)
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(100, 20)
        Me.txtStatus.TabIndex = 8
        '
        'frmEDIWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(738, 429)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.barStatus)
        Me.Controls.Add(Me.chkTest)
        Me.Controls.Add(Me.cmdDone)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmEDIWizard"
        Me.Text = "frmEDIWizard"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents cmdDone As System.Windows.Forms.Button
    Friend WithEvents chkTest As System.Windows.Forms.CheckBox
    Friend WithEvents barStatus As System.Windows.Forms.ProgressBar
    Friend WithEvents txtStatus As System.Windows.Forms.TextBox
End Class
