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
        // folder.*としてコピーすることを許可する拡張子一覧
        readonly string[] permissionImageExtList =
        {
            "jpg",
            "jpeg",
            "png",
            "gif",
            "bmp",
        };

        public MainForm()
        {
            InitializeComponent();
        }

        bool IsImageFile(string path)
        {
            string fileExt = Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(fileExt)) return false; // 拡張子のないファイルは除外
            fileExt = fileExt.Remove(0, 1); // 先頭のピリオドを削除

            return permissionImageExtList.Any(testExt => testExt == fileExt); // permissionImageExtListに存在するか
        }

        void AddAttributes(string path, FileAttributes attr)
        {
            FileAttributes fa = File.GetAttributes(path);
            if (fa.HasFlag(attr)) return;
            File.SetAttributes(path, fa | attr);
        }

        void RemoveAttributes(string path, FileAttributes attr)
        {
            var fa = File.GetAttributes(path);
            if (!fa.HasFlag(attr)) return;
            File.SetAttributes(path, fa & ~attr);
        }

        int GetTotalAmountOfWork(string path, bool isRecurse)
        {
            int sumWorkNum = 0;
            string sourceFile = Directory.GetFiles(path)?.FirstOrDefault(file => IsImageFile(file));
            if (sourceFile != null)
            {
                ++sumWorkNum;
            }

            if (isRecurse)
            {
                foreach (string subDirPath in Directory.GetDirectories(path))
                {
                    sumWorkNum += GetTotalAmountOfWork(subDirPath, isRecurse);
                }
            }

            return sumWorkNum;
        }

        void GenerateThumbnail(string path)
        {
            string sourceFile = Directory.GetFiles(path)?.FirstOrDefault(file => IsImageFile(file));

            if (sourceFile != null)
            {
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
                //catch (UnauthorizedAccessException e)
                //{
                //    MessageBox.Show(e.Message, "UnauthorizedAccessException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                catch (Exception e)
                {
                    //とりあえず黙らせる
                }

                // 隠しファイル化判定
                AddAttributes(thumbnailPath, this.checkBox_IsHiddenFile.Checked ? FileAttributes.Hidden : FileAttributes.Normal);
                // CopyでUnauthorizedAccessExceptionが出る対策になるらしい
                // ファイルの属性がないのはよくないため？
            }

            // 再帰処理許可判定
            if (!this.checkBox_IsRecursion.Checked) return;

            foreach (string subDirPath in Directory.GetDirectories(path))
            {
                GenerateThumbnail(subDirPath);
            }
        }

        bool p_working;
        bool Working
        {
            //get { return p_working; }
            set
            {
                p_working = value;
                this.button_Execution.Enabled = !value;
            }
        }

        void button_DirectorySelect_Click(object sender, EventArgs e)
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

        void button_Execution_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBox_DirectoryName.Text))
            {
                MessageBox.Show(@"作業フォルダを指定してください");
                return;
            }

            Working = true;
            backgroundWorker.RunWorkerAsync();
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // TODO:プログレスバーのために、総作業量と進捗率を取得する

            int totalAmountOfWork = GetTotalAmountOfWork(this.textBox_DirectoryName.Text, this.checkBox_IsRecursion.Checked);


            GenerateThumbnail(this.textBox_DirectoryName.Text);
            //backgroundWorker.ReportProgress(percent);
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Working = false;
            MessageBox.Show(@"終了しました");
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            // ドラッグドロップ時にカーソルの形状を変更
            e.Effect = DragDropEffects.All;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            // ファイルが渡されていなければ何もしない
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            // 渡されたファイルに対して処理を行う
            string dropPath = ((string[]) e.Data.GetData(DataFormats.FileDrop))[0];

            // ディレクトリならテキストボックスに代入
            if (File.GetAttributes(dropPath).HasFlag(FileAttributes.Directory))
            {
                textBox_DirectoryName.Text = dropPath;
            }
        }
    }
}
