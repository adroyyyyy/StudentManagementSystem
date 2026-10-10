# StudentManagementSystem


A Windows Forms desktop app (C#) that manages student records stored in a Microsoft Access database through OLE DB.

### Features
- Add a new student
- View all students in a DataGridView
- Select a student from the grid to load their details into the form
- Update a selected student (in-progress)
- Delete a selected student (with yes/no confirmation) (in-progress)
- Clear the input fields
- Load / Refresh the grid from the database
- Input validation (no empty fields, age must be a positive number)
- Error handling around every database operation, so the app does not carsh

### Technologies
- C# and Windows Forms (WinForms)
- .NET 10 (Windows)
- Visual Studio
- Microsoft Access database (StudentDatabase.accdb)
- OLE DB (System.Data.OleDb): OleDbConnection, OleDbCommand, OleDbDataAdapter


### Requirements
- Windows (WinForms and the Access driver do not run on macOS)
- Visual Studio with the .NET desktop development workload
- Microsoft Access Database Engine (x64), a free driver from Microsoft. Microsoft Access itself is not required to run the app.
- NuGet package: System.Data.OleDb

### Project structure
StudentManagementSystem/
  Form1.cs            Main form and all logic
  Form1.Designer.cs   Auto-generated form layout
  Program.cs          Entry point
  StudentDatabase.accdb
