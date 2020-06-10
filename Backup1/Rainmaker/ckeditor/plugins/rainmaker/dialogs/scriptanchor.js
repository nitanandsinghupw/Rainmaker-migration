CKEDITOR.dialog.add('rmScriptAnchorDialog', function(editor) {
    return {
        title: 'RainMaker Script Anchor',
        minWidth: 350,
        minHeight: 150,
        onShow: function() {
            delete this.anchor;
            var element = this.getParentEditor().getSelection().getStartElement();
            if (element && element.is('a')) {
                this.anchor = element;
                this.setupContent(element);
            }
        },
        onOk: function() {
            var editor = this.getParentEditor(),
				element = this.anchor,
				isInsertMode = !element;

            var fake = element ? CKEDITOR.htmlParser.fragment.fromHtml(element.getOuterHtml()).children[0]
					: new CKEDITOR.htmlParser.element('a');
            this.commitContent(fake);

            var writer = new CKEDITOR.htmlParser.basicWriter();
            fake.writeHtml(writer);
            var newElement = CKEDITOR.dom.element.createFromHtml(writer.getHtml(), editor.document);

            if (isInsertMode)
                editor.insertElement(newElement);
            else {
                newElement.replace(element);
                editor.getSelection().selectElement(newElement);
            }
        },
        contents: [
			{
			    id: 'rmScriptPage',
			    label: 'RainMaker Script Page',
			    title: 'RainMaker Script Page',
			    elements:
				[
			        {
			            id: 'rmScriptPageName',
			            type: 'select',
			            label: 'RainMaker Script Pages',
			            items:
                        [
                            ['Select RainMaker Script Page', '']
                        ],
			            validate: CKEDITOR.dialog.validate.notEmpty('Please select a RainMaker script page.'),
			            onLoad: function() {
			                var select = this;
			                $.each(arrScripts, function(index, item) {
			                    select.add(item[1], item[0]);
			                });
			            },
			            setup: function(element) {
			                this.setValue(
                                element.getAttribute('value') ||
                                ''
                            );
			            },
			            commit: function(element) {
			                var val = this.getValue();
			                var selectElement = this.getInputElement().$;
			                var selectedOptionText = selectElement.options[selectElement.selectedIndex].text;

			                if (val) {
			                    element.attributes['value'] = val;
			                    element.attributes['href'] = "javascript:LoadScript('" + val + "');";
			                    element.children.length = 0;
			                    element.add(new CKEDITOR.htmlParser.text(selectedOptionText))
			                }
			                else {
			                    delete element.attributes['href'];
			                }
			            }
			        }
				]
			}
		]
    };
});