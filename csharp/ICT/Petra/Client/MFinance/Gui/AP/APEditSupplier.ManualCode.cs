//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2012 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.Interfaces.MFinance.AP.UIConnectors;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPEditSupplier
    {
        AccountsPayableTDS FMainDS;
        Int32 FLedgerNumber;
        ALedgerRow FLedgerRow = null;

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitializeManualCode()
        {
            FMainDS = new AccountsPayableTDS();
        }

        /// <summary>
        /// we need the ledger number for the account and costcentre selections, which are dependant on the ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                ALedgerTable Tbl = TRemote.MFinance.AP.WebConnectors.GetLedgerInfo(FLedgerNumber);
                FLedgerRow = Tbl[0];

                TFinanceControls.InitialiseAccountList(ref cmbAPAccount, FLedgerNumber, true, false, false, false);
                TFinanceControls.InitialiseAccountList(ref cmbDefaultBankAccount, FLedgerNumber, true, false, false, true);
                TFinanceControls.InitialiseAccountList(ref cmbExpenseAccount, FLedgerNumber, true, false, false, false);
                TFinanceControls.InitialiseCostCentreList(ref cmbCostCentre, FLedgerNumber, true, false, false, false);
            }
        }


        /// <summary>
        /// called from APMain when adding new supplier;
        /// initialises a new dataset
        /// </summary>
        /// <param name="APartnerKey"></param>
        public void CreateNewSupplier(Int64 APartnerKey)
        {
            // TODO: let the server do that? what about messages on failure?

            // check for existing supplier record
            if (FUIConnector.CanFindSupplier(APartnerKey))
            {
                MessageBox.Show(Catalog.GetString("There is already a supplier record for this partner!"));
                EditSupplier(APartnerKey);
                return;
            }

            FPetraUtilsObject.SetChangedFlag();

            AApSupplierRow row = FMainDS.AApSupplier.NewRowTyped();
            row.PartnerKey = APartnerKey;

            row.CurrencyCode = FLedgerRow.BaseCurrency;
            FMainDS.AApSupplier.Rows.Add(row);

            ShowData(row);

            // Make PartnerKey Readonly again -
            // ShowData makes it writeable because it is the Primary Key of the Table and we are adding a record!
            txtPartnerKey.ReadOnly = true;
        }

        /// <summary>
        /// edit an existing supplier
        /// </summary>
        /// <param name="APartnerKey"></param>
        public void EditSupplier(Int64 APartnerKey)
        {
            FMainDS = FUIConnector.GetData(APartnerKey);
            ShowData(FMainDS.AApSupplier[0]);
        }

        /// <summary>
        /// open the Partner Edit screen for the supplier
        /// </summary>
        private void EditPartner(object sender, EventArgs e)
        {
            FPetraUtilsObject.WriteToStatusBar("Opening Partner in Partner Edit screen...");
            this.Cursor = Cursors.WaitCursor;

            try
            {
                TFrmPartnerEdit frm = new TFrmPartnerEdit(this);
                frm.SetParameters(TScreenMode.smEdit, FMainDS.AApSupplier[0].PartnerKey);
                frm.Show();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool ValidateAccountCurrency(string AccountRef, string AccountType)

        {
            bool CurrencyIsOk = true;
            AAccountTable AccountList = (AAccountTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);
            AccountList.DefaultView.RowFilter = String.Format("a_ledger_number_i={0} AND a_account_code_c='{1}'",
                FLedgerNumber, AccountRef);

            if (AccountList.DefaultView.Count == 1)
            {
                AAccountRow AccountDetail = (AAccountRow)AccountList.DefaultView[0].Row;
                if (FMainDS.AApSupplier[0].CurrencyCode == FLedgerRow.BaseCurrency)
                {
                    CurrencyIsOk = (AccountDetail.ForeignCurrencyFlag == false);
                }
                else
                {
                    CurrencyIsOk = ((AccountDetail.ForeignCurrencyFlag == true) && (AccountDetail.ForeignCurrencyCode == FMainDS.AApSupplier[0].CurrencyCode));
                }

                if (!CurrencyIsOk)
                {
                    MessageBox.Show(String.Format(Catalog.GetString("The {0} must be a {1} currency account."), 
                        AccountType, FMainDS.AApSupplier[0].CurrencyCode), "Validation");
                    FMainDS.AApSupplier.Rows[0].EndEdit();
                    TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList); // scrub the cache so that I'll notice if the user makes a change
                }

            }
            else
            {
                MessageBox.Show(String.Format(Catalog.GetString("Unable to access {0} account {1}"),
                    AccountType, AccountRef), "Error");
                FMainDS.AApSupplier.Rows[0].EndEdit();
                TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList); // scrub the cache - perhaps I'll get a different answer next time!
                CurrencyIsOk = false;
            }


            return CurrencyIsOk;
        }

        /// <summary>
        /// save the changes on the screen
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

            // Don't allow saving if user is still editing a Detail of a List
            if (FPetraUtilsObject.InDetailEditMode())
            {
                return false;
            }

            FMainDS.AApSupplier.Rows[0].BeginEdit();
            GetDataFromControls(FMainDS.AApSupplier[0]);

            if (FMainDS.AApSupplier[0].IsDefaultApAccountNull())
            {
                MessageBox.Show(Catalog.GetString("Please select an AP account (eg. 9100)"));
                FMainDS.AApSupplier.Rows[0].EndEdit();
                return false;
            }

            // The account would usually be 9100-AP account.
            if (FMainDS.AApSupplier[0].DefaultApAccount != "9100")
            {
                if (MessageBox.Show(Catalog.GetString("You are not using the standard AP account (9100) - is this OK?"), 
                    "Verification", MessageBoxButtons.YesNo)
                    != System.Windows.Forms.DialogResult.Yes)
                {
                    FMainDS.AApSupplier.Rows[0].EndEdit();
                    return false;
                }
            }

            // Don't store with invalid currency value.
            //
            if (FMainDS.AApSupplier[0].CurrencyCode == "")
            {
                FMainDS.AApSupplier[0].CurrencyCode = FLedgerRow.BaseCurrency;
            }

            // If this is a foreign currency supplier, it must be linked to accounts in that currency.
            // (And if it's not, it mustn't be!)

            if (!ValidateAccountCurrency(FMainDS.AApSupplier[0].DefaultBankAccount, "Bank"))
            {
                return false;
            }
