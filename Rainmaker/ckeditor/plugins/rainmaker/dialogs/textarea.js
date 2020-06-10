CKEDITOR.dialog.add( 'rmTextAreaDialog', function (editor)
{
	return {
		title : 'RainMaker Text Area',
		minWidth : 250,
		minHeight : 100,
		onShow : function()
		{
			delete this.textarea;

			var element = this.getParentEditor().getSelection().getSelectedElement();
			if ( element && element.getName() == "textarea" )
			{
				this.textarea = element;
				this.setupContent( element );
			}
            else 
            {
                this.getContentElement("rmFieldDefinition", "rmFieldName").setValue('');
            }
		},
		onOk : function()
		{
			var editor,
				element = this.textarea,
				isInsertMode = !element;

			if ( isInsertMode )
			{
				editor = this.getParentEditor();
				element = editor.document.createElement('textarea');
			}
			this.commitContent(element);

			if (isInsertMode)
				editor.insertElement(element);
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
			            id : 'rmFieldName',
                        type: 'select',
                        label: 'RainMaker Fields',
                        items :
                        [
                            ['Select RainMaker Field', '']
                        ],
                        validate: CKEDITOR.dialog.validate.notEmpty('Please select a RainMaker field.'),
                        onLoad: function()
                        {
                            var select = this;
                            $.each(arrRMFields, function(index, item) {
                                if (arrRMFields_size[index] > 0) select.add(item);
                            });
                        },
			            setup : function( element )
			            {
                            this.setValue(
                                element.data('rmFieldNameSaved') ||
                                ''
                            );
			            },
			            commit : function (element)
			            {
				            var value = this.getValue();
				            if (value) 
				            {
					            element.data('rmFieldNameSaved', value);
					            element.setAttribute('name', value.toLowerCase());
					            element.setAttribute('_o_n_b_lur', 'TruncateData(this, 5000)');
					            element.$.value = element.$.defaultValue = '##' + value + '##';
					        }
				            else
				            {
					            element.data('rmFieldNameSaved', false);
					            element.removeAttribute('name');
					            element.removeAttribute('_o_n_b_lur');
					            element.$.value = element.$.defaultValue = '';
				            }
			            }
		            }
				]
			}
		]
	};
});
