using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;
using Microsoft.LightSwitch.Threading;

namespace LightSwitchApplication
{
    public partial class ProjectsListDetail
    {
        private string _fileName;
        private byte[] _fileContent;

        partial void DownloadAttachment_Execute()
        {
            Dispatchers.Main.BeginInvoke(() =>
            {
                var extension =
                    Attachments.SelectedItem.Name.Substring(Attachments.SelectedItem.Name.LastIndexOf('.'));

                var saveFileDialog = new SaveFileDialog
                {
                    DefaultFileName = Attachments.SelectedItem.Name,
                    DefaultExt = "*" + extension,
                    Filter = string.Format("Internal Type ({0})|*{0}", extension)
                };

                if (saveFileDialog.ShowDialog().GetValueOrDefault(false))
                {
                    using (var fs = saveFileDialog.OpenFile())
                    {
                        fs.Write(Attachments.SelectedItem.Content, 0, Attachments.SelectedItem.Content.Length);
                    }
                }
            });

        }

        partial void UploadAttachment_Execute()
        {
            Dispatchers.Main.EnsureInvoke(() =>
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();

                if (openFileDialog.File != null && openFileDialog.File.Exists)
                {
                    _fileContent = File.ReadAllBytes(openFileDialog.File.FullName);
                    _fileName = openFileDialog.File.Name;
                }
            }, true);

            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                Attachments.AddNew();
                Attachments.SelectedItem.Name = _fileName;
                Attachments.SelectedItem.Content = _fileContent;
                _fileName = string.Empty;
            }
        }
    }
}
