using Microsoft.Win32;
using System.Text.Json;

namespace Main.Support_Tools
{
    public class AppSetting //AppSettings is used for user settings that can be saved and loaded
    {
        //Internal properties
        public enum InternalTheme { Light, Dark } //Not for public use, only for internal app use
        public enum InternalSettingSaveMode { File, Registry } //Not for public use, only for internal app use


        //Settings properties here (ensure they are public with get and set, and NO STATIC!)
        public int UpdateChecker_RetryMaxCount { get; set; } = 3; //INT (DWORD) ONLY!
        public bool UpdateChecker_AutoCheck { get; set; } = true; //INT (DWORD) ONLY!
        public InternalTheme App_Theme { get; set; } = InternalTheme.Light; //default is light, for user use
        public InternalSettingSaveMode App_SettingSaveMode { get; set; } = InternalSettingSaveMode.Registry; //INT (DWORD) ONLY!
        public Dictionary<string, bool> ProtectedFiles_IOLockBool { get; set; } = []; //Format is: Key = FilePath, Value = IsLocked (true/false)
        public int SmartWriteSelector_Threshold { get; set; } = 10; //default is 10 files
        public bool SmartWriteSelector_AllOutMode { get; set; } = false; //default is false (normal mode)
        public string App_SettingSaveMode_FilePath { get; set; } = string.Empty; //default is empty, user can set path to save settings file
        public int OptimizeSpeedOrRes { get; set; } = 0; //Optimized for best performance

