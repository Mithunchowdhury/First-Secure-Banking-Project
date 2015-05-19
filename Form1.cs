using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerAndAccountInformation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Account aAccount = new Account();

            aAccount.name = customerNameTextBox.Text;
            aAccount.email = emailTextBox.Text;
            aAccount.accoutNumber = accountNumberTextBox.Text;
            aAccount.OpeningDate = openingDateTextBox.Text;
            aAccount.balance = 0;

            if (aAccount.name == "" || aAccount.email == "" || aAccount.accoutNumber == "" || aAccount.OpeningDate == "")

            {
                MessageBox.Show("Find out all the information");
            }

            else if (aAccount.accoutNumber.Length < 8)
            {
                MessageBox.Show("Account number must be 7 charter");
            }
            else
            {
                string connectring = "SERVER=MITHUN-PC;Database=CustomerInfoDB;Integrated Security=true";
                SqlConnection connection = new SqlConnection(connectring);
                string query = "INSERT INTO accounts VALUES('" + aAccount.name + "','" + aAccount.email + "','" +
                               aAccount.accoutNumber + "', '" + aAccount.OpeningDate + "','" + aAccount.balance + "')";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected>0)
                {

                    MessageBox.Show("Customer Information Saved Successfully");
                }
                else
                {
                    MessageBox.Show("Operation Failed");
                }

            }
        }

        private void depositButton_Click(object sender, EventArgs e)
        {
            Account aAccount = new Account();
            aAccount.accoutNumber = accountNumberBox.Text;
            aAccount.balance = double.Parse(amountTextBox.Text);
            float balance = GetBalance(aAccount.accoutNumber);

            if (aAccount.accoutNumber == "" || amountTextBox.Text == "")
            {
                MessageBox.Show("fill out the boxes perfrom the operation");


            }
            else if (!IsAccountNumberExists(aAccount.accoutNumber))
            {
                MessageBox.Show("This account number doesn't exist in your bank");
            }
            else
            {



                if (aAccount.balance <= 0)
                {
                    MessageBox.Show("You can't deposit zero or less than zero amount");
                }
                else
                {
                    customerListView.Items.Clear();


                    
                    float result = balance+ (float) aAccount.balance;

                    string connectring = "SERVER=MITHUN-PC;Database=CustomerInfoDB;Integrated Security=true";
                    SqlConnection connection = new SqlConnection(connectring);

                    string query = "UPDATE  accounts SET balance='" + result + "'WHERE Account_number='" +
                                   aAccount.accoutNumber + "'";

                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Deposit Successfully");
                        accountNumberBox.Text = "";
                        amountTextBox.Text = "";
                        GetDataInListView();
                    }
                    else
                    {
                        MessageBox.Show("Operation Failed");
                    }

                }

            }
        }

        private bool IsAccountNumberExists(string accountno)
        {
            bool accountexists = false;
            string connectring ="SERVER=MITHUN-PC;Database=CustomerInfoDB;Integrated Security=true";
            SqlConnection connection = new SqlConnection(connectring);

            string query = "SELECT Account_number FROM accounts WHERE Account_number='" + accountno + "'";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                accountexists = true;
            }

            reader.Close();
            connection.Close();

            return accountexists;

        }

        private void withdrawButton_Click(object sender, EventArgs e)
        {
            Account account = new Account();

            account.accoutNumber = accountNumberBox.Text;
            account.balance = double.Parse(amountTextBox.Text);


            if (account.accoutNumber == "" || amountTextBox.Text == "")
            {
                MessageBox.Show("Fill out the boxes to perform operation");
            }

            else if (!IsAccountNumberExists(account.accoutNumber))
            {
                MessageBox.Show("This account number doesn't exist in your bank");
            }

            else
            {


                float balance = GetBalance(account.accoutNumber);


                if (account.balance <= 0)
                {
                    MessageBox.Show("You can't withdraw zero or less than zero amount");
                }

                else if (balance < account.balance)
                {
                    MessageBox.Show("U don't have enough amount to perform this operation");
                }

                else
                {
                    customerListView.Items.Clear();


                    float result = balance - (float) account.balance;
                    string connectring = "SERVER=MITHUN-PC;Database=CustomerInfoDB;Integrated Security=true";

                    SqlConnection connection = new SqlConnection(connectring);

                    string query = "UPDATE  accounts SET balance='" + result + "'WHERE Account_number='" +
                                   account.accoutNumber + "'";

                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("WithDraw Money Successfully");
                        accountNumberBox.Text = "";
                        amountTextBox.Text = "";
                        GetDataInListView();
                    }
                    else
                    {
                        MessageBox.Show("Operation Failed");
                    }
                }

            }



        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            Account aAccount = new Account();
          
            List<Account> accounts = new List<Account>();

            aAccount.accoutNumber = accountNumbertext.Text;
            string connectring = "SERVER=MITHUN-PC;Database=CustomerInfoDB;Integrated Security=true";

            SqlConnection connection = new SqlConnection(connectring);

            string query = "SELECT Customer_name,Account_number,Opening_date,balance FROM Accounts WHERE (Account_number LIKE'" + aAccount.accoutNumber +"%')";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader(); 

            while (reader.Read())
            {
                Account account1= new Account();

                account1.name = reader["Customer_name"].ToString();

                account1.accoutNumber = reader["Account_number"].ToString();

                account1.OpeningDate = reader["Opening_date"].ToString();

                account1.balance = float.Parse(reader["Balance"].ToString());

                accounts.Add(account1);
            }

            reader.Close();
            connection.Close();

            customerListView.Items.Clear();

            foreach (var account1 in accounts)
            {
                ListViewItem item = new ListViewItem();

                item.Text = account1.name;
                item.SubItems.Add(account1.accoutNumber);
                item.SubItems.Add(account1.OpeningDate);
                item.SubItems.Add(account1.balance.ToString());

                customerListView.Items.Add(item);
            }

        }
         private void GetTextBoxesClear()
        {
            customerNameTextBox.Text = string.Empty;
            emailTextBox.Text = string.Empty;
            accountNumberTextBox.Text = string.Empty;
            openingDateTextBox.Text = string.Empty;
        }
         private void GetDataInListView()
        {
            List< Account >accounts=new List<Account>();
            string connectring = "SERVER=MITHUN-PC;Database=CustomerInfoDB;Integrated Security=true";

            SqlConnection connection = new SqlConnection(connectring);

            string query = "SELECT Customer_name,Account_number,Opening_date,balance FROM accounts";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Account aAccount = new Account();

                aAccount.name = reader["Customer_name"].ToString();
                
                aAccount.accoutNumber= reader["Account_number"].ToString();

                aAccount.OpeningDate = reader["Opening_date"].ToString();

                aAccount.balance = float.Parse(reader["Balance"].ToString());

                accounts.Add(aAccount);

            }

            reader.Close();
            connection.Close();


            foreach (var account in accounts)
            {
                ListViewItem item=new ListViewItem();

                item.Text = account.name;
                item.SubItems.Add(account.accoutNumber);
                item.SubItems.Add(account.OpeningDate);
                item.SubItems.Add(account.balance.ToString());

                customerListView.Items.Add(item);
            }

        }


         private float  GetBalance(string accountNo)
        {
            float balance = 0;
            string connectring = "SERVER=MITHUN-PC;Database=CustomerInfoDB;Integrated Security=true";
             SqlConnection connection = new SqlConnection(connectring);

            string query = "SELECT balance FROM accounts WHERE Account_number='" + accountNo + "'";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                balance = float.Parse(reader["balance"].ToString());
            }

            reader.Close();
            connection.Close();

            return balance;

        }



        }

    }

         


            










   
               

    

