window.ShowAppAlert = function (message, mode) {
    if (mode == "success") {
        toastr.success(message)
    } else if (mode == "warn") {
        toastr.warning(message)
    } else if (mode == "error") {
        toastr.error(message)
    } else {
        toastr.info(message);
    }
}

window.ShowAlert = function (message, mode) {
    alert(message);
}

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    //Blobs allow you to construct file like objects on the client that you can pass to apis that expect urls instead of requiring the server provides the file.
    //Blob gives JavaScript something like temporary files,
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

window.downloadExcelFile = (filename, content) => { 
    // thanks to Geral Barre : https://www.meziantou.net/generating-and-downloading-a-file-in-a-blazor-webassembly-application.htm 

    // Create the URL
    const file = new File([content], filename, { type: "application/octet-stream" });
    const exportUrl = URL.createObjectURL(file);

    // Create the <a> element and click on it
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = filename;
    a.target = "_self";
    a.click();

    // We don't need to keep the object url, let's release the memory
    // On Safari it seems you need to comment this line... (please let me know if you know why)
    URL.revokeObjectURL(exportUrl);
}