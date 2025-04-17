<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmServer
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
        lstLogs = New ListBox()
        btnStartServer = New Button()
        SuspendLayout()
        ' 
        ' lstLogs
        ' 
        lstLogs.BackColor = Color.Black
        lstLogs.BorderStyle = BorderStyle.FixedSingle
        lstLogs.ForeColor = Color.White
        lstLogs.FormattingEnabled = True
        lstLogs.ItemHeight = 15
        lstLogs.Location = New Point(12, 12)
        lstLogs.Name = "lstLogs"
        lstLogs.Size = New Size(776, 377)
        lstLogs.TabIndex = 0
        ' 
        ' btnStartServer
        ' 
        btnStartServer.FlatStyle = FlatStyle.Flat
        btnStartServer.Location = New Point(342, 397)
        btnStartServer.Name = "btnStartServer"
        btnStartServer.Size = New Size(123, 41)
        btnStartServer.TabIndex = 1
        btnStartServer.Text = "Start Server"
        btnStartServer.UseVisualStyleBackColor = True
        ' 
        ' frmServer
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.Black
        ClientSize = New Size(800, 450)
        Controls.Add(btnStartServer)
        Controls.Add(lstLogs)
        ForeColor = Color.White
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        MaximizeBox = False
        Name = "frmServer"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Server"
        ResumeLayout(False)
    End Sub

    Friend WithEvents lstLogs As ListBox
    Friend WithEvents btnStartServer As Button
End Class
