window.downloadFileFromBytes = (fileName, bytesBase64) => {
    const link = document.createElement('a');
    link.href = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,' + bytesBase64;
    link.download = fileName;
    link.click();
};
