using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FolderThumbnailGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // folder.*としてコピーすることを許可する拡張子一覧
        readonly string[] permissionImageExtList =
        {
            "jpg",
            "jpeg",
            "png",
            "gif",
            "bmp",
        };

        bool IsImageFile(string path)
        {
            string ext = Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(ext)) return false; // 拡張子のないファイルは除外
            ext = ext.Remove(0, 1); // 先頭のピリオドを削除

            foreach (string testExt in permissionImageExtList)
            {
                if (ext == testExt) return true;
            }

            return false; // permissionImageExtListに存在しない拡張子だった
        }

        void AddAttributes(string path, FileAttributes attr)
        {
            FileAttributes fa = File.GetAttributes(path);
            if ((fa & attr) == attr) return;
            File.SetAttributes(path, fa | attr);
        }

        void RemoveAttributes(string path, FileAttributes attr)
        {
            var fa = File.GetAttributes(path);
            if ((fa & attr) != attr) return;
            File.SetAttributes(path, fa & ~attr);
        }

        void GenerateThumbnail(string path)
        {
            string sourceFile = Directory.GetFiles(path)?.FirstOrDefault(file => IsImageFile(file));

            if (sourceFile != null)
            {
                //string thumbnailPath = $@"{path}\folder{Path.GetExtension(sourceFile)}";
                string thumbnailPath = $@"{path}\folder.jpg";

                RemoveAttributes(path, FileAttributes.ReadOnly); // ディレクトリの読み取り専用解除
                if (File.Exists(thumbnailPath) && this.checkBox_IsOverwrite.Checked)
                {
                    // すでにファイルが存在する場合、読み取り専用解除
                    RemoveAttributes(thumbnailPath, FileAttributes.ReadOnly);
                }

                try
                {
                    File.Copy(sourceFile, thumbnailPath, this.checkBox_IsOverwrite.Checked); // 上書き許可判定
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show(e.Message, "UnauthorizedAccessException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (this.checkBox_IsHiddenFile.Checked) // 隠しファイル化判定
                {
                    AddAttributes(thumbnailPath, FileAttributes.Hidden);
                }
                else
                {
                    // CopyでUnauthorizedAccessExceptionが出る対策になるらしい
                    // ファイルの属性がないのはよくないため？
                    AddAttributes(thumbnailPath, FileAttributes.Normal);
                }
            }

            // 再帰処理許可判定
            if (!this.checkBox_IsRecursion.Checked) return;

            foreach (string subDirPath in Directory.GetDirectories(path))
            {
                GenerateThumbnail(subDirPath);
            }
        }

        private void button_DirectorySelect_Click(object sender, EventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog())
            {
                if (this.textBox_DirectoryName.Text != null) cofd.DefaultFileName = this.textBox_DirectoryName.Text;
                cofd.IsFolderPicker = true;
                CommonFileDialogResult result = cofd.ShowDialog();
                if (result == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(cofd.FileName))
                {
                    this.textBox_DirectoryName.Text = cofd.FileName;
                }
            }
        }

        private void button_Execution_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBox_DirectoryName.Text))
            {
                MessageBox.Show(@"作業フォルダを指定してください");
                return;
            }

            GenerateThumbnail(this.textBox_DirectoryName.Text);
            MessageBox.Show(@"終了しました");
        }
    }
}
