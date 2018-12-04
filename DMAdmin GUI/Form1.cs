using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admin_API_SDK;
using Admin_API_SDK.Models;

namespace DMAdmin_GUI
{
    public partial class Form1 : Form
    {
        DMAdmin admin = new DMAdmin();
        public Form1()
        {
            InitializeComponent();
        }

        private void StartWaitCursor()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
        }

        private void EndWaitCursor()
        {
            this.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
        }

        private async void getSessionKeyButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            string baseUrl = "";

            string encryptionKey = "";
            string companyAutomationId = "";
            string email = "";

            string userId = "";
            int uid = 0;
            string singleSignOnId = "";
            int temp = 0;

            Authentication.GetSessionKeyRequest user = new Authentication.GetSessionKeyRequest();

            if (authenticationBaseURLTextBox.Text == "")
            {
                admin = new DMAdmin();
            }
            else
            {
                baseUrl = authenticationBaseURLTextBox.Text;
                admin = new DMAdmin(baseUrl);
            }

            if (authenticationEncryptionKeyTextBox.Text == "")
            {
                authenticationRichTextBox.Text = "Encryption Key cannot be blank.";

                EndWaitCursor();
                return;
            }
            else
            {
                encryptionKey = authenticationEncryptionKeyTextBox.Text;

                if (authenticationCompanyAutomationIdTextBox.Text == "")
                {
                    authenticationRichTextBox.Text = "Automation Id cannot be blank.";

                    EndWaitCursor();
                    return;
                }
                else
                {
                    companyAutomationId = authenticationCompanyAutomationIdTextBox.Text;

                    if (authenticationEmailTextBox.Text == "")
                    {
                        authenticationRichTextBox.Text = "Email cannot be blank.";

                        EndWaitCursor();
                        return;
                    }
                    else
                    {
                        email = authenticationEmailTextBox.Text;

                        if (authenticationUserIdTextBox.Text == "")
                        {
                            userId = "";
                        }
                        else
                        {
                            userId = authenticationUserIdTextBox.Text;
                        }

                        if (authenticationUIDTextBox.Text == "")
                        {
                            uid = 0;
                        }
                        else
                        {
                            if (int.TryParse(authenticationUIDTextBox.Text, out temp) == false)
                            {
                                authenticationRichTextBox.Text = "UID must be an integer";

                                EndWaitCursor();
                                return;
                            }
                            else
                            {
                                uid = int.Parse(authenticationUIDTextBox.Text);
                            }
                        }

                        if (authenticationSingleSignOnIdTextBox.Text == "")
                        {
                            singleSignOnId = "";
                        }
                        else
                        {
                            singleSignOnId = authenticationSingleSignOnIdTextBox.Text;
                        }

                        user = new Authentication.GetSessionKeyRequest
                        {
                            Identity = new Authentication.IdentityObject
                            {
                                UID = uid,
                                Email = email,
                                UserId = userId,
                                SingleSignOnId = singleSignOnId
                            },

                            TimeStamp = DateTime.UtcNow
                        };

                        try
                        {
                            string sessionKey = await admin.Authentication.GetSessionKey(encryptionKey, email, companyAutomationId);
                            authenticationGetSessionKeyButton.Enabled = false;
                            authenticationRichTextBox.Text = string.Format("SessionKey: {0}" + Environment.NewLine, sessionKey);

                            authenticationBaseURLTextBox.ResetText();
                            authenticationEncryptionKeyTextBox.ResetText();
                            authenticationUserIdTextBox.ResetText();
                            authenticationCompanyAutomationIdTextBox.ResetText();
                            authenticationUIDTextBox.ResetText();
                            authenticationEmailTextBox.ResetText();
                            authenticationSingleSignOnIdTextBox.ResetText();
                            authenticationLogOutButton.Enabled = true;

                            EndWaitCursor();
                        }
                        catch (Exception ex)
                        {
                            authenticationRichTextBox.Text = ex.Message;

                            EndWaitCursor();
                            return;
                        }
                    }
                }
            }
        }

        //Broken method
        private async void getEncryptionKeyButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            string email = "";
            string userId = "";
            int uid = 0;
            string singleSignOnId = "";
            int temp = 0;

            Authentication.IdentityObject2 request = new Authentication.IdentityObject2();
            string response = "";

            if (authenticationEmailTextBox.Text == "")
            {
                authenticationRichTextBox.Text = "Email cannot be blank.";

                EndWaitCursor();
                return;
            }
            else
            {
                email = authenticationEmailTextBox.Text;

                userId = authenticationUserIdTextBox.Text;

                if (authenticationUIDTextBox.Text == "")
                {
                    uid = 0;
                }
                else
                {
                    if (int.TryParse(authenticationUIDTextBox.Text, out temp) == false)
                    {
                        authenticationRichTextBox.Text = "UID must be an integer.";

                        EndWaitCursor();
                        return;
                    }
                    else
                    {
                        uid = int.Parse(authenticationUIDTextBox.Text);
                    }
                }
                singleSignOnId = authenticationSingleSignOnIdTextBox.Text;
            }

            request = new Authentication.IdentityObject2
            {
                //UserId = userId,
                //UID = uid,
                Email = email
                //SingleSignOnId = singleSignOnId
            };

            try
            {
                response = await admin.Authentication.GetEncryptionKey(request);

                EndWaitCursor();
            }
            catch (Exception ex)
            {
                authenticationRichTextBox.Text = ex.Message;

                EndWaitCursor();
                return;
            }

            if (response != null)
            {
                authenticationRichTextBox.Text = string.Format("New Encryption Key: {0}", response);
            }
            else
            {
                authenticationRichTextBox.Text = "Response is null.";

                EndWaitCursor();
                return;
            }
        }

        private async void listUserAccountsButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();
            listAccountRichTextBox.ResetText();

            int userTypeId = 0;
            int pageNumber = 0;
            string companyId = "";
            string orderBy = "";
            string filter = "";
            int temp = 0;

            Account.ListUserAccountsRequest request = new Account.ListUserAccountsRequest();
            Account.ListUserAccountsResponse response = new Account.ListUserAccountsResponse();

            if (listPageNumberUpDown.Value < 0)
            {
                listAccountRichTextBox.Text = "Page number cannot be negative.";

                EndWaitCursor();
                return;
            }
            else
            { 
                pageNumber = Decimal.ToInt32(listPageNumberUpDown.Value);

                if (listOrderByComboBox.Text == "")
                {
                    listAccountRichTextBox.Text = "OrderBy cannot be blank.";

                    EndWaitCursor();
                    return;
                }
                else
                {
                    orderBy = listOrderByComboBox.Text;

                    if (listFilterTextBox.Text == "")
                    {
                        filter = "";
                    }
                    else
                    {
                        filter = listFilterTextBox.Text;
                    }

                    if (listUserTypeIdTextBox.Text == "")
                    {
                        userTypeId = 0;
                    }
                    else
                    {
                        if (int.TryParse(listUserTypeIdTextBox.Text, out temp) == false)
                        {
                            listAccountRichTextBox.Text = "UserId must be an integer.";

                            EndWaitCursor();
                            return;
                        }
                        else
                        {
                            userTypeId = int.Parse(listUserTypeIdTextBox.Text);
                        }
                    }

                    if (listCompanyIdTextBox.Text == "")
                    {
                        companyId = "0";
                    }
                    else
                    {
                        companyId = listCompanyIdTextBox.Text;
                    }
                }

                request = new Account.ListUserAccountsRequest
                {
                    UserTypeId = userTypeId,
                    PageNumber = pageNumber,
                    CompanyId = companyId,
                    OrderBy = orderBy,
                    Filter = filter
                };

                try
                {
                    response = await admin.Account.ListUserAccounts(request);

                    listUserTypeIdTextBox.ResetText();
                    listPageNumberUpDown.Value = 1;
                    listCompanyIdTextBox.ResetText();
                    listOrderByComboBox.ResetText();
                    listFilterTextBox.ResetText();

                    EndWaitCursor();
                }
                catch (Exception ex)
                {
                    listAccountRichTextBox.Text = ex.Message;

                    EndWaitCursor();
                    return;
                }
            }

            if (response != null)
            {
                listAccountRichTextBox.Text += string.Format("\tAccounts:" + Environment.NewLine);
                for (int i = 0; i < response.Accounts.Count(); i++)
                {
                    listAccountRichTextBox.Text += string.Format("\t\tUniqueId: {0}" + Environment.NewLine, response.Accounts[i].UniqueId);
                    listAccountRichTextBox.Text += string.Format("\t\tAid: {0}" + Environment.NewLine, response.Accounts[i].Aid);
                    listAccountRichTextBox.Text += string.Format("\t\tCompanyId: {0}" + Environment.NewLine, response.Accounts[i].CompanyId);
                    listAccountRichTextBox.Text += string.Format("\t\tCreated: {0}" + Environment.NewLine, response.Accounts[i].Created);
                    listAccountRichTextBox.Text += string.Format("\t\tLastNotice: {0}" + Environment.NewLine, response.Accounts[i].LastNotice);
                    listAccountRichTextBox.Text += string.Format("\t\tDiskSpaceUsed: {0}" + Environment.NewLine, response.Accounts[i].DiskSpaceUsed);
                    listAccountRichTextBox.Text += string.Format("\t\tEmailId: {0}" + Environment.NewLine, response.Accounts[i].EmailId);
                    listAccountRichTextBox.Text += string.Format("\t\tEmail: {0}" + Environment.NewLine, response.Accounts[i].Email);
                    listAccountRichTextBox.Text += string.Format("\t\tEmployeeId: {0}" + Environment.NewLine, response.Accounts[i].EmployeeId);
                    listAccountRichTextBox.Text += string.Format("\t\tFirstName: {0}" + Environment.NewLine, response.Accounts[i].FirstName);
                    listAccountRichTextBox.Text += string.Format("\t\tLastName: {0}" + Environment.NewLine, response.Accounts[i].LastName);
                    listAccountRichTextBox.Text += string.Format("\t\tLastLogin: {0}" + Environment.NewLine, response.Accounts[i].LastLogin);
                    listAccountRichTextBox.Text += string.Format("\t\tMessagesReceived: {0}" + Environment.NewLine, response.Accounts[i].MessagesReceived);
                    listAccountRichTextBox.Text += string.Format("\t\tMessagesSent: {0}" + Environment.NewLine, response.Accounts[i].MessagesSent);
                    listAccountRichTextBox.Text += string.Format("\t\tUserId: {0}" + Environment.NewLine, response.Accounts[i].UserId);
                    listAccountRichTextBox.Text += string.Format("\t\tUserTypeId: {0}" + Environment.NewLine, response.Accounts[i].UserTypeId);
                    listAccountRichTextBox.Text += string.Format("\t\tErrors: " + Environment.NewLine);

                    for (int j = 0; j < response.Accounts[i].Errors.Count; j++)
                    {
                        if (response.Accounts[i].Errors.ContainsKey("Email"))
                        {
                            List<string> emailListErrors = response.Accounts[i].Errors["Email"];
                            foreach (string emailError in emailListErrors)
                            {
                                listAccountRichTextBox.Text += string.Format("\t\t\t{0}" + Environment.NewLine, emailError);
                            }
                        }
                        if (response.Accounts[i].Errors.ContainsKey("UserTypeId"))
                        {
                            List<string> userTypeIdListErrors = response.Accounts[i].Errors["UserTypeId"];
                            foreach (string userTypeError in userTypeIdListErrors)
                            {
                                listAccountRichTextBox.Text += string.Format("\t\t\t{0}" + Environment.NewLine, userTypeError);
                            }
                        }
                    }

                    listAccountRichTextBox.Text += Environment.NewLine;
                }
                listAccountRichTextBox.Text += string.Format("\tCount: {0}" + Environment.NewLine, response.Count);
                listAccountRichTextBox.Text += string.Format("\tCurrentPage: {0}" + Environment.NewLine, response.CurrentPage);
                listAccountRichTextBox.Text += string.Format("\tTotalPages: {0}" + Environment.NewLine, response.TotalPages);
                listAccountRichTextBox.Text += string.Format("\tTotalUsers: {0}" + Environment.NewLine, response.TotalUsers);
                listAccountRichTextBox.Text += string.Format("\tPageSize: {0}" + Environment.NewLine, response.PageSize);
            }
            else
            {
                listAccountRichTextBox.Text = "Response is null.";
            }
        }

        private async void createUserButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();
            createUserRichTextBox.ResetText();

            string email = "";
            string userId = "";
            string singleSignOnId = "";
            string password = "";
            string firstName = "";
            string lastName = "";
            string phone = "";
            int companyId = 0;
            string employeeId = "";
            string miscellaneous = "";
            bool disabled = false;
            int userTypeId = 0;
            bool receiveOffers = false;
            int temp = 0;

            Account.CreateUserRequest request = new Account.CreateUserRequest();
            Account.CreateUserResponse response = new Account.CreateUserResponse();

            if (createEmailTextBox.Text == "")
            {
                createUserRichTextBox.Text = "Email cannot be blank.";

                EndWaitCursor();
                return;
            }
            else
            {
                email = createEmailTextBox.Text;

                if (createUserTypeIdTextBox.Text == "")
                {
                    createUserRichTextBox.Text = "UserTypeId cannot be blank.";

                    EndWaitCursor();
                    return;
                }
                else
                {
                    if (int.TryParse(createUserTypeIdTextBox.Text, out temp) == false)
                    {
                        createUserRichTextBox.Text = "UserTypeId must be an integer";

                        EndWaitCursor();
                        return;
                    }
                    else
                    {
                        userTypeId = int.Parse(createUserTypeIdTextBox.Text);

                        userId = createUserIdTextBox.Text;
                        singleSignOnId = createSingleSignOnIdTextBox.Text;
                        password = createPasswordTextBox.Text;
                        firstName = createFirstNameTextBox.Text;
                        lastName = createLastNameTextBox.Text;
                        phone = createPhoneNumberTextBox.Text;

                        if (createCompanyIdTextBox.Text == "")
                        {
                            companyId = int.Parse("0");
                        }
                        else
                        {
                            companyId = int.Parse(createCompanyIdTextBox.Text);
                        }

                        employeeId = createEmployeeIdTextBox.Text;
                        miscellaneous = createMiscellaneousTextBox.Text;

                        if (createDisabledCheckBox.Checked == true)
                        {
                            disabled = true;
                        }
                        else
                        {
                            disabled = false;
                        }

                        if (createReceiveOffersCheckBox.Checked == true)
                        {
                            receiveOffers = true;
                        }
                        else
                        {
                            receiveOffers = false;
                        }

                        request = new Account.CreateUserRequest
                        {
                            Email = email,
                            UserId = userId,
                            SingleSignOnId = singleSignOnId,
                            Password = password,
                            FirstName = firstName,
                            LastName = lastName,
                            Phone = phone,
                            CompanyId = companyId,
                            EmployeeId = employeeId,
                            Miscellaneous = miscellaneous,
                            Disabled = disabled,
                            UserTypeId = userTypeId,
                            ReceiveOffers = receiveOffers
                        };

                        try
                        {
                            response = await admin.Account.CreateUserAccount(request);

                            createEmailTextBox.ResetText();
                            createUserIdTextBox.ResetText();
                            createSingleSignOnIdTextBox.ResetText();
                            createPasswordTextBox.ResetText();
                            createFirstNameTextBox.ResetText();
                            createLastNameTextBox.ResetText();
                            createPhoneNumberTextBox.ResetText();
                            createCompanyIdTextBox.ResetText();
                            createEmployeeIdTextBox.ResetText();
                            createMiscellaneousTextBox.ResetText();
                            createUserTypeIdTextBox.ResetText();
                            createDisabledCheckBox.Checked = false;
                            createReceiveOffersCheckBox.Checked = false;

                            EndWaitCursor();
                        }
                        catch (Exception ex)
                        {
                            createUserRichTextBox.Text = ex.Message;

                            EndWaitCursor();
                            return;
                        }
                    }
                }
            }

            if (response != null)
            {
                createUserRichTextBox.Text += string.Format("\tUniqueId: {0}" + Environment.NewLine, response.UniqueId);
                createUserRichTextBox.Text += string.Format("\tEmail: {0}" + Environment.NewLine, response.Email);
                createUserRichTextBox.Text += string.Format("\tUserId: {0}" + Environment.NewLine, response.UserId);
                createUserRichTextBox.Text += string.Format("\tSingleSignOnId: {0}" + Environment.NewLine, response.SingleSignOnId);
                createUserRichTextBox.Text += string.Format("\tPassword: {0}" + Environment.NewLine, response.Password);
                createUserRichTextBox.Text += string.Format("\tFirstName: {0}" + Environment.NewLine, response.FirstName);
                createUserRichTextBox.Text += string.Format("\tLastName: {0}" + Environment.NewLine, response.LastName);
                createUserRichTextBox.Text += string.Format("\tPhone: {0}" + Environment.NewLine, response.Phone);
                createUserRichTextBox.Text += string.Format("\tCompanyId: {0}" + Environment.NewLine, response.CompanyId);
                createUserRichTextBox.Text += string.Format("\tEmployeeId: {0}" + Environment.NewLine, response.EmployeeId);
                createUserRichTextBox.Text += string.Format("\tMiscellaneous: {0}" + Environment.NewLine, response.Miscellaneous);
                createUserRichTextBox.Text += string.Format("\tDisabled: {0}" + Environment.NewLine, response.Disabled);
                createUserRichTextBox.Text += string.Format("\tUserTypeId: {0}" + Environment.NewLine, response.UserTypeId);
                createUserRichTextBox.Text += string.Format("\tButtonUser: {0}" + Environment.NewLine, response.ButtonUser);
                createUserRichTextBox.Text += string.Format("\tReceiveOffers: {0}" + Environment.NewLine, response.ReceiveOffers);
                createUserRichTextBox.Text += string.Format("\tErrors: ");

                if (response.Errors.ContainsKey("Email"))
                {
                    List<string> emailListErrors = response.Errors["Email"];
                    foreach (string emailError in emailListErrors)
                    {
                        createUserRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, emailError);
                    }
                }
                if (response.Errors.ContainsKey("UserTypeId"))
                {
                    List<string> userTypeIdListErrors = response.Errors["UserTypeId"];
                    foreach (string userTypeError in userTypeIdListErrors)
                    {
                        createUserRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, userTypeError);
                    }
                }
            }
            else
            {
                createUserRichTextBox.Text = "Response is null.";
            }
        }

        private void showHiddenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (createPasswordVisibilityCheckBox.Checked)
            {
                createPasswordTextBox.PasswordChar = '\0';
            }
            else
            {
                createPasswordTextBox.PasswordChar = '*';
            }
        }

        private void encryptionKeyVisibilityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (authenticationAncryptionKeyVisibilityCheckBox.Checked)
            {
                authenticationEncryptionKeyTextBox.PasswordChar = '\0';
            }
            else
            {
                authenticationEncryptionKeyTextBox.PasswordChar = '*';
            }
        }

        private async void viewUserButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            viewUserRichTextBox.ResetText();

            int uid = 0;
            string email = "";
            string userId = "";
            string singleSignOnId = "";
            int temp = 0;

            Account.ViewUserRequest request = new Account.ViewUserRequest();
            Account.ViewUserResponse response = new Account.ViewUserResponse();

            if (viewUIDTextBox.Text == "")
            {
                viewUserRichTextBox.Text = "UID cannot be blank.";

                EndWaitCursor();
                return;
            }
            else
            {
                if (int.TryParse(viewUIDTextBox.Text, out temp) == false)
                {
                    viewUserRichTextBox.Text = "UID must be an integer.";

                    EndWaitCursor();
                    return;
                }
                else
                {
                    uid = int.Parse(viewUIDTextBox.Text);

                    if (viewEmailTextBox.Text == "")
                    {
                        viewUserRichTextBox.Text = "Email cannot be blank.";

                        EndWaitCursor();
                        return;
                    }
                    else
                    {
                        email = viewEmailTextBox.Text;

                        userId = viewUserIdTextBox.Text;
                        singleSignOnId = viewSingleSignOnIdTextBox.Text;

                        request = new Account.ViewUserRequest
                        {
                            UID = uid,
                            Email = email,
                            UserId = userId,
                            SingleSignOnId = singleSignOnId
                        };

                        try
                        {
                            response = await admin.Account.ViewUserAccount(request);
                            viewUserIdTextBox.ResetText();
                            viewEmailTextBox.ResetText();
                            viewUserIdTextBox.ResetText();
                            viewSingleSignOnIdTextBox.ResetText();

                            EndWaitCursor();
                        }
                        catch (Exception ex)
                        {
                            viewUserRichTextBox.Text = ex.Message;

                            EndWaitCursor();
                            return;
                        }
                    }
                }

                if (response != null)
                {
                    viewUserRichTextBox.Text += "User:" + Environment.NewLine;
                    viewUserRichTextBox.Text += string.Format("\tUniqueId: {0}" + Environment.NewLine, response.UniqueId);
                    viewUserRichTextBox.Text += string.Format("\tEmail: {0}" + Environment.NewLine, response.Email);
                    viewUserRichTextBox.Text += string.Format("\tUserId: {0}" + Environment.NewLine, response.UserId);
                    viewUserRichTextBox.Text += string.Format("\tSingleSignOnId: {0}" + Environment.NewLine, response.SingleSignOnId);
                    viewUserRichTextBox.Text += string.Format("\tPassword: {0}" + Environment.NewLine, response.Password);
                    viewUserRichTextBox.Text += string.Format("\tFirstName: {0}" + Environment.NewLine, response.FirstName);
                    viewUserRichTextBox.Text += string.Format("\tLastName: {0}" + Environment.NewLine, response.LastName);
                    viewUserRichTextBox.Text += string.Format("\tPhone: {0}" + Environment.NewLine, response.Phone);
                    viewUserRichTextBox.Text += string.Format("\tCompanyId: {0}" + Environment.NewLine, response.CompanyId);
                    viewUserRichTextBox.Text += string.Format("\tEmployeeId: {0}" + Environment.NewLine, response.EmployeeId);
                    viewUserRichTextBox.Text += string.Format("\tMiscellaneous: {0}" + Environment.NewLine, response.Miscellaneous);
                    viewUserRichTextBox.Text += string.Format("\tDisabled: {0}" + Environment.NewLine, response.Disabled);
                    viewUserRichTextBox.Text += string.Format("\tUserTypeId: {0}" + Environment.NewLine, response.UserTypeId);
                    viewUserRichTextBox.Text += string.Format("\tButtonUser: {0}" + Environment.NewLine, response.ButtonUser);
                    viewUserRichTextBox.Text += string.Format("\tReceiveOffers: {0}" + Environment.NewLine, response.ReceiveOffers);
                    viewUserRichTextBox.Text += string.Format("\tErrors: ");

                    if (response.Errors.ContainsKey("Email"))
                    {
                        List<string> emailListErrors = response.Errors["Email"];
                        foreach (string emailError in emailListErrors)
                        {
                            viewUserRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, emailError);
                        }
                    }
                    if (response.Errors.ContainsKey("UserTypeId"))
                    {
                        List<string> userTypeIdListErrors = response.Errors["UserTypeId"];
                        foreach (string userTypeError in userTypeIdListErrors)
                        {
                            viewUserRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, userTypeError);
                        }
                    }
                }
                else
                {
                    viewUserRichTextBox.Text = "Response is null.";
                }
            }
        }

        private async void updateUserButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            updateRichTextBox.ResetText();

            int uniqueId = 0;
            string email = "";
            string userId = "";
            string singleSignOnId = "";
            string password = "";
            string firstName = "";
            string lastName = "";
            string phone = "";
            int companyId = 0;
            string employeeId = "";
            string miscellaneous = "";
            bool disabled = false;
            int userTypeId = 0;
            bool buttonUser = false;
            bool receiveOffers = false;
            int temp = 0;

            Account.UpdateUserRequest request = new Account.UpdateUserRequest();
            Account.UpdateUserResponse response = new Account.UpdateUserResponse();

            if (updateUniqueIdTextBox.Text == "")
            {
                updateRichTextBox.Text = "UniqueId cannot be blank.";

                EndWaitCursor();
                return;
            }
            else
            {
                if (int.TryParse(updateUniqueIdTextBox.Text, out temp) == false)
                {
                    updateRichTextBox.Text = "UniqueId must be an integer.";

                    EndWaitCursor();
                    return;
                }
                else
                {
                    uniqueId = int.Parse(updateUniqueIdTextBox.Text);

                    if (updateEmailTextBox.Text == "")
                    {
                        updateRichTextBox.Text = "Email cannot be blank.";

                        EndWaitCursor();
                        return;
                    }
                    else
                    {
                        email = updateEmailTextBox.Text;

                        if (updateUserTypeIdTextBox.Text == "")
                        {
                            updateRichTextBox.Text = "UserTypeId cannot be blank.";

                            EndWaitCursor();
                            return;
                        }
                        else
                        {
                            if (int.TryParse(updateUserTypeIdTextBox.Text, out temp) == false)
                            {
                                updateRichTextBox.Text = "UserTypeId must be an integer.";

                                EndWaitCursor();
                                return;
                            }
                            else
                            {
                                userTypeId = int.Parse(updateUserTypeIdTextBox.Text);

                                userId = updateUserIdTextBox.Text;
                                singleSignOnId = updateSingleSignOnIdTextBox.Text;
                                password = updatePasswordTextBox.Text;
                                firstName = updateFirstNameTextBox.Text;
                                lastName = updateLastNameTextBox.Text;
                                phone = updatePhoneTextBox.Text;
                                
                                if (updateCompanyIdTextBox.Text == "")
                                {
                                    companyId = 0;
                                }
                                else
                                {
                                    if (int.TryParse(updateCompanyIdTextBox.Text, out temp) == false)
                                    {
                                        updateCompanyIdTextBox.Text = "CompanyId must be an integer.";

                                        EndWaitCursor();
                                        return;
                                    }
                                    else
                                    {
                                        companyId = int.Parse(updateCompanyIdTextBox.Text);
                                    }
                                }

                                employeeId = updateEmployeeIdTextBox.Text;
                                miscellaneous = updateMiscellaneousTextBox.Text;

                                if (updateDisabledCheckBox.Checked)
                                {
                                    disabled = true;
                                }
                                else
                                {
                                    disabled = false;
                                }

                                if (updateButtonUserCheckBox.Checked)
                                {
                                    buttonUser = true;
                                }
                                else
                                {
                                    buttonUser = false;
                                }

                                if (updateReceiveOffersCheckBox.Checked)
                                {
                                    receiveOffers = true;
                                }
                                else
                                {
                                    receiveOffers = false;
                                }

                                request = new Account.UpdateUserRequest
                                {
                                    UniqueId = uniqueId,
                                    Email = email,
                                    UserId = userId,
                                    SingleSignOnId = singleSignOnId,
                                    Password = password,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    Phone = phone,
                                    CompanyId = companyId,
                                    EmployeeId = employeeId,
                                    Miscellaneous = miscellaneous,
                                    Disabled = disabled,
                                    UserTypeId = userTypeId,
                                    ButtonUser = buttonUser,
                                    ReceiveOffers = receiveOffers
                                };

                                try
                                {
                                    response = await admin.Account.UpdateUserAccount(request);

                                    updateUniqueIdTextBox.ResetText();
                                    updateEmailTextBox.ResetText();
                                    updateUserIdTextBox.ResetText();
                                    updateSingleSignOnIdTextBox.ResetText();
                                    updatePasswordTextBox.ResetText();
                                    updatePasswordVisibilityCheckBox.Checked = false;
                                    updateFirstNameTextBox.ResetText();
                                    updateLastNameTextBox.ResetText();
                                    updatePhoneTextBox.ResetText();
                                    updateCompanyIdTextBox.ResetText();
                                    updateEmployeeIdTextBox.ResetText();
                                    updateMiscellaneousTextBox.ResetText();
                                    updateUserTypeIdTextBox.ResetText();
                                    updateDisabledCheckBox.Checked = false;
                                    updateReceiveOffersCheckBox.Checked = false;
                                    updateButtonUserCheckBox.Checked = false;

                                    EndWaitCursor();
                                }
                                catch (Exception ex)
                                {
                                    updateRichTextBox.Text = ex.Message;

                                    EndWaitCursor();
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            if (response != null)
            {
                updateRichTextBox.Text += "User:" + Environment.NewLine;
                updateRichTextBox.Text += string.Format("\tUniqueId: {0}" + Environment.NewLine, response.UniqueId);
                updateRichTextBox.Text += string.Format("\tEmail: {0}" + Environment.NewLine, response.Email);
                updateRichTextBox.Text += string.Format("\tUserId: {0}" + Environment.NewLine, response.UserId);
                updateRichTextBox.Text += string.Format("\tSingleSignOnId: {0}" + Environment.NewLine, response.SingleSignOnId);
                updateRichTextBox.Text += string.Format("\tPassword: {0}" + Environment.NewLine, response.Password);
                updateRichTextBox.Text += string.Format("\tFirstName: {0}" + Environment.NewLine, response.FirstName);
                updateRichTextBox.Text += string.Format("\tLastName: {0}" + Environment.NewLine, response.LastName);
                updateRichTextBox.Text += string.Format("\tPhone: {0}" + Environment.NewLine, response.Phone);
                updateRichTextBox.Text += string.Format("\tCompanyId: {0}" + Environment.NewLine, response.CompanyId);
                updateRichTextBox.Text += string.Format("\tEmployeeId: {0}" + Environment.NewLine, response.EmployeeId);
                updateRichTextBox.Text += string.Format("\tMiscellaneous: {0}" + Environment.NewLine, response.Miscellaneous);
                updateRichTextBox.Text += string.Format("\tDisabled: {0}" + Environment.NewLine, response.Disabled);
                updateRichTextBox.Text += string.Format("\tUserTypeId: {0}" + Environment.NewLine, response.UserTypeId);
                updateRichTextBox.Text += string.Format("\tButtonUser: {0}" + Environment.NewLine, response.ButtonUser);
                updateRichTextBox.Text += string.Format("\tReceiveOffers: {0}" + Environment.NewLine, response.ReceiveOffers);
                updateRichTextBox.Text += string.Format("\tErrors: ");

                if (response.Errors.ContainsKey("Email"))
                {
                    List<string> emailListErrors = response.Errors["Email"];
                    foreach (string emailError in emailListErrors)
                    {
                        updateRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, emailError);
                    }
                }
                if (response.Errors.ContainsKey("UserTypeId"))
                {
                    List<string> userTypeIdListErrors = response.Errors["UserTypeId"];
                    foreach (string userTypeError in userTypeIdListErrors)
                    {
                        updateRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, userTypeError);
                    }
                }
            }
            else
            {
                updateRichTextBox.Text = "Response is null";
                return;
            }
        }

        private void deleteUserButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            deleteRichTextBox.ResetText();

            int uid = 0;
            string email = "";
            string userId = "";
            string singleSignOnId = "";
            int temp = 0;

            Account.DeleteUserRequest request = new Account.DeleteUserRequest();

            if (deleteUIDTextBox.Text == "")
            {
                deleteRichTextBox.Text = "UID cannot be blank.";

                EndWaitCursor();
                return;
            }
            else
            {
                if (int.TryParse(deleteUIDTextBox.Text, out temp) == false)
                {
                    deleteRichTextBox.Text = "UID must be an integer.";

                    EndWaitCursor();
                    return;
                }
                else
                {
                    uid = int.Parse(deleteUIDTextBox.Text);

                    if (deleteEmailTextBox.Text == "")
                    {
                        deleteRichTextBox.Text = "Email cannot be blank.";

                        EndWaitCursor();
                        return;
                    }
                    else
                    {
                        email = deleteEmailTextBox.Text;

                        userId = deleteUserIdTextBox.Text;
                        singleSignOnId = deleteSingleSignOnIdTextBox.Text;

                        request = new Account.DeleteUserRequest
                        {
                            UID = uid,
                            Email = email,
                            UserId = userId,
                            SingleSignOnId = singleSignOnId
                        };

                        try
                        {
                            admin.Account.DeleteUser(request);

                            deleteUserIdTextBox.ResetText();
                            deleteEmailTextBox.ResetText();
                            deleteUserIdTextBox.ResetText();
                            deleteSingleSignOnIdTextBox.ResetText();

                            deleteRichTextBox.Text = "User has been deleted successfully.";
                            EndWaitCursor();
                        }
                        catch (Exception ex)
                        {
                            deleteRichTextBox.Text = ex.Message;

                            EndWaitCursor();
                            return;
                        }
                    }
                }
            }
        }

        private async void getUserTypesButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            getUserTypesRichTextBox.ResetText();

            List<Account.GetUserTypesResponse> response = new List<Account.GetUserTypesResponse>();

            try
            {
                response = await admin.Account.GetUserTypes();

                EndWaitCursor();
            }
            catch (Exception ex)
            {
                getUserTypesRichTextBox.Text = ex.Message;

                EndWaitCursor();
                return;
            }

            if (response != null)
            {
                getUserTypesRichTextBox.Text += "UserTypes:" + Environment.NewLine;

                for (int i = 0; i < response.Count; i++)
                {
                    getUserTypesRichTextBox.Text += string.Format("\tTypeId: {0}" + Environment.NewLine, response[i].TypeId);
                    getUserTypesRichTextBox.Text += string.Format("\tDescription: {0}" + Environment.NewLine, response[i].Description);
                    getUserTypesRichTextBox.Text += string.Format("\tMinimumPasswordLength: {0}" + Environment.NewLine, response[i].MinimumPasswordLength);
                    getUserTypesRichTextBox.Text += string.Format("\tRequiredPasswordCategories: {0}" + Environment.NewLine, response[i].RequiredPasswordCategories);
                    getUserTypesRichTextBox.Text += string.Format("\tCategories: " + Environment.NewLine);
                    getUserTypesRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, response[i].Categories[0]);
                    getUserTypesRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, response[i].Categories[1]);
                    getUserTypesRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, response[i].Categories[2]);
                    getUserTypesRichTextBox.Text += string.Format("\t\t{0}" + Environment.NewLine, response[i].Categories[3]);
                    getUserTypesRichTextBox.Text += Environment.NewLine;
                }
            }
            else
            {
                getUserTypesRichTextBox.Text = "Response is null";
            }
        }

        private async void getCompanySMTPCredentialsButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            smtpGatewayRichTextBox.ResetText();

            SMTP_Gateway.SMTPCredentialsResponse response = new SMTP_Gateway.SMTPCredentialsResponse();

            try
            {
                response = await admin.SMTPGateway.GetCompanySMTPCredentials();

                EndWaitCursor();
            }
            catch (Exception ex)
            {
                smtpGatewayRichTextBox.Text = ex.Message;

                EndWaitCursor();
                return;
            }

            if (response != null)
            {
                int endpointsLength = response.Endpoints.Length;

                smtpGatewayRichTextBox.Text += "SMTP Credentials" + Environment.NewLine;
                smtpGatewayRichTextBox.Text += string.Format("\tUserId: {0}" + Environment.NewLine, response.UserId);
                smtpGatewayRichTextBox.Text += string.Format("\tPassword: {0}" + Environment.NewLine, response.Password);
                smtpGatewayRichTextBox.Text += string.Format("\tEndpoints: ");

                for (int i = 0; i < endpointsLength; i++)
                {
                    smtpGatewayRichTextBox.Text += string.Format("\tIpAddress: {0}" + Environment.NewLine, response.Endpoints[i].IpAddress);
                    smtpGatewayRichTextBox.Text += string.Format("\tSubnet: {0}" + Environment.NewLine, response.Endpoints[i].IpAddress);
                    smtpGatewayRichTextBox.Text += string.Format("\tIncoming: {0}" + Environment.NewLine, response.Endpoints[i].IpAddress);
                    smtpGatewayRichTextBox.Text += string.Format("\tOutgoing: {0}" + Environment.NewLine, response.Endpoints[i].IpAddress);
                    smtpGatewayRichTextBox.Text += string.Format("\tPort: {0}" + Environment.NewLine, response.Endpoints[i].IpAddress);
                    smtpGatewayRichTextBox.Text += string.Format("\tDomain: {0}" + Environment.NewLine, response.Endpoints[i].IpAddress);
                }

                smtpGatewayRichTextBox.Text += Environment.NewLine;
            }
            else
            {
                smtpGatewayRichTextBox.Text = "Response is null.";
            }
        }

        private async void resetPasswordButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            smtpGatewayRichTextBox.ResetText();

            SMTP_Gateway.ResetPasswordResponse response = new SMTP_Gateway.ResetPasswordResponse();

            try
            {
                response = await admin.SMTPGateway.ResetPassword();

                EndWaitCursor();
            }
            catch (Exception ex)
            {
                smtpGatewayRichTextBox.Text = ex.Message;

                EndWaitCursor();
                return;
            }

            if (response != null)
            {
                int resetLength = response.Endpoints.Length;

                smtpGatewayRichTextBox.Text += "Reset Password:" + Environment.NewLine;
                smtpGatewayRichTextBox.Text += string.Format("\tUserId: {0}" + Environment.NewLine, response.UserId);
                smtpGatewayRichTextBox.Text += string.Format("\tPassword: {0}" + Environment.NewLine, response.Password);
                smtpGatewayRichTextBox.Text += "\tEndpoints: " + Environment.NewLine;

                for (int i = 0; i < resetLength; i++)
                {
                    smtpGatewayRichTextBox.Text += string.Format("\tIpAddress: {0}" + Environment.NewLine, response.Endpoints[i].IpAddress);
                    smtpGatewayRichTextBox.Text += string.Format("\tSubnet: {0}" + Environment.NewLine, response.Endpoints[i].Subnet);
                    smtpGatewayRichTextBox.Text += string.Format("\tIncoming: {0}" + Environment.NewLine, response.Endpoints[i].Incoming);
                    smtpGatewayRichTextBox.Text += string.Format("\tOutgoing: {0}" + Environment.NewLine, response.Endpoints[i].IpAddress);
                    smtpGatewayRichTextBox.Text += string.Format("\tPort: {0}" + Environment.NewLine, response.Endpoints[i].Port);
                    smtpGatewayRichTextBox.Text += string.Format("\tDomain: {0}" + Environment.NewLine, response.Endpoints[i].Domain);
                    smtpGatewayRichTextBox.Text += Environment.NewLine;
                }
            }
            else
            {
                smtpGatewayRichTextBox.Text = "Response is null.";
            }
        }

        private async void authenticationLogOutButton_Click(object sender, EventArgs e)
        {
            StartWaitCursor();

            try
            {
                string response = await admin.Authentication.RevokeSessionKey();

                authenticationRichTextBox.Text = "SessionKey has been revoked.";

                authenticationGetSessionKeyButton.Enabled = true;
                authenticationLogOutButton.Enabled = false;

                EndWaitCursor();
            }
            catch (Exception ex)
            {
                authenticationRichTextBox.Text = ex.Message;

                EndWaitCursor();
                return;
            }
        }
    }
}
