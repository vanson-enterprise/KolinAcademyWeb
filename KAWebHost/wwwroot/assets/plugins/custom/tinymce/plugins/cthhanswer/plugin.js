tinymce.PluginManager.add('cthhanswer', function (editor, url) {
	var openDialog = function (initialData) {
		return editor.windowManager.open({
			title: 'CTH highlight answers',
			body: {
				type: 'panel',
				items: [
					{
						type: 'input',
						name: 'order',
						label: 'Order',
						inputMode: 'number'
					}
				]
			},
			buttons: [
				{
					type: 'cancel',
					text: 'Close'
				},
				{
					type: 'submit',
					text: 'Save',
					primary: true
				}
			],
			onSubmit: function (api) {
				var cContent = editor.selection.getContent();
				var data = api.getData();
				var order = data.order;
				var selectedNode = editor.selection.getNode();
				var tagName = $(selectedNode).prop("tagName");
				var strClass = $(selectedNode).prop("class");
				editor.undoManager.transact(function () {
					if (tagName && strClass && tagName === "SPAN" && strClass.indexOf('answerhighlight') >= 0) {
						if (order) {
							$(selectedNode).removeClass();
							$(selectedNode).addClass("answerhighlight q" + order);
						} else {
							$(selectedNode).replaceWith($(selectedNode).text());
						}
					} else {
						if (cContent && order) {
							editor.focus();
							var content = "<span class='answerhighlight q" + data.order + "'>" + editor.selection.getContent() + "</span>";
							editor.selection.setContent(content);
						}
					}
					api.close();
					editor.focus();
				});
			},
			initialData: initialData
		});
	};

	// Add a button that opens a window
	editor.ui.registry.addButton('cthhanswer', {
		text: 'Highlight answers',
		onAction: function () {
			// Open window
			var selectedNode = editor.selection.getNode();
			var tagName = $(selectedNode).prop("tagName");
			var strClass = $(selectedNode).prop("class");
			if (tagName && strClass && tagName === "SPAN" && strClass.indexOf('answerhighlight') >= 0) {
				var order = strClass.replace('answerhighlight q', '');
				openDialog({
					order: order
				})
			} else {
				openDialog();
			}
		}
	});

	// Adds a menu item, which can then be included in any menu via the menu/menubar configuration
	editor.ui.registry.addMenuItem('cthhanswer', {
		text: 'Highlight answers',
		onAction: function () {
			// Open window
			openDialog();
		}
	});

	return {
		getMetadata: function () {
			return {
				name: "CTH highlight answers",
				url: "http://tienganhk12.com"
			};
		}
	};
});