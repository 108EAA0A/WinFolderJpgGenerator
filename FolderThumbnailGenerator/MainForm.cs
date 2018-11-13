using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;


namespace FolderThumbnailGenerator
{
    public partial class MainForm : Form
    {
        // 圧縮時の圧縮率(default:75)
        const long compressQuality = 75L;

        const int thumbnailNormalSidesSize = 200;
        const int thumbnailLongSidesSize = 300;

        // folder.jpgの作成元にすることを許可する拡張子一覧
        readonly string[] permissionImageExtList =
        {
            "jpg",
            "jpeg",
            "png",
            "gif",
            "bmp",
            "tiff",
            "cr2",
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

            // permissionImageExtListに存在するかを大文字小文字を区別しないで比較する
            return permissionImageExtList.Any(testExt => string.Compare(testExt, fileExt, true) == 0);
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

        int GetTotalAmountOfWork(string path)
        {
            int sumWorkNum = 0;
            string sourceFile = Directory.GetFiles(path)?.FirstOrDefault(file => IsImageFile(file));
            if (sourceFile != null)
            {
                ++sumWorkNum;
            }

            if (!this.checkBox_IsRecursion.Checked) return sumWorkNum;

            foreach (string subDirPath in Directory.GetDirectories(path))
            {
                sumWorkNum += GetTotalAmountOfWork(subDirPath);
            }

            return sumWorkNum;
        }

        Size getResizeSize(Size size) => getResizeSize(size.Width, size.Height);
        Size getResizeSize(int width, int height)
        {
            //if (src.Width <= thumbnailSize && src.Height <= thumbnailSize) ;

            // とりあえず縮小のみ実装

            int x = width;
            int y = height;

            if (!(width <= thumbnailNormalSidesSize && height <= thumbnailNormalSidesSize))
            {
                double ratio = (thumbnailNormalSidesSize / (double) (width >= height ? width : height));
                x = (int)(width * ratio);
                y = (int)(height * ratio);
            }

            return new Size(x, y);
        }


        void GenerateThumbnail(string path)
        {
            var thumbnailPath = $@"{path}\folder.jpg";
            var isThumbnailExist = File.Exists(thumbnailPath);
            string sourcePath = Directory.GetFiles(path)?.FirstOrDefault(file => file != thumbnailPath && IsImageFile(file));


            // 拡張子一覧に合うファイルが存在しない、または、ファイルが既に存在し、上書きが許可されていない場合は作業しない
            if (sourcePath != null && !(isThumbnailExist && !this.checkBox_IsOverwrite.Checked))
            {
                // ディレクトリの読み取り専用解除
                RemoveAttributes(path, FileAttributes.ReadOnly);

                // ファイルが既に存在し、上書きが許可されている場合
                if (isThumbnailExist && this.checkBox_IsOverwrite.Checked)
                {
                    // 既存ファイルの読み取り専用解除
                    //RemoveAttributes(thumbnailPath, FileAttributes.ReadOnly);

                    // 既存ファイルを消去
                    File.Delete(thumbnailPath);
                }

                // 圧縮判定
                if (this.checkBox_IsImageCompress.Checked)
                {
                    ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageEncoders()
                        .First(ici => ici.FormatID == ImageFormat.Jpeg.Guid);
                    EncoderParameters encParams = new EncoderParameters(1);
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, compressQuality);

                    using (var src = new Bitmap(sourcePath))
                    {
                        using (var dest = new Bitmap(src, getResizeSize(src.Size)))
                        {
                            dest.Save(thumbnailPath, jpegEncoder, encParams);
                        }
                    }
                }
                else
                {
                    try
                    {
                        File.Copy(sourcePath, thumbnailPath, this.checkBox_IsOverwrite.Checked); // 上書き許可判定
                    }
                    //catch (UnauthorizedAccessException e)
                    //{
                    //    MessageBox.Show(e.Message, "UnauthorizedAccessException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    catch (Exception e)
                    {
                        //とりあえず黙らせる
                    }
                }

                // 隠しファイル化判定
                if (!isThumbnailExist)
                {
                    // NormalはCopyでUnauthorizedAccessExceptionが出る対策になるらしい
                    // ファイルの属性がないのはよくないため？
                    AddAttributes(thumbnailPath, this.checkBox_IsHiddenFile.Checked ? FileAttributes.Hidden : FileAttributes.Normal);
                }
                else if(this.checkBox_IsHiddenFile.Checked)
                {
                    AddAttributes(thumbnailPath, FileAttributes.Hidden);
                }

                // 作業進捗率用
                this.backgroundWorker.ReportProgress(++completeWorkNum);
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

            // BackgroundWorkerが処理中でなければ
            if (!this.backgroundWorker.IsBusy)
            {
                Working = true;
                this.progressBar.Maximum = GetTotalAmountOfWork(this.textBox_DirectoryName.Text);
                this.backgroundWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(@"作業中です");
            }
        }

        int completeWorkNum = 0;

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // TODO:プログレスバーのために、総作業量と進捗率を取得する

            completeWorkNum = 0;

            this.backgroundWorker.ReportProgress(0);
            if (this.progressBar.Maximum > 0)
            {
                GenerateThumbnail(this.textBox_DirectoryName.Text);
            }
            this.backgroundWorker.ReportProgress(this.progressBar.Maximum);
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Working = false;
            MessageBox.Show(@"終了しました");
        }

        void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            // ドラッグドロップ時にカーソルの形状を変更
            e.Effect = DragDropEffects.All;
        }

        void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            // ファイルが渡されていなければ何もしない
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            // 渡されたファイルに対して処理を行う
            string dropPath = ((string[]) e.Data.GetData(DataFormats.FileDrop))[0];

            // ディレクトリならテキストボックスに代入
            if (File.GetAttributes(dropPath).HasFlag(FileAttributes.Directory))
            {
                this.textBox_DirectoryName.Text = dropPath;
            }
        }
    }
}
