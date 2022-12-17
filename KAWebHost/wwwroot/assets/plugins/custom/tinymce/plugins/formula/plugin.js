(function (tinymce) {
	tinymce.create('tinymce.plugins.Formula', {
		init: function (editor, url) {
			var options = editor.getParam('formula') || {};
			var path = options.path || url;
			console.log(path, editor);
			editor.ui.registry.addButton('formula', {
				icon: path + '/img/formula.png',
				tooltip: 'Insert Formula',
				onAction: showFormulaDialog.bind(this, editor, path),
				onSetup: function (buttonApi) {
					const editorEventCallback = function (eventApi) {
						buttonApi.setDisabled(eventApi.element.className.indexOf('fm-editor-equation') > -1 && eventApi.element.nodeName.toLowerCase() == "img");
					};
					editor.on('NodeChange', editorEventCallback);
					return function (buttonApi) {
						editor.off('NodeChange', editorEventCallback);
					}
				}
			});			
		}
	});
	tinymce.PluginManager.requireLangPack('formula', 'en,es,fr_FR');
	tinymce.PluginManager.add('formula', tinymce.plugins.Formula);


	function showFormulaDialog(editor, url) {
		editor.windowManager.openUrl({
			title: "Formula",
			url: buildUrl(editor, url),
			buttons: [
				{
					type: 'cancel',
					name: 'closeButton',
					text: 'Cancel'
				},
				{
					type: 'custom',
					name: 'submitButton',
					text: 'Insert Formula'
				}				
			],
			onCancel: function (dialogApi) {
				
			},
			onAction: function (dialogApi) {
				var frameCount = window.frames.length;
				var frame;
				for (var i = 0; i < frameCount; i++) {
					if (window.frames[i].location.pathname.indexOf("formula/index.html")>0) {
						frame = window.frames[i];						
					}
				}
				if (frame && frame.getData) {
					frame.getData(function (src, mlang, equation) {
						if (src) {
							editor.insertContent('<img class="fm-editor-equation" src="' + src + '" data-mlang="' + mlang + '" data-equation="' + encodeURIComponent(equation) + '"/>');
						}
						dialogApi.close();
					});
				} else {
					dialogApi.close();
				}
			}
		});
	}

	function buildIFrame(editor, url) {
		var currentNode = editor.selection.getNode();
		var lang = editor.getParam('language') || 'en';
		var mlangParam = '';
		var equationParam = '';
		if (currentNode.nodeName.toLowerCase() == 'img' && currentNode.className.indexOf('fm-editor-equation') > -1) {
			if (currentNode.getAttribute('data-mlang')) mlangParam = "&mlang=" + currentNode.getAttribute('data-mlang');
			if (currentNode.getAttribute('data-equation')) equationParam = '&equation=' + currentNode.getAttribute('data-equation');
		}
		var html = '<iframe name="tinymceFormula" id="tinymceFormula" src="' + url + '/index.html' + '?lang=' + lang + mlangParam + equationParam + '" scrolling="no" frameborder="0"></iframe>';
		return html;
	}

	function buildUrl(editor, url) {
		var currentNode = editor.selection.getNode();
		var lang = editor.getParam('language') || 'en';
		var mlangParam = '';
		var equationParam = '';
		if (currentNode.nodeName.toLowerCase() == 'img' && currentNode.className.indexOf('fm-editor-equation') > -1) {
			if (currentNode.getAttribute('data-mlang')) mlangParam = "&mlang=" + currentNode.getAttribute('data-mlang');
			if (currentNode.getAttribute('data-equation')) equationParam = '&equation=' + currentNode.getAttribute('data-equation');
		}
		var returnUrl = url + '/index.html' + '?lang=' + lang + mlangParam + equationParam;
		return returnUrl;
	}
})(window.tinymce);
