using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsUniversity
{
    public partial class Form1 : Form
    {
        UniversityEntities ctx = new UniversityEntities();
        BindingSource bs1 = new BindingSource();
        BindingSource bs2 = new BindingSource();
        BindingSource bs3 = new BindingSource();

        public Form1()
        {
            InitializeComponent();
 
            bs1.DataSource = ctx.Students.Local.ToBindingList();
            bs2.DataSource = ctx.Subjects.Local.ToBindingList();
            bs3.DataSource = ctx.Specializations.Local.ToBindingList();

            dataGridViewStudents.DataSource = bs1;
            dataGridViewSubjects.DataSource = bs2;
            dataGridViewSpecializations.DataSource = bs3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            studentBindingSource.DataSource = ctx.Students.ToList();
            specializationBindingSource.DataSource = ctx.Specializations.ToList();
            subjectBindingSource.DataSource = ctx.Subjects.ToList();
        }

        private void dataGridViewStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ctx.SaveChanges();

            studentBindingSource.DataSource = ctx.Students.ToList();
            specializationBindingSource.DataSource = ctx.Specializations.ToList();
            subjectBindingSource.DataSource = ctx.Subjects.ToList();
        }

        private void dataGridViewStudents_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
           
        }
    }
}
