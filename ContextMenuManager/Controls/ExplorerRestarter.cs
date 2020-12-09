﻿using BulePointLilac.Controls;
using BulePointLilac.Methods;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ContextMenuManager.Controls
{
    public sealed class ExplorerRestarter : MyListItem
    {
        public ExplorerRestarter()
        {
            this.Visible = false;
            this.Dock = DockStyle.Bottom;
            this.Image = AppImage.Explorer;
            this.Text = AppString.Other.RestartExplorer;
            MyToolTip.SetToolTip(BtnRestart, AppString.Tip.RestartExplorer);
            this.AddCtr(BtnRestart);
            this.CanMoveForm();
            BtnRestart.MouseDown += (sender, e) => { Explorer.ReStart(); this.Visible = false; };
            RestartHandler += (sender, e) => this.Visible = NeedRestart;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if(this.Parent != null) this.Parent.Height += Visible ? Height : -Height;
        }

        private readonly PictureButton BtnRestart = new PictureButton(AppImage.RestartExplorer);

        private static event EventHandler RestartHandler;

        private static bool needRestart;
        public static bool NeedRestart
        {
            get => needRestart;
            set
            {
                needRestart = value;
                RestartHandler?.Invoke(null, null);
            }
        }
    }

    public static class Explorer
    {
        public static void ReStart()
        {
            using(Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c taskkill -f -im explorer.exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };
                process.Start();
                process.WaitForExit();
                process.StartInfo.Arguments = "/c explorer";
                process.Start();
            }
        }
    }
}