CKEDITOR.dialog.add('rmCheckboxDialog', function( editor )
{
    return {
        title : 'RainMaker Checkbox',
        minWidth : 250,
        minHeight : 100,
        onShow : function()
        {
	        delete this.checkbox;

	        var element = this.getParentEditor().getSelection().getSelectedElement();

	        if ( element && element.getAttribute('type') == 'checkbox' )
	        {
		        this.checkbox = element;
		        this.setupContent(element);
	        }
            else 
            {
                this.getContentElement("rmFieldDefinition", "rmFieldName").setValue('');
            }
        },
        onOk : function()
        {
	        var editor,
		        element = this.checkbox,
		        isInsertMode = !element;

	        if ( isInsertMode )
	        {
		        editor = this.getParentEditor();
		        element = editor.document.createElement('input');
		        element.setAttribute('type', 'checkbox');
		        editor.insertElement(element);
	        }
	        this.commitContent( { element : element } );
        },
        contents: 
        [
	        {
		        id: 'rmFieldDefinition',
		        label: 'RainMaker Checkbox Definition',
		        title: 'RainMaker Checkbox Definition',
		        startupFocus : 'rmFieldName',
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
			            onLoad: function() {
			                var select = this;
			                $.each(arrRMFields_b, function(index, item) {
			                    select.add(item);
			                });
			            },
			            setup: function (element)
			            {
                            this.setValue(
                                element.data('rmFieldNameSaved') ||
                                ''
                            );
			            },
			            commit: function(data)
			            {
				            var element = data.element;
				            var value = this.getValue();

				            if (value) 
				            {
					            element.data('rmFieldNameSaved', value);
					            element.setAttribute('name', value);
					            element.setAttribute('value', '#' + value + '#');
					        }
				            else
				            {
					            element.data('rmFieldNameSaved', false);
					            element.removeAttribute('name');
					            element.removeAttribute('value');
				            }
			            }
		            }
		            /*
		            ,
			        {
				        id : 'txtValue',
				        type : 'text',
				        label : editor.lang.checkboxAndRadio.value,
				        'default' : '',
				        accessKey : 'V',
				        setup : function( element )
				        {
					        var value = element.getAttribute( 'value' );
					        // IE Return 'on' as default attr value.
					        this.setValue(  CKEDITOR.env.ie && value == 'on' ? '' : value  );
				        },
				        commit : function( data )
				        {
					        var element = data.element,
						        value = this.getValue();

					        if ( value && !( CKEDITOR.env.ie && value == 'on' ) )
						        element.setAttribute( 'value', value );
					        else
					        {
						        if ( CKEDITOR.env.ie )
						        {
							        // Remove attribute 'value' of checkbox (#4721).
							        var checkbox = new CKEDITOR.dom.element( 'input', element.getDocument() );
							        element.copyAttributes( checkbox, { value: 1 } );
							        checkbox.replace( element );
							        editor.getSelection().selectElement( checkbox );
							        data.element = checkbox;
						        }
						        else
							        element.removeAttribute( 'value' );
					        }
				        }
			        },
			        {
				        id : 'cmbSelected',
				        type : 'checkbox',
				        label : editor.lang.checkboxAndRadio.selected,
				        'default' : '',
				        accessKey : 'S',
				        value : "checked",
				        setup : function( element )
				        {
					        this.setValue( element.getAttribute( 'checked' ) );
				        },
				        commit : function( data )
				        {
					        var element = data.element;

					        if ( CKEDITOR.env.ie )
					        {
						        var isElementChecked = !!element.getAttribute( 'checked' ),
							        isChecked = !!this.getValue();

						        if ( isElementChecked != isChecked )
						        {
							        var replace = CKEDITOR.dom.element.createFromHtml( '<input type="checkbox"'
								           + ( isChecked ? ' checked="checked"' : '' )
								           + '/>', editor.document );

							        element.copyAttributes( replace, { type : 1, checked : 1 } );
							        replace.replace( element );
							        editor.getSelection().selectElement( replace );
							        data.element = replace;
						        }
					        }
					        else
					        {
						        var value = this.getValue();
						        if ( value )
							        element.setAttribute( 'checked', 'checked' );
						        else
							        element.removeAttribute( 'checked' );
					        }
				        }
			        }
			        */
		        ]
	        }
        ]
    };
});