/*
 * If we wanted to have only expense accounts in a single currency, we could have this,
 * but that's probably not what we want...
 * 
            if (!ValidateAccountCurrency(FMainDS.AApSupplier[0].DefaultExpAccount, "Expense"))
            {
                return false;
            }
 */

            if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
            {
                foreach (DataTable InspectDT in FMainDS.Tables)
                {
                    foreach (DataRow InspectDR in InspectDT.Rows)
                    {
                        InspectDR.EndEdit();
                    }
                }

                if (FPetraUtilsObject.HasChanges)
                {
                    FPetraUtilsObject.WriteToStatusBar("Saving data...");
                    this.Cursor = Cursors.WaitCursor;

                    AccountsPayableTDS SubmitDS = FMainDS.GetChangesTyped(true);

                    TSubmitChangesResult SubmissionResult;
                    TVerificationResultCollection VerificationResult;

                    // Submit changes to the PETRAServer
                    try
                    {
                        SubmissionResult = FUIConnector.SubmitChanges(ref SubmitDS, out VerificationResult);
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("The PETRA Server cannot be reached! Data cannot be saved!",
                            "No Server response",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        bool ReturnValue = false;

                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
/* TODO ESecurityDBTableAccessDeniedException
 *                  catch (ESecurityDBTableAccessDeniedException Exp)
 *                  {
 *                      FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
 *                      this.Cursor = Cursors.Default;
 *                      // TODO TMessages.MsgSecurityException(Exp, this.GetType());
 *                      bool ReturnValue = false;
 *                      // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
 *                      return ReturnValue;
 *                  }
 */
                    catch (EDBConcurrencyException)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;

                        // TODO TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                        bool ReturnValue = false;

                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (Exception exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
                        TLogging.Log(
                            "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(),
                            TLoggingType.ToLogfile);
                        MessageBox.Show(
                            "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                            "For details see the log file: " + TLogging.GetLogFileName(),
                            "Server connection error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        bool ReturnValue = false;

                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }

                    switch (SubmissionResult)
                    {
                        case TSubmitChangesResult.scrOK:

                            // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                            FMainDS.AcceptChanges();

                            // Merge back with data from the Server (eg. for getting Sequence values)
                            FMainDS.Merge(SubmitDS, false);

                            // need to accept the new modification ID
                            FMainDS.AcceptChanges();

                            // Update UI
                            FPetraUtilsObject.WriteToStatusBar("Data successfully saved.");
                            this.Cursor = Cursors.Default;

                            // TODO EnableSave(false);

                            // We don't have unsaved changes anymore
                            FPetraUtilsObject.DisableSaveButton();

                            // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                            return true;

                        case TSubmitChangesResult.scrError:

                            // TODO scrError
                            break;

                        case TSubmitChangesResult.scrNothingToBeSaved:

                            // TODO scrNothingToBeSaved
                            break;

                        case TSubmitChangesResult.scrInfoNeeded:

                            // TODO scrInfoNeeded
                            break;
                    }
                }
            }

            return false;
        }
    }
}