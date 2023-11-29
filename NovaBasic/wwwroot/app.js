window.createMonacoEditor = (id) => {
	require.config({ paths: { vs: './monaco/vs' } });
	require(['vs/editor/editor.main'], function () {
		monaco.languages.register({ id: 'NovaBASIC' });
		monaco.languages.setMonarchTokensProvider('NovaBASIC', window.NovaBasic);
		window.editor = monaco.editor.create(document.getElementById(id), {
			value: '',
			language: "NovaBASIC",
			automaticLayout: true,
		});
	});
}

window.getMonacoEditorValue = () => {
	return window.editor.getValue();
}