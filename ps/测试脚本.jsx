var dodo = function (info)
{
    app.documents.add();// 新建一个文档
}

dodo();

// Create a color to be used with the fill command
//alert("[fill][begin]");
var colorRef = new SolidColor
colorRef.rgb.red = 255
colorRef.rgb.green = 100
colorRef.rgb.blue = 0
// Now apply fill to the current selection
app.activeDocument.selection.fill(colorRef)
//alert("[fill][end]");

//alert("[alert][begin]");
if( BridgeTalk.appName == "photoshop" ) {
	alert("photoshop.创建");
}else{
	alert("non photoshop创建");
}
//alert("[alert][end]");