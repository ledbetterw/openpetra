//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of UC_ConferenceSelection.ManualCode.
    /// </summary>
    public partial class TFrmUC_ConferenceSelection
    {
        /// <summary>publish conference selection check box</summary>
        public bool AllConferenceSelected
        {
            get
            {
                return rbtAllConferences.Checked;
            }
        }

        /// <summary>publish conference selection check box</summary>
        public bool OneConferenceSelected
        {
            get
            {
                return rbtConference.Checked;
            }
        }

        /// <summary>publish conference key</summary>
        public String ConferenceKey
        {
            get
            {
                return txtConference.Text;
            }
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;

            rbtAllAttendees.Checked = true;
            txtExtract.Enabled = false;
        }

        /// <summary>
        /// set the functions and column names that are available
        /// </summary>
        /// <param name="AAvailableFunctions"></param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
        }

        /// <summary>
        /// read the values from the controls and give them to the calculator
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            if (rbtConference.Checked)
            {
                ACalculator.AddParameter("param_conferenceselection", "one conference");
            }
            else if (rbtAllConferences.Checked)
            {
                ACalculator.AddParameter("param_conferenceselection", "all conferences");
            }

            if (rbtAllAttendees.Checked)
            {
                ACalculator.AddParameter("param_attendeeselection", "all attendees");
            }
            else if (rbtExtract.Checked)
            {
                ACalculator.AddParameter("param_attendeeselection", "from extract");
            }
            else if (rbtOneAttendee.Checked)
            {
                ACalculator.AddParameter("param_attendeeselection", "one attendee");
            }

            ACalculator.AddParameter("param_partnerkey", txtOneAttendee.Text);
            ACalculator.AddParameter("param_conferencekey", txtConference.Text);
            ACalculator.AddParameter("param_conferencename", txtConference.LabelText);
            ACalculator.AddParameter("param_extractname", txtExtract.Text);

            TVerificationResult VerificationResult;

            if ((AReportAction == TReportActionEnum.raGenerate)
                && (rbtExtract.Checked)
                && (txtExtract.Text.Length == 0))
            {
                VerificationResult = new TVerificationResult("Select an extract for running the report.",
                    "No extract was selected!",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            if ((AReportAction == TReportActionEnum.raGenerate)
                && (rbtOneAttendee.Checked)
                && (txtOneAttendee.Text == "0000000000"))
            {
                VerificationResult = new TVerificationResult("Select a partner for whom to run the report.",
                    "No partner was selected!",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            if ((AReportAction == TReportActionEnum.raGenerate)
                && (rbtConference.Checked)
                && (txtConference.Text == "0000000000"))
            {
                VerificationResult = new TVerificationResult("Select a conference to run the report against to.",
                    "No conference was selected!",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            bool ShowSelectCampaignOptionsDialog = true;

            if (ShowSelectCampaignOptionsDialog
                && (AReportAction == TReportActionEnum.raGenerate)
                && (rbtConference.Checked)
                && (FPetraUtilsObject.GetVerificationResultCount() == 0))
            {
                List <KeyValuePair <long, string>>ConferenceList;
                DialogResult DlgResult = TFrmSelectCampaignOption.OpenSelectCampaignOptionDialog(
                    Convert.ToInt64(txtConference.Text), this, true, out ConferenceList);

                if (((DlgResult != DialogResult.OK) || (ConferenceList.Count == 0))
                    && (DlgResult != DialogResult.None))
                {
                    VerificationResult = new TVerificationResult(
                        "You must chose at least one campaign option from the \"Select Campaign Option\" Dialog.",
                        "No campaign option was selected!",
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                string CampaignOptions = "";
                string CampaignOptionsCode = "";

                foreach (KeyValuePair <long, string>CampaignOption in ConferenceList)
                {
                    CampaignOptions = CampaignOptions + CampaignOption.Key.ToString() + ",";
                    CampaignOptionsCode = CampaignOptionsCode + CampaignOption.Value + ",";
                }

                if (CampaignOptions.Length > 0)
                {
                    // Remove the last comma
                    CampaignOptions = CampaignOptions.Remove(CampaignOptions.Length - 1);
                    CampaignOptionsCode = CampaignOptionsCode.Remove(CampaignOptionsCode.Length - 1);
                }
                else
                {
                    CampaignOptions = txtConference.Text;
                    CampaignOptionsCode = txtConference.LabelText;
                }

                ACalculator.AddParameter("param_conferenceoptions", CampaignOptions);
                ACalculator.AddParameter("param_conferenceoptionscode", CampaignOptionsCode);
            }
        }

        /// <summary>
        /// initialise the controls using the parameters
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            if (AParameters.Get("param_conferenceselection").ToString() == "one conference")
            {
                rbtConference.Checked = true;
            }
            else if (AParameters.Get("param_conferenceselection").ToString() == "all conferences")
            {
                rbtAllConferences.Checked = true;
            }

            String AttendeeSelection = AParameters.Get("param_attendeeselection").ToString();

            if (AttendeeSelection == "all attendees")
            {
                rbtAllAttendees.Checked = true;
            }
            else if (AttendeeSelection == "from extract")
            {
                rbtExtract.Checked = true;
            }
            else if (AttendeeSelection == "one attendee")
            {
                rbtOneAttendee.Checked = true;
            }

            txtOneAttendee.Text = AParameters.Get("param_partnerkey").ToString();

            if (AParameters.Exists("param_conferencekey"))
            {
                txtConference.Text = AParameters.Get("param_conferencekey").ToString();
            }

            txtExtract.Text = AParameters.Get("param_extractname").ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtConferenceSelectionChange(object sender, EventArgs e)
        {
            txtConference.Enabled = rbtConference.Checked;
            txtConference.Enabled = rbtConference.Checked;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtAttendeeSelectionChange(object sender, EventArgs e)
        {
            txtExtract.Enabled = rbtExtract.Checked;
            txtOneAttendee.Enabled = rbtOneAttendee.Checked;
        }

        /// <summary>
        /// Adds an event to the txtAutoPopulatedButtonLabel
        /// </summary>
        /// <param name="AEventHandler">Conference changed event handler</param>
        public void AddEventHandler(Ict.Petra.Client.CommonControls.TDelegateConferenceChanged AEventHandler)
        {
            txtConference.ConferenceChanged += AEventHandler;
        }
    }
}