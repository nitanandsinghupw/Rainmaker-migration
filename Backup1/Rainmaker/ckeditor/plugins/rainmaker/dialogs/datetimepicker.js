CKEDITOR.dialog.add('rmDateTimeFieldDialog', function(editor) {
    return {
        title: 'RainMaker DateTime Picker Field',
        minWidth: 250,
        minHeight: 100,
        onShow: function() {
            delete this.textField;

            var element = this.getParentEditor().getSelection().getSelectedElement();
            if (element && element.getName() == 'input' &&
                (element.getAttribute('data-type') == 'dtpicker' || !element.getAttribute('type'))) {
                this.textField = element;
                this.setupContent(element);
            }
            else {
                this.getContentElement("rmFieldDefinition", "rmFieldName").setValue('');
            }
        },
        onOk: function() {
            var editor,
		        element = this.textField,
		        isInsertMode = !element;

            if (isInsertMode) {
                editor = this.getParentEditor();
                element = editor.document.createElement("input");
                //element.type = "dtpicker";
                //this.getContentElement("type", "dtpicker");
                //element = editor.document.appendChild(input);
                //if (element.setAttribute) {
                //    element.setAttribute('type', 'dtpicker');
                //}
                //$(element).attr('type', 'dtpicker');
            }

            if (isInsertMode)
                editor.insertElement(element);
            this.commitContent({ element: element });
        },
        contents:
        [
	        {
	            id: 'rmFieldDefinition',
	            label: 'RainMaker Field Definition',
	            title: 'RainMaker Field Definition',
	            elements:
		        [
		            {
		                id: 'rmFieldName',
		                type: 'select',
		                label: 'RainMaker Fields',
		                items:
                        [
                            ['Select RainMaker Field', '']
                        ],
		                validate: CKEDITOR.dialog.validate.notEmpty('Please select a RainMaker field.'),
		                onLoad: function() {
		                    var select = this;
		                    $.each(arrRMFields_datetime, function(index, item) {
		                        select.add(item);
		                    });
		                },
		                setup: function(element) {

		                    this.setValue(
                                element.data('rmFieldNameSaved') ||
                                ''
                            );
		                },
		                commit: function(data) {
		                    var element = data.element;
		                    var value = this.getValue();

		                    if (value) {

		                        element.data('type', 'dtpicker');
		                        element.data('rmFieldNameSaved', value);
		                        element.setAttribute('name', value.toLowerCase());
		                        element.setAttribute('data-cke-saved-name', value.toLowerCase());
		                        element.setAttribute('value', '#' + value + '#');
		                        element.setAttribute('maxLength', '17');

		                    }
		                    else {
		                        element.data('rmFieldNameSaved', false);
		                        element.removeAttribute('name');
		                        element.removeAttribute('value');
		                        element.removeAttribute('maxLength');
		                    }
		                }
		            }
		        ]
	        }
        ]
    };
});
