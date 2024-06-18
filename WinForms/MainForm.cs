using FlickrNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AuthForm form = new AuthForm();
            form.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PhotoSearchForm form = new PhotoSearchForm();
            form.ShowDialog();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (FlickrManager.OAuthToken == null || FlickrManager.OAuthToken.Token == null)
            {
                MessageBox.Show("You must authenticate before you can upload a photo.");
                return;
            }

            UploadForm form = new UploadForm();
            form.ShowDialog();
        }
        static string USERID = "19435011@N00";
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var f = FlickrManager.GetAuthInstance();
            int iPage = 1;
            do
            {
                try
                {
                    var oSets = f.PhotosetsGetList(USERID, iPage, 500, PhotoSearchExtras.None);

                    foreach ( var oSet in oSets.Where(p => p.Title.StartsWith("Taken")) )
                    {
                        f.PhotosetsDelete(oSet.PhotosetId);
                        Debug.WriteLine("Ditched " + oSet.Title);
                        //dic.Add(oSet.PhotosetId, oSet.Title)
                    }
                    iPage++;
                    if ( iPage > 100 )
                    { break; }
                } catch ( Exception ex )
                {
                    break;
                }
            } while ( true );
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var f = FlickrManager.GetAuthInstance();
            var d = new SortedDictionary<string,string>();
            int iPage = 1;
            do
            {
                try
                {
                    var oSets = f.PhotosetsGetList(USERID, iPage, 500, PhotoSearchExtras.None);

                    foreach ( var oSet in oSets)
                    {
                        d.Add(oSet.Title, oSet.PhotosetId);
                    }
                    iPage++;
                    if ( iPage > 10 )
                    { break; }
                } catch ( Exception ex )
                {
                    break;
                }
            } while ( true );
            var s = string.Join(",",d.Values);
            f.PhotosetsOrderSets(s);
        }
    }
}
