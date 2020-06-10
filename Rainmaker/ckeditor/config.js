﻿/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	
	config.toolbar = 'RainMakerToolbar';
    config.toolbar_RainMakerToolbar =
    [
	    { name: 'document',		items : [ 'Source','-','Save','NewPage','DocProps','Preview','Print','-' ] },
	    { name: 'clipboard',	items : [ 'Cut','Copy','Paste','PasteText','PasteFromWord','-','Undo','Redo' ] },
	    { name: 'editing',		items : [ 'Find','Replace','-','SelectAll','-','SpellChecker' ] },
	    // { name: 'forms',		items : [ 'Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField' ] },
	    '/',
	    { name: 'basicstyles',	items : [ 'Bold','Italic','Underline','Strike','-','RemoveFormat' ] },
	    { name: 'paragraph',	items : [ 'NumberedList','BulletedList','-','Outdent','Indent','-','-','JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock','-','BidiLtr','BidiRtl' ] },
	   // { name: 'links',		items : [ 'Link','Unlink','Anchor' ] },
	    { name: 'insert',		items : [ 'Table','HorizontalRule','Smiley' ] },
	    '/',
	    { name: 'styles',		items : [ 'Styles','Format','Font','FontSize' ] },
	    { name: 'colors',		items : [ 'TextColor','BGColor' ] },

	    { name: 'rainmaker', items: ['RainMakerTextField', 'RainMakerTextArea', 'RainMakerSelect', 'RainMakerButton', 'RainMakerScriptAnchor', 'RainMakerDateTimeField', 'RainMakerCheckbox'] }
    ];
    
    config.extraPlugins = 'rainmaker';
};
