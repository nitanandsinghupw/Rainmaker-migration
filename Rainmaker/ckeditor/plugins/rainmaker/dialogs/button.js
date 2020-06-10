CKEDITOR.dialog.add('rmButtonDialog', function( editor )
{
	return {
		title : 'RainMaker Button',
		minWidth : 350,
		minHeight : 150,
		onShow : function()
		{
			delete this.button;
			var element = this.getParentEditor().getSelection().getSelectedElement();
			if ( element && element.is('input') )
			{
				var type = element.getAttribute('type');
				if ( type in { button:1, reset:1, submit:1 } )
				{
					this.button = element;
					this.setupContent(element);
				}
			}
		},
		onOk : function()
		{
			var editor = this.getParentEditor(),
				element = this.button,
				isInsertMode = !element;

			var fake = element ? CKEDITOR.htmlParser.fragment.fromHtml( element.getOuterHtml() ).children[ 0 ]
					: new CKEDITOR.htmlParser.element( 'input' );
			this.commitContent(fake);

			var writer = new CKEDITOR.htmlParser.basicWriter();
			fake.writeHtml( writer );
			var newElement = CKEDITOR.dom.element.createFromHtml( writer.getHtml(), editor.document );

			if ( isInsertMode )
				editor.insertElement( newElement );
			else
			{
				newElement.replace( element );
				editor.getSelection().selectElement( newElement );
			}
		},
		contents : [
			{
		        id : 'rmFieldDefinition',
		        label : 'RainMaker Field Definition',
		        title : 'RainMaker Field Definition',
				elements : 
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
                            $.each(arrResultcodes, function(index, item) {
                                select.add(item, item);
                            });
                        },
			            setup : function(element)
			            {
                            this.setValue(
                                element.getAttribute('name') ||
                                ''
                            );
			            },
			            commit : function(element)
			            {
		                    var val = this.getValue();
		                    if (val)
		                    {
                                element.attributes['type'] = 'button';
                                element.attributes['name'] = val;
                                element.attributes['value'] = val;
                                element.attributes['h_r_ef'] = 'DisposeCall(&quot;' + val + '&quot;);';
		                    }
		                    else
		                    {
                                delete element.attributes['type'];
                                delete element.attributes['name'];
                                delete element.attributes['value'];
                                delete element.attributes['h_r_ef'];
		                    }
			            }
		            }
				]
			}
		]
	};
});
