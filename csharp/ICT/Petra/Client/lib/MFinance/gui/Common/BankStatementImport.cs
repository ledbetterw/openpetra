// auto generated with nant generateWinforms from BankStatementImport.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2010 by OM International
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
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui.Common
{

  /// auto generated: Import Bank Statements
  public partial class TFrmBankStatementImport: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;

    /// constructor
    public TFrmBankStatementImport(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblSelectBankAccount.Text = Catalog.GetString("Select Bank Account:");
      this.rbtListAll.Text = Catalog.GetString("ListAll");
      this.rbtListUnmatched.Text = Catalog.GetString("ListUnmatched");
      this.rbtListGift.Text = Catalog.GetString("ListGift");
      this.rbtListGL.Text = Catalog.GetString("ListGL");
      this.rgrSelectTransaction.Text = Catalog.GetString("Select Transaction");
      this.rbtUnmatched.Text = Catalog.GetString("Unmatched");
      this.rbtGift.Text = Catalog.GetString("Gift");
      this.rbtGL.Text = Catalog.GetString("GL");
      this.rgrTransactionCategory.Text = Catalog.GetString("Transaction Category");
      this.txtDonorKey.ButtonText = Catalog.GetString("Find");
      this.btnAddGiftDetail.Text = Catalog.GetString("&Add");
      this.btnRemoveGiftDetail.Text = Catalog.GetString("&Delete");
      this.lblAmount.Text = Catalog.GetString("Amount:");
      this.lblMotivationDetail.Text = Catalog.GetString("Motivation Detail:");
      this.lblGiftAccount.Text = Catalog.GetString("Account:");
      this.lblGiftCostCentre.Text = Catalog.GetString("Cost Centre:");
      this.lblGLNarrative.Text = Catalog.GetString("Narrative:");
      this.lblGLReference.Text = Catalog.GetString("Reference:");
      this.lblGLAccount.Text = Catalog.GetString("Account:");
      this.lblGLCostCentre.Text = Catalog.GetString("Cost Centre:");
      this.tbbImportNewStatement.Text = Catalog.GetString("&Import new statement");
      this.tbbSeparator0.Text = Catalog.GetString("Separator");
      this.tbbDeleteStatement.Text = Catalog.GetString("Delete Statement");
      this.tbbSeparator1.Text = Catalog.GetString("Separator");
      this.tbbSaveMatches.Text = Catalog.GetString("Save Matches");
      this.tbbCreateGiftBatch.Text = Catalog.GetString("Create Gift Batch");
      this.tbbCreateGLBatch.Text = Catalog.GetString("CreateGL Batch");
      this.mniImportNewStatement.Text = Catalog.GetString("&Import new statement");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Import Bank Statements");
      #endregion

      this.txtAmount.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtGLNarrative.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtGLReference.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

    }

    private void TFrmPetra_Activated(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Activated(sender, e);
    }

    private void TFrmPetra_Load(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Load(sender, e);
    }

    private void TFrmPetra_Closing(object sender, CancelEventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Closing(sender, e);
    }

    private void Form_KeyDown(object sender, KeyEventArgs e)
    {
        FPetraUtilsObject.Form_KeyDown(sender, e);
    }

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position
    }

#region Implement interface functions

    /// auto generated
    public void RunOnceOnActivation()
    {
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actAddGiftDetail")
        {
            btnAddGiftDetail.Enabled = e.Enabled;
        }
        if (e.ActionName == "actRemoveGiftDetail")
        {
            btnRemoveGiftDetail.Enabled = e.Enabled;
        }
        if (e.ActionName == "actImportNewStatement")
        {
            tbbImportNewStatement.Enabled = e.Enabled;
            mniImportNewStatement.Enabled = e.Enabled;
        }
        if (e.ActionName == "actDeleteStatement")
        {
            tbbDeleteStatement.Enabled = e.Enabled;
        }
        if (e.ActionName == "actSaveMatches")
        {
            tbbSaveMatches.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCreateGiftBatch")
        {
            tbbCreateGiftBatch.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCreateGLBatch")
        {
            tbbCreateGLBatch.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion
  }
}