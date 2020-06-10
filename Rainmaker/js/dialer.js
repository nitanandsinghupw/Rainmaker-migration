$(function() {

    var $validationDialog = $("#validationDialog");
    $validationDialog.dialog({
        autoOpen: false,
        closeOnEscape: true,
        height: 400,
        width: 400,
        modal: true,
        resizable: false,
        title: "Validation Error",
        buttons:
		[
			{
			    text: "OK",
			    click: function() { $(this).dialog("close"); }
			}
		]
    });

    var $dispositionDialog = $("#dispositionDialog");
    $dispositionDialog.dialog({
        autoOpen: false,
        closeOnEscape: true,
        height: 400,
        width: 400,
        modal: true,
        resizable: false,
        title: "Call Disposition",
        buttons:
		[
			{
			    text: "OK",
			    click: function() { 
					var disposition = $("#dispositions").val();
					
					if (disposition === null)
					{
						alert("Please select a disposition.");
					}
					else 
					{
						DisposeCall(disposition, false);
					}
			    }
			},
			{
			    text: "Cancel",
			    click: function() { $(this).dialog("close"); }
			}
		]
    });

    $("#btnDispose").click(function() {
        $dispositionDialog.dialog("open");
    });

});

function CampaignCall(campaignForm, campaignData)
{
	var thisCampaignCall = this;

	this.campaignForm = campaignForm;
	this.campaignData = campaignData;

	this.disposition = null;
}

CampaignCall.prototype.saveAndDispose = function(callback)
{
	var thisCampaignCall = this;

	if(thisCampaignCall.campaignForm.isValid())
	{
		thisCampaignCall.campaignData.setModifiedFields(thisCampaignCall.campaignForm.getModifiedFieldsInfo());

		thisCampaignCall.campaignData.save(function(data)
		{
			thisCampaignCall.dispose(callback);
		});
	}
	else
	{
		if(callback) callback({ error: "FormValidationException" });
	}
}

CampaignCall.prototype.save = function(callback)
{
	var thisCampaignCall = this;

	if(thisCampaignCall.campaignForm.isValid())
	{
		thisCampaignCall.campaignData.setModifiedFields(thisCampaignCall.campaignForm.getModifiedFieldsInfo());

		thisCampaignCall.campaignData.save(function(data) {
			if(callback) callback(data);
		});
	}
	else
	{
		if(callback) callback({ error: "FormValidationException" });
	}
}

CampaignCall.prototype.dispose = function(callback)
{
	var thisCampaignCall = this;

	if(thisCampaignCall.disposition === null) throw "Invalid Disposition Code.";

	var data = { resultcodename: thisCampaignCall.disposition };

	data.pagefrom = thisCampaignCall.campaignForm.pageName;

	if(thisCampaignCall.campaignForm.isScript) data.FromScript = true;

	$.ajax
	({
		type: "GET",
		url: "CallDisposition.aspx",
		cache: false,
		data: data,
		success: function(data, textStatus, request)
		{
			if(callback) callback(data);
		}
	});
}


CampaignCall.prototype.setDisposition = function(disposition)
{
	this.disposition = disposition;
}

CampaignCall.prototype.showValidationError = function(context)
{
	var content = "";

	switch (context.error)
	{
		case "FormValidationException":
			var $ul = $("<ul class=\"invalidFields\" />");

			$.each(this.campaignForm.getInvalidFieldsInfo(), function(i, fieldInfo) {
				$ul.append($("<li>" + fieldInfo.name + "</li>"));
			});

			content = $ul;
			break;
		default:
			content = (context.error ? context.error : "<p>Unspecified error occurred.  Call was NOT save.</p>");
			break;
	}

	this.campaignForm.showValidationError(content);
}

function CampaignData(campaignId)
{
	this.campaignId = campaignId;

	this.modifiedFields = [];
}

CampaignData.prototype.setModifiedFields = function(modifiedFieldsInfo)
{
	this.modifiedFields = modifiedFieldsInfo;

	if(this.modifiedFields.length>0)
		this.modifiedFields.push({ name: "uniquekey", value: this.campaignId });
}

CampaignData.prototype.save = function(callback) {
    var thisCampaignData = this;

    if (thisCampaignData.modifiedFields.length > 0) {
        $.ajax
	    ({
	        type: "POST",
	        url: "PostCampaignDetails.aspx",
	        data:
		    {
		        fields: JSON.stringify(thisCampaignData.modifiedFields)
		    },
	        success: function(data, textStatus, request) {
	            if (callback) callback(data);
	        }
	    });
    }
    else {
        if (callback) callback({});
    }
}

