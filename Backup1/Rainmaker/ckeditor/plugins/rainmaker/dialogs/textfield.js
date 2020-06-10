CKEDITOR.dialog.add('rmTextFieldDialog', function(editor) {
    return {
        title: 'RainMaker Text Field',
        minWidth: 250,
        minHeight: 100,
        onShow: function() {
            delete this.textField;

            var element = this.getParentEditor().getSelection().getSelectedElement();
            if (element && element.getName() == 'input' &&
                (element.getAttribute('type') == 'text' || !element.getAttribute('type'))) {
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
                element = editor.document.createElement('input');
                element.setAttribute('type', 'text');
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
			                $.each(arrRMFields, function(index, item) {
			                    if (arrRMFields_size[index] > 0) select.add(item);
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
			                    
			                    element.data('rmFieldNameSaved', value);
			                    element.setAttribute('name', value.toLowerCase());
			                    element.setAttribute('data-cke-saved-name', value.toLowerCase());
			                    element.setAttribute('value', '#' + value + '#');
			                    element.setAttribute('maxLength', arrRMFields_size[arrRMFields.indexOf(value)]);

			                }
			                else {
			                    element.data('rmFieldNameSaved', false);
			                    element.removeAttribute('name');
			                    element.removeAttribute('value');
			                    element.removeAttribute('maxLength');
			                }
			            }
			        },
		            {
		                type: 'checkbox',
		                id: 'readonly',
		                label: 'Read Only',
		                'default': false,
		                setup: function(element) {
		                    this.setValue(
                                element.getAttribute('readonly')
                            );
		                },
		                commit: function(data) {
		                    var element = data.element;
		                    if (this.getValue())
		                        element.setAttribute('readonly', 'readonly');
		                    else
		                        element.removeAttribute('readonly');
		                }
		            }
		        ]
	        }
        ]
    };
});
