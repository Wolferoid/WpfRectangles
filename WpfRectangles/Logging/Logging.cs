using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfRectangles.Logging
{
    public class Log
    {
        string _path;
        bool _isFile;
        TextBox _textBox;

        public Log(bool isFile, TextBox textBox, string path = "")
        {
            _path = path;
            _isFile = isFile;
            _textBox = textBox;
        }

        public void Info(string text)
        {
            if (_isFile)
            { LogFile(text); }
            else
            { LogConsole(text); }
        }

        private void LogFile(string text)
        {
            _textBox.AppendText(string.Format($"{DateTime.Now}\t{text}\n"));
            _textBox.UpdateLayout();
        }

        private void LogConsole(string text)
        {
            using (var sw = File.AppendText(_path))
            {
                sw.WriteLine(string.Format($"{DateTime.Now}\t{text}"));
            }
        }
    }
}
