// noinspection JSUnusedGlobalSymbols

/**
 * Creates a downloadable file from the given content as JSON.
 * It uses the Blob API to construct the file and initiates a download by simulating a click event on an anchor element.
 * @param filename The name of the file to be downloaded.
 * @param content The content inside the file to be downloaded.
 */
function downloadFile(filename, content) {
    // TODO: Test the JSON formatter.
    const formattedContent = JSON.stringify(content, null, 2);
    const blob = new Blob([formattedContent], { type: 'application/json' });
    const link = document.createElement("a");

    link.href = window.URL.createObjectURL(blob);
    link.download = filename;
    link.click();

    window.URL.revokeObjectURL(link.href);
}
