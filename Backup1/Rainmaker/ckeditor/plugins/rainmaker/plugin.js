CKEDITOR.plugins.add('rainmaker',
{
    init: function(editor) {
        var dialogPath = this.path + 'dialogs/';

        editor.addCommand('rmCheckboxDialog', new CKEDITOR.dialogCommand('rmCheckboxDialog'));
        editor.ui.addButton('RainMakerCheckbox',
        {
            label: 'RainMaker Checkbox',
            command: 'rmCheckboxDialog',
            icon: CKEDITOR.getUrl('skins/kama/icons.png'),
            iconOffset: 48
        });
        CKEDITOR.dialog.add('rmCheckboxDialog', dialogPath + 'checkbox.js');

        editor.addCommand('rmTextFieldDialog', new CKEDITOR.dialogCommand('rmTextFieldDialog'));
        editor.ui.addButton('RainMakerTextField',
        {
            label: 'RainMaker Text Field',
            command: 'rmTextFieldDialog',
            icon: CKEDITOR.getUrl('skins/kama/icons.png'),
            iconOffset: 50
        });
        CKEDITOR.dialog.add('rmTextFieldDialog', dialogPath + 'textfield.js');

        editor.addCommand('rmTextAreaDialog', new CKEDITOR.dialogCommand('rmTextAreaDialog'));
        editor.ui.addButton('RainMakerTextArea',
        {
            label: 'RainMaker Text Area',
            command: 'rmTextAreaDialog',
            icon: CKEDITOR.getUrl('skins/kama/icons.png'),
            iconOffset: 51
        });
        CKEDITOR.dialog.add('rmTextAreaDialog', dialogPath + 'textarea.js');

        editor.addCommand('rmSelectDialog', new CKEDITOR.dialogCommand('rmSelectDialog'));
        editor.ui.addButton('RainMakerSelect',
        {
            label: 'RainMaker Select',
            command: 'rmSelectDialog',
            icon: CKEDITOR.getUrl('skins/kama/icons.png'),
            iconOffset: 52
        });
        CKEDITOR.dialog.add('rmSelectDialog', dialogPath + 'select.js');

        editor.addCommand('rmButtonDialog', new CKEDITOR.dialogCommand('rmButtonDialog'));
        editor.ui.addButton('RainMakerButton',
        {
            label: 'RainMaker Button',
            command: 'rmButtonDialog',
            icon: CKEDITOR.getUrl('skins/kama/icons.png'),
            iconOffset: 53
        });
        CKEDITOR.dialog.add('rmButtonDialog', dialogPath + 'button.js');

        editor.addCommand('rmScriptAnchorDialog', new CKEDITOR.dialogCommand('rmScriptAnchorDialog'));
        editor.ui.addButton('RainMakerScriptAnchor',
        {
            label: 'RainMaker Script Anchor',
            command: 'rmScriptAnchorDialog',
            icon: CKEDITOR.getUrl('skins/kama/icons.png'),
            iconOffset: 34
        });
        CKEDITOR.dialog.add('rmScriptAnchorDialog', dialogPath + 'scriptanchor.js');

        editor.addCommand('rmDateTimeFieldDialog', new CKEDITOR.dialogCommand('rmDateTimeFieldDialog'));
        editor.ui.addButton('RainMakerDateTimeField',
        {
            label: 'RainMaker DateTime Picker Field',
            command: 'rmDateTimeFieldDialog',
            icon: CKEDITOR.getUrl('skins/kama/icons.png'),
            iconOffset: 60
        });
        CKEDITOR.dialog.add('rmDateTimeFieldDialog', dialogPath + 'datetimepicker.js');
        

        if (editor.addMenuItems) {
            editor.removeMenuItem('checkbox');
            editor.removeMenuItem('textfield');
            editor.removeMenuItem('textarea');
            editor.removeMenuItem('select');
            editor.removeMenuItem('button');

            editor.addMenuGroup('rmGroup');
            editor.addMenuItems
            (
                {
                    rmCheckboxItem:
                    {
                        label: 'RainMaker Checkbox',
                        icon: CKEDITOR.getUrl('skins/kama/icons.png'),
                        iconOffset: 48,
                        command: 'rmCheckboxDialog',
                        group: 'rmGroup'
                    },
                    rmTextFieldItem:
                    {
                        label: 'RainMaker Text Field',
                        icon: CKEDITOR.getUrl('skins/kama/icons.png'),
                        iconOffset: 50,
                        command: 'rmTextFieldDialog',
                        group: 'rmGroup'
                    },
                    rmTextAreaItem:
                    {
                        label: 'RainMaker Text Area',
                        icon: CKEDITOR.getUrl('skins/kama/icons.png'),
                        iconOffset: 51,
                        command: 'rmTextAreaDialog',
                        group: 'rmGroup'
                    },
                    rmSelectItem:
                    {
                        label: 'RainMaker Select',
                        icon: CKEDITOR.getUrl('skins/kama/icons.png'),
                        iconOffset: 52,
                        command: 'rmSelectDialog',
                        group: 'rmGroup'
                    },
                    rmButtonItem:
                    {
                        label: 'RainMaker Button',
                        icon: CKEDITOR.getUrl('skins/kama/icons.png'),
                        iconOffset: 53,
                        command: 'rmButtonDialog',
                        group: 'rmGroup'
                    },
                    rmScriptAnchorItem:
                    {
                        label: 'RainMaker Script Anchor',
                        icon: CKEDITOR.getUrl('skins/kama/icons.png'),
                        iconOffset: 34,
                        command: 'rmScriptAnchorDialog',
                        group: 'rmGroup'
                    },
                    rmDateTimeFieldItem:
                    {
                        label: 'RainMaker DateTime Picker Field',
                        icon: CKEDITOR.getUrl('skins/kama/icons.png'),
                        iconOffset: 60,
                        command: 'rmDateTimeFieldDialog',
                        group: 'rmGroup'
                    }
                }
            );
        }

        if (editor.contextMenu) {
            editor.contextMenu.addListener(function(element) {
                if (element && !element.isReadOnly()) {
                    var name = element.getName();

                    if (name == 'select')
                        return { rmSelectItem: CKEDITOR.TRISTATE_OFF };

                    if (name == 'textarea')
                        return { rmTextAreaItem: CKEDITOR.TRISTATE_OFF };

                    if (name == 'input') {
                        if (element.getAttribute('data-type') == 'dtpicker') {
                            return { rmDateTimeFieldItem: CKEDITOR.TRISTATE_OFF };
                        }
                        switch (element.getAttribute('type')) {
                            case 'button':
                                return { rmButtonItem: CKEDITOR.TRISTATE_OFF };
                            case 'checkbox':
                                return { rmCheckboxItem: CKEDITOR.TRISTATE_OFF };

                            default:
                                return { rmTextFieldItem: CKEDITOR.TRISTATE_OFF };
                        }
                    }

                    if (name == 'a') {
                        return { rmScriptAnchorItem: CKEDITOR.TRISTATE_OFF };
                    }
                }
            });
        }
        
        editor.on('doubleclick', function(evt) {
            var element = evt.data.element;

            if (element.is('select')) evt.data.dialog = 'rmSelectDialog';
            else if (element.is('textarea')) evt.data.dialog = 'rmTextAreaDialog';
            else if (element.is('input')) {
            
                switch (element.getAttribute('type')) {
                    case 'button':
                        evt.data.dialog = 'rmButtonDialog';
                        break;
                    case 'checkbox':
                        evt.data.dialog = 'rmCheckboxDialog';
                        break;
                    default:
                        evt.data.dialog = 'rmTextFieldDialog';
                        break;
                }
                if (element.getAttribute('data-type') == 'dtpicker') {

                    evt.data.dialog = 'rmDateTimeFieldDialog';

                }
            }
            else if (element.is('a')) evt.data.dialog = 'rmScriptAnchorDialog';
        });
    }
});
