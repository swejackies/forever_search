#target photoshop
app.bringToFront();

//有用的参数集
//app.documents.length 
//app.activeDocument.activeLayer;

var inputFolder = Folder.selectDialog("先选择一个输入文件夹");
//var outputFolder = Folder.selectDialog("然后选择一个输出文件夹");

var fileList = inputFolder.getFiles();
for (var i = 0; i<fileList.length; i++) {

    if (fileList[i] instanceof File && !fileList[i].hidden && checkPSDFile(fileList[i])) {

        open(fileList[i]);
        runNow();
    }
}

function checkPSDFile(inFileName) {

    var lastDot = inFileName.toString().lastIndexOf(".");

    if (lastDot == -1) {
        return false;
    }

    var extension = inFileName.toString().substr(lastDot+1);
    extension = extension.toLowerCase();

    if (extension == "psd") {
        return true;
    }
    return false;
}

function runNow() {

    if (documents.length == 0) {

        alert("没有可处理的文档");

    } else {
        if (activeDocument.mode != DocumentMode.RGB) {
            //如果要保存为PNG格式，必须先将文档颜色模式改变RGB
            app.activeDocument.changeMode(ChangeMode.RGB);
        }
        var name = app.activeDocument.name;

        var lastDot = name.lastIndexOf(".");

        if (lastDot>-1) {
            name = name.substr(0, lastDot);// 默认的文件名，除去文件psd后缀的名称
        }
        var layers = activeDocument.layers;

        for (var i = 0, j = 0; i<layers.length; i++) {
            var layer = layers[i];
            layer.name = i;
            layer.visible = false;
        }
        for (var i = 0; i<layers.length; i++) {
            var layer = layers[i];
            layer.visible = true;

            //与PSD源文件相同的路径
            var path = activeDocument.path;
            var saveFile = new File(path+"/"+name+"_"+layer.name+".png");

            //输出到指定文件夹
            //var saveFile = new File(outputFolder + "/" + name + "_" + layer.name +".png"); 
            if (saveFile.exists) {
                saveFile.remove();
            }

            var pngSaveOptions = new PNGSaveOptions();
            activeDocument.saveAs(saveFile,pngSaveOptions,true,Extension.LOWERCASE);

            layer.visible = false;
        }
        activeDocument.close(SaveOptions.SAVECHANGES);//保存并关闭源文件
        // activeDocument.close(SaveOptions.DONOTSAVECHANGES);//不保存关闭源文件
    }
}