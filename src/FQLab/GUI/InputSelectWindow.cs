using System.Collections.ObjectModel;
using System.IO.Abstractions;
using Terminal.Gui.App;
using Terminal.Gui.Configuration;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

public class InputSelectWindow : Window
{
    private UIController _controller;
    public InputSelectWindow(UIController controller)
    {
        Title = $"FQLab - The spectral playground";

        _controller = controller;

        var fileBrowser = new SimpleFileBrowser(Directory.GetCurrentDirectory());

        fileBrowser.OnFileSelected = path =>
        {
            if (!_controller.TryPlayFile(path))
            {
                MessageBox.ErrorQuery("Decoding file", "Error file could not be decoded", "OK");
            }
        };
        
        Add(fileBrowser);
    }
}

public class SimpleFileBrowser : View
{
    private ListView _files;
    private TextField _pathField;
    private Button _exitBtn;

    private string _currentDir;

    public Action<string>? OnFileSelected;

    public SimpleFileBrowser(string startPath)
    {
        _currentDir = startPath;

        Title = "File Browser";
        Width = Dim.Fill();
        Height = Dim.Fill();

        _pathField = new TextField()
        {
            Text = $"{startPath}",
            Width = Dim.Fill(),
            X = 1,
            Y = 1
        };
        Add(_pathField);

        _files = new ListView()
        {
            Y = Pos.Bottom(_pathField) + 1,
            Width = Dim.Fill(),
            Height = Dim.Fill(2)
        };
        _files.OpenSelectedItem += (s, e) =>
        {
            var name = e.Value?.ToString();
            if (string.IsNullOrWhiteSpace(name)) return;

            if (name == "..")
            {
                var parent = Directory.GetParent(_currentDir);
                if (parent is not null)
                {
                    _currentDir = parent.FullName;
                    UpdateFileList();
                }
                return;
            }

            var path = Path.Combine(_currentDir, name);

            if (Directory.Exists(path))
            {
                _currentDir = path;
                UpdateFileList();
            }
            else
            {
                OnFileSelected?.Invoke(path);
            }
        };
        
        Add(_files);

        _exitBtn = new Button()
        {
            Text = "Exit",
            X = Pos.Center(),
            Y = Pos.Bottom(_files) + 1
        };
        _exitBtn.Accepting += (s, e) =>
        {
            Application.RequestStop();
            e.Handled = true;
        };
        Add(_exitBtn);

        UpdateFileList();
    }

    private void UpdateFileList()
    {
        _pathField.Text = _currentDir;
        
        var entries = new List<string>();
        
        if (Directory.GetParent(_currentDir) is not null)
            entries.Add("..");
        
        var dirs = Directory.GetDirectories(_currentDir)
            .Select(Path.GetFileName)
            .OrderBy(x => x);

        var files = Directory.GetFiles(_currentDir)
            .Select(Path.GetFileName)
            .OrderBy(x => x);

        entries.AddRange(dirs);
        entries.AddRange(files);

        _files.SetSource(new ObservableCollection<string>(entries));
    }
}