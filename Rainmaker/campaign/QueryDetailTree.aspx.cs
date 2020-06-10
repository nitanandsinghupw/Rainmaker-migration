using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Rainmaker.Common.DomainModel;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.AgentsWS;

namespace Rainmaker.Web.campaign
{
    public partial class QueryDetailTree : PageBase
    {
        string strQueryCondition = string.Empty;

        #region Page Events
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // **** Rewrite for new events coming from panels, not pop ups
            if (!IsPostBack)
            {
                Campaign objCampaign;
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    anchHome.InnerText = objCampaign.Description; // Replaced Short description
                    QueryTree qt = BuildQueryTreeFromDatabase(objCampaign);
                    if (qt != null)
                    {
                        txtQueryName.Text = qt[0].QueryName;
                        lblRODate.Text = qt[0].DateCreated.ToString("MM/dd/yyyy");
                        lblROModifiedDate.Text = qt[0].DateModified.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        lblRODate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                        lblROModifiedDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    Session["QueryTree"] = qt;
                    RenderQueryTree(objCampaign, qt);
                    try
                    {
                        if (ConfigurationManager.AppSettings["QMDebug"].ToLower() == "yes")
                        {
                            ActivityLogger.WriteAdminEntry(objCampaign, "Query tree loaded from database.");
                            WriteQueryTreeToLog(qt);
                        }
                    }
                    catch { }
                }
                Session["DeletedQueryDetailList"] = null;
            }
            // *** Remove in V3
            //try
            //{
            //    if (Session["NewQueryNode"] != null)
            //    {
            //        // Add a new node
            //        QueryTreeItem qti = (QueryTreeItem)Session["NewQueryNode"];
            //        AddNewTreeNode(qti);
            //        Session["NewQueryNode"] = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    PageMessage = "Exception adding new tree node :" + ex.Message;
            //}

            //try
            //{
            //    if (Session["NodeChange"] != null)
            //    {
            //        // Add a new node
            //        Campaign objCampaign = (Campaign)Session["Campaign"];
            //        QueryTree qt = (QueryTree)Session["QueryTree"];
            //        RenderQueryTree(objCampaign, qt);
            //        Session["NodeChange"] = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    PageMessage = "Exception on new node edit :" + ex.Message;
            //}

            try
            {
                if (hdnQueryToOverwrite.Value == "true")
                {
                    // Delete the existing query
                    if (txtQueryName.Text.Length > 0)
                    {
                        Campaign objCampaign;
                        objCampaign = (Campaign)Session["Campaign"];
                        CampaignService objCampaignService = new CampaignService();
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                        objCampaignService.DeleteQueryByName(xDocCampaign, txtQueryName.Text);

                        // Save query to avoid duplicate exception
                        SaveQueryToDB();
                    }
                }
                hdnQueryToOverwrite.Value = "";
                hdnDuplicateQuery.Value = "";
            }
            catch (Exception ex)
            {
                PageMessage = "Exception saving duplicate query :" + ex.Message;
            }
        }

        private void AddNewTreeNode(QueryTreeItem newQueryTreeItem)
        {
            try
            {
                QueryTree qt = (QueryTree)Session["QueryTree"];
                QueryTree newQt = new QueryTree();
                QueryTreeItem newQti = newQueryTreeItem;

                if (qt == null)
                {
                    // First node to new query
                    newQti.DateCreated = DateTime.Now;
                    newQti.DateModified = DateTime.Now;
                    newQti.TreeNodeID = 1;
                    newQti.QueryName = trvQueryConditions.Nodes[0].Text;
                }
                else
                {
                    if ((newQueryTreeItem.NewNodeAddIndex) >= qt.Count)
                    {
                        // This is insert at the last level, just duplicate the tree
                        newQt = qt;
                    }
                    else
                    {
                        // Added somewhere in the middle, adjust keys and logical order
                        // First, duplicate all nodes prior to the new one as is
                        for (int i = 0; i < newQueryTreeItem.NewNodeAddIndex; i++)
                        {
                            newQt.Add(i, qt[i]);
                        }

                        for (int i = (newQueryTreeItem.NewNodeAddIndex); i < qt.Count; i++)
                        {
                            // Adjust indexes of any nodes below this one
                            QueryTreeItem qti = qt[i];
                            qti.LogicalOrder++;
                            newQt.Add((i + 1), qti);
                        }
                    }


                    // Add common elements not specific to node type
                    if (newQt.Count > 0)
                    {
                        newQti.QueryID = newQt[0].QueryID;
                        newQti.QueryName = newQt[0].QueryName;
                        newQti.DateCreated = newQt[0].DateCreated;
                        newQti.DateModified = newQt[0].DateModified;
                    }
                    // Assign new NodeID
                    int maxTreeNodeID = 0;
                    for (int i = 0; i < qt.Count; i++)
                    {
                        if (maxTreeNodeID < qt[i].TreeNodeID)
                        {
                            maxTreeNodeID = qt[i].TreeNodeID;
                        }
                    }
                    newQti.TreeNodeID = maxTreeNodeID + 1;

                }
                newQti.LogicalOrder = newQti.NewNodeAddIndex;

                // Build node type specific item stuff
                switch (newQti.NodeType)
                {
                    case NodeType.RootCondition:
                        break;
                    case NodeType.SubQuery:
                        break;
                    case NodeType.SubSetParent:
                        newQti.SubsetLogicalOrder = 0;
                        int maxSubset = 0;
                        if (qt != null)
                        {
                            if (newQti.ParentTreeNodeID == 0)
                            {
                                newQti.SubsetLevel = 1;
                            }
                            else
                            {
                                for (int i = (newQti.NewNodeAddIndex - 1); i >= 0; i--)
                                {
                                    if (newQt[i].TreeNodeID == newQti.ParentTreeNodeID)
                                    {
                                        newQti.SubsetLevel = newQt[i].SubsetLevel + 1;
                                        break;
                                    }
                                }
                            }
                            newQti.SubsetLevel = (newQt[newQti.NewNodeAddIndex - 1].SubsetLevel + 1);
                            for (int i = 0; i < qt.Count; i++)
                            {
                                if (maxSubset < qt[i].SubsetID)
                                {
                                    maxSubset = qt[i].SubsetID;
                                }
                            }
                        }
                        else
                        {
                            newQti.SubsetLevel = 1;
                        }
                        newQti.SubsetID = maxSubset + 1;
                        break;
                    case NodeType.SubSetChildCondition:
                        int maxSubsetLogical = 0;
                        for (int i = 0; i < qt.Count; i++)
                        {
                            if (qt[i].SubsetID == newQti.SubsetID)
                            {
                                if (maxSubsetLogical < qt[i].SubsetLogicalOrder)
                                {
                                    maxSubsetLogical = qt[i].SubsetLogicalOrder;
                                }
                            }
                        }
                        newQti.SubsetLogicalOrder = maxSubsetLogical + 1;
                        for (int i = (newQti.NewNodeAddIndex - 1); i >= 0; i--)
                        {
                            if (newQt[i].TreeNodeID == newQti.ParentTreeNodeID)
                            {
                                newQti.SubsetLevel = newQt[i].SubsetLevel;
                                break;
                            }
                        }
                        break;
                }
                newQt.Add(newQti.NewNodeAddIndex, newQti);

                Campaign objCampaign = (Campaign)Session["Campaign"];

                RenderQueryTree(objCampaign, newQt);

                try
                {
                    if (ConfigurationManager.AppSettings["QMDebug"].ToLower() == "yes")
                    {
                        ActivityLogger.WriteAdminEntry(objCampaign, "Added new query node at index {0}.", newQti.NewNodeAddIndex);
                        WriteQueryTreeToLog(newQt);
                    }
                }
                catch { }
                Session["QueryTree"] = newQt;
                return;
            }
            catch (Exception ex)
            {
                PageMessage = "Exception adding new query node to session tree:" + ex.Message;
            }
        }

        protected void lbtnShowHideSQL_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnSender = (LinkButton)sender;

