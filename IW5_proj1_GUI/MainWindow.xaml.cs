using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using IW5_proj1;

namespace IW5_proj1_GUI
{
    /// <summary>
    /// Interaction logic for CustomerList.xaml
    /// </summary>
    public partial class CustomerList : Window
    {
        #region Class Variables
        private CustomersCollection m_customers;
        private List<object> m_checkedToDelete;
        private XMLWorker<CustomersCollection> m_worker;
        
        private bool m_editMode = false;
        private bool m_dataVal = true;
        
        private string m_title;
        
        private string m_ex;
        private int m_customerCount = 0;
        #endregion

        #region Constructors
        public CustomerList()
        {
            InitializeComponent();
            
            //create collection for customers
            m_customers = new CustomersCollection();

            //create list for delete
            m_checkedToDelete = new List<object>();

            //new xml worker, use default file Customers.xml in ./ dir
            m_worker = new XMLWorker<CustomersCollection>();

            //load default file
            this.m_customers = m_worker.Load(out m_ex);
            m_customerCount = m_customers.Count;

            m_title = @"Customers.xml";

            //set binding data source
            customersFromFile.ItemsSource = m_customers;

            //update window title [data file]
            this.Title += " | Active File: " + m_title;

            this.Show();
        }
        #endregion

        #region TextBoxOperations
        /// <summary>
        /// Resets the text boxes.
        /// </summary>
        private void resetTextBoxes()
        {
            txtBoxAddAge.Clear();
            txtBoxAddCity.Clear();
            txtBoxAddCountry.Clear();
            txtBoxAddName.Clear();
            txtBoxAddStreet.Clear();
            txtBoxAddSurName.Clear();
            txtBoxAddZipcode.Clear();
        }

        /// <summary>
        /// Fills the text boxes.
        /// </summary>
        private void fillTextBoxes()
        {
            Person selectedPerson = (Person)customersFromFile.SelectedItem;

            if (selectedPerson != null)
            {
                txtBoxAddName.Text = selectedPerson.Name;
                txtBoxAddSurName.Text = selectedPerson.Surname;

                if (selectedPerson.m_sex == "Male")
                    comboBoxAddSex.SelectedIndex = 0;
                else
                    comboBoxAddSex.SelectedIndex = 1;

                txtBoxAddAge.Text = selectedPerson.m_age.ToString();

                txtBoxAddStreet.Text = selectedPerson.m_address.m_street;
                txtBoxAddCity.Text = selectedPerson.m_address.m_city;
                txtBoxAddZipcode.Text = selectedPerson.m_address.m_zipcode;
                txtBoxAddCountry.Text = selectedPerson.m_address.m_country;
            }
        }

        /// <summary>
        /// Validates the text boxes.
        /// </summary>
        private void validateTextBoxes()
        {
            m_dataVal = true;
            uint number = 0;
            uint zipcode = 0;

            Brush redBrush = new SolidColorBrush(Colors.Red);
            Brush blackBrush = new SolidColorBrush(Colors.Black);

            if (string.IsNullOrEmpty(txtBoxAddName.Text) || !Regex.IsMatch(txtBoxAddName.Text, @"^[a-zA-Z]+$"))
            {
                labelName.Foreground = redBrush;
                m_dataVal = false;
            }
            else
            {
                labelName.Foreground = blackBrush;
            }
            if (string.IsNullOrEmpty(txtBoxAddSurName.Text) || !Regex.IsMatch(txtBoxAddSurName.Text, @"^[a-zA-Z]+$"))
            {
                labelSurName.Foreground = redBrush;
                m_dataVal = false;
            }
            else
            {
                labelSurName.Foreground = blackBrush;
            }
                

            if (string.IsNullOrEmpty(txtBoxAddStreet.Text))
            {
                labelStreet.Foreground = redBrush;
                m_dataVal = false;
            }
            else
            {
                labelStreet.Foreground = blackBrush;
            }

            if (string.IsNullOrEmpty(txtBoxAddCity.Text) || !Regex.IsMatch(txtBoxAddCity.Text, @"^[a-z A-Z]+$"))
            {
                labelCity.Foreground = redBrush;
                m_dataVal = false;
            }
            else
            {
                labelCity.Foreground = blackBrush;
            }
            
            if (string.IsNullOrEmpty(txtBoxAddZipcode.Text) || !uint.TryParse(txtBoxAddZipcode.Text.Trim(), out zipcode))
            {
                labelZipcode.Foreground = redBrush;

                m_dataVal = false;
            }
            else
            {
                labelZipcode.Foreground = blackBrush;
            }
            
            if (string.IsNullOrEmpty(txtBoxAddCountry.Text))
            {
                labelCountry.Foreground = redBrush;
                m_dataVal = false;
            }
            else
            {
                labelCountry.Foreground = blackBrush;
            }

            if(!uint.TryParse(txtBoxAddAge.Text.Trim(), out number))
            {
                labelAge.Foreground = redBrush;
                m_dataVal = false;
            }
            else
            {
                labelAge.Foreground = blackBrush;
            }
        }
        #endregion

        /// <summary>
        /// Handles the Click event of the buttonAddCustomer control. Add/edit customer
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            validateTextBoxes();

