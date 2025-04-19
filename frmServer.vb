Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Threading

Public Class frmServer
    Private server As TcpListener
    Private clients As New List(Of TcpClient)
    Private clientUsernames As New Dictionary(Of TcpClient, String)
    Private serverThread As Thread
    Private isRunning As Boolean = False

    ' Start the server
    Private Sub btnStartServer_Click(sender As Object, e As EventArgs) Handles btnStartServer.Click
        If Not isRunning Then
            server = New TcpListener(IPAddress.Any, 5000)
            server.Start()
            isRunning = True
            serverThread = New Thread(AddressOf ListenForClients)
            serverThread.IsBackground = True
            serverThread.Start()
            Log("✅ Server started on port 5000.")
            btnStartServer.Enabled = False
        End If
    End Sub

    ' Listen for incoming client connections
    Private Sub ListenForClients()
        While isRunning
            Try
                Dim client As TcpClient = server.AcceptTcpClient()
                SyncLock clients
                    clients.Add(client)
                End SyncLock
                Log("👤 Client connected: " & client.Client.RemoteEndPoint.ToString())

                ' Start a new thread to handle this client
                Dim clientThread As New Thread(AddressOf HandleClient)
                clientThread.IsBackground = True
                clientThread.Start(client)
            Catch ex As Exception
                Log("❌ Error accepting client: " & ex.Message)
            End Try
        End While
    End Sub
    Private Sub HandleClient(clientObj As Object)
        Dim client As TcpClient = CType(clientObj, TcpClient)
        Dim stream As NetworkStream = client.GetStream()
        Dim reader As New StreamReader(stream)
        Dim writer As New StreamWriter(stream) With {.AutoFlush = True}

        Try
            While True
                Dim message As String = reader.ReadLine()
                If message IsNot Nothing Then
                    Log("📩 Received: " & message)

                    If message.StartsWith("[CONNECT]:") Then
                        ' Format: [CONNECT]:userID:username
                        Dim parts = message.Split(":"c)
                        If parts.Length = 3 Then
                            Dim username As String = parts(2)
                            SyncLock clientUsernames
                                clientUsernames(client) = username
                            End SyncLock
                            BroadcastUserList()
                        End If

                    ElseIf message.StartsWith("[MSG]:") Then
                        ' Broadcast message to all clients
                        Broadcast(message)

                    Else
                        ' Unknown message, broadcast as raw text
                        Broadcast(message)
                    End If
                End If
            End While
        Catch ex As Exception
            Log("❌ Client disconnected or error: " & ex.Message)
        Finally
            ' Clean up when client disconnects
            SyncLock clients
                clients.Remove(client)
            End SyncLock

            SyncLock clientUsernames
                If clientUsernames.ContainsKey(client) Then
                    clientUsernames.Remove(client)
                End If
            End SyncLock

            client.Close()
            BroadcastUserList()
        End Try
    End Sub

    Private Sub SendPrivateMessage(receiverUsername As String, message As String, senderClient As TcpClient)
        Dim receiverClient As TcpClient = Nothing

        ' Find the client corresponding to the receiver's username
        SyncLock clientUsernames
            For Each client In clientUsernames.Keys
                If clientUsernames(client) = receiverUsername Then
                    receiverClient = client
                    Exit For
                End If
            Next
        End SyncLock

        ' Get the current timestamp
        Dim timestamp As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        ' If the receiver is found, send the message to that client
        If receiverClient IsNot Nothing Then
            Try
                ' Send the message directly to the recipient with timestamp
                Dim writer As New StreamWriter(receiverClient.GetStream()) With {.AutoFlush = True}
                writer.WriteLine($"[MSG]:{clientUsernames(senderClient)}:{receiverUsername}:{timestamp}:{message}")
            Catch ex As Exception
                Log("Error sending private message: " & ex.Message)
            End Try
        Else
            Log("User not found: " & receiverUsername)
        End If
    End Sub

    Private Sub BroadcastUserList()
        Dim usernames As New List(Of String)

        SyncLock clientUsernames
            usernames.AddRange(clientUsernames.Values)
        End SyncLock

        Dim userListMessage As String = "[USERS]:" & String.Join(",", usernames)

        Broadcast(userListMessage)
    End Sub

    Private Sub Log(text As String)
        If lstLogs.InvokeRequired Then
            lstLogs.Invoke(Sub() lstLogs.Items.Add(text))
        Else
            lstLogs.Items.Add(text)
        End If
    End Sub

    Private Sub Broadcast(message As String)
        SyncLock clients
            For Each c In clients
                Try
                    Dim writer As New StreamWriter(c.GetStream()) With {.AutoFlush = True}
                    writer.WriteLine(message)
                Catch ex As Exception
                    ' Ignore failed client
                End Try
            Next
        End SyncLock
    End Sub

    Private Sub frmServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class