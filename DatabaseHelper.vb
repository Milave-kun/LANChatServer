Imports System.Data.SQLite
Imports System.IO

Module DatabaseHelper

    Public DB_PATH As String = Application.StartupPath & "\Database\chatapp.db"

    Public Sub InitializeDatabase()
        ' Delete the old database if it exists
        If File.Exists(DB_PATH) Then
            Try
                File.Delete(DB_PATH)
                MessageBox.Show("Old database deleted.", "Success")
            Catch ex As Exception
                MessageBox.Show("Error deleting old database: " & ex.Message, "Error")
                Return
            End Try
        End If

        ' Create folder if not exists
        If Not Directory.Exists(Application.StartupPath & "\Database") Then
            Directory.CreateDirectory(Application.StartupPath & "\Database")
        End If

        ' Create database file if not exists
        If Not File.Exists(DB_PATH) Then
            SQLiteConnection.CreateFile(DB_PATH)

            Using conn As New SQLiteConnection("Data Source=" & DB_PATH)
                conn.Open()

                Dim cmd As New SQLiteCommand(conn)

                ' Create Users table with updated fields
                cmd.CommandText = "
                CREATE TABLE Users (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    Password TEXT NOT NULL,
                    FirstName TEXT,
                    LastName TEXT,
                    Birthday TEXT,
                    Address TEXT,
                    PhoneNumber TEXT,
                    Status TEXT DEFAULT 'Offline',
                    Avatar TEXT,
                    Blocked INTEGER DEFAULT 0,
                    Role TEXT DEFAULT 'User'
                );"
                cmd.ExecuteNonQuery()

                ' Create ChatLogs table
                cmd.CommandText = "
                CREATE TABLE ChatLogs (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SenderID INTEGER,
                    ReceiverID INTEGER,
                    MessageText TEXT,
                    Timestamp TEXT,
                    FOREIGN KEY(SenderID) REFERENCES Users(UserID),
                    FOREIGN KEY(ReceiverID) REFERENCES Users(UserID)
                );"
                cmd.ExecuteNonQuery()

                conn.Close()
            End Using

            MessageBox.Show("New database created successfully!", "Success")
        End If
    End Sub

End Module
