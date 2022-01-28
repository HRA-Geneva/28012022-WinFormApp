using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Data;
using WinFormsApp1.Enums;
using WinFormsApp1.Helper;
using WinFormsApp1.Models;

namespace WinFormsApp1
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            if(Session.User == null)
            {
                Session.MainForm.Show();
                this.Hide();
            }

            if (Session.User.UserRole != UserRole.Admin && Session.User.UserRole != UserRole.Moderator)
            {
                MessageBox.Show("Siz admin və ya moderator deyilsiniz falan!");
                return;
            }
           
            InitializeComponent();

            

            btn_edit.Hide();
            btn_delete.Hide();
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Session.MainForm.Close();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            #region Istifadecilerin bazadan alinib listView ya doldurulamasi
            List<User> users = ApplicationDbContext.Users;

            foreach (User user in users)
            {
                list_users.Items.Add(user);
            }
            #endregion

            #region Rollarin comboboxa doldurulmasi
            string[] roles = Enum.GetNames(typeof(UserRole));
            foreach (string role in roles)
            {
                cmbx_status.Items.Add(role);
            }
            #endregion
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if(Session.User.UserRole == UserRole.Moderator)
            {
                MessageBox.Show("Moderator istifadəçi əlavə edə bilməz!");
                return;
            }

            try
            {
                string name = txbx_name.Text;
                string surname = txbx_surname.Text;
                string email = txbx_email.Text;
                string password = txbx_password.Text;

                UserRole role = (UserRole)Enum.Parse(typeof(UserRole),cmbx_status.SelectedItem.ToString());
              
                if(Session.User.UserRole == UserRole.Moderator && role == UserRole.Admin)
                {
                    MessageBox.Show("Moderator Admin əlavə edə bilməz!");
                    return;
                }

                //Validation start

                //Validation end

                User user = new User(name, surname, email, password, role);

                ApplicationDbContext.Users.Add(user);

                list_users.Items.Add(user);

                txbx_name.Text = "";
                txbx_surname.Text ="";
                txbx_password.Text ="";
                txbx_email.Text = "";

                MessageBox.Show("Istifadəçi uğurla əlavə olundu.");
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
          
        }

        private Guid EditUserId;

        private void list_users_DoubleClick(object sender, EventArgs e)
        {
            User user = (User)list_users.SelectedItem;

            txbx_name.Text = user.Name;
            txbx_surname.Text = user.Surname;
            txbx_password.Text = user.Password;
            txbx_email.Text = user.Email;
            cmbx_status.SelectedItem = Enum.GetName(typeof(UserRole), user.UserRole);

            EditUserId = user.Id;

            btn_edit.Show();
            btn_delete.Show();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (Session.User.UserRole == UserRole.Moderator)
            {
                MessageBox.Show("Moderator istifadəçinin məlumatlarını dəyişə bilməz!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Əminsinizmi?", "Diqqət", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //Istifadecini tap
                User user = ApplicationDbContext.Users
                                                    .Where(c => c.Id == EditUserId)
                                                        .FirstOrDefault();

                if (user == null)
                {
                    MessageBox.Show("Belə bir istifadəçi yoxdur.");
                    return;
                }

                //Update ele

                user.Name = txbx_name.Text;
                user.Surname = txbx_surname.Text;
                user.Email = txbx_email.Text;
                user.Password = txbx_password.Text;

                UserRole role = (UserRole)Enum.Parse(typeof(UserRole), cmbx_status.SelectedItem.ToString());

                user.UserRole = role;

                //List viewnu bosalt
                list_users.Items.Clear();

                //List viewnu doldur
                List<User> users = ApplicationDbContext.Users;

                foreach (User u in users)
                {
                    list_users.Items.Add(u);
                }

                MessageBox.Show("Istifadəçi məlumatları düzəliş edildi!");


                txbx_name.Text = "";
                txbx_surname.Text = "";
                txbx_password.Text = "";
                txbx_email.Text = "";


                btn_delete.Hide();
                btn_edit.Hide();

            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
           
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (Session.User.UserRole == UserRole.Moderator)
            {
                MessageBox.Show("Moderator istifadəçi silə bilməz!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Əminsinizmi?", "Diqqət", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                User user = ApplicationDbContext.Users.Where(c => c.Id == EditUserId).FirstOrDefault();

                if (user == null)
                {
                    MessageBox.Show("Belə bir istifadəçi yoxdur.");
                    return;
                }

                ApplicationDbContext.Users.Remove(user);
                MessageBox.Show("Istifadəçi uğurla silindi!");


                txbx_name.Text = "";
                txbx_surname.Text = "";
                txbx_password.Text = "";
                txbx_email.Text = "";

                btn_delete.Hide();
                btn_edit.Hide();

                list_users.Items.Remove(user);
            }
            else
            {
                return;
            }
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            Session.User = null;
            Session.MainForm.Show();
            this.Hide();
        }
    }
}