            if (m_dataVal)
            {
                if (m_editMode == true)
                {
                    uint newPersonAge = Convert.ToUInt32(txtBoxAddAge.Text);
                    newPersonAge = uint.Parse(txtBoxAddAge.Text);

                    int index = customersFromFile.SelectedIndex;

                    m_customers[index].Name = txtBoxAddName.Text;
                    m_customers[index].Surname = txtBoxAddSurName.Text;
                    m_customers[index].m_age = newPersonAge;

                    //check for person sex
                    if (comboBoxAddSex.SelectedIndex == 0)
                        m_customers[index].m_sex = "Male";
                    else
                        m_customers[index].m_sex = "Female";

                    m_customers[index].m_address.m_street = txtBoxAddStreet.Text;
                    m_customers[index].m_address.m_city = txtBoxAddCity.Text;
                    m_customers[index].m_address.m_zipcode = txtBoxAddZipcode.Text;
                    m_customers[index].m_address.m_country = txtBoxAddCountry.Text;

                    CollectionViewSource.GetDefaultView(this.m_customers).Refresh();

                    resetTextBoxes();
                }
                else
                {
                    Person.T_Sex newPersonSex;

                    uint newPersonAge = Convert.ToUInt32(txtBoxAddAge.Text);
                    newPersonAge = uint.Parse(txtBoxAddAge.Text);

                    //check for person sex
                    if (comboBoxAddSex.SelectedIndex == 0)
                        newPersonSex = Person.T_Sex.T_MALE;
                    else
                        newPersonSex = Person.T_Sex.T_FEMALE;

                    //create Person object
                    var newPerson = new Person(txtBoxAddName.Text, txtBoxAddSurName.Text,
                                            newPersonSex, newPersonAge,
                                            txtBoxAddStreet.Text, txtBoxAddCity.Text, txtBoxAddZipcode.Text, txtBoxAddCountry.Text, m_customerCount);

                    //add customer to group (list)
                    m_customers.Add(newPerson);
                    m_customerCount++;
                }
            }
            else
                MessageBox.Show("Missing some required data or wrong data format!");
        }

        /// <summary>
        /// Handles the Checked event of the checkBoxDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkBoxDelete_Checked(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            var item = cb.DataContext;

            if(customersFromFile.SelectionMode == SelectionMode.Multiple || (customersFromFile.SelectedItems.Count == 0))
                customersFromFile.SelectedItems.Add(item);
        }

        /// <summary>
        /// Handles the Click event of the buttonDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            foreach (object o in customersFromFile.SelectedItems)
            {
                if (o != null)
                    m_checkedToDelete.Add(o);
            }

            for (int i = m_checkedToDelete.Count - 1; i >= 0; i--)
            {
                m_customers.RemoveAt(customersFromFile.Items.IndexOf(m_checkedToDelete[i]));
                
                m_customerCount = m_customers.Count;
            }

            //reset list
            m_checkedToDelete.Clear();
        }

        /// <summary>
        /// Delete all customers.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            m_customers.Clear();
        }

        /// <summary>
        /// Handles the Unchecked event of the checkBoxDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxDelete_Unchecked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            var item = cb.DataContext;

            customersFromFile.SelectedItems.Remove(item);
        }

        /// <summary>
        /// Fills textboxes on listview selection change.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void customersFromFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person selectedPerson = (Person)customersFromFile.SelectedItem;

            if (rbtnEdit.IsChecked == true)
            {
                fillTextBoxes();
            }
                
        }

        /// <summary>
        /// Textboxes are set for editting customers' details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void rbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //single mode selection
            if (this.rbtnEdit.IsChecked == true)
            {
                customersFromFile.SelectionMode = SelectionMode.Single;
                checkBoxMultiSelect.IsChecked = false;
            }

            //set working mode to edit
            this.m_editMode = true;
            this.buttonAddCustomer.Content = "Edit";

            fillTextBoxes();
            validateTextBoxes();
        }

        /// <summary>
        /// Handles the Click event of the rbtnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void rbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.m_editMode = false;
            this.buttonAddCustomer.Content = "Add";
        }

        /// <summary>
        /// Opens file dialog for loading XML files.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fileMenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Customers"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string path = dlg.FileName;
                string filename = System.IO.Path.GetFileName(path);

                this.Title = "Customers | Active File: " + filename;

                m_worker = new XMLWorker<CustomersCollection>(path);
                this.m_customers = m_worker.Load(out m_ex);
                this.m_customerCount = m_customers.Count;

                if (m_ex != "")
                    MessageBox.Show(m_ex);

                //set binding data source
                customersFromFile.ItemsSource = m_customers;
            }
        }

        /// <summary>
        /// Handles the Click event of the fileMenuItemSaveAs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void fileMenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Customers"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                m_title = dlg.SafeFileName;

                this.Title = "Customers | Active File: " + m_title;
                m_worker.setOutputFile(filename);

                this.m_worker.Save(m_customers);
            }
        }

        /// <summary>
        /// Clears all textboxes when clicked on Reset button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            resetTextBoxes();
        }

        private void checkBoxMultiSelect_Unchecked(object sender, RoutedEventArgs e)
        {
            customersFromFile.SelectionMode = SelectionMode.Single;
        }

        private void checkBoxMultiSelect_Checked(object sender, RoutedEventArgs e)
        {
            customersFromFile.SelectionMode = SelectionMode.Multiple;
        }

        private void fileMenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            this.m_worker.Save(m_customers);
        }
    }
}