using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private StoreManager mng;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mng = new StoreManager();
            FillTreeView(mng);
        }

        private void FillTreeView(StoreManager mng)
        {
            locationsTreeView.Nodes.Clear();
            foreach(StoreManager.Location loc in mng.Locations)
            {
                locationsTreeView.Nodes.Add(new TreeNode(loc.Name.ToString()));
                foreach(StoreName storeName in loc.StoreNames)
                {
                    locationsTreeView.Nodes[mng.Locations.IndexOf(loc)].Nodes.Add(new TreeNode(storeName.ToString()));
                }
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = locationsTreeView.SelectedNode;
            TreeNode parent = node.Parent;
            if (parent != null)
            {
                StoreManager.Location loc = mng.Locations[parent.Index];
                StoreLocation lName = loc.Name;
                StoreName sName = loc.StoreNames[node.Index];
                mng.getCertificates(lName, sName);
                Console.WriteLine($"parent {parent.Text} {parent.Index} child {node.Text} {node.Index}");
            }
        }
    }
}
