using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MitsubaArchivizer.GUI.Properties;
using MitsubaArchivizer.Processors;
using MitsubaArchivizer.Utils;

namespace MitsubaArchivizer.GUI
{
    public partial class MainForm : Form
    {
        private int _total = 0;
        private int _current = 0;

        public MainForm()
        {
            InitializeComponent();
            Text += $" (commit {ThisAssembly.Git.Commit} @ {ThisAssembly.Git.Branch})";
            ThreadParser.OnPostCount += ThreadParser_OnPostCount;
            ThreadParser.OnPostParsing += ThreadParser_OnPostParsing;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var stylesPath = Path.Combine(Environment.CurrentDirectory, "Resources", "styles");

            if (!Directory.Exists(stylesPath))
            {
                stylesPath = Path.Combine(Assembly.GetExecutingAssembly().Location, "Resources", "styles");
            }

            if (!Directory.Exists(stylesPath))
            {
                htmlGroupBox.Enabled = false;
                htmlGroupBox.Text += " (Missing resources)";
                return;
            }

            foreach (var file in Directory.GetFiles(stylesPath))
            {
                if (file.EndsWith(".css"))
                {
                    styleComboBox.Items.Add(Path.GetFileName(file));
                }
            }

            styleComboBox.SelectedIndex = 0;

            if (string.IsNullOrEmpty(Settings.Default.LastPath))
            {
                outputDirTextbox.Text = PathUtils.GetBaseOutputPath(string.Empty);
            }
            else
            {
                outputDirTextbox.Text = Settings.Default.LastPath;
            }
        }

        private void outputDirectoryButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select output directory";
                dlg.ShowNewFolderButton = true;
                dlg.SelectedPath = outputDirTextbox.Text;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    outputDirTextbox.Text = dlg.SelectedPath;
                    archivizeButton.Enabled = true;

                    Settings.Default.LastPath = dlg.SelectedPath;
                    Settings.Default.Save();
                }
            }
        }

        private async void archivizeButton_Click(object sender, EventArgs e)
        {
            if (!threadListTextBox.Lines.Any())
            {
                return;
            }

            threadListTextBox.Enabled = false;
            mediaGroupBox.Enabled = false;
            jsonGroupBox.Enabled = false;
            htmlGroupBox.Enabled = false;
            archivizeButton.Enabled = false;
            outputDirectoryButton.Enabled = false;
            
            var processors = new List<IProcessor>();

            if (jsonEnabledCheckbox.Checked)
            {
                processors.Add(new JsonSerializerProcessor(outputDirTextbox.Text, formattedCheckbox.Checked));
            }

            if (htmlEnabledCheckbox.Checked)
            {
                processors.Add(new HtmlGeneratorProcessor(outputDirTextbox.Text, namesCheckbox.Checked, coloredNamesCheckbox.Checked, samefagCheckbox.Checked, (string)styleComboBox.SelectedItem, prettifyCheckbox.Checked));
            }

            MediaResolver resolver;
            if (mediaEnabledCheckbox.Checked)
            {
                var exts = mediaExtTextbox.Text.Split(',');
                var thumbExts = thumbnailExtTextBox.Text.Split(',');
                
                resolver = new MediaResolver(true, outputDirTextbox.Text, groupByExtensionsCheckbox.Checked, thumbnailsEnabledCheckbox.Checked, exts, thumbExts);
                resolver.OnPostWithMediaCount += Resolver_OnPostWithMediaCount;
                resolver.OnProcessingPostMedia += Resolver_OnProcessingPostMedia;
            }
            else
            {
                resolver = new MediaResolver();
            }

            var pipeline = new ProcessorPipeline(processors, resolver);

            pipeline.OnProcessorInvoked += Pipeline_OnProcessorInvoked;

            foreach (var line in threadListTextBox.Lines)
            {
                try
                {
                    statusTextLabel.Text = "Parsing posts...";

                    var thread = await ThreadParser.TryParse(line);

                    if (mediaEnabledCheckbox.Checked)
                    {
                        statusTextLabel.Text = "Resolving media...";

                        foreach (var post in thread.Posts)
                        {
                            if (post.File != null)
                            {
                                _total++;

                                if (resolver.DownloadThumbnails && !string.IsNullOrEmpty(post.File.FileThumbUrl))
                                {
                                    _total++;
                                }
                            }
                        }
                    }

                    await pipeline.Process(thread);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), $"Exception thrown while processing {line}", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            Reset();
        }

        void Reset()
        {
            threadListTextBox.Enabled = true;
            mediaGroupBox.Enabled = true;
            jsonGroupBox.Enabled = true;
            htmlGroupBox.Enabled = true;
            archivizeButton.Enabled = true;
            outputDirectoryButton.Enabled = true;

            statusTextLabel.Text = "Idle";
            statusProgressBar.Visible = false;
            _total = 0;
            _current = 0;
        }

        private void UpdateProgressBarValue()
        {
            statusProgressBar.Visible = true;
            statusProgressBar.Style = ProgressBarStyle.Blocks;
            statusProgressBar.Maximum = _total;
            statusProgressBar.Value = _current;
        }

        private void ThreadParser_OnPostParsing(int idx)
        {
            statusTextLabel.Text = $"Parsing posts... ({idx + 1}/{_total})";
            _current = idx;
            UpdateProgressBarValue();
        }

        private void ThreadParser_OnPostCount(int count)
        {
            _total = count;
        }

        private void Pipeline_OnProcessorInvoked(string name)
        {
            statusTextLabel.Text = name;
            statusProgressBar.Visible = true;
            statusProgressBar.Style = ProgressBarStyle.Marquee;
        }

        private void Resolver_OnProcessingPostMedia(Models.Post post, int idx)
        {
            statusTextLabel.Text = $"Downloading media: {post.File.FileName} ({idx + 1}/{_total})";
            _current = idx;
            UpdateProgressBarValue();
        }

        private void Resolver_OnPostWithMediaCount(int count)
        {
            _total = count;
        }
    }
}
