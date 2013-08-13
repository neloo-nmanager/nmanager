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
    public partial class AttachmentsGrid
    {
        private Attachment _newAttachement;

        private string _fileName;
        private byte[] _fileContent;

        partial void UploadAttachement_Execute()
        {
            Dispatchers.Main.EnsureInvoke(() =>
             {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();

                if (openFileDialog.File != null && !openFileDialog.File.Exists)
                {
                    return;
                }

                _fileContent = File.ReadAllBytes(openFileDialog.File.FullName);
                _fileName = openFileDialog.File.Name;
                 ;

             }, true);

            this.Attachments.AddNew();
            this.Attachments.SelectedItem.Name = _fileName;
            this.Attachments.SelectedItem.Content = _fileContent;
        }

        partial void DownloadAttachement_Execute()
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
    }
}