function CampaignForm(context)
{
	this.fieldInfo = context.fieldInfo;
	this.isScript = context.isScript;
	this.pageName = context.pageName;
	this.$form = context.$form;
	this.$validationDialog = context.$validationDialog;

	this.formInfo = {};
	this.modifiedFields = [];
	this.invalidFields = [];

	this.collectFormInfo();
}

CampaignForm.FIELDINFO_NAME_COLUMN = 0;
CampaignForm.FIELDINFO_TYPE_COLUMN = 1;
CampaignForm.FIELDINFO_VALUE_COLUMN = 2;

CampaignForm.prototype.submit = function()
{
	this.$form.submit();
}

CampaignForm.prototype.collectFormInfo = function()
{
	var thisCampaignForm = this;

	$.each(thisCampaignForm.fieldInfo, function(fieldIndex, fieldInfo)
	{
		var fieldName = fieldInfo[CampaignForm.FIELDINFO_NAME_COLUMN];
		var $element = $("[data-rmfieldnamesaved][name = "+fieldName+"], [data-rmfieldnamesaved][data-name = "+fieldName+"]");

		if($element.length>0)
		{
			var fieldType = fieldInfo[CampaignForm.FIELDINFO_TYPE_COLUMN];
			var fieldValue = fieldInfo[CampaignForm.FIELDINFO_VALUE_COLUMN];
			var value = thisCampaignForm.getElementValue($element);

			var isModified = value !== fieldValue;
			var isValid = thisCampaignForm.isFieldValid(fieldType, value)

			if(isModified) thisCampaignForm.modifiedFields.push(fieldName);
			if(!isValid) thisCampaignForm.invalidFields.push(fieldName);

			thisCampaignForm.formInfo[fieldName] = {
				fieldType: fieldType,
				fieldValue: fieldValue,
				value: value,
				isModified: isModified,
				isValid: isValid
			};
		}
	});
}

CampaignForm.prototype.isModified = function()
{
	return this.modifiedFields.length>0;
}

CampaignForm.prototype.isValid = function()
{
	return this.invalidFields.length === 0;
}

CampaignForm.prototype.getModifiedFieldsInfo = function()
{
	var modifiedFieldsInfo = [];

	for(var i = 0; i < this.modifiedFields.length; i++)
	{
		var fieldName = this.modifiedFields[i];
		var fieldInfo = this.formInfo[fieldName];

		modifiedFieldsInfo.push({ name: fieldName, value: fieldInfo.value, type: fieldInfo.fieldType });
	}

	return modifiedFieldsInfo;
}


CampaignForm.prototype.getInvalidFieldsInfo = function()
{
	var invalidFieldsInfo = [];

	for(var i = 0; i < this.invalidFields.length; i++)
	{
		var fieldName = this.invalidFields[i];
		var fieldInfo = this.formInfo[fieldName];

		invalidFieldsInfo.push({ name: fieldName, value: fieldInfo.value, type: fieldInfo.fieldType });
	}

	return invalidFieldsInfo;
}

CampaignForm.prototype.getElementValue = function($element)
{
	var elementValue = null;

	switch($element.attr("type"))
	{
		case "checkbox":
			elementValue = $element.is(":checked");
			break
		default:
			elementValue = $element.val().trim();
			break;
	}

	return elementValue;
}

CampaignForm.prototype.setValue = function(id, value)
{
	var $element = $("#"+id);

	if($element.length>0)
	{
		switch($element.attr("type"))
		{
			case "checkbox":
				$element.prop("checked", value === true);
				break;
			default:
				$element.val(value);
				break;
		}
	}
	else
	{
		throw "Element not found.  Value not set.";
	}
}

CampaignForm.prototype.isFieldValid = function(fieldType, value)
{
	var isValid = true;

	switch(fieldType)
	{
		case "dec":
			isValid = !isNaN(value);
			break;
		case "dt":
			isValid = true;
			break;
		case "i":
			isValid = /^\d+$/.test(value);
			break;
	}

	return isValid;
}

CampaignForm.prototype.showValidationError = function(content)
{
	this.$validationDialog.html(content ? content : "").dialog("open");
}