        // Saving and loading settings
        public static void SaveSettingsFile(AppSetting settings, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, JsonSerializer.Serialize(settings, Config.CachedJsonOptions));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public static AppSetting LoadSettings(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    return JsonSerializer.Deserialize<AppSetting>(File.ReadAllText(filePath), Config.CachedJsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
            return new AppSetting();
        }

        public static AppSetting LoadSettings()
        {
            try
            {
                var settings = LoadSettingsRegistry();
                if (settings.App_SettingSaveMode == InternalSettingSaveMode.File &&
                    !string.IsNullOrEmpty(settings.App_SettingSaveMode_FilePath) &&
                    File.Exists(settings.App_SettingSaveMode_FilePath))
                {
                    return LoadSettings(settings.App_SettingSaveMode_FilePath);
                }
                return settings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
                MessageBox.Show($"Error loading settings: {ex.Message}");
                return new AppSetting();
            }
        }

        public static bool IsDefault(AppSetting settings) => settings == GetDefaultAppSettings();

        public static void InitializeDefaultRegistrySettings()
        {
            var key = Config.GetRegKey();
            key.SetValue(Config.Reg_RetryMaxCount, 3, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_AutoCheck, 1, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_Theme, 0, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_SmartWriteSelectorAllOutMode, 0, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_SmartWriteSelectorThreshold, 10, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_SettingSaveMode, "Registry", RegistryValueKind.String);
            key.SetValue(Config.Reg_AppSetting_FilePath, string.Empty, RegistryValueKind.String);
            key.SetValue(Config.Reg_OptimizeSpeedOrRes, 0, RegistryValueKind.DWord);
        }

        public static AppSetting GetDefaultAppSettings() => new()
        {
            UpdateChecker_RetryMaxCount = 3,
            UpdateChecker_AutoCheck = true,
            App_Theme = InternalTheme.Light,
            ProtectedFiles_IOLockBool = [],
            SmartWriteSelector_Threshold = 10,
            SmartWriteSelector_AllOutMode = false,
            App_SettingSaveMode = InternalSettingSaveMode.Registry,
            App_SettingSaveMode_FilePath = string.Empty,
            OptimizeSpeedOrRes = 0
        };

        public static AppSetting LoadSettingsRegistry()
        {
            var key = Config.GetRegKey();
            if (key == null) return GetDefaultAppSettings();

            return new AppSetting
            {
                UpdateChecker_RetryMaxCount = (int)(key.GetValue(Config.Reg_RetryMaxCount) ?? 3),
                UpdateChecker_AutoCheck = Convert.ToBoolean(key.GetValue(Config.Reg_AutoCheck) ?? true),
                App_Theme = (InternalTheme)((int)(key.GetValue(Config.Reg_Theme) ?? 0)),
                SmartWriteSelector_Threshold = (int)(key.GetValue(Config.Reg_SmartWriteSelectorThreshold) ?? 10),
                SmartWriteSelector_AllOutMode = Convert.ToBoolean(key.GetValue(Config.Reg_SmartWriteSelectorAllOutMode) ?? false),
                App_SettingSaveMode = ((string)(key.GetValue(Config.Reg_SettingSaveMode) ?? "Registry")) == "File"
                    ? InternalSettingSaveMode.File : InternalSettingSaveMode.Registry,
                App_SettingSaveMode_FilePath = (string)(key.GetValue(Config.Reg_AppSetting_FilePath) ?? string.Empty),
                OptimizeSpeedOrRes = (int)(key.GetValue(Config.Reg_OptimizeSpeedOrRes) ?? 0)
            };
        }

        public static void SaveSettingsRegistry(AppSetting settings)
        {
            var key = Config.GetRegKey();
            if (key == null) return;
            key.SetValue(Config.Reg_RetryMaxCount, settings.UpdateChecker_RetryMaxCount, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_AutoCheck, settings.UpdateChecker_AutoCheck ? 1 : 0, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_Theme, (int)settings.App_Theme, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_SmartWriteSelectorThreshold, settings.SmartWriteSelector_Threshold, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_SmartWriteSelectorAllOutMode, settings.SmartWriteSelector_AllOutMode ? 1 : 0, RegistryValueKind.DWord);
            key.SetValue(Config.Reg_SettingSaveMode, settings.App_SettingSaveMode == InternalSettingSaveMode.File ? "File" : "Registry", RegistryValueKind.String);
            key.SetValue(Config.Reg_AppSetting_FilePath, settings.App_SettingSaveMode_FilePath ?? string.Empty, RegistryValueKind.String);
            key.SetValue(Config.Reg_OptimizeSpeedOrRes, settings.OptimizeSpeedOrRes, RegistryValueKind.DWord);
        }

        public static void SaveSettings(AppSetting settings)
        {
            if (settings.App_SettingSaveMode == InternalSettingSaveMode.File &&
                !string.IsNullOrEmpty(settings.App_SettingSaveMode_FilePath) &&
                File.Exists(settings.App_SettingSaveMode_FilePath))
            {
                SaveSettingsFile(settings, settings.App_SettingSaveMode_FilePath);
            }
            else
            {
                SaveSettingsRegistry(settings);
            }
        }

        //Theme helper
        public static void SetTheme(Form form, InternalTheme theme)
        {
            form.BackColor = theme == InternalTheme.Dark ? Config.Color_Background : Config.Color_LightBackground;
            form.ForeColor = theme == InternalTheme.Dark ? Config.Color_TextColor : Config.Color_LightTextColor;

            foreach (Control ctrl in form.Controls)
            {
                ApplyThemeToControl(ctrl, theme);
            }
        }
        private static void ApplyThemeToControl(Control ctrl, InternalTheme theme)
        {
            if (theme == InternalTheme.Dark)
            {
                switch (ctrl)
                {
                    case Button btn:
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.BackColor = Config.Color_ButtonBackground;
                        btn.ForeColor = Config.Color_TextColor;
                        btn.FlatAppearance.BorderColor = Config.Color_Highlight;
                        break;
                    case TextBox tb:
                        tb.BackColor = Config.Color_ControlBackground;
                        tb.ForeColor = Config.Color_TextColor;
                        tb.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case ComboBox combo:
                        combo.FlatStyle = FlatStyle.Flat;
                        combo.BackColor = theme == InternalTheme.Dark
                            ? Config.Color_ControlBackground
                            : Config.Color_LightControlBackground;
                        combo.ForeColor = theme == InternalTheme.Dark
                            ? Config.Color_TextColor
                            : Config.Color_LightTextColor;
                        break;
                    case DataGridView dgv:
                        dgv.BackgroundColor = Config.Color_Background;
                        dgv.DefaultCellStyle.BackColor = Config.Color_ControlBackground;
                        dgv.DefaultCellStyle.ForeColor = Config.Color_TextColor;
                        dgv.ColumnHeadersDefaultCellStyle.BackColor = Config.Color_ButtonBackground;
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Config.Color_TextColor;
                        break;
                    case MenuStrip menu:
                        menu.BackColor = Config.Color_ControlBackground;
                        menu.ForeColor = Config.Color_TextColor;
                        foreach (ToolStripMenuItem item in menu.Items)
                        {
                            item.BackColor = Config.Color_ControlBackground;
                            item.ForeColor = Config.Color_TextColor;
                        }
                        break;
                    case TabControl tabCtrl:
                        tabCtrl.DrawMode = TabDrawMode.OwnerDrawFixed;
                        tabCtrl.Tag = theme; //Store the theme in Tag
                        tabCtrl.DrawItem -= TabControl_DrawItem; // Remove if already attached
                        tabCtrl.DrawItem += TabControl_DrawItem; // Add once

                        tabCtrl.BackColor = theme == InternalTheme.Dark
                            ? Config.Color_ControlBackground
                            : Config.Color_LightControlBackground;

                        tabCtrl.ForeColor = theme == InternalTheme.Dark
                            ? Config.Color_TextColor
                            : Config.Color_LightTextColor;

                        // Continue applying to TabPages
                        foreach (TabPage tabPage in tabCtrl.TabPages)
                        {
                            tabPage.BackColor = theme == InternalTheme.Dark
                                ? Config.Color_ControlBackground
                                : Config.Color_LightControlBackground;

                            tabPage.ForeColor = theme == InternalTheme.Dark
                                ? Config.Color_TextColor
                                : Config.Color_LightTextColor;

                            foreach (Control child in tabPage.Controls)
                            {
                                ApplyThemeToControl(child, theme);
                            }
                        }

                        break;
                    default:
                        ctrl.BackColor = Config.Color_ControlBackground;
                        ctrl.ForeColor = Config.Color_TextColor;
                        break;
                }
            }
            else // Light Theme
            {
                switch (ctrl)
                {
                    case Button btn:
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.BackColor = Config.Color_LightButtonBackground;
                        btn.ForeColor = Config.Color_LightTextColor;
                        btn.FlatAppearance.BorderColor = Config.Color_LightHighlight;
                        break;
                    case TextBox tb:
                        tb.BackColor = Config.Color_LightControlBackground;
                        tb.ForeColor = Config.Color_LightTextColor;
                        tb.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case ComboBox combo:
                        combo.FlatStyle = FlatStyle.Flat;
                        combo.BackColor = theme == InternalTheme.Dark
                            ? Config.Color_ControlBackground
                            : Config.Color_LightControlBackground;
                        combo.ForeColor = theme == InternalTheme.Dark
                            ? Config.Color_TextColor
                            : Config.Color_LightTextColor;
                        break;
                    case DataGridView dgv:
                        dgv.BackgroundColor = Config.Color_LightBackground;
                        dgv.DefaultCellStyle.BackColor = Config.Color_LightControlBackground;
                        dgv.DefaultCellStyle.ForeColor = Config.Color_LightTextColor;
                        dgv.ColumnHeadersDefaultCellStyle.BackColor = Config.Color_LightButtonBackground;
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Config.Color_LightTextColor;
                        break;
                    case MenuStrip menu:
                        menu.BackColor = Config.Color_LightControlBackground;
                        menu.ForeColor = Config.Color_LightTextColor;
                        foreach (ToolStripMenuItem item in menu.Items)
                        {
                            item.BackColor = Config.Color_LightControlBackground;
                            item.ForeColor = Config.Color_LightTextColor;
                        }
                        break;
                    case TabControl tabCtrl:
                        tabCtrl.DrawMode = TabDrawMode.OwnerDrawFixed;
                        tabCtrl.Tag = theme; // Store the theme in Tag
                        tabCtrl.DrawItem -= TabControl_DrawItem; // Remove if already attached
                        tabCtrl.DrawItem += TabControl_DrawItem; // Add once

                        tabCtrl.BackColor = Config.Color_LightControlBackground;
                        tabCtrl.ForeColor = Config.Color_LightTextColor;

                        // Continue applying to TabPages
                        foreach (TabPage tabPage in tabCtrl.TabPages)
                        {
                            tabPage.BackColor = Config.Color_LightControlBackground;
                            tabPage.ForeColor = Config.Color_LightTextColor;

                            foreach (Control child in tabPage.Controls)
                            {
                                ApplyThemeToControl(child, theme);
                            }
                        }

                        break;
                    default:
                        ctrl.BackColor = Config.Color_LightControlBackground;
                        ctrl.ForeColor = Config.Color_LightTextColor;
                        break;
                }
            }

            // Recursively apply theme to child controls (like those in TabPages, Panels, GroupBoxes, etc.)
            foreach (Control child in ctrl.Controls)
            {
                ApplyThemeToControl(child, theme);
            }

            // Special handling for TabControl -> apply theme to each TabPage
            if (ctrl is TabControl tabControl)
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    tabPage.BackColor = theme == InternalTheme.Dark ? Config.Color_ControlBackground : Config.Color_LightControlBackground;
                    tabPage.ForeColor = theme == InternalTheme.Dark ? Config.Color_TextColor : Config.Color_LightTextColor;

                    foreach (Control child in tabPage.Controls)
                    {
                        ApplyThemeToControl(child, theme);
                    }
                }
            }
        }
        private static void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tabControl) return;

            var theme = tabControl.Tag is InternalTheme t ? t : InternalTheme.Light;

            var tabPage = tabControl.TabPages[e.Index];
            var bounds = e.Bounds;
            var text = tabPage.Text;

            // Determine colors
            Color backColor = theme == InternalTheme.Dark ? Config.Color_ButtonBackground : Config.Color_LightButtonBackground;
            Color textColor = theme == InternalTheme.Dark ? Config.Color_TextColor : Config.Color_LightTextColor;

            // Highlight selected tab
            if (e.Index == tabControl.SelectedIndex)
            {
                backColor = theme == InternalTheme.Dark ? Config.Color_Highlight : Config.Color_LightHighlight;
                textColor = theme == InternalTheme.Dark ? Config.Color_TextColor : Config.Color_LightTextColor;
            }

            using SolidBrush backBrush = new(backColor);
            using SolidBrush textBrush = new(textColor);
            using StringFormat stringFormat = new()
            { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.FillRectangle(backBrush, bounds);
            e.Graphics.DrawString(text, e.Font, textBrush, bounds, stringFormat);
        }
    }
}