                if (lbtnSender.CommandName == "show")
                {
                    hdnShowSQL.Value = "true";
                    lbtnShowSQL.Visible = false;
                    lbtnHideSQL.Visible = true;
                    tblSQL.Visible = true;
                }
                else
                {
                    hdnShowSQL.Value = "false";
                    lbtnShowSQL.Visible = true;
                    lbtnHideSQL.Visible = false;
                    tblSQL.Visible = false;
                }
                if (hdnShowSQL.Value == "true")
                {
                    lblSQL.Text = BuildSQLFromTree();
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception in show/hide event :" + ex.Message;
            }
        }

        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtQueryName.Text))
            {
                PageMessage = "Please enter a name for the query before you save.";
                txtQueryName.BorderColor = System.Drawing.Color.Red;
            }
            else
            {
                txtQueryName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#050040");
            }

            SaveQueryToDB();
        }
        /// <summary>
        /// Cancel Query Detail 
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["QueryID"] != null)
                Response.Redirect("~/campaign/QueryDetailTree.aspx?QueryID=" + Request.QueryString["QueryID"].ToString());
            else
                Response.Redirect("~/campaign/QueryDetailTree.aspx");

        }

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            Session["DeletedQueryDetailList"] = null;
            if (Request.QueryString["DataManager"] != null)
            {
                Response.Redirect("~/campaign/DataPortal.aspx");
            }
            else
            {
                //Response.Redirect("~/campaign/QueryStatus.aspx");
                Response.Redirect("~/campaign/Home.aspx");
            }
        }

        protected void lbtnTestQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string strQueryCondition = BuildSQLFromTree();
                if (strQueryCondition == "There are currently no conditions in this query.")
                {
                    PageMessage = "There must be at least one condition to test the query.";
                    return;
                }
                if (!TestSQLSyntax(strQueryCondition, (QueryTree)Session["QueryTree"]))
                    return;

                strQueryCondition = BuildQueryCondition(strQueryCondition);
                Campaign objCampaign;
                CampaignService objCampaignService = new CampaignService();
                objCampaign = (Campaign)Session["Campaign"];
                DataSet dsCampaignQueryData;
                try
                {
                    dsCampaignQueryData = objCampaignService.GetCampaignData(objCampaign.CampaignDBConnString, strQueryCondition);
                    if (dsCampaignQueryData.Tables[0] != null)
                        PageMessage = "Query executed successfully." + Environment.NewLine + "Records returned : " + dsCampaignQueryData.Tables[0].Rows.Count.ToString();
                }
                catch (Exception ex)
                {
                    PageMessage = "Error Executing Query : " + ex.Message;
                    return;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "There has been an error testing your query, please make sure your conditions are valid and try again. " + ex.Message;
            }
        }
        #endregion

        #region Panel Events
        protected void lbtnElementNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AddEditTreeItem"] == null) return;
                QueryTreeItem qti = (QueryTreeItem)Session["AddEditTreeItem"];

                switch (ddlElementType.SelectedValue)
                {
                    case "0":
                        // Condition
                        if (qti.ParentTreeNodeID != 0)
                        {
                            qti.NodeType = NodeType.SubSetChildCondition;
                        }
                        else
                        {
                            qti.NodeType = NodeType.RootCondition;
                        }
                        break;
                    case "1":
                        // Subset
                        qti.NodeType = NodeType.SubSetParent;
                        break;
                    case "2":
                        // Sub Query
                        qti.NodeType = NodeType.SubQuery;
                        break;
                }

                pnlChooseElementType.Visible = false;

                if (hdnEditingFirstNode.Value.ToLower() == "false")
                {
                    pnlLogical.Visible = true;
                }
                else
                {
                    switch (qti.NodeType)
                    {
                        case NodeType.RootCondition:
                        case NodeType.SubSetChildCondition:
                            pnlCondition.Visible = true;
                            lbtnAddCondition.Visible = true;
                            lbtnEditCondition.Visible = false;
                            lblCreateCondition.Visible = true;
                            lblEditCondition.Visible = false;
                            BindConditionControls();
                            txtEnterValue.Text = "";
                            break;
                        case NodeType.SubSetParent:
                            pnlSubset.Visible = true;
                            lbtnAddSubset.Visible = true;
                            lbtnEditSubset.Visible = false;
                            txtSubsetName.Text = "";
                            break;
                        case NodeType.SubQuery:
                            pnlSubQuery.Visible = true;
                            lbtnAddSubQuery.Visible = true;
                            lbtnEditSubQuery.Visible = false;
                            BindQueryList();
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                PageMessage = "Error in element next event: " + ex.Message;
            }
        }
        protected void lbtnAddLogical_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AddEditTreeItem"] == null) return;
                QueryTreeItem qti = (QueryTreeItem)Session["AddEditTreeItem"];

                if (RdoOr.Checked)
                {
                    qti.LogicalOperator = "OR";
                }
                if (RdoAnd.Checked)
                {
                    qti.LogicalOperator = "AND";
                }

                pnlLogical.Visible = false;
                if (hdnNodeEdit.Value == "true")
                {
                    hdnNodeEdit.Value = "";
                    ShowEditPanel(qti);
                    return;
                }
                switch (qti.NodeType)
                {
                    case NodeType.RootCondition:
                    case NodeType.SubSetChildCondition:
                        pnlCondition.Visible = true;
                        lbtnAddCondition.Visible = true;
                        lbtnEditCondition.Visible = false;
                        lblCreateCondition.Visible = true;
                        lblEditCondition.Visible = false;
                        BindConditionControls();
                        txtEnterValue.Text = "";
                        break;
                    case NodeType.SubSetParent:
                        pnlSubset.Visible = true;
                        lbtnAddSubset.Visible = true;
                        lbtnEditSubset.Visible = false;
                        txtSubsetName.Text = "";
                        break;
                    case NodeType.SubQuery:
                        pnlSubQuery.Visible = true;
                        lbtnAddSubQuery.Visible = true;
                        lbtnEditSubQuery.Visible = false;
                        BindQueryList();
                        break;
                }

            }
            catch (Exception ex)
            {
                PageMessage = "Error in add logical next event: " + ex.Message;
            }
        }

        protected void lbtnAddSubset_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSubsetName.Text))
                {
                    PageMessage = ("You must enter a name for your condition subset.");
                    return;
                }

                if (Session["AddEditTreeItem"] == null) return;
                QueryTreeItem qti = (QueryTreeItem)Session["AddEditTreeItem"];

                qti.SubsetName = txtSubsetName.Text;

                AddNewTreeNode(qti);
                HideAllEditPanels();
            }
            catch (Exception ex)
            {
                PageMessage = "Error in add subset event: " + ex.Message;
            }
        }

        protected void lbtnAddSubQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AddEditTreeItem"] == null) return;
                QueryTreeItem qti = (QueryTreeItem)Session["AddEditTreeItem"];

                qti.SubQueryID = Convert.ToInt64(ddlQueryList.SelectedValue);
                qti.SubQueryName = ddlQueryList.SelectedItem.Text;

                AddNewTreeNode(qti);
                HideAllEditPanels();
            }
            catch (Exception ex)
            {
                PageMessage = "Error in add subquery event: " + ex.Message;
            }
        }

        protected void ddlCriteria_Change(object sender, EventArgs e)
        {
            try
            {
                CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);
                if (ddlPickByName.Visible)
                    ddlPickByName.SelectedValue = "0";

                // 2012-06-12 Dave Pollastrini
                // Changed BindOperator to take a datatype.
                /*
                bool isDate = false;
                string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();
                if (dataType == "integer" || dataType == "date")
                {
                    isDate = true;
                }
                */

                string[] fieldInfo = ddlCriteria.SelectedValue.Split(':');
                string dataType = 
                    fieldInfo[0].Equals("NeverCallFlag", StringComparison.InvariantCultureIgnoreCase)
                    ? "boolean" : fieldInfo[1];

                BindOperator(ddlOperator, dataType);
                if (dataType == "String")
                {
                    chkboxnumber.Visible = true;
                    lblnumber.Visible = true;
                } else {
                    chkboxnumber.Visible = false;
                    chkboxnumber.Checked = false;
                    lblnumber.Visible = false;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception in Criteria change " + ex.Message;
            }
        }

        protected void ddlOperator_Change(object sender, EventArgs e)
        {
            CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);
            if (ddlPickByName.Visible)
                ddlPickByName.SelectedValue = "0";
            txtEnterValue.Text = "";
            if (ddlOperator.SelectedItem.Text.IndexOf("Null") > 0)
            {
                ddlPickByName.Enabled = false;
                txtEnterValue.Enabled = false;
            }
            else
            {
                ddlPickByName.Enabled = true;
                txtEnterValue.Enabled = true;
            }

        }

        protected void lbtnAddCondition_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("To add a valid condition, you must: \r\n");
                bool condIsValid = true;

                if ((!RdoAnd.Checked) && (!RdoOr.Checked) && (Request.QueryString["FirstNode"].ToString() != "true"))
                {
                    sb.Append(" - Enter a logical operator for a new element. \r\n");
                    condIsValid = false;
                }

                if (ddlCriteria.SelectedValue == "0")
                {
                    sb.Append(" - Select a database field to evaluate. \r\n");
                    condIsValid = false;
                }

                if (ddlOperator.SelectedValue == "0")
                {
                    sb.Append(" - Select an operator for the condition. \r\n");
                    condIsValid = false;
                }

                if (txtEnterValue.Visible && txtEnterValue.Enabled)
                {
                    if (txtEnterValue.Text.Length < 1)
                    {
                        sb.Append(" - Enter a value to compare. \r\n");
                        condIsValid = false;
                    }
                }
                else
                {
                    if (ddlPickByName.Enabled && ddlPickByName.SelectedValue == "0")
                    {
                        sb.Append(" - Select a value to compare. \r\n");
                        condIsValid = false;
                    }
                }

                if (!condIsValid)
                {
                    PageMessage = sb.ToString();
                    return;
                }
                if (Session["AddEditTreeItem"] == null) return;
                QueryTreeItem qti = (QueryTreeItem)Session["AddEditTreeItem"];
                qti.SearchCriteria = ddlCriteria.SelectedItem.Text;

                string[] fieldInfo = ddlCriteria.SelectedValue.Split(':');
                string dataType =
                    fieldInfo[0].Equals("NeverCallFlag", StringComparison.InvariantCultureIgnoreCase)
                    ? "boolean" : fieldInfo[1];

                if (txtEnterValue.Visible)
                {
                    qti.SearchString = txtEnterValue.Text.Trim();
                }
                else if (dataType.Equals("boolean"))
                {
                    qti.SearchString = string.Empty;
                }
                else
                {
                    qti.SearchString = ddlPickByName.SelectedValue.Trim();
                    qti.NodeLabelNameSubstitute = ddlPickByName.SelectedItem.Text;
                }

                string operatorCondition = ddlOperator.SelectedValue;
                if (txtEnterValue.Text.Trim().Length <= 10 && ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0
                    && ddlOperator.SelectedItem.Text == "Equals")
                {
                    try
                    {
                        txtEnterValue.Text = Convert.ToDateTime(txtEnterValue.Text.Trim()).ToString("MM/dd/yyyy");
                    }
                    catch { }
                    operatorCondition = "Convert(Varchar(10),{0},101) = '{1}'";
                }
                if (ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0)
                {
                    if (txtEnterValue.Visible && qti.SearchString.Length <= 10)
                    {
                        if (ddlOperator.SelectedItem.Text == "Greater Than" ||
                            ddlOperator.SelectedItem.Text == "Less than Equal")
                        {
                            qti.SearchString = qti.SearchString + " 23:59:59";
                        }
                    }
                }
                if (chkboxnumber.Visible && chkboxnumber.Checked)
                {
                    operatorCondition = operatorCondition.Replace("'", "");
                   
                }
                qti.SearchOperator = operatorCondition;
                
                //string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();
                if (dataType == "date")
                {
                    qti.IsDateField = true;
                }

                AddNewTreeNode(qti);
                HideAllEditPanels();
            }
            catch (Exception ex)
            {
                PageMessage = "Error in add condition event: " + ex.Message;
            }
        }

        protected void lbtnEditCondition_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("To edit to a valid condition, you must: \r\n");
                bool condIsValid = true;

                if ((!RdoAnd.Checked) && (!RdoOr.Checked) && (Request.QueryString["FirstNode"].ToString() != "true"))
                {
                    sb.Append(" - Enter a logical operator for a new element. \r\n");
                    condIsValid = false;
                }

                if (ddlCriteria.SelectedValue == "0")
                {
                    sb.Append(" - Select a database field to evaluate. \r\n");
                    condIsValid = false;
                }

                if (ddlOperator.SelectedValue == "0")
                {
                    sb.Append(" - Select an operator for the condition. \r\n");
                    condIsValid = false;
                }

                if (txtEnterValue.Visible && txtEnterValue.Enabled)
                {
                    if (txtEnterValue.Text.Length < 1)
                    {
                        sb.Append(" - Enter a value to compare. \r\n");
                        condIsValid = false;
                    }
                }
                else
                {
                    if (ddlPickByName.Enabled && ddlPickByName.SelectedValue == "0")
                    {
                        sb.Append(" - Select a value to compare. \r\n");
                        condIsValid = false;
                    }
                }

                if (!condIsValid)
                {
                    PageMessage = sb.ToString();
                    return;
                }
                if (Session["AddEditTreeItem"] == null) return;
                QueryTreeItem qti = (QueryTreeItem)Session["AddEditTreeItem"];
                QueryTreeItem newQti = new QueryTreeItem();
                QueryTree qt = (QueryTree)Session["QueryTree"];
                for (int i = 0; i < qt.Count; i++)
                {
                    if (qt[i] == qti)
                    {
                        newQti = qt[i];
                        break;
                    }
                }

                newQti.SearchCriteria = ddlCriteria.SelectedItem.Text;

                string[] fieldInfo = ddlCriteria.SelectedValue.Split(':');
                string dataType =
                    fieldInfo[0].Equals("NeverCallFlag", StringComparison.InvariantCultureIgnoreCase)
                    ? "boolean" : fieldInfo[1];

                if (txtEnterValue.Visible)
                {
                    newQti.SearchString = txtEnterValue.Text.Trim();
                    newQti.NodeLabelNameSubstitute = txtEnterValue.Text.Trim();
                }
                else if (dataType.Equals("boolean"))
                {
                    qti.SearchString = string.Empty;
                }
                else
                {
                    newQti.SearchString = ddlPickByName.SelectedValue.Trim();
                    newQti.NodeLabelNameSubstitute = ddlPickByName.SelectedItem.Text;
                }

                string operatorCondition = ddlOperator.SelectedValue;
                if (txtEnterValue.Text.Trim().Length <= 10 && ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0
                    && ddlOperator.SelectedItem.Text == "Equals")
                {
                    try
                    {
                        txtEnterValue.Text = Convert.ToDateTime(txtEnterValue.Text.Trim()).ToString("MM/dd/yyyy");
                    }
                    catch { }
                    operatorCondition = "Convert(Varchar(10),{0},101) = '{1}'";
                }
                if (ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0)
                {
                    if (txtEnterValue.Visible && newQti.SearchString.Length <= 10)
                    {
                        if (ddlOperator.SelectedItem.Text == "Greater Than" ||
                            ddlOperator.SelectedItem.Text == "Less than Equal")
                        {
                            newQti.SearchString = newQti.SearchString + " 23:59:59";
                        }
                    }
                }
                if (chkboxnumber.Visible && chkboxnumber.Checked)
                {
                    operatorCondition = operatorCondition.Replace("'", "");
                    
                }
                newQti.SearchOperator = operatorCondition;

                // string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();
                if (dataType == "date")
                {
                    newQti.IsDateField = true;
                }

                Campaign objCampaign = (Campaign)Session["Campaign"];
                RenderQueryTree(objCampaign, qt);
                HideAllEditPanels();
            }
            catch (Exception ex)
            {
                PageMessage = "Error in edit condition event: " + ex.Message;
            }
        }


        protected void lbtnDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Delete element selected
                QueryTree qt = (QueryTree)Session["QueryTree"];
                QueryTree newQt = new QueryTree();
                List<long> DeletedQueryDetails;
                if (Session["DeletedQueryDetailList"] != null)
                {
                    DeletedQueryDetails = (List<long>)Session["DeletedQueryDetailList"];
                }
                else
                {
                    DeletedQueryDetails = new List<long>();
                }
                int deleteIndex = 0;
                for (int i = 0; i < qt.Count; i++)
                {
                    if (qt[i].TreeNodeID.ToString() == trvQueryConditions.SelectedNode.Value)
                    {
                        deleteIndex = i;
                        break;
                    }
                }
                QueryTreeItem qti = qt[deleteIndex];
                switch (qti.NodeType)
                {
                    case NodeType.RootCondition:
                    case NodeType.SubSetChildCondition:
                    case NodeType.SubQuery:
                        for (int i = 0; i < qt.Count; i++)
                        {
                            if (i < deleteIndex)
                            {
                                newQt.Add(i, qt[i]);
                            }
                            if (i > deleteIndex)
                            {
                                newQt.Add((i - 1), qt[i]);
                                newQt[i - 1].LogicalOrder--;
                                if (newQt[i - 1].SubsetID == qti.SubsetID)
                                {
                                    newQt[i - 1].SubsetLogicalOrder--;
                                }
                            }
                        }
                        if (qti.QueryDetailID > 0)
                        {
                            DeletedQueryDetails.Add(qti.QueryDetailID);
                        }
                        break;
                    case NodeType.SubSetParent:
                        int deleteNodeChunkCount = 1;
                        if (qti.QueryDetailID > 0)
                        {
                            DeletedQueryDetails.Add(qti.QueryDetailID);
                        }
                        for (int i = 0; i < qt.Count; i++)
                        {
                            if (i < deleteIndex)
                            {
                                newQt.Add(i, qt[i]);
                            }
                            if (i > deleteIndex)
                            {
                                if (qt[i].ParentValuePath.IndexOf(string.Format("/{0}", qti.TreeNodeID)) < 1)
                                {
                                    newQt.Add((i - deleteNodeChunkCount), qt[i]);
                                }
                                else
                                {
                                    deleteNodeChunkCount++;
                                    if (qt[i].QueryDetailID > 0)
                                    {
                                        DeletedQueryDetails.Add(qt[i].QueryDetailID);
                                    }
                                }
                            }
                        }
                        break;
                }
                Session["QueryTree"] = newQt;
                Session["DeletedQueryDetailList"] = DeletedQueryDetails;
                Campaign objCampaign = (Campaign)Session["Campaign"];
                RenderQueryTree(objCampaign, newQt);
                HideAllEditPanels();
                lbtnDeleteSelected.Enabled = false;
            }
            catch (Exception ex)
            {
                PageMessage = "Error in edit subset event: " + ex.Message;
            }
        }


        protected void lbtnEditSubset_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSubsetName.Text))
                {
                    PageMessage = ("You must enter a name for your condition subset.");
                    return;
                }

                if (Session["AddEditTreeItem"] == null) return;
                QueryTreeItem qti = (QueryTreeItem)Session["AddEditTreeItem"];
                QueryTreeItem newQti = new QueryTreeItem();
                QueryTree qt = (QueryTree)Session["QueryTree"];
                for (int i = 0; i < qt.Count; i++)
                {
                    if (qt[i] == qti)
                    {
                        newQti = qt[i];
                        break;
                    }
                }

                newQti.SubsetName = txtSubsetName.Text;

                Campaign objCampaign = (Campaign)Session["Campaign"];
                RenderQueryTree(objCampaign, qt);
                HideAllEditPanels();

            }
            catch (Exception ex)
            {
                PageMessage = "Error in edit subset event: " + ex.Message;
            }
        }

        protected void lbtnEditSubQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AddEditTreeItem"] == null) return;
                QueryTreeItem qti = (QueryTreeItem)Session["AddEditTreeItem"];
                QueryTreeItem newQti = new QueryTreeItem();
                QueryTree qt = (QueryTree)Session["QueryTree"];
                for (int i = 0; i < qt.Count; i++)
                {
                    if (qt[i] == qti)
                    {
                        newQti = qt[i];
                        break;
                    }
                }

                newQti.SubQueryID = Convert.ToInt64(ddlQueryList.SelectedValue);
                newQti.SubQueryName = ddlQueryList.SelectedItem.Text;

                Campaign objCampaign = (Campaign)Session["Campaign"];
                RenderQueryTree(objCampaign, qt);
                HideAllEditPanels();
            }
            catch (Exception ex)
            {
                PageMessage = "Error in edit sub query event: " + ex.Message;
            }
        }

        protected void lbtnCancelEdit_Click(object sender, EventArgs e)
        {
            HideAllEditPanels();
            hdnEditingFirstNode = null;
            hdnNodeEdit = null;

            //for (int i = 0; i < trvQueryConditions.Nodes.Count; i++)
            //{
            //    if (trvQueryConditions.Nodes[i].Selected)
            //    {
            //        trvQueryConditions.Nodes[i].Selected = false;
            //        break;
            //    }
            //}
            QueryTree qt;
            if (Session["QueryTree"] != null)
            {
                qt = (QueryTree)Session["QueryTree"];
            }
            else
            {
                qt = new QueryTree();
            }
            Campaign objCampaign = (Campaign)Session["Campaign"];
            RenderQueryTree(objCampaign, qt);

        }

        #endregion

        #region Tree Events
        protected void trvQueryConditions_SelectedChange(object sender, EventArgs e)
        {
            try
            {
                hdnEditingFirstNode.Value = "";
                hdnNodeEdit.Value = "";
                if (trvQueryConditions.SelectedNode.Value == "0")
                { return; }
                QueryTree qt;
                if (Session["QueryTree"] != null)
                {
                    qt = (QueryTree)Session["QueryTree"];
                }
                else
                {
                    qt = new QueryTree();
                }



                if (trvQueryConditions.SelectedNode.Value.StartsWith("New"))
                {
                    lbtnDeleteSelected.Enabled = false;
                    QueryTreeItem qti = new QueryTreeItem();
                    // Add new element selected
                    string[] strValueArgs = trvQueryConditions.SelectedNode.Value.Split('&');
                    string strIndex = strValueArgs[0].TrimStart('N', 'e', 'w', '@');
                    int addIndex = Convert.ToInt16(strIndex);
                    qti.NewNodeAddIndex = addIndex;
                    qti.ParentTreeNodeID = Convert.ToInt16(strValueArgs[1].TrimStart('P', 'a', 'r', 'e', 'n', 't', '='));
                    qti.SubsetID = Convert.ToInt16(strValueArgs[2].TrimStart('S', 'u', 'b', 's', 'e', 't', '='));
                    qti.ParentSubsetID = Convert.ToInt16(strValueArgs[3].TrimStart('P', 'a', 'r', 'e', 'n', 't', 'S', 'u', 'b', 's', 'e', 't', '='));
                    Session["AddEditTreeItem"] = qti;
                    if (strValueArgs[4].EndsWith("true"))
                    {
                        ShowAddEditElementPanel(addIndex, true, true, qt);
                    }
                    else
                    {
                        ShowAddEditElementPanel(addIndex, true, false, qt);
                    }
                    ddlElementType.Items.Clear();
                    BindElementTypes(ddlElementType);
                }
                else
                {
                    // Existing element selected, edit
                    int addIndex = 0;
                    for (int i = 0; i < qt.Count; i++)
                    {
                        if (qt[i].TreeNodeID.ToString() == trvQueryConditions.SelectedNode.Value)
                        {
                            addIndex = i;
                            break;
                        }
                    }
                    lbtnDeleteSelected.Enabled = true;
                    ShowAddEditElementPanel(addIndex, false, false, qt);
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error in selected change event: " + ex.Message;
            }
            return;
        }



        #endregion

        #region Private Methods

        private void RenderQueryTree(Campaign objCampaign, QueryTree objQueryTree)
        {
            // Go through the QueryTree collection object and format nodes accordingly.
            try
            {
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                TreeNode currentRootNode = trvQueryConditions.FindNode("0");
                Dictionary<TreeNode, string> nodeAttributes = new Dictionary<TreeNode, string>();
                Dictionary<TreeNode, NodeType> nodeTypes = new Dictionary<TreeNode, NodeType>();
                if (currentRootNode != null)
                {
                    trvQueryConditions.Nodes.Remove(currentRootNode);
                }
                string rootNodeLabel = "New Query";
                if (txtQueryName.Text.Length > 0)
                {
                    rootNodeLabel = txtQueryName.Text;
                }
                TreeNode NewRootNode = new TreeNode(rootNodeLabel, "0");
                NewRootNode.PopulateOnDemand = false;
                NewRootNode.SelectAction = TreeNodeSelectAction.None;
                nodeAttributes.Add(NewRootNode, string.Format("Edit@{0}", "0"));
                nodeTypes.Add(NewRootNode, NodeType.RootNode);
                trvQueryConditions.Nodes.Add(NewRootNode);
                if (objQueryTree != null)
                {
                    PopulateParentNodeValuePaths(objQueryTree);
                    ApplyLogicalSqlElements(objQueryTree);

                    for (int i = 0; i < objQueryTree.Count; i++)
                    {
                        QueryTreeItem qti = objQueryTree[i];
                        qti.NodeLabel = BuildNodeLabel(qti);

                        string nodeUrl = "";
                        switch (qti.NodeType)
                        {
                            case NodeType.RootCondition:
                                //TreeNode NewConditionNode = new TreeNode(qti.NodeLabel, qti.TreeNodeID.ToString());
                                TreeNode NewConditionNode = new TreeNode(qti.NodeLabel, qti.TreeNodeID.ToString());
                                NewConditionNode.PopulateOnDemand = false;
                                NewConditionNode.ImageUrl = "~/images/TreeFilterIcon.png";
                                nodeAttributes.Add(NewConditionNode, string.Format("Edit@{0}", i.ToString()));
                                nodeTypes.Add(NewConditionNode, qti.NodeType);
                                nodeUrl = string.Format("javascript:window.showModalDialog('../campaign/EditQueryItem.aspx?'+ ( new Date() ).getTime()+'&TreeNodeIndex={0}' ,'EditQueryItem','dialogWidth:415px;dialogHeight:400px;edge:Raised;center:Yes;resizable:No;status:No');document.frmQueryDetailTree.submit();", i.ToString());
                                //NewConditionNode.NavigateUrl = nodeUrl;
                                NewRootNode.ChildNodes.Add(NewConditionNode);
                                break;
                            case NodeType.SubQuery:

                                DataSet dsSubQueryDetails;
                                DataTable dtSubQueryConditions = new DataTable();
                                dsSubQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, qti.SubQueryID.ToString());
                                dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                                qti.SubQueryName = dtSubQueryConditions.Rows[0]["QueryName"].ToString();
                                string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString(); ;
                                string strFilteredSubQueryConditions = "";

                                if (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") > 0)
                                    strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                                if (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") > 0)
                                    strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));

                                qti.SubQueryConditions = strFilteredSubQueryConditions.Trim();

                                TreeNode NewSubQNode = new TreeNode(qti.NodeLabel, qti.TreeNodeID.ToString());
                                NewSubQNode.PopulateOnDemand = false;
                                NewSubQNode.ImageUrl = "~/images/TreeQueryIcon.png";
                                NewSubQNode.ToolTip = qti.SubQueryConditions;
                                //nodeUrl = string.Format("javascript:window.showModalDialog('../campaign/EditQueryItem.aspx?'+ ( new Date() ).getTime()+'&TreeNodeIndex={0}' ,'EditQueryItem','dialogWidth:415px;dialogHeight:400px;edge:Raised;center:Yes;resizable:No;status:No');document.frmQueryDetailTree.submit();", i.ToString());
                                //NewSubQNode.NavigateUrl = nodeUrl;
                                nodeAttributes.Add(NewSubQNode, string.Format("Edit@{0}", i.ToString()));
                                nodeTypes.Add(NewSubQNode, qti.NodeType);
                                if (qti.ParentTreeNodeID > 0)
                                {
                                    TreeNode parentNode = trvQueryConditions.FindNode(qti.ParentValuePath);
                                    parentNode.ChildNodes.Add(NewSubQNode);
                                    if (i == (objQueryTree.Count - 1))
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    NewRootNode.ChildNodes.Add(NewSubQNode);
                                }
                                break;
                            case NodeType.SubSetParent:
                                TreeNode NewSubParentNode = new TreeNode(qti.NodeLabel, qti.TreeNodeID.ToString());
                                NewSubParentNode.PopulateOnDemand = false;
                                NewSubParentNode.ImageUrl = "~/images/Tree3FiltersIcon.png";
                                //nodeUrl = string.Format("javascript:window.showModalDialog('../campaign/EditQueryItem.aspx?'+ ( new Date() ).getTime()+'&TreeNodeIndex={0}' ,'EditQueryItem','dialogWidth:415px;dialogHeight:400px;edge:Raised;center:Yes;resizable:No;status:No');document.frmQueryDetailTree.submit();", i.ToString());
                                //NewSubParentNode.NavigateUrl = nodeUrl;
                                int newChildInsertIndex = i + 1;
                                for (int j = (objQueryTree.Count - 1); j > i; j--)
                                {
                                    if (objQueryTree[j].ParentTreeNodeID == qti.TreeNodeID || objQueryTree[j].ParentValuePath.IndexOf(string.Format(@"/{0}/", qti.TreeNodeID)) > 0)
                                    {
                                        // Found the insert index
                                        newChildInsertIndex = j + 1;
                                        break;
                                    }
                                }
                                nodeAttributes.Add(NewSubParentNode, string.Format("Edit@{0}&Parent={1}&Subset={2}&ParentSubset={3}", newChildInsertIndex.ToString(), qti.TreeNodeID, qti.SubsetID, qti.ParentSubsetID));
                                nodeTypes.Add(NewSubParentNode, qti.NodeType);
                                if (qti.ParentTreeNodeID > 0)
                                {
                                    TreeNode parentNode = trvQueryConditions.FindNode(qti.ParentValuePath);
                                    parentNode.ChildNodes.Add(NewSubParentNode);
                                    if (i == (objQueryTree.Count - 1))
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    NewRootNode.ChildNodes.Add(NewSubParentNode);
                                }
                                break;
                            case NodeType.SubSetChildCondition:
                                TreeNode subSetParentNode = trvQueryConditions.FindNode(qti.ParentValuePath);
                                if (subSetParentNode != null)
                                {
                                    TreeNode NewSubConditionNode = new TreeNode(qti.NodeLabel, qti.TreeNodeID.ToString());
                                    NewSubConditionNode.PopulateOnDemand = false;
                                    NewSubConditionNode.ImageUrl = "~/images/TreeFilterIcon.png";
                                    nodeAttributes.Add(NewSubConditionNode, string.Format("Edit@{0}", i.ToString()));
                                    nodeTypes.Add(NewSubConditionNode, qti.NodeType);
                                    subSetParentNode.ChildNodes.Add(NewSubConditionNode);
                                }
                                break;
                        }
                    }
                }

                foreach (KeyValuePair<TreeNode, string> kvp in nodeAttributes)
                {
                    TreeNode tn = kvp.Key;
                    if (nodeTypes[tn] == NodeType.SubSetParent)
                    {
                        TreeNode NewAddNode;
                        if (tn.ChildNodes.Count == 0)
                        {
                            NewAddNode = new TreeNode("Add New Element", string.Format("New@{0}&First=true", kvp.Value.TrimStart('E', 'd', 'i', 't', '@')));
                        }
                        else
                        {
                            NewAddNode = new TreeNode("Add New Element", string.Format("New@{0}&First=false", kvp.Value.TrimStart('E', 'd', 'i', 't', '@')));
                        }

                        NewAddNode.PopulateOnDemand = false;
                        //***NewAddNode.ToolTip = "Add a new condition, external query or sub query.";
                        NewAddNode.ToolTip = kvp.Value;
                        //NewAddNode.NavigateUrl = nodeUrl;
                        tn.ChildNodes.Add(NewAddNode);
                    }

                }

                TreeNode NewRootAddNode;
                if (objQueryTree == null || objQueryTree.Count < 1)
                {
                    NewRootAddNode = new TreeNode("Add New Element", string.Format("New@{0}&Parent=0&Subset=0&ParentSubset=0&First=true", 0));
                }
                else
                {
                    NewRootAddNode = new TreeNode("Add New Element", string.Format("New@{0}&Parent=0&Subset=0&ParentSubset=0&First=false", (objQueryTree.Count)));
                }
                NewRootAddNode.PopulateOnDemand = false;
                NewRootAddNode.ToolTip = "Add a new condition, external query or sub query.";
                //NewRootAddNode.NavigateUrl = newAddUrl;
                NewRootNode.ChildNodes.Add(NewRootAddNode);
                trvQueryConditions.ExpandAll();
                Session["QueryTree"] = objQueryTree;
                try
                {
                    if (hdnShowSQL.Value == "true")
                    {
                        lblSQL.Text = BuildSQLFromTree();
                        tblSQL.Visible = true;
                        lbtnHideSQL.Visible = true;
                        lbtnShowSQL.Visible = false;
                    }
                    else
                    {
                        tblSQL.Visible = false;
                        lbtnHideSQL.Visible = false;
                        lbtnShowSQL.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    PageMessage = "Exception showing / hiding raw sql :" + ex.Message;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error rendering query tree: " + ex.Message;
            }
        }

        private void ApplyLogicalSqlElements(QueryTree objQueryTree)
        {
            try
            {
                for (int i = 0; i < objQueryTree.Count; i++)
                {
                    QueryTreeItem qti = objQueryTree[i];
                    bool isFirstOnLevel = true;
                    qti.OpenParenPrefixCount = 0;
                    bool isLastOnLevel = true;
                    qti.CloseParenSuffixCount = 0;
                    qti.ApplyLogicalOperator = false;

                    // First, determine if I am first on my level = no logical Prefix
                    for (int j = (i - 1); j >= 0; j--)
                    {
                        if (objQueryTree[j].ParentTreeNodeID == qti.ParentTreeNodeID)
                        {
                            qti.ApplyLogicalOperator = true;
                            break;
                        }
                    }

                    if (qti.NodeType == NodeType.SubSetChildCondition || (qti.NodeType == NodeType.SubQuery && qti.ParentTreeNodeID != 0))
                    {
                        QueryTreeItem myParentNode = new QueryTreeItem();
                        // Calculate parentheses
                        // Open paren determination ---
                        for (int j = (i - 1); j >= 0; j--)
                        {
                            if (objQueryTree[j].ParentTreeNodeID == qti.ParentTreeNodeID)
                            {
                                isFirstOnLevel = false;
                            }
                            if (objQueryTree[j].TreeNodeID == qti.ParentTreeNodeID)
                            {
                                // Got the parent, populate for later use
                                myParentNode = objQueryTree[j];
                                break;
                            }
                        }

                        if (isFirstOnLevel)
                        {
                            // We know this is a first level condition.  Now determine if it's 1 or 2 open parens
                            qti.OpenParenPrefixCount = 1;
                            for (int j = (i + 1); j < objQueryTree.Count; j++)
                            {
                                if (objQueryTree[j].ParentTreeNodeID != qti.ParentTreeNodeID && objQueryTree[j].NodeType != NodeType.SubSetParent)
                                {
                                    QueryTreeItem testParentNode = new QueryTreeItem();
                                    for (int k = (j - 1); k >= 0; k--)
                                    {
                                        if (objQueryTree[k].TreeNodeID == objQueryTree[j].ParentTreeNodeID)
                                        {
                                            // Got the parent, populate for later use
                                            testParentNode = objQueryTree[k];
                                            break;
                                        }
                                    }
                                    if (testParentNode.ParentValuePath == myParentNode.ParentValuePath)
                                    {
                                        qti.OpenParenPrefixCount = 2;
                                        break;
                                    }
                                }
                            }
                        }

                        // Now, check for closed parens

                        for (int j = (i + 1); j < objQueryTree.Count; j++)
                        {
                            if ((objQueryTree[j].ParentTreeNodeID == qti.ParentTreeNodeID))
                            {
                                isLastOnLevel = false;
                                break;
                            }
                        }

                        if (isLastOnLevel)
                        {
                            qti.CloseParenSuffixCount = 1;
                            bool lastOnTree = true;
                            if (i != (objQueryTree.Count - 1))
                            {
                                for (int j = (i + 1); j < objQueryTree.Count; j++)
                                {
                                    if ((objQueryTree[j].NodeType != NodeType.SubSetParent))
                                    {
                                        lastOnTree = false;
                                        break;
                                    }
                                }
                            }

                            if (lastOnTree)
                            {
                                qti.CloseParenSuffixCount = 0;
                                // Last in tree, clean up all open parens
                                for (int j = i; j >= 0; j--)
                                {
                                    qti.CloseParenSuffixCount = qti.CloseParenSuffixCount + objQueryTree[j].OpenParenPrefixCount;
                                    qti.CloseParenSuffixCount = qti.CloseParenSuffixCount - objQueryTree[j].CloseParenSuffixCount;
                                }
                                if (isFirstOnLevel)
                                {
                                    qti.CloseParenSuffixCount++;
                                }
                            }
                            else
                            {
                                if (objQueryTree[i + 1].ParentValuePath.Length < myParentNode.ParentValuePath.Length)
                                {
                                    // the next guy is further up the tree, determine how far
                                    string[] nextGuyValuePath = objQueryTree[i + 1].ParentValuePath.Split('/');
                                    string[] myValuePath = qti.ParentValuePath.Split('/');
                                    qti.CloseParenSuffixCount = myValuePath.Length - nextGuyValuePath.Length;
                                }
                            }
                        }
                    }

                    if (qti.NodeType == NodeType.SubQuery)
                    {
                        qti.OpenParenPrefixCount++;
                        qti.CloseParenSuffixCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error determining logical SQL elements: " + ex.Message;
            }
        }

        private void PopulateParentNodeValuePaths(QueryTree objQueryTree)
        {
            try
            {
                for (int i = 0; i < objQueryTree.Count; i++)
                {
                    string valuePath = "";
                    QueryTreeItem qti = objQueryTree[i];

                    if (qti.ParentTreeNodeID == 0)
                        qti.ParentValuePath = "0";
                    else
                    {
                        for (int j = i; j >= 0; j--)
                        {
                            if (objQueryTree[j].TreeNodeID == qti.ParentTreeNodeID)
                            {
                                valuePath = string.Format("{0}/{1}", objQueryTree[j].ParentValuePath, qti.ParentTreeNodeID.ToString());
                                break;
                            }
                        }
                        qti.ParentValuePath = valuePath;
                    }
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error extrpolating node value path: " + ex.Message;
                return;
            }
        }


        private QueryTree BuildQueryTreeFromDatabase(Campaign objCampaign)
        {
            // Populates a Query tree object, which will follow the session and store all query info until saved
            try
            {
                if (Request.QueryString["QueryID"] == null)
                {
                    return null;
                }
                DataSet dsQueryDetails;
                DataTable dtQueryConditions = new DataTable();
                string strQueryID = Request.QueryString["QueryID"].ToString();
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, strQueryID);
                dtQueryConditions = dsQueryDetails.Tables[0];
                QueryTree qt = new QueryTree();

                if (dtQueryConditions.Rows.Count < 1)
                {
                    qt = null;
                }
                else
                {
                    // Loop through the dataset rows and build nodes
                    for (int i = 0; i < dtQueryConditions.Rows.Count; i++)
                    {
                        QueryTreeItem qti = new QueryTreeItem();

                        if (dtQueryConditions.Rows[i]["QueryDetailID"] != DBNull.Value)
                        {
                            qti.QueryDetailID = Convert.ToInt64(dtQueryConditions.Rows[i]["QueryDetailID"]);
                        }
                        else
                        {
                            qti.QueryDetailID = 0;
                        }

                        if (dtQueryConditions.Rows[i]["QueryID"] != DBNull.Value)
                        {
                            qti.QueryID = Convert.ToInt64(dtQueryConditions.Rows[i]["QueryID"]);
                        }
                        else
                        {
                            qti.QueryID = 0;
                        }

                        if (dtQueryConditions.Rows[i]["QueryName"] != DBNull.Value)
                        {
                            qti.QueryName = dtQueryConditions.Rows[i]["QueryName"].ToString();
                        }
                        else
                        {
                            qti.QueryName = null;
                        }

                        if (dtQueryConditions.Rows[i]["SearchCriteria"] != DBNull.Value)
                        {
                            qti.SearchCriteria = dtQueryConditions.Rows[i]["SearchCriteria"].ToString();
                        }
                        else
                        {
                            qti.SearchCriteria = null;
                        }

                        if (dtQueryConditions.Rows[i]["SearchOperator"] != DBNull.Value)
                        {
                            qti.SearchOperator = dtQueryConditions.Rows[i]["SearchOperator"].ToString();
                        }
                        else
                        {
                            qti.SearchOperator = null;
                        }

                        if (dtQueryConditions.Rows[i]["SearchString"] != DBNull.Value)
                        {
                            qti.SearchString = dtQueryConditions.Rows[i]["SearchString"].ToString();
                        }
                        else
                        {
                            qti.SearchCriteria = null;
                        }

                        if (dtQueryConditions.Rows[i]["LogicalOperator"] != DBNull.Value)
                        {
                            qti.LogicalOperator = (dtQueryConditions.Rows[i]["LogicalOperator"].ToString()).Trim();
                        }
                        else
                        {
                            qti.LogicalOperator = null;
                        }

                        if (dtQueryConditions.Rows[i]["LogicalOrder"] != DBNull.Value)
                        {
                            qti.LogicalOrder = Convert.ToInt16(dtQueryConditions.Rows[i]["LogicalOrder"]);
                        }
                        else
                        {
                            qti.LogicalOrder = 0;
                        }

                        if (dtQueryConditions.Rows[i]["SubQueryID"] != DBNull.Value)
                        {
                            qti.SubQueryID = Convert.ToInt64(dtQueryConditions.Rows[i]["SubQueryID"]);
                            if (qti.SubQueryID > 0)
                            {
                                DataSet dsSubQueryDetails;
                                DataTable dtSubQueryConditions = new DataTable();
                                dsSubQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, qti.SubQueryID.ToString());
                                dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                                qti.SubQueryName = dtSubQueryConditions.Rows[0]["QueryName"].ToString();
                                string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString(); ;
                                string strFilteredSubQueryConditions = "";

                                if (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") > 0)
                                    strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                                if (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") > 0)
                                    strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));

                                qti.SubQueryConditions = strFilteredSubQueryConditions.Trim();
                            }
                        }
                        else
                        {
                            qti.SubQueryID = 0;
                        }

                        if (dtQueryConditions.Rows[i]["SubsetID"] != DBNull.Value)
                        {
                            qti.SubsetID = Convert.ToInt16(dtQueryConditions.Rows[i]["SubsetID"]);
                        }
                        else
                        {
                            qti.SubsetID = 0;
                        }

                        if (dtQueryConditions.Rows[i]["SubsetName"] != DBNull.Value)
                        {
                            qti.SubsetName = dtQueryConditions.Rows[i]["SubsetName"].ToString();
                        }
                        else
                        {
                            qti.SubsetName = null;
                        }

                        if (dtQueryConditions.Rows[i]["SubsetLevel"] != DBNull.Value)
                        {
                            qti.SubsetLevel = Convert.ToInt16(dtQueryConditions.Rows[i]["SubsetLevel"]);
                        }
                        else
                        {
                            qti.SubsetLevel = 0;
                        }

                        if (dtQueryConditions.Rows[i]["SubsetLogicalOrder"] != DBNull.Value)
                        {
                            qti.SubsetLogicalOrder = Convert.ToInt16(dtQueryConditions.Rows[i]["SubsetLogicalOrder"]);
                        }
                        else
                        {
                            qti.SubsetLogicalOrder = 0;
                        }

                        if (dtQueryConditions.Rows[i]["ParentSubsetID"] != DBNull.Value)
                        {
                            qti.ParentSubsetID = Convert.ToInt16(dtQueryConditions.Rows[i]["ParentSubsetID"].ToString());
                        }
                        else
                        {
                            qti.ParentSubsetID = 0;
                        }

                        if (dtQueryConditions.Rows[i]["TreeNodeID"] != DBNull.Value)
                        {
                            qti.TreeNodeID = Convert.ToInt16(dtQueryConditions.Rows[i]["TreeNodeID"]);
                        }
                        else
                        {
                            qti.TreeNodeID = 0;
                        }

                        if (dtQueryConditions.Rows[i]["ParentTreeNodeID"] != DBNull.Value)
                        {
                            qti.ParentTreeNodeID = Convert.ToInt16(dtQueryConditions.Rows[i]["ParentTreeNodeID"]);
                        }
                        else
                        {
                            qti.ParentTreeNodeID = 0;
                        }

                        if (dtQueryConditions.Rows[i]["DateCreated"] != DBNull.Value)
                        {
                            qti.DateCreated = Convert.ToDateTime(dtQueryConditions.Rows[i]["DateCreated"]);
                        }
                        else
                        {
                            qti.DateCreated = DateTime.Now;
                        }

                        if (dtQueryConditions.Rows[i]["DateModified"] != DBNull.Value)
                        {
                            qti.DateModified = Convert.ToDateTime(dtQueryConditions.Rows[i]["DateModified"]);
                        }
                        else
                        {
                            qti.DateModified = DateTime.Now;
                        }

                        if (dtQueryConditions.Rows[i]["ElementText"] != DBNull.Value)
                        {
                            qti.NodeLabelNameSubstitute = dtQueryConditions.Rows[i]["ElementText"].ToString(); ;
                        }
                        else
                        {
                            qti.NodeLabelNameSubstitute = "";
                        }

                        // Determine the tree node attributes to store now.
                        // First, determine the nodetype
                        if (qti.SubsetID < 1 && qti.SubQueryID < 1)
                        {
                            qti.NodeType = NodeType.RootCondition;
                        }
                        if (qti.SubQueryID > 0)
                        {
                            qti.NodeType = NodeType.SubQuery;
                        }
                        if (qti.SubsetID > 0 && qti.SubsetLogicalOrder < 1)
                        {
                            qti.NodeType = NodeType.SubSetParent;
                        }
                        if (qti.SubsetID > 0 && qti.SubsetLogicalOrder > 0)
                        {
                            qti.NodeType = NodeType.SubSetChildCondition;
                        }

                        qt.Add(i, qti);
                    }
                }
                return qt;

            }
            catch (Exception ex)
            {
                PageMessage = "Error building query tree collection from database: " + ex.Message;
                return null;
            }
        }

        private string BuildNodeLabel(QueryTreeItem queryTreeItem)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string nodeLabel = "";

                if (queryTreeItem.ApplyLogicalOperator)
                {
                    sb.Append(queryTreeItem.LogicalOperator + " ");
                }

                for (int i = 0; i < queryTreeItem.OpenParenPrefixCount; i++)
                {
                    sb.Append("(");
                }

                switch (queryTreeItem.NodeType)
                {
                    case NodeType.SubQuery:
                        sb.Append("Query: " + queryTreeItem.SubQueryName);
                        break;
                    case NodeType.SubSetParent:
                        sb.Append("Sub-Set: " + queryTreeItem.SubsetName);
                        break;
                    default:
                        sb.AppendFormat("{0} ", queryTreeItem.SearchCriteria);

                        switch (queryTreeItem.SearchOperator)
                        {
                            case "{0} = '{1}'":
                                sb.AppendFormat("= ");
                                break;
                            case "{0} LIKE '%{1}%'":
                                sb.AppendFormat("Contains ");
                                break;
                            case "{0} NOT LIKE '%{1}%'":
                                sb.AppendFormat("Does Not Contain ");
                                break;
                            case "{0} LIKE '{1}%'":
                                sb.AppendFormat("Begins With ");
                                break;
                            case "{0} LIKE '%{1}'":
                                sb.AppendFormat("Ends With ");
                                break;
                            case "{0} > '{1}'":
                                sb.AppendFormat("> ");
                                break;
                            case "{0} < '{1}'":
                                sb.AppendFormat("< ");
                                break;
                            case "{0} >= '{1}'":
                                sb.AppendFormat(">= ");
                                break;
                            case "{0} <= '{1}'":
                                sb.AppendFormat("<= ");
                                break;
                            case "{0} Is Null {1}":
                                sb.AppendFormat("Is Null");
                                nodeLabel = sb.ToString();
                                return nodeLabel;
                            case "{0} Is Not Null {1}":
                                sb.AppendFormat("Is Not Null");
                                nodeLabel = sb.ToString();
                                return nodeLabel;
                            case "{0} <> 0":
                                sb.AppendFormat("Is True");
                                nodeLabel = sb.ToString();
                                return nodeLabel;
                            case "{0} = 0":
                                sb.AppendFormat("Is False");
                                nodeLabel = sb.ToString();
                                return nodeLabel;
                            case "{0} <> '{1}'":
                                sb.AppendFormat("<> ");
                                break;

                            case "{0} = {1}":
                                sb.AppendFormat("= ");
                                break;
                            case "{0} LIKE %{1}%":
                                sb.AppendFormat("Contains ");
                                break;
                            case "{0} NOT LIKE %{1}%":
                                sb.AppendFormat("Does Not Contain ");
                                break;
                            case "{0} LIKE {1}%":
                                sb.AppendFormat("Begins With ");
                                break;
                            case "{0} LIKE %{1}":
                                sb.AppendFormat("Ends With ");
                                break;
                            case "{0} > {1}":
                                sb.AppendFormat("> ");
                                break;
                            case "{0} < {1}":
                                sb.AppendFormat("< ");
                                break;
                            case "{0} >= {1}":
                                sb.AppendFormat(">= ");
                                break;
                            case "{0} <= {1}":
                                sb.AppendFormat("<= ");
                                break;
                            
                            case "{0} <> {1}":
                                sb.AppendFormat("<> ");
                                break;

                        }

                        if (string.IsNullOrEmpty(queryTreeItem.NodeLabelNameSubstitute))
                            sb.AppendFormat("{0}", queryTreeItem.SearchString);
                        else
                            sb.AppendFormat("{0}", queryTreeItem.NodeLabelNameSubstitute);
                        break;
                }

                for (int i = 0; i < queryTreeItem.CloseParenSuffixCount; i++)
                {
                    sb.Append(")");
                }

                nodeLabel = sb.ToString();
                return nodeLabel;
            }
            catch (Exception ex)
            {
                PageMessage = "Error building node label: " + ex.Message;
                return "";
            }
        }

        private string BuildSQLFromTree()
        {

            try
            {
                QueryTree objQueryTree = (QueryTree)Session["QueryTree"];

                if (objQueryTree == null || objQueryTree.Count < 1)
                {
                    return "There are currently no conditions in this query.";
                }

                //List<QueryTreeItem> openParenthesesParents = new List<QueryTreeItem>();
                StringBuilder sb = new StringBuilder();


                for (int i = 0; i < objQueryTree.Count; i++)
                {
                    QueryTreeItem qti = objQueryTree[i];
                    if (qti.ApplyLogicalOperator)
                    {
                        sb.AppendFormat(" {0} ", qti.LogicalOperator);
                    }
                    for (int j = 0; j < qti.OpenParenPrefixCount; j++)
                    {
                        sb.Append("(");
                    }
                    switch (qti.NodeType)
                    {
                        case NodeType.RootCondition:
                        case NodeType.SubSetChildCondition:
                            sb.AppendFormat(qti.SearchOperator, qti.SearchCriteria, qti.SearchString.Replace("'", "''"));
                            //sb.Append(" ");
                            break;
                        case NodeType.SubQuery:
                            sb.Append(qti.SubQueryConditions);
                            //sb.Append(" ");
                            break;
                    }
                    for (int j = 0; j < qti.CloseParenSuffixCount; j++)
                    {
                        sb.Append(")");
                    }
                }

                string SQLstring = sb.ToString().Trim();
                SQLstring = SQLRemoveTrailingLogicals(SQLstring);
                if (SQLstring.Length < 3)
                {
                    return "There are currently no conditions in this query.";
                }
                return SQLstring;
            }
            catch (Exception ex)
            {
                PageMessage = "Error building SQL string from tree: " + ex.Message;
                return "There are currently no conditions in this query.";
            }

        }

        private string SQLRemoveTrailingLogicals(string sqlString)
        {
            bool allGone = false;
            string SQLstring = sqlString;
            // Remove any leftover logical operators on the end of the string
            while (!allGone)
            {
                SQLstring = SQLstring.Trim();
                if (SQLstring.EndsWith("AND"))
                    SQLstring = SQLstring.Substring(0, SQLstring.Length - 3);
                if (SQLstring.EndsWith("OR"))
                    SQLstring = SQLstring.Substring(0, SQLstring.Length - 2);
                if (SQLstring.EndsWith("AND "))
                    SQLstring = SQLstring.Substring(0, SQLstring.Length - 4);
                if (SQLstring.EndsWith("OR "))
                    SQLstring = SQLstring.Substring(0, SQLstring.Length - 3);
                if (SQLstring.StartsWith("AND"))
                    SQLstring = SQLstring.Substring(3, SQLstring.Length - 3);
                if (SQLstring.StartsWith("OR"))
                    SQLstring = SQLstring.Substring(2, SQLstring.Length - 2);
                if (SQLstring.StartsWith(" AND"))
                    SQLstring = SQLstring.Substring(4, SQLstring.Length - 4);
                if (SQLstring.StartsWith(" OR"))
                    SQLstring = SQLstring.Substring(3, SQLstring.Length - 3);
                SQLstring = SQLstring.Trim();
                if (SQLstring.EndsWith("AND") || SQLstring.EndsWith("OR") || SQLstring.StartsWith("AND") || SQLstring.StartsWith("OR"))
                { allGone = false; }
                else
                { allGone = true; }
            }
            return SQLstring;
        }

        /// <summary>
        /// Save Query Detail
        /// </summary>
        private void SaveQueryToDB()
        {
            if (txtQueryName.Text.Length < 1)
            {
                PageMessage = "Please enter a name for your query in order to save.";
                return;
            }
            string strQueryCondition = BuildSQLFromTree();
            if (strQueryCondition == "There are currently no conditions in this query.")
            {
                PageMessage = "There must be at least one condition to save the query.";
                return;
            }
            if (!TestSQLSyntax(strQueryCondition, (QueryTree)Session["QueryTree"]))
                return;
            // Setup objects needed
            QueryTree qt = (QueryTree)Session["QueryTree"];
            Campaign objCampaign = (Campaign)Session["Campaign"];
            QueryDetail objQueryDetail = new QueryDetail();
            Query objQuery = new Query();
            StringBuilder sbQuery = new StringBuilder();
            List<XmlNode> queryDetailList = new List<XmlNode>();

            try
            {
                if (Request.QueryString["QueryID"] != null)
                {
                    objQuery.QueryID = Convert.ToInt64(Request.QueryString["QueryID"]);
                }
                objQuery.QueryName = txtQueryName.Text;
                objQuery.QueryCondition = BuildQueryCondition(strQueryCondition);
                if (Session["DeletedQueryDetailList"] != null)
                {
                    List<long> DeletedQueryDetails = (List<long>)Session["DeletedQueryDetailList"];
                    XmlDocument xDocCampaign1 = new XmlDocument();
                    xDocCampaign1.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    CampaignService objCampaignService1 = new CampaignService();
                    foreach (long qdid in DeletedQueryDetails)
                    {
                        objCampaignService1.DeleteQueryDetail(xDocCampaign1, qdid);
                    }
                    DeletedQueryDetails.Clear();
                }

                for (int i = 0; i < qt.Count; i++)
                {
                    objQueryDetail.SearchCriteria = qt[i].SearchCriteria;
                    objQueryDetail.SearchOperator = qt[i].SearchOperator;
                    objQueryDetail.SearchString = qt[i].SearchString;
                    objQueryDetail.LogicalOperator = qt[i].LogicalOperator;
                    objQueryDetail.LogicalOrder = qt[i].LogicalOrder;
                    objQueryDetail.SubQueryID = qt[i].SubQueryID;
                    objQueryDetail.SubsetID = qt[i].SubsetID;
                    objQueryDetail.SubsetName = qt[i].SubsetName;
                    objQueryDetail.ParentSubsetID = qt[i].ParentSubsetID;
                    objQueryDetail.SubsetLevel = qt[i].SubsetLevel;
                    objQueryDetail.SubsetLogicalOrder = qt[i].SubsetLogicalOrder;
                    objQueryDetail.TreeNodeID = qt[i].TreeNodeID;
                    objQueryDetail.ParentTreeNodeID = qt[i].ParentTreeNodeID;
                    objQueryDetail.ElementText = qt[i].NodeLabelNameSubstitute;
                    if (qt[i].QueryDetailID > 0)
                    {
                        objQueryDetail.QueryDetailID = qt[i].QueryDetailID;
                        objQueryDetail.DateModified = DateTime.Now;
                    }
                    else
                    {
                        objQueryDetail.QueryDetailID = 0;
                        objQueryDetail.DateCreated = DateTime.Now;
                        objQueryDetail.DateModified = DateTime.Now;
                    }
                    XmlDocument xDocQueryDetail = new XmlDocument();
                    xDocQueryDetail.LoadXml(Serialize.SerializeObject(objQueryDetail, "QueryDetail"));
                    queryDetailList.Add(xDocQueryDetail);


                }

                XmlDocument xDocQuery = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocQuery.LoadXml(Serialize.SerializeObject(objQuery, "Query"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                CampaignService objCampaignService = new CampaignService();
                objQuery = (Query)Serialize.DeserializeObject(objCampaignService.QueryDetailInsertUpdate(
                xDocCampaign, queryDetailList.ToArray(), xDocQuery), "Query");

                Session["DeletedQueryDetailList"] = null;

                if (Request.QueryString["DataManager"] != null)
                {
                    Response.Redirect("~/campaign/DataPortal.aspx");
                }
                else
                {
                    //Response.Redirect("~/campaign/QueryStatus.aspx");
                    Response.Redirect("~/campaign/Home.aspx");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("DuplicateQueryException") >= 0)
                    hdnDuplicateQuery.Value = "true";
            }
        }

        private void BindElementTypes(DropDownList ddl)
        {
            try
            {
                ddl.Items.Add(new ListItem("Condition", "0"));
                ddl.Items.Add(new ListItem("Sub-Query", "1"));
                ddl.Items.Add(new ListItem("Existing Query", "2"));
            }
            catch { }
        }

        private bool TestSQLSyntax(string sqlStatement, QueryTree qt)
        {
            string strQueryCondition = "";
            string strQueryTreeDump = "";
            Campaign objCampaign = (Campaign)Session["Campaign"];
            try
            {
                XmlDocument xQueryTree = new XmlDocument();

                foreach (QueryTreeItem qti in qt.Values)
                {
                    xQueryTree.LoadXml(Serialize.SerializeObject(qti, qti.GetType()));
                }
                strQueryTreeDump = xQueryTree.ToString();
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xQueryTree.WriteTo(xw);
                strQueryTreeDump = sw.ToString();
                strQueryCondition = BuildQueryCondition(sqlStatement);
                dsQueryTester.ConnectionString = objCampaign.CampaignDBConnString;
                dsQueryTester.SelectCommand = strQueryCondition;
                dsQueryTester.Select(DataSourceSelectArguments.Empty);
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteException(ex, "Admin");
                ActivityLogger.WriteAdminEntry(objCampaign, "Failed query tree query '{0}'.", strQueryCondition);
                ActivityLogger.WriteAdminEntry(objCampaign, "Query tree dump = '{0}'.", strQueryTreeDump);
                PageMessage = "The query builder has created SQL which contains an error.\r\nThe conditions have been written to a log.\r\nPlease try to rebuild your query.";
                return false;
            }
        }

        private string GetLogicalOperator(QueryTree qt, QueryTreeItem qti, int currentIndex)
        {
            string operatorAppend = "";

            try
            {
                for (int i = (currentIndex + 1); i <= qt.Count; i++)
                {
                    if (qt[i].ParentTreeNodeID == qti.ParentTreeNodeID)
                    {
                        operatorAppend = qt[i].LogicalOperator;
                        return operatorAppend;
                    }

                }
                return "";
            }
            catch
            { return ""; }

        }

        // Derializer for tree dump above
        //public void ReadXml(System.Xml.XmlReader reader)
        //{
        //    // Used while Deserialization

        //    // Move past container
        //    reader.Read();

        //    // Deserialize and add the BizEntitiy objects
        //    while (reader.NodeType != XmlNodeType.EndElement)
        //    {
        //        BizEntity entity;

        //        entity = Serializer.Deserialize(reader) as BizEntity;
        //        reader.MoveToContent();
        //        this.Dictionary.Add(entity.Key, entity);
        //    }
        //}
        #endregion

        #region AddEditMethods
        private void ShowAddEditElementPanel(int addEditIndex, bool isNewNode, bool isFirstNode, QueryTree objQueryTree)
        {
            try
            {
                pnlChooseElementType.Visible = false;
                pnlLogical.Visible = false;
                pnlSubQuery.Visible = false;
                pnlSubset.Visible = false;
                pnlCondition.Visible = false;

                if (isNewNode)
                {
                    // New node, go through whole process
                    if (isFirstNode)
                    {
                        hdnEditingFirstNode.Value = "true";
                    }
                    else
                    {
                        hdnEditingFirstNode.Value = "false";
                    }
                    pnlChooseElementType.Visible = true;
                    pnlLogical.Visible = false;
                    pnlSubQuery.Visible = false;
                    pnlSubset.Visible = false;
                    pnlCondition.Visible = false;
                    return;
                }
                else
                {
                    QueryTreeItem qti = objQueryTree[addEditIndex];
                    Session["AddEditTreeItem"] = qti;
                    if (qti.ApplyLogicalOperator)
                    {
                        switch (qti.LogicalOperator.ToLower())
                        {
                            case "and":
                                pnlLogical.Visible = true;
                                RdoAnd.Checked = true;
                                hdnNodeEdit.Value = "true";
                                break;
                            case "or":
                                pnlLogical.Visible = true;
                                RdoOr.Checked = true;
                                hdnNodeEdit.Value = "true";
                                break;
                            default:
                                ShowEditPanel(qti);
                                break;
                        }
                    }
                    else
                    {
                        ShowEditPanel(qti);
                    }
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error showing panel: " + ex.Message;
            }
        }

        private void ShowEditPanel(QueryTreeItem qti)
        {
            try
            {
                switch (qti.NodeType)
                {
                    case NodeType.RootCondition:
                    case NodeType.SubSetChildCondition:
                        pnlCondition.Visible = true;
                        BindConditionControls(qti);
                        lblEditCondition.Visible = true;
                        lblCreateCondition.Visible = false;
                        lbtnAddCondition.Visible = false;
                        lbtnEditCondition.Visible = true;
                        break;
                    case NodeType.SubQuery:
                        pnlSubQuery.Visible = true;
                        BindQueryList(qti);
                        lbtnAddSubQuery.Visible = false;
                        lbtnEditSubQuery.Visible = true;
                        break;
                    case NodeType.SubSetParent:
                        pnlSubset.Visible = true;
                        txtSubsetName.Text = qti.SubsetName;
                        lbtnAddSubset.Visible = false;
                        lbtnEditSubset.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error showing edit panel: " + ex.Message;
            }
        }

        private void HideAllEditPanels()
        {
            try
            {
                pnlChooseElementType.Visible = false;
                pnlCondition.Visible = false;
                pnlLogical.Visible = false;
                pnlSubQuery.Visible = false;
                pnlSubset.Visible = false;
            }
            catch (Exception ex)
            {
                PageMessage = "Error showing edit panel: " + ex.Message;
            }



        }
        #endregion

        #region Edit Control Builders
        private void BindQueryList(QueryTreeItem qti)
        {
            DataSet dsQuerList;
            DataTable dtQueryList = new DataTable();

            try
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsQuerList = objCampService.GetQueryList(xDocCampaign);
                dtQueryList = dsQuerList.Tables[0];

                foreach (DataRow row in dtQueryList.Rows)
                {
                    ListItem ltm = new ListItem(row["QueryName"].ToString(), row["QueryID"].ToString());
                    string strSubQueryConditions = row["QueryCondition"].ToString();
                    string strFilteredSubQueryConditions = "";

                    if (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                    if (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));

                    strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                    ltm.Attributes.Add("title", strFilteredSubQueryConditions);
                    ddlQueryList.Items.Add(ltm);
                }

                ddlQueryList.SelectedValue = qti.SubQueryID.ToString();

            }
            catch (Exception ex)
            {
                PageMessage = "Exception binding query list: " + ex.Message;
            }
        }

        private void BindConditionControls(QueryTreeItem qti)
        {
            try
            {
                BindColumnsDropdown(ddlCriteria, new ListItem());
                BindAdditionalFields(ddlCriteria);

                ddlCriteria.SelectedIndex =
                        ddlCriteria.Items.IndexOf(ddlCriteria.Items.FindByText(qti.SearchCriteria));

                // 2012-06-12 Dave Pollastrini
                // Changed BindOperator to take a datatype.
                /*
                BindOperator(ddlOperator, qti.IsDateField);
                */

                string[] fieldInfo = ddlCriteria.SelectedValue.Split(':');
                string dataType =
                    fieldInfo[0].Equals("NeverCallFlag", StringComparison.InvariantCultureIgnoreCase)
                    ? "boolean" : fieldInfo[1];

                BindOperator(ddlOperator, dataType);

                ddlOperator.SelectedValue = qti.SearchOperator;

                CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);

                if (ddlOperator.SelectedItem.Text.IndexOf("Null") > 0)
                {
                    ddlPickByName.Enabled = false;
                    txtEnterValue.Enabled = false;
                    return;
                }

                if (txtEnterValue.Visible)
                {
                    txtEnterValue.Text = qti.SearchString;
                }
                else
                {
                    //ddlPickByName.SelectedItem.Text = qti.NodeLabelNameSubstitute;
                    ddlPickByName.SelectedValue = qti.SearchString;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception binding condition controls: " + ex.Message;
            }
        }

        private void BindAdditionalFields(DropDownList ddl)
        {
            Campaign objCampaign = (Campaign)Session["Campaign"];
            DataSet dsFields;
            try
            {
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();

                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsFields = objCampService.GetCampaignFields(xDocCampaign);

                ddl.Items.Add(new ListItem("UniqueKey", "UniqueKey:Integer"));
                foreach (DataRow row in dsFields.Tables[0].Rows)
                {
                    if (!(bool)(row["IsDefault"]))
                    {
                        ddl.Items.Add(new ListItem(row["FieldName"].ToString(),
                            row["FieldName"].ToString() + ":" + row["FieldType"].ToString()));
                    }
                }
            }
            catch { }
        }

        private void CheckForNameDropDowns(DropDownList ddlCriteria, DropDownList ddlOperator, DropDownList ddlPickByName, TextBox txtEnterValue)
        {
            try
            {
                if (ddlOperator.SelectedValue == "{0} = '{1}'" || ddlOperator.SelectedValue == "{0} <> '{1}'")
                {
                    switch (ddlCriteria.SelectedValue)
                    {
                        case "AgentID:String":
                        case "AgentID":
                            // we have agent selected and equals, build agent list.
                            ddlPickByName.Items.Clear();
                            BindAgentNames(ddlPickByName);
                            txtEnterValue.Visible = false;
                            ddlPickByName.Visible = true;
                            break;
                        case "CallResultCode:Integer":
                        case "CallResultCode":
                            // we have agent selected and equals, build agent list.
                            ddlPickByName.Items.Clear();
                            BindResultNames(ddlPickByName);
                            txtEnterValue.Visible = false;
                            ddlPickByName.Visible = true;
                            break;
                        default:
                            txtEnterValue.Visible = true;
                            ddlPickByName.Visible = false;
                            break;
                    }
                }
                else
                {

                    // 2012-06-12 Dave Pollastrini
                    // Don't show value text box for boolean data type.
                    // txtEnterValue.Visible = true;

                    string[] fieldInfo = ddlCriteria.SelectedValue.Split(':');
                    string dataType =
                        fieldInfo[0].Equals("NeverCallFlag", StringComparison.InvariantCultureIgnoreCase)
                        ? "boolean" : fieldInfo[1];

                    txtEnterValue.Visible = !dataType.Equals("boolean");

                    ddlPickByName.Visible = false;
                }
            }
            catch { }
        }

        private void BindAgentNames(DropDownList ddl)
        {
            try
            {
                DataSet dsAgentList;
                AgentService objAgentService = new AgentService();
                dsAgentList = objAgentService.GetAgentList();
                ddl.Items.Add(new ListItem("Select an Agent", "0"));
                foreach (DataRow row in dsAgentList.Tables[0].Rows)
                {
                    ddl.Items.Add(new ListItem(row["AgentName"].ToString(),
                        row["AgentID"].ToString()));
                }
            }
            catch { }
        }

        private void BindResultNames(DropDownList ddl)
        {
            DataSet dsResultCodes;
            try
            {
                CampaignService objCampService = new CampaignService();
                Campaign objCampaign = (Campaign)Session["Campaign"];
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsResultCodes = objCampService.GetResultCodes(xDocCampaign);
                ddl.Items.Add(new ListItem("Select a Call Result", "0"));
                foreach (DataRow row in dsResultCodes.Tables[0].Rows)
                {
                    ddl.Items.Add(new ListItem(row["Description"].ToString(),
                        row["ResultCodeID"].ToString()));
                }
            }
            catch { }
        }

        private void BindQueryList()
        {
            DataSet dsQuerList;
            DataTable dtQueryList = new DataTable();

            try
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsQuerList = objCampService.GetQueryList(xDocCampaign);
                dtQueryList = dsQuerList.Tables[0];

                foreach (DataRow row in dtQueryList.Rows)
                {
                    ListItem ltm = new ListItem(row["QueryName"].ToString(), row["QueryID"].ToString());
                    string strSubQueryConditions = row["QueryCondition"].ToString();
                    string strFilteredSubQueryConditions = "";

                    if (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                    if (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));

                    strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                    ltm.Attributes.Add("title", strFilteredSubQueryConditions);
                    ddlQueryList.Items.Add(ltm);
                }
                //lbxQueries.Attributes.Add("onmouseover", "this.title=this.options[this.SelectedIndex].title");
            }
            catch (Exception ex)
            {
                PageMessage = "Exception building query list: " + ex.Message;
            }
        }

        private void BindConditionControls()
        {
            BindColumnsDropdown(ddlCriteria, new ListItem("Select a Field", "0"));

            BindAdditionalFields(ddlCriteria);


            // 2012-06-12 Dave Pollastrini
            // Changed BindOperator to take a datatype.
            /*
            bool isDateField = false;
            try
            {
                isDateField = ddlCriteria.SelectedValue.IndexOf(":date") > 0;
            }
            catch { }

            BindOperator(ddlOperator, isDateField);
            */

            // string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();
            string dataType = string.Empty;
            string[] fieldInfo = ddlCriteria.SelectedValue.Split(':');
            if (fieldInfo.Length > 1)
            {
                dataType =
                    fieldInfo[0].Equals("NeverCallFlag", StringComparison.InvariantCultureIgnoreCase)
                    ? "boolean" : fieldInfo[1];
            }

            BindOperator(ddlOperator, dataType);
        }
        #endregion

        #region Debug Tools

        private void WriteQueryTreeToLog(QueryTree objQueryTree)
        {
            Campaign objCampaign = null;
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
            }
            try
            {
                ActivityLogger.WriteAdminEntry(objCampaign, "-------------- Query Tree Dump -----------------", strQueryCondition);
                for (int i = 0; i < objQueryTree.Count; i++)
                {
                    QueryTreeItem qti = objQueryTree[i];
                    ActivityLogger.WriteAdminEntry(objCampaign, "--- Tree Node ID {0} '{1}' at Index {2} ---", qti.TreeNodeID, qti.NodeLabel, i);
                    ActivityLogger.WriteAdminEntry(objCampaign, "NodeType = {0}", qti.NodeType);
                    ActivityLogger.WriteAdminEntry(objCampaign, "LogicalOrder = {0}", qti.LogicalOrder);
                    ActivityLogger.WriteAdminEntry(objCampaign, "ParentTreeNodeID = {0}", qti.ParentTreeNodeID);
                    ActivityLogger.WriteAdminEntry(objCampaign, "ParentValuePath = {0}", qti.ParentValuePath);
                    ActivityLogger.WriteAdminEntry(objCampaign, "LogicalOperator = {0}", qti.LogicalOperator);
                    ActivityLogger.WriteAdminEntry(objCampaign, "ApplyLogicalOperator = {0}", qti.ApplyLogicalOperator);
                    ActivityLogger.WriteAdminEntry(objCampaign, "OpenParenPrefixCount = {0}", qti.OpenParenPrefixCount);
                    ActivityLogger.WriteAdminEntry(objCampaign, "CloseParenSuffixCount = {0}", qti.CloseParenSuffixCount);
                    ActivityLogger.WriteAdminEntry(objCampaign, "SubsetID = {0}", qti.SubsetID);
                    ActivityLogger.WriteAdminEntry(objCampaign, "SubsetLevel = {0}", qti.SubsetLevel);
                }
            }
            catch { }
        }
        #endregion

    }
}
